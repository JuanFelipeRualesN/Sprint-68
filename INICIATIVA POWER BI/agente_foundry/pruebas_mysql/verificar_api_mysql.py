"""Compara lo que devuelve la API (modo MySQL) con lo recalculado POR FUERZA BRUTA en Python sobre los mismos datos.
Es una segunda implementación independiente: si el SQL de ConsultasMySql.cs se equivoca (fronteras de fecha, filtro de rol,
universo de materias/escuelas, orden, empates, zona horaria...), aquí aparece la diferencia.

Uso:  python3 verificar_api_mysql.py http://127.0.0.1:5101 0        (URL de la API, offset UTC en minutos que usa la API)
"""
import datetime as dt
import json
import sys
import unicodedata
from functools import cmp_to_key
import urllib.error
import urllib.parse
import urllib.request

import datos_prueba as dp

BASE = sys.argv[1].rstrip("/")
OFF = int(sys.argv[2]) if len(sys.argv) > 2 else 0
USUARIOS, CONEX, RES = dp.generar()
ROL = {u[0]: u[1] for u in USUARIOS}
ESC_DE = {u[0]: u[2] for u in USUARIOS}
NOM_ESC = dict(dp.ESCUELAS)
NOM_MAT = dict(dp.MATERIAS)
# 3.er argumento = modo de la API:
#   (vacío) "paridad": criterio de Power BI (ORE que contiene DEMOSTRACION + «Facilitadores MSJ»); correos Require SÍ cuentan   [por defecto]
#   "dominio":         paridad + Datos__DominiosExcluidos=requiretechnology.com + Datos__EscuelasExcluidas="Escuela Demo T3 School,Escuela Facilitadores MSJ"
#   "incluir":         Datos__ExcluirCuentasDePrueba=false (no se excluye nada)
MODO = sys.argv[3] if len(sys.argv) > 3 else "paridad"
EXCL = MODO != "incluir"
CON_DOMINIO = MODO == "dominio"
NOMBRES_EXCL = {"Escuela Facilitadores MSJ"} | ({"Escuela Demo T3 School"} if CON_DOMINIO else set())
EMAIL = {u[0]: dp.email_de(u[0], u[3], u[4]) for u in USUARIOS}
ESC_EXCL = {s for s, n in dp.ESCUELAS if "DEMOSTRACION" in dp.ORE[s] or n in NOMBRES_EXCL}


def cuenta_conexion(uid):   # conexiones: visible, y no rol client (como vw_connections_report)
    return visible(uid) and ROL[uid] != 1


def visible(uid):         # ¿el usuario cuenta? (sin cuentas de prueba: ni correo interno ni escuela excluida)
    if not EXCL:
        return True
    if ESC_DE[uid] in ESC_EXCL:
        return False
    return not (CON_DOMINIO and EMAIL[uid].endswith("@" + dp.DOMINIO_INTERNO))
fallos, total = [], 0


def api(ruta, **q):
    url = f"{BASE}{ruta}?{urllib.parse.urlencode(q)}"
    try:
        with urllib.request.urlopen(url, timeout=30) as r:
            return r.status, json.loads(r.read())
    except urllib.error.HTTPError as e:
        return e.code, json.loads(e.read())


# Campos que la API calcula a partir de los valores base (porcentajes, variaciones). Este verificador comprueba los valores
# base por fuerza bruta; si la fila esperada no trae uno de estos campos, se quita de la fila de la API antes de comparar.
DERIVADOS = {"porcentaje_del_total", "conexiones_por_usuario", "porcentaje_del_total_completadas",
             "diferencia_con_periodo_anterior", "variacion_pct"}


def sin_derivados(obtenido, esperado):
    if isinstance(obtenido, list) and isinstance(esperado, list) and len(obtenido) == len(esperado) \
            and all(isinstance(o, dict) and isinstance(e, dict) for o, e in zip(obtenido, esperado)):
        return [{k: v for k, v in o.items() if k in e or k not in DERIVADOS} for o, e in zip(obtenido, esperado)]
    return obtenido


