"""API de consultas de EJEMPLO para el agente de Foundry (REQ-3741, paso 2).

Todos los datos son inventados y se generan con una semilla fija: sirven para probar
que el agente elige bien la consulta, no para dar cifras reales. Cada respuesta lleva
fuente = "datos_de_ejemplo". Solo usa la libreria estandar de Python.

Uso:  python3 api_ejemplo.py            (escucha en http://127.0.0.1:8787)
      EYA_API_KEY=clave python3 api_ejemplo.py   (exige el header x-api-key)
"""
import json
import os
import random
from datetime import date, datetime, timedelta
from http.server import BaseHTTPRequestHandler, ThreadingHTTPServer
from urllib.parse import parse_qs, urlparse

FUENTE = "datos_de_ejemplo"
HOY = date(2026, 11, 11)

# "Área" todavía no está definida en el ticket: aquí se usa área = materia (supuesto).
AREAS = ["Matemáticas", "Lectura", "Ciencias", "Estudios Sociales", "Inglés"]
ESCUELAS = [f"Escuela Ejemplo {c}" for c in "ABCDEFGHIJKL"]   # K y L no tienen actividad
ESCUELAS_ACTIVAS = ESCUELAS[:10]
MAESTROS = [(f"Maestro Ejemplo {i + 1:02d}", ESCUELAS[i % 12]) for i in range(36)]


def _generar():
    rnd = random.Random(2026)
    conexiones, pruebas = [], []
    dia = date(2026, 1, 12)
    while dia <= date(2026, 11, 10):
        if dia.weekday() < 5:
            for nombre, escuela in MAESTROS:
                if escuela in ESCUELAS_ACTIVAS and rnd.random() < 0.35:
                    conexiones.append((dia, escuela, nombre, rnd.randint(1, 3)))
            for escuela in ESCUELAS_ACTIVAS:
                for area in AREAS:
                    asignadas = rnd.randint(0, 12)
                    peso = 1.0 + 0.4 * AREAS.index(area)
                    completadas = min(asignadas, int(asignadas * rnd.random() * peso * 0.6))
                    pruebas.append((dia, area, escuela, asignadas, completadas))
        dia += timedelta(days=1)
    return conexiones, pruebas


CONEXIONES, PRUEBAS = _generar()


class ErrorConsulta(Exception):
    pass


def _fecha(params, nombre):
    valor = params.get(nombre)
    if not valor:
        raise ErrorConsulta(f"Falta el parámetro '{nombre}' (formato AAAA-MM-DD).")
    try:
        return datetime.strptime(valor, "%Y-%m-%d").date()
    except ValueError:
        raise ErrorConsulta(f"'{nombre}' debe tener el formato AAAA-MM-DD.")


def _rango(params, desde="desde", hasta="hasta"):
    d, h = _fecha(params, desde), _fecha(params, hasta)
    if d > h:
        raise ErrorConsulta(f"'{desde}' no puede ser posterior a '{hasta}'.")
    return d, h


def _orden_limite(params):
    orden = params.get("orden", "desc")
    if orden not in ("asc", "desc"):
        raise ErrorConsulta("'orden' debe ser 'asc' o 'desc'.")
    try:
        limite = int(params.get("limite", 10))
    except ValueError:
        raise ErrorConsulta("'limite' debe ser un número entero.")
    if not 1 <= limite <= 50:
        raise ErrorConsulta("'limite' debe estar entre 1 y 50.")
    return orden, limite


def _ordenar(filas, clave, orden, limite):
    filas = sorted(filas, key=lambda f: (f[clave], f["nombre"]), reverse=(orden == "desc"))
    return filas[:limite]


def _envoltorio(periodo, filtros, resultados, nota=None):
    r = {"fuente": FUENTE, "periodo": periodo, "filtros": filtros, "resultados": resultados}
    if nota:
        r["nota"] = nota
    return r


def uso_por_area(p):
    d, h = _rango(p)
    orden, limite = _orden_limite(p)
    acc = {a: [0, 0] for a in AREAS}
    for dia, area, _esc, asig, comp in PRUEBAS:
        if d <= dia <= h:
            acc[area][0] += asig
            acc[area][1] += comp
    filas = [{"nombre": a, "pruebas_asignadas": v[0], "pruebas_completadas": v[1]} for a, v in acc.items()]
    filas = _ordenar(filas, "pruebas_completadas", orden, limite)
    return _envoltorio({"desde": str(d), "hasta": str(h)}, {"orden": orden, "limite": limite}, filas,
                       "Uso = pruebas completadas; «área» se tomó como materia (definición pendiente).")


def conexiones_por_escuela(p):
    d, h = _rango(p)
    orden, limite = _orden_limite(p)
    acc = {e: 0 for e in ESCUELAS}          # incluye escuelas con cero conexiones
    for dia, esc, _m, n in CONEXIONES:
        if d <= dia <= h:
            acc[esc] += n
    filas = [{"nombre": e, "conexiones": c} for e, c in acc.items()]
    return _envoltorio({"desde": str(d), "hasta": str(h)}, {"orden": orden, "limite": limite},
                       _ordenar(filas, "conexiones", orden, limite))


