"""Compara lo que devuelve la API (modo MySQL) con lo recalculado POR FUERZA BRUTA en Python sobre los mismos datos.
Es una segunda implementación independiente: si el SQL de ConsultasMySql.cs se equivoca (fronteras de fecha, filtro de rol,
universo de materias/escuelas, orden, empates, zona horaria...), aquí aparece la diferencia.

Uso:  python3 verificar_api_mysql.py http://127.0.0.1:5101 0        (URL de la API, offset UTC en minutos que usa la API)
      python3 verificar_api_mysql.py http://127.0.0.1:5101 0 paridad auth
El 4.º argumento «auth» (Fase 4 · A4) es para la API con Auth__Modo=plataforma apuntando a usersapi_falso.py (puerto 5199): todas las
comprobaciones de siempre se hacen como admin (Token tok-admin) y se añade el bloque [auth]. Variables de entorno opcionales del bloque:
EYA_API_KEY (la misma de la API → prueba la identidad de servicio), ORIGEN_FRONT (origen CORS permitido; por defecto https://front.ejemplo.invalid),
USERSAPI_FALSA (por defecto http://127.0.0.1:5199). Si la API tiene el asistente habilitado con foundry_falso.py, también se prueba /asistente/*.
"""
import datetime as dt
import json
import os
import sys
import unicodedata
from functools import cmp_to_key
import urllib.error
import urllib.parse
import urllib.request

import datos_prueba as dp
import leer_xlsx

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
AUTH = len(sys.argv) > 4 and sys.argv[4] == "auth"
HEADERS = {"Token": "tok-admin"} if AUTH else {}      # con «auth», las comprobaciones de siempre van como admin
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
        with urllib.request.urlopen(urllib.request.Request(url, headers=HEADERS), timeout=30) as r:
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
        print(f"  ✗ {nombre}\n      API      : {json.dumps(obtenido, ensure_ascii=False, default=str)[:400]}\n      ESPERADO : {json.dumps(esperado, ensure_ascii=False, default=str)[:400]}")
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
    ult = {}
    for u, h in CONEX:
        if a <= h < b and ESC_DE[u] in cnt and cuenta_conexion(u):
            d = (h + dt.timedelta(minutes=OFF)).date().isoformat()
            ult[ESC_DE[u]] = max(ult.get(ESC_DE[u], d), d)
    filas = [(NOM_ESC[s], n, len(usr[s]), ult.get(s)) for s, n in cnt.items()]
    return [dict(nombre=f[0], conexiones=f[1], usuarios_conectados=f[2], **({"ultima_conexion": f[3]} if f[3] else {})) for f in orden(filas, lambda f: f[1], desc, limite)]


def esperado_maestros(desde, hasta, desc, limite, escuela=None):
    a, b = rango(desde, hasta)
    cnt = {}
    for u in USUARIOS:
        if u[1] == 5 and u[2] is not None and visible(u[0]) and (escuela is None or NOM_ESC.get(u[2]) == escuela):
            cnt[u[0]] = 0
    for u, h in CONEX:
        if u in cnt and a <= h < b:
            cnt[u] += 1
    ult = {}
    for u, h in CONEX:
        if u in cnt and a <= h < b:
            d = (h + dt.timedelta(minutes=OFF)).date().isoformat()
            ult[u] = max(ult.get(u, d), d)
    por_id = {u[0]: u for u in USUARIOS}
    filas = [(f"{por_id[i][3]} {por_id[i][4]}", NOM_ESC.get(por_id[i][2], "(sin escuela)"), n, i) for i, n in cnt.items()]
    filas = ordenar(filas, lambda f: f[2], lambda f: f[0], desc, lambda f: f[3])
    return [dict(nombre=f[0], escuela=f[1], conexiones=f[2], **({"ultima_conexion": ult[f[3]]} if f[3] in ult else {})) for f in filas[:limite]]


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


def esperado_patron_dia(desde, hasta, escuela=None):
    """Conexiones y usuarios distintos por día de la semana (lunes..domingo) en el día LOCAL; siempre 7 filas."""
    a, b = rango(desde, hasta)
    cnt, usr = [0] * 7, [set() for _ in range(7)]
    for u, h in CONEX:
        if a <= h < b and cuenta_conexion(u) and ESC_DE[u] is not None and (escuela is None or NOM_ESC.get(ESC_DE[u]) == escuela):
            w = (h + dt.timedelta(minutes=OFF)).weekday()
            cnt[w] += 1; usr[w].add(u)
    nombres = ["lunes", "martes", "miércoles", "jueves", "viernes", "sábado", "domingo"]
    return [dict(nombre=n, conexiones=cnt[i], usuarios_conectados=len(usr[i])) for i, n in enumerate(nombres)]


def esperado_patron_rol(desde, hasta, escuela=None):
    a, b = rango(desde, hasta)
    nom = dict(dp.ROLES)
    cnt, usr = {}, {}
    for u, h in CONEX:
        if a <= h < b and cuenta_conexion(u) and ESC_DE[u] is not None and (escuela is None or NOM_ESC.get(ESC_DE[u]) == escuela):
            r = nom[ROL[u]]
            cnt[r] = cnt.get(r, 0) + 1; usr.setdefault(r, set()).add(u)
    return [dict(nombre=r, conexiones=n, usuarios_conectados=len(usr[r])) for r, n in sorted(cnt.items(), key=lambda kv: (-kv[1], kv[0]))]


def esperado_suscripcion(agrupar, escuela=None, solo_escuelas=None):
    """Usuarios por estado de suscripción por fuerza bruta. Universo = rol ≠ client, fuera de cuentas de prueba (con exclusión, los sin escuela quedan fuera como en SQL),
    opcionalmente solo de ciertas escuelas (alcance). Categoría: alguna 'active' → activa; si no, alguna con «cancel» → cancelada; si no, con filas → inactiva; sin filas → sin.
    Devuelve (filas por grupo [nombre, activas, inactivas, canceladas, sin, total], totales (a, i, c, s), estados_originales). Con los datos reales (solo active/canceled) inactivas = 0."""
    filas_sub = {}
    for u, st in dp.suscripciones(USUARIOS):
        filas_sub.setdefault(u, []).append(st)
    rol_n = dict(dp.ROLES)
    grupos, tot, estados = {}, [0, 0, 0, 0], {}
    for u in USUARIOS:
        uid, rol, esc = u[0], u[1], u[2]
        if rol == 1 or not visible(uid) or (EXCL and esc is None):
            continue
        nom_esc = NOM_ESC.get(esc)
        if solo_escuelas is not None and nom_esc not in solo_escuelas:
            continue
        if escuela is not None and nom_esc != escuela:
            continue
        sts = filas_sub.get(uid, [])
        cat = 3 if not sts else 0 if any(x == "active" for x in sts) else 2 if any("cancel" in x for x in sts) else 1
        clave = "total" if agrupar == "total" else (nom_esc or "(sin escuela)") if agrupar == "escuela" else rol_n[rol]
        g = grupos.setdefault(clave, [0, 0, 0, 0])
        g[cat] += 1
        tot[cat] += 1
        for x in set(sts):
            estados[x] = estados.get(x, 0) + 1
    filas = [[k, g[0], g[1], g[2], g[3], sum(g)] for k, g in grupos.items()]
    return filas, tot, estados


def esperado_participacion(desde, hasta):
    """activos = suscripción 'active' (sin revisar cancelación), rol ≠ client, con escuela; conectados = distintos con conexión en el período."""
    a, b = rango(desde, hasta)
    activos = {u for u, st in dp.suscripciones(USUARIOS) if st == "active"}
    dom = lambda u: not (EXCL and CON_DOMINIO and EMAIL[u].endswith("@" + dp.DOMINIO_INTERNO))
    filas = []
    for s, nombre in dp.ESCUELAS:
        if EXCL and s in ESC_EXCL:
            continue
        act = sum(1 for u in ROL if ESC_DE[u] == s and ROL[u] != 1 and u in activos and dom(u))
        con = len({u for u, h in CONEX if a <= h < b and ESC_DE[u] == s and ROL[u] != 1 and dom(u)})
        if act > 0 or con > 0:
            filas.append(dict(nombre=nombre, usuarios_activos=act, usuarios_conectados=con,
                              porcentaje_participacion=round(100.0 * con / act, 1) if act else None))
    return filas


def ordenar_participacion(filas, desc):
    return sorted(filas, key=lambda f: (f["porcentaje_participacion"] is None,
                                        -(f["porcentaje_participacion"] or 0) if desc else (f["porcentaje_participacion"] or 0), kn(f["nombre"])))


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

print("\n[filtros «una sola vez» / «más de una conexión»]")
for desde, hasta, mn, mx in [("2026-01-01", "2026-11-10", 1, 1), ("2026-09-01", "2026-09-30", 2, None), ("2026-01-01", "2026-01-31", 0, 0),
                                 ("2026-10-01", "2026-10-07", 1, 1), ("2026-10-20", "2026-10-22", 1, 1), ("2026-11-09", "2026-11-12", 2, None),
                                 ("2026-01-11", "2026-01-11", 1, 1), ("2026-01-18", "2026-01-18", 1, 1)]:
    q = dict(desde=desde, hasta=hasta, orden="asc", limite=50, min_conexiones=mn)
    if mx is not None: q["max_conexiones"] = mx
    _, r = api("/conexiones-por-maestro", **q)
    esp = [f for f in esperado_maestros(desde, hasta, False, 5000) if f["conexiones"] >= mn and (mx is None or f["conexiones"] <= mx)][:50]
    igual(f"maestros {desde}..{hasta} min={mn} max={mx}", r["resultados"], esp)
    igual(f"  resumen maestros_que_cumplen_el_filtro", r["resumen"]["maestros_que_cumplen_el_filtro"],
          len([f for f in esperado_maestros(desde, hasta, False, 5000) if f["conexiones"] >= mn and (mx is None or f["conexiones"] <= mx)]))
    q["limite"] = 50
    _, r = api("/conexiones-por-escuela", **q)
    esp = [f for f in esperado_escuelas(desde, hasta, False, 50) if f["conexiones"] >= mn and (mx is None or f["conexiones"] <= mx)]
    igual(f"escuelas {desde}..{hasta} min={mn} max={mx}", r["resultados"], esp)
