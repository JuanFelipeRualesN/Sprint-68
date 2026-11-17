"""Prueba local del agente (REQ-3741, paso 2): el modelo de Foundry recibe las 5 consultas
del openapi.json como herramientas, elige cuales llamar para cada pregunta del ticket y
responde con lo que devuelve la API de ejemplo.

Ojo: esto prueba la eleccion de consultas y la redaccion. NO prueba la herramienta OpenAPI
de Foundry (esa llama a la API desde Azure y necesita una URL publica).

Uso:  python3 probar_agente_local.py                       (API de ejemplo en Python, sin referencias)
      EYA_BASE_URL=http://127.0.0.1:5099 python3 probar_agente_local.py   (API .NET local: ejemplo o MySQL)
      ... --preguntas archivo.txt                          (una pregunta por línea, en vez de las 7 del ticket)

Al final de cada pregunta corre un VERIFICADOR: toda cifra de la respuesta debe existir en lo que devolvió
la API (o en la pregunta) y el bloque "Referencias" debe traer el período realmente consultado.

Requisitos: az login hecho, azure-ai-projects y azure-identity instalados.
"""
import json
import os
import re
import sys
import threading
import urllib.error
import urllib.parse
import urllib.request

from azure.ai.projects import AIProjectClient
from azure.identity import DefaultAzureCredential

import api_ejemplo
from instrucciones import instrucciones

ENDPOINT = os.environ.get(
    "FOUNDRY_PROJECT_ENDPOINT",
    "https://chatbotaipowerbi.services.ai.azure.com/api/projects/proj-default",
)
MODELO = os.environ.get("FOUNDRY_MODEL_DEPLOYMENT_NAME", "gpt-4.1-mini")
BASE = os.environ.get("EYA_BASE_URL", "http://127.0.0.1:8787").rstrip("/")
CLAVE = os.environ.get("EYA_API_KEY", "")

INSTRUCCIONES = instrucciones()

PREGUNTAS = [
    "¿Cuál fue el área más utilizada entre el 1 de septiembre y el 10 de noviembre de 2026?",
    "¿Cuáles fueron las áreas menos utilizadas entre el 1 de septiembre y el 10 de noviembre de 2026?",
    "¿Cuáles fueron las escuelas con mayor cantidad de conexiones en lo corrido de 2026?",
    "¿Cuáles maestros registraron la mayor cantidad de conexiones durante el año 2026?",
    "¿Cómo se compara el uso de las áreas entre septiembre y octubre de 2026?",
    "¿Cuáles fueron las tendencias de uso por trimestre en 2026?",
    "¿Qué escuelas presentaron menor actividad entre el 1 de septiembre y el 10 de noviembre de 2026?",
]


def herramientas_desde_spec(spec):
    herramientas, rutas = [], {}
    for ruta, metodos in spec["paths"].items():
        op = metodos["get"]
        props, requeridos = {}, []
        for p in op["parameters"]:
            if "$ref" in p:
                p = spec["components"]["parameters"][p["$ref"].split("/")[-1]]
            esquema = {k: v for k, v in p["schema"].items() if k in ("type", "enum")}
            esquema["description"] = p["description"]
            props[p["name"]] = esquema
            if p.get("required"):
                requeridos.append(p["name"])
        herramientas.append({
            "type": "function",
            "name": op["operationId"],
            "description": f'{op["summary"]}. {op["description"]}',
            "parameters": {"type": "object", "properties": props, "required": requeridos},
        })
        rutas[op["operationId"]] = ruta
    return herramientas, rutas


def llamar_api(ruta, argumentos):
    url = f"{BASE}{ruta}?{urllib.parse.urlencode(argumentos)}"
    try:
        req = urllib.request.Request(url, headers={"x-api-key": CLAVE} if CLAVE else {})
        with urllib.request.urlopen(req, timeout=20) as r:
            return r.read().decode("utf-8")
    except urllib.error.HTTPError as e:
        return e.read().decode("utf-8")


def preguntar(openai, herramientas, rutas, pregunta):
    llamadas, salidas_api = [], []
    resp = openai.responses.create(model=MODELO, instructions=INSTRUCCIONES, input=pregunta,
                                   tools=herramientas)
    for _ in range(5):
        pendientes = [o for o in resp.output if o.type == "function_call"]
        if not pendientes:
            break
        salidas = []
        for o in pendientes:
            args = json.loads(o.arguments or "{}")
            llamadas.append((o.name, args))
            salida = llamar_api(rutas[o.name], args)
            salidas_api.append(salida)
            salidas.append({"type": "function_call_output", "call_id": o.call_id, "output": salida})
        resp = openai.responses.create(model=MODELO, instructions=INSTRUCCIONES, input=salidas,
                                       previous_response_id=resp.id, tools=herramientas)
    return llamadas, resp.output_text, salidas_api


def _normalizar(token):
    return re.sub(r"\D", "", token).lstrip("0") or "0"   # 1.834 -> 1834 ; 12,5 -> 125 ; 09 -> 9 (mismo criterio en ambos lados)


def _componentes_fecha(texto):
    """Año, mes y día de cada fecha ISO del texto: la respuesta puede escribirlos en prosa ('19 de octubre de 2026')."""
    return {_normalizar(x) for m in re.findall(r"(\d{4})-(\d{2})-(\d{2})", texto) for x in m}


def _cifras(texto):
    texto = re.sub(r"\d{4}-\d{2}-\d{2}(T[\d:.]+Z?)?", " ", texto)      # fechas ISO: se verifican aparte (referencias)
    return {_normalizar(x) for x in re.findall(r"\d+(?:[.,]\d+)*", texto)} - {""}