def igual(nombre, obtenido, esperado):
    global total
    obtenido = sin_derivados(obtenido, esperado)
    total += 1
    if obtenido != esperado:
        fallos.append(nombre)
        print(f"  ✗ {nombre}\n      API      : {json.dumps(obtenido, ensure_ascii=False)[:400]}\n      ESPERADO : {json.dumps(esperado, ensure_ascii=False)[:400]}")
    else:
        print(f"  ✓ {nombre}")


def rango(desde, hasta):          # [inicio, fin) en UTC para días locales desde..hasta inclusive
    d = dt.datetime.fromisoformat(desde) - dt.timedelta(minutes=OFF)
    h = dt.datetime.fromisoformat(hasta) + dt.timedelta(days=1) - dt.timedelta(minutes=OFF)
    return d, h


def kn(texto):          # clave de nombre como la collation utf8mb4_0900_ai_ci: sin tildes ni mayúsculas
    return "".join(c for c in unicodedata.normalize("NFD", texto) if not unicodedata.combining(c)).lower()


def ordenar(filas, valor, nombre, desc, desempate=lambda f: 0):
    """valor y nombre en el mismo sentido (como ORDER BY v DESC, nombre DESC); desempate final siempre ascendente."""
    s = -1 if desc else 1
    def cmp(a, b):
        for x, y, sg in ((valor(a), valor(b), s), (kn(nombre(a)), kn(nombre(b)), s), (desempate(a), desempate(b), 1)):
            if x != y:
                return sg * (-1 if x < y else 1)
        return 0
    return sorted(filas, key=cmp_to_key(cmp))


def orden(filas, clave, desc, limite):
    return ordenar(filas, clave, lambda f: f[0], desc)[:limite]


MAT_CON_PRUEBAS = sorted({m for _, m, _, _ in RES})


def esperado_area(desde, hasta, desc, limite):
    a, b = rango(desde, hasta)
    filas = []
    for m in MAT_CON_PRUEBAS:
        r = [x for x in RES if x[1] == m and ROL[x[0]] == 3 and visible(x[0])]
        asig = sum(1 for x in r if a <= x[2] < b)
        comp = sum(1 for x in r if a <= x[2] < b and x[3])          # cohorte por fecha de asignación
        filas.append((NOM_MAT[m], asig, comp, round(100.0 * comp / asig, 1) if asig else None))
    return [dict(nombre=f[0], pruebas_asignadas=f[1], pruebas_completadas=f[2],
                 **({"porcentaje_completado": f[3]} if f[3] is not None else {}))
            for f in orden(filas, lambda f: f[2], desc, limite)]


def esperado_comparar(da, ha, db, hb):
    (a1, b1), (a2, b2) = rango(da, ha), rango(db, hb)
    out = []
    for m in MAT_CON_PRUEBAS:
        r = [x for x in RES if x[1] == m and ROL[x[0]] == 3 and x[3] and visible(x[0])]
        va = sum(1 for x in r if a1 <= x[2] < b1)                    # cohorte por fecha de asignación, ya completadas
        vb = sum(1 for x in r if a2 <= x[2] < b2)
        f = dict(nombre=NOM_MAT[m], pruebas_completadas_a=va, pruebas_completadas_b=vb, diferencia=vb - va)
        if va:
            f["variacion_pct"] = round(100.0 * (vb - va) / va, 1)
        out.append(f)
    return sorted(out, key=lambda f: kn(f["nombre"]))


# universo de «por escuela» = escuelas con al menos un ESTUDIANTE o alguna conexión (de un usuario que cuenta), y no excluidas
ESC_CON_USUARIOS = sorted(({u[2] for u in USUARIOS if u[2] and u[1] == 3} | {ESC_DE[u] for u, _ in CONEX if ESC_DE[u] and cuenta_conexion(u)})
                          - ({s for s in ESC_EXCL} if EXCL else set()))