# Guardas: que el filtro «una sola vez» NO se esté probando solo con resultados vacíos
_, r = api("/conexiones-por-maestro", desde="2026-10-01", hasta="2026-10-07", min_conexiones=1, max_conexiones=1, limite=50)
igual("hay maestros con una sola conexión (prueba no vacía)", len(r["resultados"]) > 0 and all(f["conexiones"] == 1 for f in r["resultados"]), True)
_hay = False                                  # algún día de enero (cualquiera de las dos zonas horarias) tiene escuelas con exactamente 1 conexión
for _d in range(11, 19):
    _, r = api("/conexiones-por-escuela", desde=f"2026-01-{_d:02d}", hasta=f"2026-01-{_d:02d}", min_conexiones=1, max_conexiones=1, limite=50)
    assert all(f["conexiones"] == 1 for f in r["resultados"])
    _hay = _hay or len(r["resultados"]) > 0
igual("hay escuelas con una sola conexión (prueba no vacía)", _hay, True)
_, r = api("/conexiones-por-maestro", desde="2026-01-01", hasta="2026-11-10", limite=3)
igual("sin filtro no aparece el campo maestros_que_cumplen_el_filtro", "maestros_que_cumplen_el_filtro" in r["resumen"], False)
st, _ = api("/conexiones-por-maestro", desde="2026-01-01", hasta="2026-11-10", min_conexiones=5, max_conexiones=2)
igual("min mayor que max → 400", st, 400)
st, _ = api("/conexiones-por-escuela", desde="2026-01-01", hasta="2026-11-10", min_conexiones="x")
igual("min no numérico → 400", st, 400)

print("\n[patrones_conexion]")
for desde, hasta, ag, esc in [("2026-01-01", "2026-11-12", "dia_semana", None), ("2026-09-01", "2026-09-30", "dia_semana", None), ("2026-01-01", "2026-11-12", "dia_semana", "Escuela Prueba B"),
                              ("2026-01-01", "2026-11-12", "rol", None), ("2026-08-01", "2026-10-31", "rol", "Escuela Prueba C")]:
    q = dict(desde=desde, hasta=hasta, agrupar=ag)
    if esc: q["escuela"] = esc
    _, r = api("/patrones-conexion", **q)
    esp = (esperado_patron_dia if ag == "dia_semana" else esperado_patron_rol)(desde, hasta, esc)
    igual(f"{ag} {desde}..{hasta} esc={esc}", r["resultados"], esp)
    igual(f"  resumen total {ag} esc={esc}", r["resumen"]["total_conexiones"], sum(f["conexiones"] for f in esp))
_, r = api("/patrones-conexion", desde="2026-01-01", hasta="2026-11-12")
mx_ = max(f["conexiones"] for f in r["resultados"])
igual("resumen: día con más conexiones y empate", (r["resumen"]["conexiones_del_mayor"], r["resumen"]["hay_empate_en_el_mayor"]),
      (mx_, sum(1 for f in r["resultados"] if f["conexiones"] == mx_) > 1))
igual("el rol client nunca aparece", "client" in [f["nombre"] for f in api("/patrones-conexion", desde="2026-01-01", hasta="2026-11-12", agrupar="rol")[1]["resultados"]], False)
st, _ = api("/patrones-conexion", desde="2026-01-01", hasta="2026-11-12", agrupar="hora")
igual("agrupar=hora (no existe) → 400", st, 400)

print("\n[participacion_escuelas]")
for desde, hasta, desc, lim in [("2026-01-01", "2026-11-12", True, 50), ("2026-09-01", "2026-09-30", False, 50), ("2026-07-01", "2026-12-31", True, 3)]:
    _, r = api("/participacion-escuelas", desde=desde, hasta=hasta, orden="desc" if desc else "asc", limite=lim)
    todas = ordenar_participacion(esperado_participacion(desde, hasta), desc)
    igual(f"{desde}..{hasta} {'desc' if desc else 'asc'} lim {lim}", r["resultados"], todas[:lim])
    igual(f"  resumen global", (r["resumen"]["total_usuarios_activos"], r["resumen"]["total_usuarios_conectados"], r["resumen"]["escuelas_consideradas"]),
          (sum(f["usuarios_activos"] for f in todas), sum(f["usuarios_conectados"] for f in todas), len(todas)))
_, r = api("/participacion-escuelas", desde="2026-01-01", hasta="2026-11-12", escuela="Escuela Prueba B")
igual("con escuela solo sale esa", [f["nombre"] for f in r["resultados"]], ["Escuela Prueba B"])
_, r = api("/participacion-escuelas", desde="2026-01-01", hasta="2026-11-12", limite=2)
igual("limite 2 → aviso HAY MÁS FILAS", "HAY MÁS FILAS" in r.get("nota", ""), True)

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
      sorted(["consulta", "fuente", "columna_fecha", "periodo", "filtros", "datos_hasta", "total_filas", "generado_en", "resumen", "alcance"]))
igual("alcance declarado en la referencia (sin sesión de plataforma = todas las escuelas)", r.get("alcance"), "todas las escuelas")
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

# ───────────────────────── Fase 4: exportar a Excel y gráficas ─────────────────────────
print("\n[fase 4: exportar]")
TIPO_XLSX = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"


def api_bin(ruta, **q):
    url = f"{BASE}{ruta}?{urllib.parse.urlencode(q)}"
    try:
        with urllib.request.urlopen(urllib.request.Request(url, headers=HEADERS), timeout=60) as r:
            return r.status, dict(r.headers), r.read()
    except urllib.error.HTTPError as e:
        return e.code, dict(e.headers), e.read()


def exportar(consulta, **q):
    """(estado, cabeceras, hojas, nº de fórmulas) del .xlsx; hojas = {nombre: filas}."""
    st, h, cuerpo = api_bin(f"/exportar/{consulta}", formato="xlsx", **q)
    if st != 200:
        return st, h, None, 0
    hojas, formulas = leer_xlsx.leer(cuerpo, con_formulas=True)
    return st, h, hojas, formulas


def grafica(consulta, **q):
    st, _, cuerpo = api_bin(f"/exportar/{consulta}", formato="grafica", **q)
    return st, json.loads(cuerpo)


def filas_json(filas):
    """Resultados de la API → (claves en orden de aparición, filas como listas; None = celda vacía)."""
    claves = []
    for f in filas:
        for k in f:
            if k not in claves:
                claves.append(k)
    return claves, [[f.get(k) for k in claves] for f in filas]


def rectangular(filas, n):
    """El lector omite las celdas vacías del final de cada fila: se rellenan con None hasta n columnas."""
    return [f + [None] * (n - len(f)) for f in filas]


def tabla_referencias(hojas):
    d = {}
    for fila in hojas["Referencias"]:
        if len(fila) >= 2 and fila[0] is not None:
            d.setdefault(fila[0], fila[1])
    return d


def txt(v):                 # como Columnas.Texto en C#
    return "sí" if v is True else "no" if v is False else str(v)


D = dict(desde="2026-01-01", hasta="2026-11-10")
CMP = dict(desde_a="2026-09-01", hasta_a="2026-09-30", desde_b="2026-10-01", hasta_b="2026-10-31")
# (consulta, parámetros, nombre de archivo esperado, (tipo, layout) de gráfica Recharts, columna de categorías, series)
CASOS = [
    ("uso-por-area", dict(desde="2026-09-01", hasta="2026-11-10"), "uso_por_area_2026-09-01_2026-11-10", ("bar", "horizontal"), "nombre", ["pruebas_asignadas", "pruebas_completadas"]),
    ("conexiones-por-escuela", D, "conexiones_por_escuela_2026-01-01_2026-11-10", ("bar", "vertical"), "nombre", ["conexiones"]),
    ("conexiones-por-maestro", D, "conexiones_por_maestro_2026-01-01_2026-11-10", ("bar", "vertical"), "nombre", ["conexiones"]),
    ("comparar-areas", CMP, "comparar_areas_a_2026-09-01_2026-09-30_b_2026-10-01_2026-10-31", ("bar", "horizontal"), "nombre", ["pruebas_completadas_a", "pruebas_completadas_b"]),
    ("comparar-escuelas", CMP, "comparar_escuelas_a_2026-09-01_2026-09-30_b_2026-10-01_2026-10-31", ("bar", "horizontal"), "nombre", ["conexiones_a", "conexiones_b"]),
    ("tendencia-conexiones", D, "tendencia_conexiones_2026-01-01_2026-11-10", ("line", None), "periodo", ["conexiones"]),
    ("conexiones-acumuladas", D, "conexiones_acumuladas_2026-01-01_2026-11-10", ("line", None), "periodo", ["conexiones_acumuladas", "usuarios_acumulados"]),
    ("uso-docentes", dict(fecha="2026-11-18"), "uso_docentes_2026-11-18", ("bar", "horizontal"), "escuela", ["maestros", "con_conexiones", "con_asignaciones"]),
    ("patrones-conexion", D, "patrones_conexion_2026-01-01_2026-11-10", ("bar", "horizontal"), "nombre", ["conexiones"]),
    ("participacion-escuelas", D, "participacion_escuelas_2026-01-01_2026-11-10", ("bar", "vertical"), "nombre", ["usuarios_activos", "usuarios_conectados"]),
    ("promedio-full-quiz", dict(D, agrupar="escuela"), "promedio_full_quiz_2026-01-01_2026-11-10", ("bar", "vertical"), "escuela", ["pruebas_asignadas", "pruebas_completadas"]),
]
# criterio de orden y medida «extra» (otra escala: va en el tooltip y la etiqueta, no como serie) de cada gráfica
PRINCIPAL = {"uso-por-area": "pruebas_completadas", "participacion-escuelas": "porcentaje_participacion", "promedio-full-quiz": "promedio_solo_realizadas"}
EXTRA = {"participacion-escuelas": "porcentaje_participacion", "promedio-full-quiz": "promedio_solo_realizadas"}

