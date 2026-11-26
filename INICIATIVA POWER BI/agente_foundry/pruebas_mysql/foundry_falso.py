"""Azure AI Foundry FALSO para probar el asistente de EyaInsightsApi sin modelo real (REQ-3741 · Fase 4 · A3/A4). Solo biblioteca estándar.

Imita POST /openai/v1/responses (Responses API): según una palabra clave de la pregunta pide una herramienta (function_call) y, cuando
recibe su resultado, contesta «Listo.». NO prueba la calidad del modelo ni la autenticación real con Foundry (identidad administrada / az login).

Palabras clave:  AJENA  → conexiones_por_maestro de «Escuela Prueba A» (una escuela del ORE 1)
                 TENDENCIA → tendencia_conexiones por mes
                 DUP   → conexiones_por_maestro DOS veces con los mismos argumentos (prueba la deduplicación)
                 DOBLE → como (otra) pero la respuesta final trae DOS mensajes idénticos, cada uno con su «Referencias: …» (caso real: texto repetido y referencias filtradas)
                 EN_LINEA → respuesta final de UN mensaje con «Referencias: …» pegado en medio del texto y el texto repetido después
                 REF_EN → UN mensaje en inglés que termina con «References: consulta=…» (sin negrita), REF_NEGRITA → «**References:**» + salto de línea + el bloque,
                 REF_FUENTES → «Sources: consulta=…», REF_FUENTES_ES → «**Fuentes:** consulta=…», REF_SIN_ROTULO → el patrón consulta=…; fuente=… sin rótulo, REF_MEDIO_EN → «References:» en medio de una frase en inglés
                 SUSCRIPCION → usuarios_por_suscripcion (total); con REF_EN etc. prueba que la gráfica (dona) llega también cuando se responde en inglés
                 (otra) → conexiones_por_maestro de todas las escuelas, limite 5
GET /instrucciones devuelve las `instructions` y la `entrada` (texto de la pregunta) de la última petición con pregunta nueva (para comprobar que la API manda las reglas del agente).
Uso:  python3 foundry_falso.py [puerto]    (por defecto 5198)
      y arrancar la API con  Foundry__Endpoint=http://127.0.0.1:5198/api/projects/p  Asistente__Habilitado=true  Asistente__Credencial=prueba
"""
import json
import os
import sys
import uuid
from http.server import BaseHTTPRequestHandler, ThreadingHTTPServer

PERIODO = {"desde": "2026-01-01", "hasta": "2026-11-10"}


def llamadas_para(pregunta):
    if "SUSCRIPCION" in pregunta:
        return [("usuarios_por_suscripcion", {})]
    if "AJENA" in pregunta:
        return [("conexiones_por_maestro", {**PERIODO, "escuela": "Escuela Prueba A", "limite": 5})]
    if "TENDENCIA" in pregunta:
        return [("tendencia_conexiones", {**PERIODO, "granularidad": "mes"})]
    if "DUP" in pregunta:
        a = ("conexiones_por_maestro", {**PERIODO, "limite": 5})
        return [a, a]
    return [("conexiones_por_maestro", {**PERIODO, "limite": 5})]


ULTIMAS = {"instructions": "", "entrada": "", "previo": None, "cabeceras": {}}
CLAVE_EXIGIDA = os.environ.get("FOUNDRY_FALSO_CLAVE")     # si está, exige la cabecera api-key con ese valor; si no, 401 con un cuerpo que REPITE la clave recibida (prueba que la API no la filtra)
PREGUNTAS = {}      # id de respuesta → pregunta (la segunda llamada solo trae previous_response_id)
REFS = ("Referencias: consulta=conexiones_por_escuela; fuente=mysql:gs_data_analysis; fecha filtrada=connected_date.connected_date; "
        "período=2026-01-01 a 2026-11-19; escuela=todas; cuentas de prueba=excluidas; orden/limite=asc/50; datos hasta=2026-11-18; filas=47; generado=2026-11-19T17:00:58.549583Z.")
TEXTO = "Desde el 1 de enero hay conexiones en varias escuelas."