def esperado_escuelas(desde, hasta, desc, limite):
    a, b = rango(desde, hasta)
    cnt = {s: 0 for s in ESC_CON_USUARIOS}
    usr = {s: set() for s in ESC_CON_USUARIOS}
    for u, h in CONEX:
        if a <= h < b and ESC_DE[u] in cnt and cuenta_conexion(u):
            cnt[ESC_DE[u]] += 1
            usr[ESC_DE[u]].add(u)
    filas = [(NOM_ESC[s], n, len(usr[s])) for s, n in cnt.items()]
    return [dict(nombre=f[0], conexiones=f[1], usuarios_conectados=f[2]) for f in orden(filas, lambda f: f[1], desc, limite)]


def esperado_maestros(desde, hasta, desc, limite, escuela=None):
    a, b = rango(desde, hasta)
    cnt = {}
    for u in USUARIOS:
        if u[1] == 5 and u[2] is not None and visible(u[0]) and (escuela is None or NOM_ESC.get(u[2]) == escuela):
            cnt[u[0]] = 0
    for u, h in CONEX:
        if u in cnt and a <= h < b:
            cnt[u] += 1
    por_id = {u[0]: u for u in USUARIOS}
    filas = [(f"{por_id[i][3]} {por_id[i][4]}", NOM_ESC.get(por_id[i][2], "(sin escuela)"), n, i) for i, n in cnt.items()]
    filas = ordenar(filas, lambda f: f[2], lambda f: f[0], desc, lambda f: f[3])
    return [dict(nombre=f[0], escuela=f[1], conexiones=f[2]) for f in filas[:limite]]


def clave(d, g):
    return {"mes": f"{d.year}-{d.month:02d}", "trimestre": f"{d.year}-T{(d.month - 1) // 3 + 1}", "anio": str(d.year)}[g]


def esperado_tendencia(desde, hasta, g, escuela=None):
    a, b = rango(desde, hasta)
    acc, d = {}, dt.date.fromisoformat(desde)
    while d <= dt.date.fromisoformat(hasta):
        acc.setdefault(clave(d, g), 0); d += dt.timedelta(days=1)
    for u, h in CONEX:
        if a <= h < b and cuenta_conexion(u) and ESC_DE[u] is not None and (escuela is None or NOM_ESC.get(ESC_DE[u]) == escuela):
            acc[clave((h + dt.timedelta(minutes=OFF)).date(), g)] += 1
    return [dict(periodo=k, conexiones=v) for k, v in sorted(acc.items())]


def clave_ac(d, g):
    return d.isoformat() if g == "dia" else clave(d, g)


def esperado_acumuladas(desde, hasta, g, escuela=None):
    """Brute force: conexiones por período, suma corrida, usuarios nuevos (primera conexión del rango) y acumulados."""
    a, b = rango(desde, hasta)
    acc, d = {}, dt.date.fromisoformat(desde)
    while d <= dt.date.fromisoformat(hasta):
        acc.setdefault(clave_ac(d, g), [0, 0]); d += dt.timedelta(days=1)
    primera = {}
    for u, h in CONEX:
        if a <= h < b and cuenta_conexion(u) and ESC_DE[u] is not None and (escuela is None or NOM_ESC.get(ESC_DE[u]) == escuela):
            dia = (h + dt.timedelta(minutes=OFF)).date()
            acc[clave_ac(dia, g)][0] += 1
            primera[u] = min(primera.get(u, dia), dia)
    for u, dia in primera.items():
        acc[clave_ac(dia, g)][1] += 1
    filas, sc, su = [], 0, 0
    for k, (c, n) in sorted(acc.items()):
        sc += c; su += n
        filas.append(dict(periodo=k, conexiones=c, conexiones_acumuladas=sc, usuarios_nuevos=n, usuarios_acumulados=su))
    return filas