def esperado_data_grafica(ruta, filas, tipo, col_cat, series, todas=False, orden="desc", ceros=False, limite=10):
    """(data esperada, total_categorias, categorias_en_cero) de una gráfica a partir de TODAS las filas de la consulta.
    Barras: sin categorías en 0 (salvo ceros), de mayor a menor por la serie principal (empate por nombre), vista previa de 10. Líneas y «día de la semana»: tal cual."""
    ordena = tipo[0] == "bar" and ruta != "patrones-conexion"
    en_cero, sel = 0, list(filas)
    if ordena:
        principal = PRINCIPAL.get(ruta, series[0])
        cero = lambda f: all((f.get(k) or 0) == 0 for k in series)
        en_cero = sum(1 for f in filas if cero(f))
        sel = [f for f in filas if ceros or not cero(f)]
        sel.sort(key=lambda f: ((f.get(principal) or 0) * (1 if orden == "asc" else -1), f[col_cat]))
    total = len(sel)
    if ordena and not todas:
        sel = sel[:limite]
    etiquetas = [f[col_cat] for f in sel]
    if ruta == "uso-docentes" and len(set(etiquetas)) != len(etiquetas):
        etiquetas = [f"{f['escuela']} ({f['periodo_desde']})" for f in sel]
    return [{"categoria": e, **{k: f.get(k) for k in series}, **({EXTRA[ruta]: f.get(EXTRA[ruta])} if ruta in EXTRA else {})} for e, f in zip(etiquetas, sel)], total, en_cero


for ruta, q, archivo, tipo, col_cat, series in CASOS:
    _, r = api("/" + ruta, **q, limite=50)
    st, h, hojas, formulas = exportar(ruta, **q)
    igual(f"{ruta}: xlsx → 200, content-type xlsx y nombre de archivo {archivo}.xlsx",
          (st, h.get("Content-Type"), f"filename={archivo}.xlsx" in h.get("Content-Disposition", ""), formulas), (200, TIPO_XLSX, True, 0))
    # la hoja Datos tiene las mismas filas, en el mismo orden, que el JSON de la consulta (las filas del JSON son un prefijo de la exportación)
    claves, esperadas = filas_json(r["resultados"])
    datos = hojas["Datos"]
    igual(f"{ruta}: hoja Datos == resultados del JSON (limite normal)", (len(datos[0]), rectangular(datos, len(claves))[1:1 + len(esperadas)]), (len(claves), esperadas))
    igual(f"{ruta}: hojas = Datos y Referencias", list(hojas), ["Datos", "Referencias"])
    # gráfica
    st, g = grafica(ruta, **q)
    esp_data, esp_total, esp_cero = esperado_data_grafica(ruta, r["resultados"], tipo, col_cat, series)
    igual(f"{ruta}: gráfica {tipo[0]}/{tipo[1]}: data (objetos planos), series y total_categorias = recálculo (barras: mayor a menor, sin ceros, 10)",
          (st, (g.get("tipo"), g.get("layout")), g["eje_x"]["clave"], [s["clave"] for s in g["series"]], g["data"], (g["filas"], g["categorias_mostradas"], g["total_categorias"]), g["consulta"], "categorias" in g["eje_x"]),
          (200, tipo, "categoria", series, esp_data, (len(esp_data), len(esp_data), esp_total), r["consulta"], False))
    igual(f"{ruta}: gráfica: categorias_en_cero / orden solo en barras ordenables", (g.get("categorias_en_cero"), g.get("orden")),
          (esp_cero, "desc") if tipo[0] == "bar" and ruta != "patrones-conexion" else (None, None))

# exportación = lista completa, contrastada con la fuerza bruta
a, b = rango("2026-01-01", "2026-11-10")


def datos_exportados(ruta, **q):
    _, _, hojas, _ = exportar(ruta, **q)
    n = len(hojas["Datos"][0])
    return hojas["Datos"][0], rectangular(hojas["Datos"], n)[1:], tabla_referencias(hojas)


for rg, min_max in [(D, {}), (dict(desde="2026-10-01", hasta="2026-10-07"), dict(min_conexiones=1, max_conexiones=1))]:
    cab, filas, ref = datos_exportados("conexiones-por-maestro", **rg, **min_max)
    todos = esperado_maestros(rg["desde"], rg["hasta"], True, 5000)
    total_c = sum(f["conexiones"] for f in todos)
    if min_max:
        todos = [f for f in todos if f["conexiones"] == 1]
    igual(f"fuerza bruta: exportar maestros {rg['desde']}..{rg['hasta']} {min_max or 'sin filtro'} = todos (nombre, escuela, conexiones, % del total)",
          [(f[0], f[1], f[2], f[3]) for f in filas],
          [(f["nombre"], f["escuela"], f["conexiones"], round(100.0 * f["conexiones"] / total_c, 1)) for f in todos])
cab, filas, ref = datos_exportados("conexiones-por-maestro", **D, min_conexiones=9999)
igual("exportar sin resultados → hoja Datos con el aviso «Sin resultados» y Referencias intacta", (cab, filas, ref["Filas exportadas"]),
      (["Sin resultados para este filtro (ver la hoja Referencias)."], [], 0))
_, r = api("/conexiones-por-maestro", **D, limite=3)
igual("exportar sin 'limite' trae TODAS las filas (> las 3 del JSON y = maestros_considerados del resumen)",
      (len(datos_exportados("conexiones-por-maestro", **D)[1]) > 3, len(datos_exportados("conexiones-por-maestro", **D)[1])),
      (True, r["resumen"]["maestros_considerados"]))
cab, filas, ref = datos_exportados("conexiones-por-maestro", **D, limite=2)
igual("exportar con limite=2 → 2 filas y Referencias dice que la lista NO es completa", (len(filas), ref["Lista completa"].startswith("NO")), (2, True))
cab, filas, ref = datos_exportados("conexiones-por-escuela", **D)
igual("fuerza bruta: exportar escuelas = todas (nombre, conexiones, usuarios conectados)",
      [(f[0], f[1], f[2]) for f in filas],
      [(f["nombre"], f["conexiones"], f["usuarios_conectados"]) for f in esperado_escuelas(D["desde"], D["hasta"], True, 5000)])
cab, filas, ref = datos_exportados("participacion-escuelas", **D)
esp = ordenar_participacion(esperado_participacion(D["desde"], D["hasta"]), True)
igual("fuerza bruta: exportar participación = todas las escuelas",
      [(f[0], f[1], f[2], f[3]) for f in filas], [(f["nombre"], f["usuarios_activos"], f["usuarios_conectados"], f["porcentaje_participacion"]) for f in esp])
cab, filas, ref = datos_exportados("tendencia-conexiones", **D)
igual("fuerza bruta: exportar tendencia mensual (período, conexiones)", [(f[0], f[1]) for f in filas],
      [(f["periodo"], f["conexiones"]) for f in esperado_tendencia(D["desde"], D["hasta"], "mes")])

# Excel vacío por falta de datos: si el período pedido empieza DESPUÉS de «Datos hasta», la hoja Datos dice la causa (también en comparaciones)
_, rs = api("/salud")
dh_conex, dh_pruebas = rs["datos_hasta_conexiones"], rs["datos_hasta_pruebas"]
fin = (dt.date.fromisoformat(dh_conex) + dt.timedelta(days=30)).isoformat()
ini = (dt.date.fromisoformat(dh_conex) + dt.timedelta(days=1)).isoformat()
for ruta, q in (("conexiones-por-maestro", dict(desde=ini, hasta=fin, min_conexiones=1)), ("conexiones-por-escuela", dict(desde=ini, hasta=fin, min_conexiones=1))):
    cab, filas, ref = datos_exportados(ruta, **q)
    igual(f"Excel vacío de {ruta} con el período después de los datos: la hoja dice la causa",
          (cab, filas), ([f"No hay datos de conexiones después del {dh_conex}; el período pedido empieza el {ini} (ver la hoja Referencias)."], []))
cab, filas, ref = datos_exportados("comparar-escuelas", desde_a=ini, hasta_a=fin, desde_b=fin, hasta_b=(dt.date.fromisoformat(fin) + dt.timedelta(days=10)).isoformat())
igual("Excel vacío de una comparación con los dos períodos después de los datos: dice la causa con los dos inicios",
      (len(cab), cab[0].startswith(f"No hay datos de conexiones después del {dh_conex}; los períodos pedidos empiezan el {ini} y el {fin}"), filas), (1, True, []))
cab, filas, ref = datos_exportados("conexiones-por-maestro", desde=D["desde"], hasta=ini, min_conexiones=9999)
igual("Excel vacío con un período que SÍ tiene datos (filtro imposible): sigue el mensaje genérico, sin inventar causa",
      cab, ["Sin resultados para este filtro (ver la hoja Referencias)."])

# Referencias: los valores numéricos van como número (sin el triángulo verde de «número guardado como texto»)
_, _, hojas, _ = exportar("conexiones-por-maestro", **D, limite=7)
ref = tabla_referencias(hojas)
igual("Referencias: «Filtro · limite» y «Filas exportadas» son números", (ref["Filtro · limite"], ref["Filas exportadas"], type(ref["Filtro · limite"]).__name__, type(ref["Filas exportadas"]).__name__),
      (7, 7, "int", "int"))
import re as _re
for ruta, q in (("conexiones-por-maestro", dict(D, min_conexiones=1, max_conexiones=3)), ("participacion-escuelas", D), ("uso-por-area", D), ("tendencia-conexiones", D)):
    _, _, hojas, _ = exportar(ruta, **q)
    como_texto = [(k, v) for k, v in tabla_referencias(hojas).items() if isinstance(v, str) and _re.fullmatch(r"-?\d+([.,]\d+)?", v.strip())]
    igual(f"Referencias de {ruta}: ningún número guardado como texto", como_texto, [])
_, _, hojas, _ = exportar("conexiones-por-maestro", **D, min_conexiones=1, max_conexiones=3)
ref = tabla_referencias(hojas)
igual("Referencias: min_conexiones y max_conexiones también son números", (ref["Filtro · min conexiones"], ref["Filtro · max conexiones"]), (1, 3))

# la participación NO se topa en 100 % (como Power BI) y la nota de corte da las cifras claras
_, r = api("/participacion-escuelas", **D, limite=2)
esp = ordenar_participacion(esperado_participacion(D["desde"], D["hasta"]), True)
igual("participación: el porcentaje más alto sale tal cual (sin tope en 100) y = fuerza bruta", r["resultados"][0]["porcentaje_participacion"], esp[0]["porcentaje_participacion"])
igual("participación: la nota de corte dice cuántas se muestran, cuántas hay, cuántas quedan fuera y por qué",
      (f"se muestran 2 de {len(esp)} escuelas" in r["nota"], f"las otras {len(esp) - 2} quedan fuera por el límite pedido" in r["nota"],
       f"{sum(1 for f in esp if f['usuarios_conectados'] == 0)} de las {len(esp)} no tienen usuarios conectados" in r["nota"]), (True, True, True))