def conexiones_por_maestro(p):
    d, h = _rango(p)
    orden, limite = _orden_limite(p)
    escuela = p.get("escuela")
    if escuela and escuela not in ESCUELAS:
        raise ErrorConsulta(f"La escuela '{escuela}' no existe.")
    acc = {m: [esc, 0] for m, esc in MAESTROS if not escuela or esc == escuela}
    for dia, esc, m, n in CONEXIONES:
        if d <= dia <= h and m in acc:
            acc[m][1] += n
    filas = [{"nombre": m, "escuela": v[0], "conexiones": v[1]} for m, v in acc.items()]
    filtros = {"orden": orden, "limite": limite}
    if escuela:
        filtros["escuela"] = escuela
    return _envoltorio({"desde": str(d), "hasta": str(h)}, filtros, _ordenar(filas, "conexiones", orden, limite))


def comparar_areas(p):
    da, ha = _rango(p, "desde_a", "hasta_a")
    db, hb = _rango(p, "desde_b", "hasta_b")

    def total(d, h):
        acc = {a: 0 for a in AREAS}
        for dia, area, _e, _asig, comp in PRUEBAS:
            if d <= dia <= h:
                acc[area] += comp
        return acc

    a, b = total(da, ha), total(db, hb)
    filas = []
    for area in AREAS:
        dif = b[area] - a[area]
        var = round(100.0 * dif / a[area], 1) if a[area] else None
        filas.append({"nombre": area, "pruebas_completadas_a": a[area], "pruebas_completadas_b": b[area],
                      "diferencia": dif, "variacion_pct": var})
    return _envoltorio({"a": {"desde": str(da), "hasta": str(ha)}, "b": {"desde": str(db), "hasta": str(hb)}}, {},
                       filas, "Si los dos periodos tienen distinta duración, la comparación no está normalizada.")


def _clave_periodo(dia, gran):
    if gran == "mes":
        return f"{dia.year}-{dia.month:02d}"
    if gran == "trimestre":
        return f"{dia.year}-T{(dia.month - 1) // 3 + 1}"
    return str(dia.year)


def tendencia_conexiones(p):
    d, h = _rango(p)
    gran = p.get("granularidad", "mes")
    if gran not in ("mes", "trimestre", "anio"):
        raise ErrorConsulta("'granularidad' debe ser 'mes', 'trimestre' o 'anio'.")
    escuela = p.get("escuela")
    if escuela and escuela not in ESCUELAS:
        raise ErrorConsulta(f"La escuela '{escuela}' no existe.")
    acc, dia = {}, d
    while dia <= h:                          # claves de todos los periodos del rango, aunque den cero
        acc.setdefault(_clave_periodo(dia, gran), 0)
        dia += timedelta(days=1)
    for dia, esc, _m, n in CONEXIONES:
        if d <= dia <= h and (not escuela or esc == escuela):
            acc[_clave_periodo(dia, gran)] += n
    filas = [{"periodo": k, "conexiones": v} for k, v in acc.items()]
    filtros = {"granularidad": gran}
    if escuela:
        filtros["escuela"] = escuela
    return _envoltorio({"desde": str(d), "hasta": str(h)}, filtros, filas)


RUTAS = {
    "/uso-por-area": uso_por_area,
    "/conexiones-por-escuela": conexiones_por_escuela,
    "/conexiones-por-maestro": conexiones_por_maestro,
    "/comparar-areas": comparar_areas,
    "/tendencia-conexiones": tendencia_conexiones,
}


class Manejador(BaseHTTPRequestHandler):
    def _responder(self, codigo, cuerpo):
        datos = json.dumps(cuerpo, ensure_ascii=False).encode("utf-8")
        self.send_response(codigo)
        self.send_header("Content-Type", "application/json; charset=utf-8")
        self.send_header("Content-Length", str(len(datos)))
        self.end_headers()
        self.wfile.write(datos)

    def do_GET(self):
        url = urlparse(self.path)
        clave = os.environ.get("EYA_API_KEY")
        if clave and url.path != "/salud" and self.headers.get("x-api-key") != clave:
            return self._responder(401, {"error": "no_autorizado", "mensaje": "Falta o es incorrecto x-api-key."})
        if url.path == "/salud":
            return self._responder(200, {"estado": "ok", "fuente": FUENTE})
        funcion = RUTAS.get(url.path)
        if not funcion:
            return self._responder(404, {"error": "no_encontrado", "mensaje": "Consulta inexistente."})
        params = {k: v[0] for k, v in parse_qs(url.query).items()}
        try:
            self._responder(200, funcion(params))
        except ErrorConsulta as e:
            self._responder(400, {"error": "parametros_invalidos", "mensaje": str(e)})

    def log_message(self, *args):
        pass


def servir(puerto=8787, host="127.0.0.1"):
    servidor = ThreadingHTTPServer((host, puerto), Manejador)
    return servidor


if __name__ == "__main__":
    # En Azure se define EYA_HOST=0.0.0.0 y PORT; en local quedan 127.0.0.1:8787.
    host = os.environ.get("EYA_HOST", "127.0.0.1")
    puerto = int(os.environ.get("PORT", "8787"))
    print(f"API de ejemplo en http://{host}:{puerto} (datos inventados). Ctrl+C para salir.", flush=True)
    servir(puerto, host).serve_forever()