def _plano(s):
    return "".join(c for c in __import__("unicodedata").normalize("NFD", s.lower()) if not __import__("unicodedata").combining(c))


def _enfocada_en_una_fila(pregunta, salidas_api):
    q = _plano(pregunta)
    for s in salidas_api:
        try:
            filas = json.loads(s).get("resultados")
        except (ValueError, AttributeError):
            continue
        if isinstance(filas, list) and any(isinstance(f, dict) and "nombre" in f and _plano(f["nombre"]) in q for f in filas):
            return True
    return False


def _filas_omitidas(texto, salidas_api):
    """Nombres de filas devueltas por la API (listas cortas) que la respuesta no menciona."""
    omitidas, cuerpo = [], _plano(texto)
    for s in salidas_api:
        try:
            filas = json.loads(s).get("resultados")
        except (ValueError, AttributeError):
            continue
        if isinstance(filas, list) and 1 < len(filas) <= 15:
            omitidas += [f["nombre"] for f in filas if isinstance(f, dict) and "nombre" in f and _plano(f["nombre"]) not in cuerpo]
    return omitidas


def verificar(pregunta, llamadas, texto, salidas_api):
    """Devuelve la lista de problemas (vacía = pasa). Son señales para revisar, no un veredicto final."""
    problemas = []
    if not llamadas:
        # Sin consulta: solo es válido si pide un dato o dice que no puede; no debe traer cifras nuevas.
        extra = _cifras(texto) - _cifras(pregunta)
        if extra:
            problemas.append(f"respondió con cifras sin llamar a ninguna consulta: {sorted(extra)[:6]}")
        return problemas
    base = _cifras(pregunta) | _componentes_fecha(pregunta)
    for s in salidas_api:
        base |= _cifras(s) | _componentes_fecha(s)
    for a in llamadas:
        base |= _cifras(json.dumps(a[1], ensure_ascii=False)) | _componentes_fecha(json.dumps(a[1]))
    cuerpo = re.sub(r"(?m)^\s*\d+[.)]\s+", "", texto.split("Referencias:")[0])   # sin numeración de listas
    inventadas = _cifras(cuerpo) - base
    if inventadas:
        problemas.append(f"cifras que NO salen de la API (¿cálculo propio o invento?): {sorted(inventadas)[:8]}")
    omitidas = _filas_omitidas(texto, salidas_api)
    if omitidas and _enfocada_en_una_fila(pregunta, salidas_api):
        omitidas = []          # la pregunta nombra una fila concreta (p. ej. «Matemáticas»): responder solo esa es correcto
    if omitidas and not re.search(r"omit|no incluy|resto|otras? \d+|entre otras|además de", texto.lower()):
        problemas.append(f"omitió {len(omitidas)} fila(s) devuelta(s) sin avisar: {omitidas[:4]}")
    if all(n == "buscar_escuela" for n, _ in llamadas):
        return problemas                                 # solo búsqueda de escuela: no hay datos que referenciar
    if "Referencias:" not in texto:
        problemas.append("falta el bloque 'Referencias:'")
    else:
        ref = texto.split("Referencias:", 1)[1]
        for _, args in llamadas:
            for k, v in args.items():
                if k.startswith(("desde", "hasta")) and isinstance(v, str) and v not in ref:
                    problemas.append(f"las referencias no citan {k}={v}")
    if any('"error"' in s for s in salidas_api) and "error" not in texto.lower() and "no pude" not in texto.lower():
        problemas.append("la API devolvió error y la respuesta no lo menciona")
    return problemas


def main():
    aqui = os.path.dirname(os.path.abspath(__file__))
    spec = json.load(open(os.path.join(aqui, "openapi.json"), encoding="utf-8"))
    herramientas, rutas = herramientas_desde_spec(spec)

    servidor = None
    if "EYA_BASE_URL" not in os.environ:               # sin URL: levanta la API de ejemplo en Python
        servidor = api_ejemplo.servir()
        threading.Thread(target=servidor.serve_forever, daemon=True).start()
    preguntas = PREGUNTAS
    if "--preguntas" in sys.argv:
        ruta = sys.argv[sys.argv.index("--preguntas") + 1]
        preguntas = [l.strip() for l in open(ruta, encoding="utf-8") if l.strip() and not l.startswith("#")]
    fallos = 0

    project = AIProjectClient(endpoint=ENDPOINT, credential=DefaultAzureCredential())
    with project.get_openai_client() as openai:
        for i, pregunta in enumerate(preguntas, 1):
            print(f"\n=== {i}. {pregunta}")
            try:
                llamadas, texto, salidas_api = preguntar(openai, herramientas, rutas, pregunta)
            except Exception as e:                      # p. ej. filtro de contenido/jailbreak de Azure (400)
                motivo = "bloqueada por el filtro de contenido de Azure (jailbreak)" if "content_filter" in str(e) else str(e)[:200]
                print(f"  -> la pregunta fue {motivo}. El servicio real debe mostrar un mensaje amable ante este error.")
                continue
            for nombre, args in llamadas:
                print(f"  -> {nombre}({json.dumps(args, ensure_ascii=False)})")
            if not llamadas:
                print("  -> (no llamó ninguna consulta)")
            print(f"  Respuesta: {texto}")
            problemas = verificar(pregunta, llamadas, texto, salidas_api)
            fallos += bool(problemas)
            print("  VERIFICADOR: " + ("OK" if not problemas else "REVISAR -> " + " | ".join(problemas)))
    print(f"\nResumen: {len(preguntas) - fallos}/{len(preguntas)} sin observaciones.")
    if servidor:
        servidor.shutdown()
    return 1 if fallos else 0


if __name__ == "__main__":
    sys.exit(main())