# última conexión por escuela y por maestro (también en el Excel)
_, r = api("/conexiones-por-escuela", **D, limite=50)
igual("ultima_conexion de cada escuela = máximo día con conexión del período (fuerza bruta; se omite si no hubo)",
      [f.get("ultima_conexion") for f in r["resultados"]], [f.get("ultima_conexion") for f in esperado_escuelas(D["desde"], D["hasta"], True, 50)])
igual("ultima_conexion: las escuelas sin conexiones en el período no la traen y las demás sí",
      (all(("ultima_conexion" in f) == (f["conexiones"] > 0) for f in r["resultados"]), any(f["conexiones"] == 0 for f in r["resultados"])), (True, True))
cab, filas, ref = datos_exportados("conexiones-por-escuela", **D)
esp_u = {f["nombre"]: f.get("ultima_conexion") for f in esperado_escuelas(D["desde"], D["hasta"], True, 5000)}
i_u = cab.index("Última conexión")
igual("Excel de conexiones-por-escuela: columna «Última conexión» = fuerza bruta", {f[0]: f[i_u] for f in filas}, esp_u)
cab, filas, ref = datos_exportados("conexiones-por-maestro", **D)
esp_m = {(f["nombre"], f["escuela"]): f.get("ultima_conexion") for f in esperado_maestros(D["desde"], D["hasta"], True, 5000)}   # (nombre, escuela) es único en los datos de prueba
i_m = cab.index("Última conexión")
igual("Excel de conexiones-por-maestro: columna «Última conexión» = fuerza bruta", sorted(((f[0], f[1]), f[i_m]) for f in filas), sorted(esp_m.items()))
_, rm = api("/conexiones-por-maestro", **D, limite=50)
igual("ultima_conexion de cada maestro (JSON) = fuerza bruta", [f.get("ultima_conexion") for f in rm["resultados"]],
      [f.get("ultima_conexion") for f in esperado_maestros(D["desde"], D["hasta"], True, 50)])
# fronteras de día: la última conexión en la medianoche UTC cae en el día local anterior con offset negativo (se prueba corriendo con offset -240)

# usuarios por estado de suscripción (fuerza bruta sobre las filas sintéticas; incluye canceladas y usuarios sin suscripción)
print("\n[usuarios por suscripción]")
for agr in ("total", "escuela", "rol"):
    st, r = api("/usuarios-por-suscripcion", agrupar=agr, limite=50)
    esp_f, esp_t, esp_e = esperado_suscripcion(agr)
    if agr == "total":
        obt = [(f["nombre"], f["usuarios"]) for f in r["resultados"]]
        igual("usuarios por suscripción, total: 3 estados (activas, canceladas, sin suscripción; «inactivas» no sale porque vale 0) = recálculo", (st, obt),
              (200, [("Activas", esp_t[0]), ("Canceladas", esp_t[2]), ("Sin suscripción", esp_t[3])]))
    else:
        igual(f"usuarios por suscripción, por {agr}: sin la clave «inactivas» (vale 0)", any("inactivas" in f for f in r["resultados"]), False)
        obt = sorted((f["nombre"], f["activas"], f["canceladas"], f["sin_suscripcion"], f["total_usuarios"]) for f in r["resultados"])
        igual(f"usuarios por suscripción, por {agr}: una fila por grupo = recálculo", (st, obt), (200, sorted((x[0], x[1], x[3], x[4], x[5]) for x in esp_f)))
        orden_ok = [f["total_usuarios"] for f in r["resultados"]] == sorted((f["total_usuarios"] for f in r["resultados"]), reverse=True)
        igual(f"usuarios por suscripción, por {agr}: ordenado de más a menos usuarios", orden_ok, True)
    rs = r["resumen"]
    igual(f"usuarios por suscripción ({agr}): resumen = totales del recálculo, estados originales y suma = total",
          (rs["activas"], rs.get("inactivas"), rs["canceladas"], rs["sin_suscripcion"], rs["total_usuarios"], rs["estados_originales"],
           rs["porcentaje_activas"], rs["activas_con_fecha_de_cancelacion_vencida"]),
          (esp_t[0], None, esp_t[2], esp_t[3], sum(esp_t), esp_e, round(100.0 * esp_t[0] / sum(esp_t), 1), 0))
_, rt = api("/usuarios-por-suscripcion")
igual("usuarios por suscripción: hay de los tres estados en los datos de prueba (la prueba no pasa vacía)",
      all(f["usuarios"] > 0 for f in rt["resultados"]), True)
igual("usuarios por suscripción: referencia sin período (período «no aplica», sin datos_hasta) y las cuentas de prueba declaradas",
      (rt["periodo"], "datos_hasta" in rt, rt["filtros"]["cuentas_de_prueba"], rt["filtros"]["agrupar"]), ("no aplica", False, "excluidas" if EXCL else "incluidas", "total"))
esc_ej = esperado_suscripcion("escuela")[0][0][0]
_, re_ = api("/usuarios-por-suscripcion", agrupar="escuela", escuela=esc_ej)
igual("usuarios por suscripción: filtro por escuela = recálculo de esa escuela", [(f["nombre"], f["total_usuarios"]) for f in re_["resultados"]],
      [(f[0], f[5]) for f in esperado_suscripcion("escuela", escuela=esc_ej)[0]])
st, rr = api("/usuarios-por-suscripcion", agrupar="x")
igual("usuarios por suscripción: agrupar inválido → 400", (st, rr.get("error")), (400, "parametros_invalidos"))
if not EXCL:   # sin exclusiones el universo es el de la tarjeta «Activos»: el total de activas debe coincidir con usuarios_activos de conexiones-acumuladas
    _, ra = api("/conexiones-acumuladas", desde="2026-01-01", hasta="2026-11-10", granularidad="anio")
    igual("usuarios por suscripción = tarjeta «Activos» de conexiones-acumuladas (sin exclusiones: mismas activas)", rt["resumen"]["activas"], ra["resumen"]["usuarios_activos"])
# exportación: Excel y gráfica
for agr in ("escuela", "rol", "total"):
    cab, filas, ref = datos_exportados("usuarios-por-suscripcion", agrupar=agr)
    esp_f, esp_t, _ = esperado_suscripcion(agr)
    if agr == "total":
        igual("Excel de usuarios por suscripción (total): 3 estados = recálculo", [(f[0], f[1]) for f in filas],
              [("Activas", esp_t[0]), ("Canceladas", esp_t[2]), ("Sin suscripción", esp_t[3])])
    else:
        igual(f"Excel de usuarios por suscripción (por {agr}): todas las filas = recálculo, con encabezados legibles",
              (cab[:5], sorted(tuple(f[:5]) for f in filas)), (["Nombre", "Activas", "Canceladas", "Sin suscripción", "Total de usuarios"], sorted((x[0], x[1], x[3], x[4], x[5]) for x in esp_f)))
    igual(f"Excel de usuarios por suscripción ({agr}): Referencias dice «no aplica» en datos hasta y el número de filas es número", (ref["Datos hasta"], isinstance(ref["Filas exportadas"], int)), ("no aplica", True))
st, g = grafica("usuarios-por-suscripcion", agrupar="rol")
esp_f, _, _ = esperado_suscripcion("rol")
igual("gráfica de usuarios por suscripción (por rol): barras agrupadas con las 3 series (sin «inactivas»), de más a menos usuarios, = recálculo",
      (st, [s["clave"] for s in g["series"]], [(p["categoria"], p["activas"], p["canceladas"], p["sin_suscripcion"]) for p in g["data"]]),
      (200, ["activas", "canceladas", "sin_suscripcion"], [(f[0], f[1], f[3], f[4]) for f in sorted(esp_f, key=lambda f: (-f[5], f[0]))]))
st, g = grafica("usuarios-por-suscripcion")
esp_t = esperado_suscripcion("total")[1]
estados = [("Activas", esp_t[0]), ("Canceladas", esp_t[2]), ("Sin suscripción", esp_t[3])]
igual("dona de usuarios por suscripción (total): tipo pie / variante dona, estados en su orden natural con su valor, = recálculo",
      (st, g["tipo"], g.get("variante"), "layout" not in g, "agrupar" not in g, [(p["categoria"], p["usuarios"]) for p in g["data"]], g["eje_x"]["titulo"]),
      (200, "pie", "dona", True, True, [e for e in estados if e[1] > 0], "Estado"))
igual("dona: total_valor (el total del centro) = suma de las porciones y el % de cada una viaja en `extra`",
      (g["total_valor"], sum(p["usuarios"] for p in g["data"]), g["extra"], [p["porcentaje_del_total"] for p in g["data"]]),
      (sum(e[1] for e in estados if e[1] > 0), sum(p["usuarios"] for p in g["data"]), {"clave": "porcentaje_del_total", "nombre": "% del total", "unidad": "%"},
       [round(100.0 * e[1] / sum(esp_t), 1) for e in estados if e[1] > 0]))
igual("dona: mostradas = total_categorias = las de valor no cero; categorias_en_cero y incluir_ceros declarados; sin orden/todas (una dona no se ordena)",
      (g["categorias_mostradas"], g["total_categorias"], g["categorias_en_cero"], g["incluir_ceros"], "orden" in g, "todas" in g),
      (len([e for e in estados if e[1] > 0]), len([e for e in estados if e[1] > 0]), len([e for e in estados if e[1] == 0]), False, False, False))
st, g0 = grafica("usuarios-por-suscripcion", incluir_ceros="true")
igual("dona con incluir_ceros=true: las 3 porciones (no existe «Inactivas» cuando vale 0)", (st, g0["tipo"], [p["categoria"] for p in g0["data"]]), (200, "pie", [e[0] for e in estados]))
st, gr = grafica("usuarios-por-suscripcion", agrupar="rol")
igual("usuarios por suscripción por rol NO es dona: barras agrupadas con 3 series (varias medidas)", (gr["tipo"], gr.get("agrupar"), len(gr["series"]), gr.get("variante")), ("bar", "grouped", 3, None))

# hoja Referencias
_, r = api("/uso-por-area", desde="2026-09-01", hasta="2026-11-10")
_, _, hojas, _ = exportar("uso-por-area", desde="2026-09-01", hasta="2026-11-10")
ref = tabla_referencias(hojas)
igual("Referencias: consulta, período, cuentas de prueba y datos hasta",
      (ref["Consulta"], ref["Período · desde"], ref["Período · hasta"], ref["Cuentas de prueba"], ref["Datos hasta"], ref["Fuente"], ref["Columna de fecha"]),
      ("uso_por_area", "2026-09-01", "2026-11-10", "excluidas" if EXCL else "incluidas", r["datos_hasta"], r["fuente"], r["columna_fecha"]))