def esperado_activos():
    """Réplica de vw_total_users_with_connection_details: status 'active', rol ≠ client, sin ORE DEMOSTRACION (y sin dominio en modo dominio).
    El filtro por NOMBRE de escuela NO aplica (la tarjeta de Power BI no tiene escuela); usuarios sin escuela quedan fuera si hay exclusión."""
    activos = {u for u, st in dp.suscripciones(USUARIOS) if st == "active"}
    def cuenta(u):
        if ROL[u] == 1 or u not in activos:
            return False
        if not EXCL:
            return True
        if ESC_DE[u] is None or "DEMOSTRACION" in dp.ORE[ESC_DE[u]]:
            return False
        return not (CON_DOMINIO and EMAIL[u].endswith("@" + dp.DOMINIO_INTERNO))
    tot = {u for u in ROL if cuenta(u)}
    con = {u for u, _ in CONEX if u in tot}
    return len(tot), len(con)


def esperado_docentes(fecha=None, periodo_desde=None, escuela=None):
    """Brute force sobre las filas de vw_teacher_activities: maestros DISTINTOS por (escuela, período), 'SI' en cualquiera de sus filas."""
    excl_nombre = NOMBRES_EXCL if EXCL else set()
    grupos = {}
    for sie, _, ore, esc, d, h, ind in dp.docentes(USUARIOS):
        if EXCL and ("DEMOSTRACION" in ore or esc in excl_nombre):
            continue
        if escuela is not None and esc != escuela:
            continue
        if periodo_desde is not None:
            if d.date() != dt.date.fromisoformat(periodo_desde):
                continue
        elif not (d.date() <= dt.date.fromisoformat(fecha) <= h.date()):
            continue
        g = grupos.setdefault((esc, d.date().isoformat(), h.date().isoformat()), {})
        prev = g.get(sie, [False] * 5)
        g[sie] = [p or x == "SI" for p, x in zip(prev, ind)]
    def fila(nombre, d, h, m):
        return dict(escuela=nombre, periodo_desde=d, periodo_hasta=h, maestros=len(m),
                    **{k: sum(1 for v in m.values() if v[i]) for i, k in enumerate(
                        ["con_conexiones", "con_planes", "planes_con_datos", "con_grupos", "con_asignaciones"])})
    filas = [fila(e, d, h, m) for (e, d, h), m in grupos.items()]
    filas.sort(key=lambda f: (-f["maestros"], kn(f["escuela"]), f["periodo_desde"]))
    todos = {}
    for m in grupos.values():
        for sie, v in m.items():
            todos[sie] = [p or x for p, x in zip(todos.get(sie, [False] * 5), v)]
    return filas, fila("TOTAL", "", "", todos)


print(f"API {BASE}  offset={OFF} min")
print("\n[salud / frescura]")
_, s = api("/salud")
igual("datos_hasta_conexiones", s["datos_hasta_conexiones"], (max(h for u, h in CONEX if cuenta_conexion(u)) + dt.timedelta(minutes=OFF)).date().isoformat())
igual("datos_hasta_pruebas", s["datos_hasta_pruebas"], (max(max(x[2], x[3] or x[2]) for x in RES if ROL[x[0]] == 3 and visible(x[0])) + dt.timedelta(minutes=OFF)).date().isoformat())

print("\n[uso_por_area]")
for desde, hasta, desc, lim in [("2026-09-01", "2026-11-10", True, 50), ("2026-09-01", "2026-11-10", False, 2),
                                 ("2026-01-01", "2026-11-10", True, 3), ("2026-12-01", "2026-12-31", True, 50)]:
    _, r = api("/uso-por-area", desde=desde, hasta=hasta, orden="desc" if desc else "asc", limite=lim)
    igual(f"{desde}..{hasta} {'desc' if desc else 'asc'} lim {lim}", r["resultados"], esperado_area(desde, hasta, desc, lim))
igual("filas = materias con pruebas (BI-SP y CD no aparecen)", sorted(x["nombre"] for x in api("/uso-por-area", desde="2026-01-01", hasta="2026-11-10", limite=50)[1]["resultados"]),
      sorted(NOM_MAT[m] for m in MAT_CON_PRUEBAS))