TEXTO_EN = "There are 110 active users, 18 canceled and 8 without a subscription."
REFS_SIN = REFS.replace("Referencias: ", "")
VARIANTES_REF = {      # palabra clave → texto final del modelo
    "REF_EN": f"{TEXTO_EN}\n\n{REFS.replace('Referencias:', 'References:')}",
    "REF_NEGRITA": f"{TEXTO_EN}\n\n**References:**\n{REFS_SIN}",
    "REF_FUENTES": f"{TEXTO_EN}\n\nSources: {REFS_SIN}",
    "REF_FUENTES_ES": f"{TEXTO}\n\n**Fuentes:** {REFS_SIN}",
    "REF_SIN_ROTULO": f"{TEXTO_EN}\n\n{REFS_SIN}",
    "REF_MEDIO_EN": f"{TEXTO_EN} References: {REFS_SIN} Ask me if you want a breakdown by school.",
    "REF_ES_NEGRITA": f"{TEXTO}\n\n**Referencias:** {REFS_SIN}",
}


def mensaje(texto):
    return {"type": "message", "content": [{"type": "output_text", "text": texto}]}


class H(BaseHTTPRequestHandler):
    def do_GET(self):
        cuerpo = json.dumps({"instructions": ULTIMAS["instructions"], "entrada": ULTIMAS["entrada"], "previo": ULTIMAS["previo"], "cabeceras": ULTIMAS["cabeceras"]}).encode()
        self.send_response(200)
        self.send_header("Content-Type", "application/json")
        self.send_header("Content-Length", str(len(cuerpo)))
        self.end_headers()
        self.wfile.write(cuerpo)

    def do_POST(self):
        n = int(self.headers.get("Content-Length") or 0)
        d = json.loads(self.rfile.read(n) or b"{}")
        ULTIMAS["cabeceras"] = {"api_key": self.headers.get("api-key"), "authorization": self.headers.get("Authorization"), "ruta": self.path, "modelo": d.get("model")}
        if CLAVE_EXIGIDA is not None and self.headers.get("api-key") != CLAVE_EXIGIDA:
            cuerpo = json.dumps({"error": {"code": "401", "message": f"Access denied due to invalid subscription key: {self.headers.get('api-key')}"}}).encode()
            self.send_response(401)
            self.send_header("Content-Type", "application/json")
            self.send_header("Content-Length", str(len(cuerpo)))
            self.end_headers()
            self.wfile.write(cuerpo)
            return
        ULTIMAS["instructions"] = d.get("instructions") or ""
        entrada = d.get("input")
        rid = "resp_" + uuid.uuid4().hex
        if isinstance(entrada, str):
            ULTIMAS["entrada"], ULTIMAS["previo"] = entrada, d.get("previous_response_id")
            PREGUNTAS[rid] = entrada
            salida = [{"type": "function_call", "name": nombre, "arguments": json.dumps(args), "call_id": f"c{i}"}
                      for i, (nombre, args) in enumerate(llamadas_para(entrada))]
        else:
            pregunta = PREGUNTAS.get(d.get("previous_response_id"), "")
            PREGUNTAS[rid] = pregunta
            variante = next((v for k, v in sorted(VARIANTES_REF.items(), key=lambda kv: -len(kv[0])) if k in pregunta), None)
            if variante:
                salida = [mensaje(variante)]
            elif "DOBLE" in pregunta:
                salida = [mensaje(f"{TEXTO}\n\n{REFS}"), mensaje(f"{TEXTO}\n\n{REFS}")]
            elif "EN_LINEA" in pregunta:
                salida = [mensaje(f"{TEXTO} {REFS} {TEXTO}")]
            else:
                salida = [mensaje("Listo.")]
        cuerpo = json.dumps({"id": rid, "output": salida}).encode()
        self.send_response(200)
        self.send_header("Content-Type", "application/json")
        self.send_header("Content-Length", str(len(cuerpo)))
        self.end_headers()
        self.wfile.write(cuerpo)

    def log_message(self, *a):
        pass


if __name__ == "__main__":
    ThreadingHTTPServer(("127.0.0.1", int(sys.argv[1]) if len(sys.argv) > 1 else 5198), H).serve_forever()