igual("Referencias: el AVISO de datos hasta 19-oct y la nota completa viajan con el archivo", (ref["Nota"] == r["nota"], "AVISO" in ref["Nota"]), (True, True))
igual("Referencias: filas exportadas, lista completa y generado en", (ref["Filas exportadas"], ref["Lista completa"].startswith("Sí"), ref["Generado en (UTC)"][:10].count("-")),
      (len(r["resultados"]), True, 2))
igual("Referencias: cada clave del resumen, una por una (los números como número)", [ref[k.replace("_", " ")] for k in r["resumen"] if r["resumen"][k] is not None],
      [v if isinstance(v, (int, float)) and not isinstance(v, bool) else txt(v) for v in r["resumen"].values() if v is not None])
_, r = api("/comparar-escuelas", **CMP)
_, _, hojas, _ = exportar("comparar-escuelas", **CMP)
ref = tabla_referencias(hojas)
igual("Referencias: comparación trae los dos períodos (a y b)", (ref["Período · a.desde"], ref["Período · a.hasta"], ref["Período · b.desde"], ref["Período · b.hasta"]),
      ("2026-09-01", "2026-09-30", "2026-10-01", "2026-10-31"))

# Instrucciones del agente (fuente única: EyaInsightsApi/instrucciones.txt): reglas de % > 100 y de listas cortadas
print("\n[instrucciones del agente]")
with open(os.path.join(os.path.dirname(os.path.abspath(__file__)), "..", "EyaInsightsApi", "instrucciones.txt"), encoding="utf-8") as _f:
    INSTR = _f.read()
igual("instrucciones: regla de porcentajes > 100 (no topar, explicar en una frase: conectados sin suscripción activa, como Power BI)",
      ("pasa de 100" in INSTR, "NO lo topes" in INSTR, "tengan o no suscripción activa" in INSTR, "igual que Power BI" in INSTR), (True, True, True, True))
igual("instrucciones: regla de listas cortadas (cuántas se muestran, cuántas hay, cuántas quedan fuera y por qué; sin «excluidas» falsas)",
      ("cuántas filas muestras" in INSTR, "cuántas existen en total" in INSTR, "POR QUÉ" in INSTR, "escuelas_sin_conectados" in INSTR, "NO se excluyen" in INSTR), (True,) * 5)

igual("instrucciones: reglas 5a (idioma de la pregunta) y 5b (solo la pregunta actual)",
      ("idioma de la PREGUNTA actual" in INSTR, "Idioma del panel" in INSTR, "SOLO LA PREGUNTA ACTUAL" in INSTR, "NUNCA retomes" in INSTR, "Idioma de la persona" in INSTR), (True, True, True, True, False))
igual("instrucciones: regla 2d del año académico por defecto (agosto a julio, decirlo y ofrecer cambiarlo)",
      ("{anio_academico_desde}" in INSTR, "agosto a julio" in INSTR, "SIEMPRE dilo" in INSTR, "ofrece cambiarlo" in INSTR), (True,) * 4)

# tipos de celda y tildes
_, _, hojas, _ = exportar("uso-por-area", **D)
datos = hojas["Datos"]
_, r = api("/uso-por-area", **D, limite=50)
igual("celdas numéricas son número (no texto) y los nombres con tilde se leen igual (Español, Matemáticas, Álgebra I)",
      (all(type(f[1]) in (int, float) and type(f[2]) in (int, float) for f in datos[1:]),
       {"Español", "Matemáticas", "Álgebra I"} <= {f[0] for f in datos[1:]}),
      (True, True))
igual("encabezados legibles en español", datos[0][:3], ["Nombre", "Pruebas asignadas", "Pruebas completadas"])

# gráfica: coherencia con el resumen y filtros
st, g = grafica("tendencia-conexiones", **D)
_, r = api("/tendencia-conexiones", **D)
igual("gráfica de tendencia: la suma de 'conexiones' en data = resumen.total_conexiones", sum(p["conexiones"] for p in g["data"]), r["resumen"]["total_conexiones"])
igual("gráfica de tendencia: título, ejes y hay_mas_filas", (g["titulo"], g["eje_x"]["titulo"], g["eje_y"]["unidad"], g["hay_mas_filas"]),
      ("Conexiones por mes", "Período", "conexiones", False))
# parámetros de la gráfica: todas, orden, incluir_ceros, limite (vista previa de 10 por defecto, sin ceros, mayor a menor)
_, rm = api("/conexiones-por-escuela", **D, limite=50)
col_esc, ser_esc = "nombre", ["conexiones"]
for nombre, params in (("vista previa (por defecto)", {}), ("todas", dict(todas="true")), ("orden=asc", dict(orden="asc")),
                       ("todas + incluir_ceros", dict(todas="true", incluir_ceros="true")), ("incluir_ceros (10 primeras)", dict(incluir_ceros="true")),
                       ("todas + asc + incluir_ceros", dict(todas="true", orden="asc", incluir_ceros="true")), ("limite=3", dict(limite=3)), ("limite=3 + asc", dict(limite=3, orden="asc"))):
    st, g = grafica("conexiones-por-escuela", **D, **params)
    esp, tot, cero = esperado_data_grafica("conexiones-por-escuela", rm["resultados"], ("bar", "vertical"), col_esc, ser_esc,
                                           todas=params.get("todas") == "true", orden=params.get("orden", "desc"), ceros=params.get("incluir_ceros") == "true", limite=params.get("limite", 10))
    igual(f"gráfica de escuelas, {nombre}: data = recálculo; mostradas/total/en cero; orden e incluir_ceros declarados",
          (st, g["data"], g["categorias_mostradas"], g["total_categorias"], g["categorias_en_cero"], g["orden"], g["incluir_ceros"], g["todas"]),
          (200, esp, len(esp), tot, cero, params.get("orden", "desc"), params.get("incluir_ceros") == "true", params.get("todas") == "true"))
st, g = grafica("conexiones-por-escuela", **D)
vals = [p["conexiones"] for p in g["data"]]
igual("gráfica de escuelas por defecto: a lo más 10, de mayor a menor, ninguna en 0 y total mayor que las mostradas",
      (len(vals) <= 10, vals == sorted(vals, reverse=True), all(v > 0 for v in vals), g["total_categorias"] >= g["categorias_mostradas"]), (True,) * 4)
st, g = grafica("conexiones-por-escuela", **D, todas="true", incluir_ceros="true", orden="asc")
igual("gráfica de escuelas todas + ceros + asc: aparecen las escuelas sin conexiones (en 0) y van primero", (g["data"][0]["conexiones"], g["total_categorias"] == len(rm["resultados"])), (0, True))
st, g = grafica("patrones-conexion", **D, orden="asc", incluir_ceros="false")
igual("gráfica de día de la semana: siempre los 7 días en su orden natural (no se ordena ni se filtra)", ([p["categoria"] for p in g["data"]], g["total_categorias"]),
      (["lunes", "martes", "miércoles", "jueves", "viernes", "sábado", "domingo"], 7))
for params, texto in ((dict(todas="quizas"), "'todas' debe ser true o false"), (dict(incluir_ceros="2"), "'incluir_ceros' debe ser true o false"), (dict(orden="zig"), "'orden' debe ser")):
    st, _, cuerpo = api_bin("/exportar/conexiones-por-escuela", formato="grafica", **D, **params)
    igual(f"gráfica con parámetro inválido {params} → 400", (st, texto in json.loads(cuerpo)["mensaje"]), (400, True))
st, _, cuerpo = api_bin("/exportar/conexiones-por-escuela", formato="grafica", **D, limite=5001)
igual("gráfica con limite=5001 → 400", st, 400)
st, g = grafica("conexiones-por-escuela", desde="2026-12-01", hasta="2026-12-31", min_conexiones=1)
igual("gráfica sin categorías (sin ceros y nada que mostrar) → 422 sin_grafica", (st, g.get("error")), (422, "sin_grafica"))
st, g = grafica("tendencia-conexiones", **D, todas="true", orden="asc")
igual("gráfica de líneas: orden natural del tiempo aunque pidan orden (sin campos de orden)", ([p["categoria"] for p in g["data"]] == sorted(p["categoria"] for p in g["data"]), "orden" in g, g["total_categorias"] == g["categorias_mostradas"]), (True, False, True))
st, g = grafica("participacion-escuelas", **D)
igual("gráfica de participación: barras agrupadas (activos y conectados lado a lado), el % en `extra` y datos hasta / cuentas de prueba",
      (g["tipo"], g.get("agrupar"), g["eje_y"]["unidad"], g["extra"], [s["clave"] for s in g["series"]], g["datos_hasta"], g["filtros"]["cuentas_de_prueba"]),
      ("bar", "grouped", "usuarios", {"clave": "porcentaje_participacion", "nombre": "% de participación", "unidad": "%"}, ["usuarios_activos", "usuarios_conectados"],
       api("/salud")[1]["datos_hasta_conexiones"], "excluidas" if EXCL else "incluidas"))
_, rp = api("/participacion-escuelas", **D, limite=50)
igual("gráfica de participación: ordenada por % de participación (el valor de la escuela con más % va primero y el % es el de la consulta)",
      (g["data"][0]["porcentaje_participacion"], g["data"][0]["porcentaje_participacion"] == max(f["porcentaje_participacion"] or 0 for f in rp["resultados"])), (g["data"][0]["porcentaje_participacion"], True))
st, gl = grafica("tendencia-conexiones", **D)
st, gs = grafica("conexiones-por-escuela", **D)
st, gc = grafica("conexiones-acumuladas", **D)
igual("especificación: `agrupar: grouped` solo en barras con varias series; `serie_temporal` solo en líneas; sin `extra` donde no hay otra escala",
      (g.get("agrupar"), gs.get("agrupar"), gl.get("serie_temporal"), gc.get("serie_temporal"), gs.get("serie_temporal"), gs.get("extra"), gl.get("agrupar")),
      ("grouped", None, True, True, None, None, None))
st, gu = grafica("uso-por-area", desde="2026-01-01", hasta="2026-11-10")
igual("especificación: uso-por-area (2 series) también va agrupada", (gu["tipo"], gu.get("agrupar"), gu.get("extra")), ("bar", "grouped", None))