print("\n[comparar_areas]")
_, r = api("/comparar-areas", desde_a="2026-09-01", hasta_a="2026-09-30", desde_b="2026-10-01", hasta_b="2026-10-31")
igual("sep vs oct", r["resultados"], esperado_comparar("2026-09-01", "2026-09-30", "2026-10-01", "2026-10-31"))

print("\n[conexiones_por_escuela]")
for desde, hasta, desc, lim in [("2026-01-01", "2026-11-10", True, 50), ("2026-09-01", "2026-11-10", False, 3),
                                 ("2026-09-01", "2026-09-01", True, 50)]:
    _, r = api("/conexiones-por-escuela", desde=desde, hasta=hasta, orden="desc" if desc else "asc", limite=lim)
    igual(f"{desde}..{hasta} {'desc' if desc else 'asc'} lim {lim}", r["resultados"], esperado_escuelas(desde, hasta, desc, lim))
_, r = api("/conexiones-por-escuela", desde="2026-01-01", hasta="2026-11-10", limite=50)
igual("universo = estudiantes o conexiones: I (sin usuarios) NO aparece; H (estudiantes, 0 conexiones) SÍ, con 0",
      ("Escuela Prueba I" not in [x["nombre"] for x in r["resultados"]],
       {"Escuela Prueba H": 0}.items() <= {x["nombre"]: x["conexiones"] for x in r["resultados"]}.items()),
      (True, True))

print("\n[conexiones_por_maestro]")
for desde, hasta, desc, lim, esc in [("2026-01-01", "2026-11-10", True, 5, None), ("2026-01-01", "2026-11-10", False, 5, None),
                                      ("2026-01-01", "2026-11-10", True, 5, "Escuela Prueba B"),
                                      ("2026-09-01", "2026-09-30", True, 50, "Escuela Prueba B")]:
    q = dict(desde=desde, hasta=hasta, orden="desc" if desc else "asc", limite=lim)
    if esc: q["escuela"] = esc
    _, r = api("/conexiones-por-maestro", **q)
    igual(f"{desde}..{hasta} {'desc' if desc else 'asc'} lim {lim} esc={esc}", r["resultados"], esperado_maestros(desde, hasta, desc, lim, esc))

print("\n[tendencia_conexiones]")
for desde, hasta, g, esc in [("2026-01-01", "2026-11-10", "trimestre", None), ("2026-08-01", "2026-11-11", "mes", None),
                              ("2026-01-01", "2026-11-12", "anio", "Escuela Prueba B"), ("2026-09-01", "2026-10-31", "mes", "Escuela Prueba H")]:
    q = dict(desde=desde, hasta=hasta, granularidad=g)
    if esc: q["escuela"] = esc
    _, r = api("/tendencia-conexiones", **q)
    igual(f"{desde}..{hasta} {g} esc={esc}", r["resultados"], esperado_tendencia(desde, hasta, g, esc))

print("\n[conexiones_acumuladas]")
for desde, hasta, g, esc in [("2026-01-01", "2026-11-12", "mes", None), ("2026-07-01", "2026-12-31", "trimestre", None),
                              ("2026-08-25", "2026-09-05", "dia", None), ("2026-01-01", "2026-11-12", "mes", "Escuela Prueba B")]:
    q = dict(desde=desde, hasta=hasta, granularidad=g)
    if esc: q["escuela"] = esc
    _, r = api("/conexiones-acumuladas", **q)
    esp = esperado_acumuladas(desde, hasta, g, esc)
    igual(f"{desde}..{hasta} {g} esc={esc}", r["resultados"], esp)
    igual(f"  resumen total/usuarios {g} esc={esc}", (r["resumen"]["total_conexiones"], r["resumen"]["usuarios_conectados"]),
          (esp[-1]["conexiones_acumuladas"], esp[-1]["usuarios_acumulados"]))
_, r = api("/conexiones-acumuladas", desde="2026-07-01", hasta="2026-12-31", granularidad="mes")
act, alguna = esperado_activos()
igual("usuarios_activos y conectados alguna vez (réplica de vw_total_users_with_connection_details)",
      (r["resumen"]["usuarios_activos"], r["resumen"]["usuarios_conectados_alguna_vez"]), (act, alguna))
