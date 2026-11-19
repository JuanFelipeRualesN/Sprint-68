"""Azure AI Foundry FALSO para probar el asistente de EyaInsightsApi sin modelo real (REQ-3741 · Fase 4 · A3/A4). Solo biblioteca estándar.

Imita POST /openai/v1/responses (Responses API): según una palabra clave de la pregunta pide una herramienta (function_call) y, cuando
recibe su resultado, contesta «Listo.». NO prueba la calidad del modelo ni la autenticación real con Foundry (identidad administrada / az login).

Palabras clave:  AJENA  → conexiones_por_maestro de «Escuela Prueba A» (una escuela del ORE 1)
                 TENDENCIA → tendencia_conexiones por mes
                 DUP   → conexiones_por_maestro DOS veces con los mismos argumentos (prueba la deduplicación)
                 (otra) → conexiones_por_maestro de todas las escuelas, limite 5
Uso:  python3 foundry_falso.py [puerto]    (por defecto 5198)
      y arrancar la API con  Foundry__Endpoint=http://127.0.0.1:5198/api/projects/p  Asistente__Habilitado=true  Asistente__Credencial=prueba
"""
import json
import sys
import uuid
from http.server import BaseHTTPRequestHandler, ThreadingHTTPServer

PERIODO = {"desde": "2026-01-01", "hasta": "2026-11-10"}


def llamadas_para(pregunta):
    if "AJENA" in pregunta:
        return [("conexiones_por_maestro", {**PERIODO, "escuela": "Escuela Prueba A", "limite": 5})]
    if "TENDENCIA" in pregunta:
        return [("tendencia_conexiones", {**PERIODO, "granularidad": "mes"})]
    if "DUP" in pregunta:
        a = ("conexiones_por_maestro", {**PERIODO, "limite": 5})
        return [a, a]
    return [("conexiones_por_maestro", {**PERIODO, "limite": 5})]


class H(BaseHTTPRequestHandler):
    def do_POST(self):
        n = int(self.headers.get("Content-Length") or 0)
        d = json.loads(self.rfile.read(n) or b"{}")
        entrada = d.get("input")
        if isinstance(entrada, str):
            salida = [{"type": "function_call", "name": nombre, "arguments": json.dumps(args), "call_id": f"c{i}"}
                      for i, (nombre, args) in enumerate(llamadas_para(entrada))]
        else:
            salida = [{"type": "message", "content": [{"type": "output_text", "text": "Listo."}]}]
        cuerpo = json.dumps({"id": "resp_" + uuid.uuid4().hex, "output": salida}).encode()
        self.send_response(200)
        self.send_header("Content-Type", "application/json")
        self.send_header("Content-Length", str(len(cuerpo)))
        self.end_headers()
        self.wfile.write(cuerpo)

    def log_message(self, *a):
        pass


if __name__ == "__main__":
    ThreadingHTTPServer(("127.0.0.1", int(sys.argv[1]) if len(sys.argv) > 1 else 5198), H).serve_forever()