# promedio-full-quiz (el esquema sintético ahora trae result_generic.total_percent_full_quiz): fuerza bruta por escuela y por materia
def esperado_promedio(agrupar, desde, hasta):
    a, b = rango(desde, hasta)
    grupos = {}
    pct = {}
    for u, m, asig, tom in RES:
        if ROL.get(u) != 3 or ESC_DE[u] is None or not visible(u) or not (a <= asig < b):
            continue
        clave = NOM_ESC[ESC_DE[u]] if agrupar == "escuela" else NOM_MAT.get(m, m)
        p = float(dp.porcentaje_quiz(u, asig, tom)) if tom is not None else None
        g = grupos.setdefault(clave, [0, 0, [], 0.0])
        g[0] += 1
        if tom is not None:
            g[1] += 1
            g[2].append(p)
            g[3] += p
    return {k: (g[0], g[1], round(sum(g[2]) / len(g[2]), 2) if g[2] else None, round(g[3] / g[0], 2)) for k, g in grupos.items()}


for agr, col in (("escuela", "escuela"), ("materia", "materia")):
    st, rq = api("/promedio-full-quiz", **D, agrupar=agr, limite=50)
    esp = esperado_promedio(agr, D["desde"], D["hasta"])
    obt = {f[col]: (f["pruebas_asignadas"], f["pruebas_completadas"], f.get("promedio_solo_realizadas"), f["promedio_todas_las_asignadas"]) for f in rq["resultados"]}
    cerca = lambda x, y: (x is None and y is None) or (x is not None and y is not None and abs(x - y) <= 0.011)
    igual(f"promedio-full-quiz por {agr}: asignadas, realizadas y promedios = fuerza bruta",
          (st, sorted(obt) == sorted(esp), all(obt[k][0] == esp[k][0] and obt[k][1] == esp[k][1] and cerca(obt[k][2], esp[k][2]) and cerca(obt[k][3], esp[k][3]) for k in esp if k in obt)), (200, True, True))
st, gq = grafica("promedio-full-quiz", **D, agrupar="escuela")
igual("gráfica de promedio-full-quiz: barras agrupadas (asignadas y completadas) con el promedio % en `extra`",
      (st, gq["tipo"], gq.get("agrupar"), [s["clave"] for s in gq["series"]], gq["extra"]["clave"], gq["extra"]["unidad"], all("promedio_solo_realizadas" in p for p in gq["data"])),
      (200, "bar", "grouped", ["pruebas_asignadas", "pruebas_completadas"], "promedio_solo_realizadas", "%", True))


# errores
st, _, _ = api_bin("/exportar/buscar-escuela", texto="prueba")
igual("exportar buscar-escuela (xlsx) → 404", st, 404)
st, _, _ = api_bin("/exportar/buscar-escuela", texto="prueba", formato="grafica")
igual("exportar buscar-escuela (gráfica) → 404", st, 404)
st, _, cuerpo = api_bin("/exportar/no-existe", **D)
igual("consulta desconocida → 404 consulta_desconocida", (st, json.loads(cuerpo)["error"]), (404, "consulta_desconocida"))
st, _, cuerpo = api_bin("/exportar/conexiones-por-maestro", formato="pdf", **D)
igual("formato=pdf → 400", (st, json.loads(cuerpo)["error"]), (400, "parametros_invalidos"))
for formato in ("xlsx", "grafica"):
    st, _, _ = api_bin("/exportar/conexiones-por-maestro", formato=formato, limite=0, **D)
    igual(f"limite=0 ({formato}) → 400", st, 400)
st, _, cuerpo = api_bin("/exportar/conexiones-por-maestro", limite=5001, **D)
igual("limite=5001 (xlsx) → 400 «entre 1 y 5000»", (st, "entre 1 y 5000" in json.loads(cuerpo)["mensaje"]), (400, True))
st, _, cuerpo = api_bin("/conexiones-por-maestro", limite=51, **D)
igual("la ruta JSON sigue limitada a 50 (limite=51 → 400 «entre 1 y 50»)", (st, "entre 1 y 50." in json.loads(cuerpo)["mensaje"]), (400, True))
for formato in ("xlsx", "grafica"):
    st, h, cuerpo = api_bin("/exportar/conexiones-por-maestro", formato=formato, escuela="Escuela Inexistente", **D)
    _, r = api("/conexiones-por-maestro", escuela="Escuela Inexistente", **D)
    igual(f"escuela inexistente ({formato}) → el mismo 400 que la ruta JSON", (st, json.loads(cuerpo)), (400, r))
st, _, cuerpo = api_bin("/exportar/conexiones-por-maestro", desde="2026-13-01", hasta="2026-11-10")
igual("fecha inválida → 400", st, 400)
st, _, cuerpo = api_bin("/exportar/participacion-escuelas", formato="grafica", escuela="Escuela Prueba B", **D)
igual("una sola fila → 422 sin_grafica", (st, json.loads(cuerpo)["error"]), (422, "sin_grafica"))
st, _, cuerpo = api_bin("/exportar/tendencia-conexiones", formato="grafica", desde="2026-10-01", hasta="2026-10-31")
igual("un solo período → 422 sin_grafica", (st, json.loads(cuerpo)["error"]), (422, "sin_grafica"))
st, h, _ = api_bin("/exportar/tendencia-conexiones", desde="2026-10-01", hasta="2026-10-31")
igual("un solo período sí se puede exportar a Excel (xlsx → 200)", st, 200)