igual("porcentaje_conexion_usuarios = conectados / activos (2 decimales, como Power BI)",
      r["resumen"]["porcentaje_conexion_usuarios"], round(100 * r["resumen"]["usuarios_conectados"] / act, 2))
_, r = api("/conexiones-acumuladas", desde="2026-07-01", hasta="2026-12-31", escuela="Escuela Prueba B")
igual("con escuela no se informan activos ni porcentajes", "usuarios_activos" in r["resumen"], False)
st, _ = api("/conexiones-acumuladas", desde="2026-01-01", hasta="2026-11-12", granularidad="dia")
igual("granularidad dia con más de 93 días → 400", st, 400)
_, tr = api("/tendencia-conexiones", desde="2026-07-01", hasta="2026-12-31", granularidad="trimestre")
_, ac = api("/conexiones-acumuladas", desde="2026-07-01", hasta="2026-12-31", granularidad="trimestre")
igual("coherencia: conexiones de acumuladas = tendencia", [x["conexiones"] for x in ac["resultados"]], [x["conexiones"] for x in tr["resultados"]])

print("\n[uso_docentes]")
for kw in [dict(fecha="2026-09-20"), dict(fecha="2026-10-20"), dict(fecha="2026-11-18"), dict(periodo_desde="2026-09-12"),
           dict(periodo_desde="2026-10-24", escuela="Escuela Prueba B"), dict(fecha="2025-01-01")]:
    _, r = api("/uso-docentes", limite=50, **kw)
    filas, tot = esperado_docentes(kw.get("fecha"), kw.get("periodo_desde"), kw.get("escuela"))
    igual(f"filas {kw}", r["resultados"], filas)
    igual(f"  resumen {kw}", {k: r["resumen"][k] for k in tot if k in r["resumen"]},
          {k: v for k, v in tot.items() if k not in ("escuela", "periodo_desde", "periodo_hasta")})
_, r = api("/uso-docentes", fecha="2025-01-01")
igual("sin períodos → aviso NO HAY DATOS", "NO HAY DATOS" in r.get("nota", ""), True)
_, r = api("/uso-docentes", fecha="2026-11-18", limite=2)
igual("limite 2 → aviso HAY MÁS FILAS y resumen de todas", ("HAY MÁS FILAS" in r.get("nota", ""), len(r["resultados"]), r["resumen"]["maestros"]),
      (True, 2, esperado_docentes("2026-11-18")[1]["maestros"]))
_, r = api("/uso-docentes")
igual("sin fecha usa hoy (período vigente)", "fecha" in r["periodo"], True)
st, _ = api("/uso-docentes", fecha="2026-02-30")
igual("fecha inválida → 400", st, 400)

print("\n[buscar_escuela y entradas hostiles]")
_, r = api("/buscar-escuela", texto="prueba b")
igual("texto 'prueba b'", r["resultados"], [{"nombre": "Escuela Prueba B"}])
_, r = api("/buscar-escuela", texto="escuela", limite=3)
igual("texto 'escuela' limite 3", [x["nombre"] for x in r["resultados"]],
      sorted((n for s, n in dp.ESCUELAS if not (EXCL and s in ESC_EXCL)), key=kn)[:3])
_, r = api("/buscar-escuela", texto="demo")
igual(f"buscar 'demo' (modo {MODO}): solo las no excluidas", sorted(x["nombre"] for x in r["resultados"]),
      sorted(n for s, n in dp.ESCUELAS if "demo" in kn(n) and not (EXCL and s in ESC_EXCL)))