# ───────────────────────── Fase 4: autenticación y alcance (A4) ─────────────────────────
if AUTH:
    import os
    import uuid
    print("\n[auth: Token de la plataforma, alcance por ORE, CORS y asistente]")
    USERS = os.environ.get("USERSAPI_FALSA", "http://127.0.0.1:5199")
    ORIGEN = os.environ.get("ORIGEN_FRONT", "https://front.ejemplo.invalid")
    CLAVE_SERVICIO = os.environ.get("EYA_API_KEY", "")
    ESC_ORE2 = {NOM_ESC[s] for s, _ in dp.ESCUELAS if dp.ORE_ID[s] == 2}          # escuelas del ORE 2 (admin-ore tok-ore2)
    ESC_ORE1 = {NOM_ESC[s] for s, _ in dp.ESCUELAS if dp.ORE_ID[s] == 1}
    JSON = "Content-Type"

    def http(metodo, ruta, token=None, extra=None, cuerpo=None, **q):
        """(estado, cabeceras en minúsculas, bytes) sin lanzar en 4xx/5xx."""
        h = dict(extra or {})
        if token:
            h["Token"] = token
        if cuerpo is not None:
            h[JSON] = "application/json"
        url = f"{BASE}{ruta}" + (f"?{urllib.parse.urlencode(q)}" if q else "")
        req = urllib.request.Request(url, data=json.dumps(cuerpo).encode() if cuerpo is not None else None, headers=h, method=metodo)
        try:
            with urllib.request.urlopen(req, timeout=60) as r:
                return r.status, {k.lower(): v for k, v in r.headers.items()}, r.read()
        except urllib.error.HTTPError as e:
            return e.code, {k.lower(): v for k, v in e.headers.items()}, e.read()

    def jget(ruta, token=None, **q):
        st, h, c = http("GET", ruta, token, None, None, **q)
        try:
            return st, json.loads(c)
        except ValueError:
            return st, {}

    def stub(metodo, ruta):
        with urllib.request.urlopen(urllib.request.Request(USERS + ruta, data=b"" if metodo == "POST" else None, method=metodo), timeout=10) as r:
            return json.loads(r.read())

    CON = dict(desde="2026-01-01", hasta="2026-11-10")

    # 1) quién entra
    st, r = jget("/conexiones-por-escuela", None, **CON)
    igual("sin Token → 401 no_autorizado", (st, r.get("error")), (401, "no_autorizado"))
    st, r = jget("/conexiones-por-escuela", "token-que-no-existe", **CON)
    igual("Token que UsersAPI rechaza → 401 no_autorizado", (st, r.get("error")), (401, "no_autorizado"))
    st, r = jget("/conexiones-por-escuela", "tok-teacher", **CON)
    igual("rol teacher (5) → 403 sin_permiso", (st, r.get("error")), (403, "sin_permiso"))
    st, r = jget("/conexiones-por-escuela", "tok-ore-sin-ore", **CON)
    igual("admin-ore SIN OreId → 403 (nunca «todo»)", (st, r.get("error")), (403, "sin_permiso"))
    st, r = jget("/conexiones-por-escuela", "tok-director-x", **CON)
    igual("director cuya escuela no se encuentra → 403", (st, r.get("error")), (403, "sin_permiso"))
    st, h, _ = http("GET", "/salud")
    igual("/salud sin Token → 200", st, 200)

    # 2) admin = lo de siempre (todas las escuelas)
    st, r = jget("/conexiones-por-escuela", "tok-admin", limite=50, **CON)
    esp = esperado_escuelas(CON["desde"], CON["hasta"], True, 50)
    igual("admin: conexiones-por-escuela = fuerza bruta de TODAS las escuelas y alcance «todas las escuelas»",
          (st, sin_derivados(r["resultados"], esp), r["alcance"]), (200, esp, "todas las escuelas"))

    # 3) admin-ore (ORE 2): solo sus escuelas, en todas las consultas
    def solo_ore2(nombre, filas, campo="nombre", esperado=None):
        nombres = [f[campo] for f in filas]
        igual(f"admin-ore: {nombre} → solo escuelas del ORE 2" + (" y = fuerza bruta" if esperado is not None else ""),
              (sorted(set(nombres) - ESC_ORE2), nombres if esperado is None else sin_derivados(filas, esperado)),
              ([], nombres if esperado is None else esperado))

    st, r = jget("/conexiones-por-escuela", "tok-ore2", limite=50, **CON)
    solo_ore2("conexiones-por-escuela", r["resultados"], esperado=[f for f in esperado_escuelas(CON["desde"], CON["hasta"], True, 5000) if f["nombre"] in ESC_ORE2])
    igual("admin-ore: alcance declarado y total = suma de sus escuelas", (r["alcance"], r["resumen"]["total_conexiones"]),
          ("ORE MUNICIPIO SEGUNDO", sum(f["conexiones"] for f in esperado_escuelas(CON["desde"], CON["hasta"], True, 5000) if f["nombre"] in ESC_ORE2)))
    st, r = jget("/conexiones-por-maestro", "tok-ore2", limite=50, **CON)
    solo_ore2("conexiones-por-maestro", r["resultados"], campo="escuela",
              esperado=[f for f in esperado_maestros(CON["desde"], CON["hasta"], True, 5000) if f["escuela"] in ESC_ORE2][:50])
    st, r = jget("/participacion-escuelas", "tok-ore2", limite=50, **CON)
    solo_ore2("participacion-escuelas", r["resultados"], esperado=[f for f in ordenar_participacion(esperado_participacion(CON["desde"], CON["hasta"]), True) if f["nombre"] in ESC_ORE2])
    st, r = jget("/uso-docentes", "tok-ore2", fecha="2026-10-01", limite=50)
    solo_ore2("uso-docentes", r["resultados"], campo="escuela")
    igual("admin-ore: uso-docentes no suma maestros de otras escuelas en el total",
          r["resumen"]["maestros"] if "maestros" in r.get("resumen", {}) else None,
          sum(f["maestros"] for f in r["resultados"]) if "maestros" in r.get("resumen", {}) else None)
    st, r = jget("/buscar-escuela", "tok-ore2", texto="Prueba", limite=50)
    solo_ore2("buscar-escuela (texto «Prueba»)", r["resultados"])
    st, r = jget("/uso-por-area", "tok-ore2", limite=50, **CON)
    igual("admin-ore: uso-por-area no cuenta pruebas de otros ORE (total ≤ el de todas las escuelas, y el ORE 2 tiene estudiantes con pruebas)",
          (st, 0 < r["resumen"]["total_pruebas_asignadas"] < jget("/uso-por-area", "tok-admin", limite=50, **CON)[1]["resumen"]["total_pruebas_asignadas"]), (200, True))
    st, rt = jget("/tendencia-conexiones", "tok-ore2", granularidad="mes", **CON)
    igual("admin-ore: tendencia suma lo mismo que conexiones-por-escuela de su ORE",
          sum(f["conexiones"] for f in rt["resultados"]), jget("/conexiones-por-escuela", "tok-ore2", limite=50, **CON)[1]["resumen"]["total_conexiones"])
    st, r = jget("/promedio-full-quiz", "tok-ore2", agrupar="escuela", limite=50, **CON)
    if st == 200:
        igual("admin-ore: promedio-full-quiz solo de sus escuelas", sorted({f["escuela"] for f in r["resultados"]} - ESC_ORE2), [])
    else:      # el esquema de prueba no trae result_generic.total_percent_full_quiz: la ruta da base_no_disponible y el alcance de esa consulta NO se prueba aquí
        print(f"  · promedio-full-quiz da {st} con el esquema de prueba (sin columna de % Full Quiz): su alcance no se prueba")
    for ruta in ("/patrones-conexion", "/conexiones-acumuladas"):
        st, r = jget(ruta, "tok-ore2", **CON)
        igual(f"admin-ore: {ruta[1:]} responde 200 con alcance de su ORE", (st, r.get("alcance")), (200, "ORE MUNICIPIO SEGUNDO"))
    # una escuela de OTRO ORE se trata como inexistente: mismo 400 (no se revela que existe)
    st, r = jget("/conexiones-por-maestro", "tok-ore2", escuela="Escuela Prueba A", **CON)
    st2, r2 = jget("/conexiones-por-maestro", "tok-ore2", escuela="Escuela Prueba Z", **CON)
    igual("escuela de OTRO ORE → 400 igual que una inexistente (mismo código y texto salvo el nombre)",
          (st, r["error"], r["mensaje"].replace("Escuela Prueba A", "X")), (st2, r2["error"], r2["mensaje"].replace("Escuela Prueba Z", "X")))
    # usuarios por suscripción respeta el alcance (ORE 2: solo usuarios de sus escuelas; director: solo su escuela)
    for tok, escs, etiqueta in (("tok-ore2", ESC_ORE2, "admin-ore (ORE 2)"), ("tok-director-1", {"Escuela Prueba A"}, "director")):
        st, r = jget("/usuarios-por-suscripcion", tok, agrupar="escuela", limite=50)
        esp_f, esp_t, _ = esperado_suscripcion("escuela", solo_escuelas=escs)
        igual(f"usuarios por suscripción de {etiqueta}: solo sus escuelas = recálculo", (st, sorted((f["nombre"], f["total_usuarios"]) for f in r["resultados"]), r["resumen"]["total_usuarios"]),
              (200, sorted((f[0], f[5]) for f in esp_f), sum(esp_t)))
    st, r = jget("/usuarios-por-suscripcion", "tok-ore2", agrupar="escuela", escuela="Escuela Prueba A")
    igual("usuarios por suscripción: escuela de otro ORE → 400 (como inexistente)", (st, r.get("error")), (400, "parametros_invalidos"))
    st, r = jget("/usuarios-por-suscripcion", None)
    igual("usuarios por suscripción sin Token → 401", st, 401)
    # ORE 1 no ve el ORE 2
    st, r = jget("/conexiones-por-escuela", "tok-ore1", limite=50, **CON)
    igual("admin-ore del ORE 1: nada de las escuelas del ORE 2", sorted({f["nombre"] for f in r["resultados"]} & ESC_ORE2), [])
    # director: solo su escuela, en todas las consultas con nombre de escuela
    st, r = jget("/conexiones-por-maestro", "tok-director-1", limite=50, **CON)
    igual("director (user 1): solo maestros de su escuela y alcance «escuela …»", (st, {f["escuela"] for f in r["resultados"]}, r["alcance"]),
          (200, {"Escuela Prueba A"}, "escuela Escuela Prueba A"))
    for ruta, campo, extra in (("/conexiones-por-escuela", "nombre", CON), ("/participacion-escuelas", "nombre", CON), ("/uso-docentes", "escuela", dict(fecha="2026-10-01")),
                               ("/buscar-escuela", "nombre", dict(texto="Prueba"))):
        st, r = jget(ruta, "tok-director-1", limite=50, **extra)
        igual(f"director: {ruta[1:]} solo trae su escuela", (st, sorted({f[campo] for f in r["resultados"]} - {"Escuela Prueba A"})), (200, []))
    st, r = jget("/conexiones-por-maestro", "tok-director-1", escuela="Escuela Prueba B", **CON)
    igual("director pide otra escuela → 400 «no existe»", (st, r.get("error")), (400, "parametros_invalidos"))

    # 4) exportaciones con alcance: xlsx y gráfica
    _, h, cuerpo = http("GET", "/exportar/conexiones-por-maestro", "tok-ore2", None, None, **CON)
    hojas, _ = leer_xlsx.leer(cuerpo, con_formulas=True)
    cab = hojas["Datos"][0]
    col = cab.index("Escuela") if "Escuela" in cab else 1
    igual("xlsx de admin-ore: ninguna fila de otro ORE, y la hoja Referencias dice el alcance",
          (sorted({f[col] for f in hojas["Datos"][1:] if f and f[col] is not None} - ESC_ORE2), tabla_referencias(hojas).get("Alcance")), ([], "ORE MUNICIPIO SEGUNDO"))
    st, g = jget("/exportar/conexiones-por-escuela", "tok-ore2", formato="grafica", **CON)
    igual("gráfica de admin-ore: solo categorías de su ORE", (st, sorted({d["categoria"] for d in g["data"]} - ESC_ORE2)), (200, []))
    st, _, cuerpo = http("GET", "/exportar/conexiones-por-maestro", "tok-ore2", None, None, escuela="Escuela Prueba A", **CON)
    igual("xlsx de una escuela de otro ORE → 400", st, 400)
    st, _, _ = http("GET", "/exportar/conexiones-por-maestro", None, None, None, **CON)
    igual("exportar sin Token → 401", st, 401)

    # 5) identidad de servicio
    if CLAVE_SERVICIO:
        st, _, _ = http("GET", "/conexiones-por-escuela", None, {"x-api-key": CLAVE_SERVICIO}, None, **CON)
        igual("x-api-key correcta sin Token → 200 (servicio, todas las escuelas)", st, 200)
        st, _, _ = http("GET", "/conexiones-por-escuela", None, {"x-api-key": CLAVE_SERVICIO + "x"}, None, **CON)
        igual("x-api-key incorrecta → 401", st, 401)
    else:
        print("  · (sin EYA_API_KEY en el entorno: no se prueba la identidad de servicio)")

    # 6) caché y falla cerrada
    nuevo = "tok-admin-" + uuid.uuid4().hex
    stub("POST", "/reiniciar")
    estados = [http("GET", "/conexiones-por-escuela", nuevo, None, None, limite=1, **CON)[0] for _ in range(5)]
    igual("caché: 5 peticiones con el mismo Token = 1 llamada a UsersAPI", (estados, stub("GET", "/llamadas")["llamadas"]), ([200] * 5, 1))
    stub("POST", "/caer")
    try:
        st, r = jget("/conexiones-por-escuela", "tok-admin-" + uuid.uuid4().hex, **CON)
    finally:
        stub("POST", "/levantar")
    igual("UsersAPI caída (Token sin caché) → 503 autenticacion_no_disponible", (st, r.get("error")), (503, "autenticacion_no_disponible"))
    st, r = jget("/conexiones-por-escuela", "tok-admin-" + uuid.uuid4().hex, **CON)
    igual("UsersAPI repuesta → vuelve a responder 200 (el 503 no se cachea)", st, 200)
    import time
    stub("POST", "/colgar")
    try:
        t0 = time.time()
        st, r = jget("/conexiones-por-escuela", "tok-admin-" + uuid.uuid4().hex, **CON)
        seg = time.time() - t0
    finally:
        stub("POST", "/levantar")
    igual("UsersAPI que no responde → 503 en ~5 s (timeout), no se queda colgada", (st, r.get("error"), 4 <= seg < 8), (503, "autenticacion_no_disponible", True))

    # 7) CORS
    st, h, _ = http("OPTIONS", "/exportar/uso-por-area", None, {"Origin": ORIGEN, "Access-Control-Request-Method": "GET", "Access-Control-Request-Headers": "token"})
    igual("preflight sin Token desde el origen del front → 204, permite el header token (no 401)",
          (st, h.get("access-control-allow-origin"), "token" in h.get("access-control-allow-headers", "").lower()), (204, ORIGEN, True))
    st, h, _ = http("OPTIONS", "/exportar/uso-por-area", None, {"Origin": "https://otro-origen.invalid", "Access-Control-Request-Method": "GET", "Access-Control-Request-Headers": "token"})
    igual("preflight desde otro origen → sin cabeceras CORS", h.get("access-control-allow-origin"), None)
    st, h, _ = http("GET", "/exportar/uso-por-area", "tok-admin", {"Origin": ORIGEN}, None, **CON)
    igual("GET desde el origen del front expone content-disposition", (st, "content-disposition" in h.get("access-control-expose-headers", "").lower()), (200, True))

    # 8) asistente (necesita foundry_falso.py y Asistente__Habilitado=true; si no, se omite)
    st, _, _ = http("POST", "/asistente/preguntar", "tok-ore2", None, {"pregunta": "hola"})
    if st == 404:
        print("  · (asistente no habilitado: no se prueban /asistente/*)")
    else:
        def pregunta(token, texto, conversacion=None, idioma=None):
            cuerpo = {"pregunta": texto}
            if conversacion:
                cuerpo["conversacion_id"] = conversacion
            if idioma:
                cuerpo["idioma"] = idioma
            st, _, c = http("POST", "/asistente/preguntar", token, None, cuerpo)
            return st, json.loads(c)

        igual("asistente sin Token → 401", http("POST", "/asistente/preguntar", None, None, {"pregunta": "hola"})[0], 401)
        igual("asistente con rol teacher → 403", http("POST", "/asistente/preguntar", "tok-teacher", None, {"pregunta": "hola"})[0], 403)
        st, r = pregunta("tok-ore2", "dame la lista de maestros en Excel")
        d = (r.get("descargas") or [{}])[0]
        igual("asistente (admin-ore): descargas[] con id, nombre y url, destacar=true si pidió Excel; referencia con el alcance",
              (st, len(r["descargas"]), d.get("url") == "/asistente/descargas/" + d.get("id", "?"), d.get("destacar"), r["referencias"][0].get("alcance")),
              (200, 1, True, True, "ORE MUNICIPIO SEGUNDO"))
        st, h, cuerpo = http("GET", d["url"], "tok-ore2")
        hojas, _ = leer_xlsx.leer(cuerpo, con_formulas=True)
        cab = hojas["Datos"][0]
        col = cab.index("Escuela") if "Escuela" in cab else 1
        igual("el enlace baja el .xlsx con el Token: content-disposition y solo filas de su ORE",
              (st, h.get("content-type"), d["nombre"] in h.get("content-disposition", ""), sorted({f[col] for f in hojas["Datos"][1:] if f and f[col]} - ESC_ORE2)),
              (200, TIPO_XLSX, True, []))
        igual("la descarga de otra persona (admin-ore del ORE 1) → 404 descarga_expirada, igual que un id inventado",
              (http("GET", d["url"], "tok-ore1")[0], http("GET", "/asistente/descargas/inventado", "tok-ore2")[0], json.loads(http("GET", d["url"], "tok-ore1")[2])["error"]),
              (404, 404, "descarga_expirada"))
        igual("la descarga sin Token → 401", http("GET", d["url"])[0], 401)
        # gráfica del chat: sale de /exportar?formato=grafica (ordenada, sin ceros, vista previa) y se puede ampliar por id (mismo dueño, mismo alcance)
        gr = (r.get("graficas") or [{}])[0]
        igual("asistente: graficas[] trae id, url de ampliación y los totales de categorías (vista previa)",
              (gr.get("id") == d.get("id"), gr.get("url") == "/asistente/graficas/" + d.get("id", "?"), gr.get("categorias_mostradas") == len(gr.get("data", [])),
               gr.get("total_categorias", 0) >= gr.get("categorias_mostradas", 1), gr.get("orden")), (True, True, True, True, "desc"))
        esp_maestros_ore2 = [f for f in esperado_maestros(CON["desde"], CON["hasta"], True, 5000) if f["escuela"] in ESC_ORE2]
        st, h, cuerpo = http("GET", gr["url"], "tok-ore2", None, None, todas="true", incluir_ceros="true", orden="asc")
        ga = json.loads(cuerpo)
        igual("asistente: GET /asistente/graficas/{id}?todas&incluir_ceros&orden=asc → TODOS los maestros de su ORE (solo los suyos), de menor a mayor",
              (st, ga["total_categorias"], ga["categorias_mostradas"], [p["conexiones"] for p in ga["data"]] == sorted(p["conexiones"] for p in ga["data"]), ga["orden"], ga["id"] == d["id"]),
              (200, len(esp_maestros_ore2), len(esp_maestros_ore2), True, "asc", True))
        igual("asistente: la gráfica ampliada de otra persona → 404; sin Token → 401; parámetro inválido → 400",
              (http("GET", gr["url"], "tok-ore1")[0], http("GET", gr["url"])[0], http("GET", gr["url"], "tok-ore2", None, None, todas="quizas")[0]), (404, 401, 400))
        st, r = pregunta("tok-ore2", "AJENA dime los maestros de la escuela A")
        igual("asistente: el admin-ore pregunta por una escuela de otro ORE → la consulta falla (400) y no recibe datos ni descargas",
              (st, [c["estado"] for c in r["consultas"]], r.get("descargas")), (200, [400], None))
        st, r = pregunta("tok-ore2", "DUP maestros")
        igual("asistente: dos consultas idénticas en el turno → una sola descarga (dedupe), destacar=false si no pidió Excel", (len(r["descargas"]), r["descargas"][0]["destacar"]), (1, False))
        st, r = pregunta("tok-ore2", "TENDENCIA grafica de conexiones por mes")
        igual("asistente: consulta graficable → graficas[] (formato Recharts) con destacar=true si pidió gráfica",
              (len(r["graficas"]), r["graficas"][0]["tipo"], r["graficas"][0]["destacar"]), (1, "line", True))
        st, r1 = pregunta("tok-ore2", "primera")
        st, r2 = pregunta("tok-ore1", "segunda", r1["conversacion_id"])
        st, r3 = pregunta("tok-ore2", "tercera", r1["conversacion_id"])
        with urllib.request.urlopen(os.environ.get("FOUNDRY_FALSO", "http://127.0.0.1:5198") + "/instrucciones", timeout=10) as _r:
            enviadas = json.loads(_r.read())["instructions"]
        TEXTO_FALSO = "Desde el 1 de enero hay conexiones en varias escuelas."
        for caso in ("DOBLE dos mensajes", "EN_LINEA referencias en medio"):
            st, r = pregunta("tok-ore2", caso)
            igual(f"asistente ({caso.split()[0]}): la respuesta sale UNA vez y sin «Referencias:» (van solo en referencias[])",
                  (st, r["respuesta"].count(TEXTO_FALSO), "Referencias" in r["respuesta"], "consulta=" in r["respuesta"], len(r["referencias"]) >= 1),
                  (200, 1, False, False, True))
        igual("asistente: la respuesta normal no se toca", pregunta("tok-ore2", "hola")[1]["respuesta"], "Listo.")
        igual("asistente: la API manda a Foundry las reglas del agente (porcentajes > 100 y listas cortadas)",
              ("4f." in enviadas and "pasa de 100" in enviadas, "4g." in enviadas and "POR QUÉ" in enviadas), (True, True))
        _hoy = (dt.datetime.utcnow() - dt.timedelta(hours=4)).date()           # el servicio lee el día en hora de Puerto Rico
        _ini = _hoy.year if _hoy.month >= 8 else _hoy.year - 1
        igual("asistente: año académico por defecto (agosto a julio) con las fechas calculadas y sin marcadores sin reemplazar",
              (f"del {_ini - 1}-08-01 al {_ini}-07-31" in enviadas, f"del {_ini}-08-01 a hoy" in enviadas, "{anio_academico" in enviadas), (True, True, False))
        def foundry():
            with urllib.request.urlopen(os.environ.get("FOUNDRY_FALSO", "http://127.0.0.1:5198") + "/instrucciones", timeout=10) as _r2:
                return json.loads(_r2.read())

        # idioma: el de la PREGUNTA manda; el del panel solo desempata y viaja siempre
        pregunta("tok-ore2", "How many users have an inactive subscription?")
        f_es = foundry()
        pregunta("tok-ore2", "How many users have an inactive subscription?", idioma="en")
        f_en = foundry()
        igual("idioma: el contexto lleva SIEMPRE el idioma del panel (español por defecto, inglés si idioma=en)",
              ("[Idioma del panel: español]" in f_es["entrada"], "[Idioma del panel: inglés]" in f_en["entrada"], "[Idioma del panel: español]" in f_en["entrada"]), (True, True, False))
        igual("idioma: la regla 5a manda responder en el idioma de la PREGUNTA (y el del panel solo si no es claro); ya no hay la regla vieja «español salvo…»",
              ("5a. IDIOMA" in f_en["instructions"], "idioma de la PREGUNTA actual" in f_en["instructions"], "Idioma del panel" in f_en["instructions"], "Idioma de la persona" in f_en["instructions"]), (True, True, True, False))

        # solo la pregunta actual: en una conversación que continúa, el mensaje va marcado como nuevo y NO se reenvía el texto anterior
        r_a = pregunta("tok-ore2", "PRIMERA indícame las escuelas visitadas y la última visita de cada una")[1]
        f_1 = foundry()
        r_b = pregunta("tok-ore2", "How many users have an inactive or canceled subscription?", r_a["conversacion_id"])[1]
        f_2 = foundry()
        marca = "[Mensaje nuevo: responde SOLO a esta pregunta"
        igual("pregunta actual: el primer mensaje no lleva marca; el siguiente de la conversación sí y no reenvía el texto del anterior (el historial lo pone previous_response_id)",
              (marca in f_1["entrada"], f_1["previo"] is None, marca in f_2["entrada"], "PRIMERA" in f_2["entrada"], f_2["previo"] is not None, r_b["conversacion_id"] == r_a["conversacion_id"]),
              (False, True, True, False, True, True))
        igual("pregunta actual: la regla 5b (solo la pregunta actual, no retomar pedidos anteriores) llega a Foundry",
              ("5b. SOLO LA PREGUNTA ACTUAL" in f_2["instructions"], "NUNCA retomes" in f_2["instructions"]), (True, True))

        igual("conversaciones: otra persona con tu conversacion_id empieza una nueva; tú continúas la tuya",
              (r2["conversacion_id"] != r1["conversacion_id"], r3["conversacion_id"] == r1["conversacion_id"]), (True, True))

print(f"\nResumen: {total - len(fallos)}/{total} comprobaciones coinciden.")
sys.exit(1 if fallos else 0)