st, r = api("/conexiones-por-maestro", desde="2026-01-01", hasta="2026-11-10", escuela="Escuela Demo Interna")
igual("escuela de ORE DEMOSTRACION como filtro: " + ("no existe (400)" if EXCL else "existe (200)"), st, 400 if EXCL else 200)
_, r = api("/buscar-escuela", texto="%%")
igual("comodín % escapado (no coincide con todo)", r["resultados"], [])
st, r = api("/conexiones-por-maestro", desde="2026-01-01", hasta="2026-11-10", escuela="x' OR '1'='1")
igual("inyección SQL en escuela → 400 'no existe'", (st, r.get("error")), (400, "parametros_invalidos"))
st, r = api("/uso-por-area", desde="2026-13-01", hasta="2026-11-10")
igual("fecha inválida → 400", st, 400)

print("\n[referencias en la respuesta]")
_, r = api("/uso-por-area", desde="2026-09-01", hasta="2026-11-10", limite=2)
igual("claves de referencia presentes", sorted(k for k in r if k != "resultados" and k != "nota"),
      sorted(["consulta", "fuente", "columna_fecha", "periodo", "filtros", "datos_hasta", "total_filas", "generado_en", "resumen"]))
igual("completadas nunca supera a asignadas (coherencia)", all(f["pruebas_completadas"] <= f["pruebas_asignadas"] for f in api("/uso-por-area", desde="2026-01-01", hasta="2026-11-10", limite=50)[1]["resultados"]), True)
igual("aviso: el período pasa de datos_hasta de PRUEBAS (19-oct)", "AVISO" in r.get("nota", "") and "2026-10-19" in r["nota"], True)
_, r = api("/conexiones-por-escuela", desde="2026-09-01", hasta="2026-11-10", limite=2)
igual("sin aviso en conexiones (datos hasta 11-nov ≥ 10-nov)", "AVISO" in r.get("nota", ""), False)

igual("filtros declara cuentas_de_prueba", r["filtros"]["cuentas_de_prueba"], "excluidas" if EXCL else "incluidas")
igual("la nota declara la exclusión (o su ausencia)", ("EXCLUYEN cuentas internas" in r["nota"]) if EXCL else ("Incluye cuentas internas" in r["nota"]), True)

print("\n[empates en el corte del límite]")
a, b = rango("2026-01-01", "2026-11-10")
ceros = sum(1 for u in USUARIOS if u[1] == 5 and u[2] is not None and visible(u[0]) and not any(x == u[0] and a <= h < b for x, h in CONEX))
_, r = api("/conexiones-por-maestro", desde="2026-01-01", hasta="2026-11-10", orden="asc", limite=2)
igual(f"maestros asc limite 2: hay {ceros} con 0 → aviso EMPATE con {ceros - 2} más",
      f"hay {ceros - 2} fila(s) más con el mismo valor (0)" in r.get("nota", ""), True)
igual("la lista sigue limitada a 2 filas", len(r["resultados"]), 2)
_, r = api("/conexiones-por-escuela", desde="2026-01-01", hasta="2026-11-10", orden="desc", limite=3)
_todas = esperado_escuelas("2026-01-01", "2026-11-10", True, 50)
_hay_empate = len(_todas) > 3 and _todas[2]["conexiones"] == _todas[3]["conexiones"]      # ¿el 3.º empata con el 4.º?
igual("aviso de EMPATE solo si el 3.º empata con el 4.º (esperado: " + ("sí" if _hay_empate else "no") + ")", "EMPATE" in r.get("nota", ""), _hay_empate)

print("\n[coherencia entre consultas: mismo total por escuela, por trimestre y por mes]")
_, e = api("/conexiones-por-escuela", desde="2026-07-01", hasta="2026-12-31", limite=50)
_, tr = api("/tendencia-conexiones", desde="2026-07-01", hasta="2026-12-31", granularidad="trimestre")
_, me = api("/tendencia-conexiones", desde="2026-07-01", hasta="2026-12-31", granularidad="mes")
sx, st_, sm = (sum(x["conexiones"] for x in z["resultados"]) for z in (e, tr, me))
igual("suma por escuela = suma por trimestre = suma por mes", (sx, st_, sm) == (sx, sx, sx), True)

print(f"\nResumen: {total - len(fallos)}/{total} comprobaciones coinciden.")
sys.exit(1 if fallos else 0)
