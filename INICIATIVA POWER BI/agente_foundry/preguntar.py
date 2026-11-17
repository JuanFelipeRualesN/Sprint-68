"""Hace UNA pregunta al agente de Foundry (eya-asistente-uso) y muestra todo lo que pasa:
la consulta que eligio, los parametros que envio, lo que devolvio la API y la respuesta final.

Uso:  python3 preguntar.py "¿Cuál fue el área más utilizada entre el 1 de septiembre y el 10 de noviembre de 2026?"
      python3 preguntar.py            (sin texto: pide la pregunta por teclado)

Requisitos: az login hecho y el agente ya creado con crear_agente.py.
"""
import json
import os
import sys

from azure.ai.projects import AIProjectClient
from azure.identity import DefaultAzureCredential

ENDPOINT = os.environ.get(
    "FOUNDRY_PROJECT_ENDPOINT",
    "https://chatbotaipowerbi.services.ai.azure.com/api/projects/proj-default",
)
AGENTE = "eya-asistente-uso"


def bonito(texto, limite=1500):
    try:
        dato = json.loads(texto)
        if isinstance(dato, dict) and isinstance(dato.get("response"), str):   # Foundry envuelve la respuesta
            dato = json.loads(dato["response"])
        texto = json.dumps(dato, ensure_ascii=False, indent=2)
    except (TypeError, ValueError):
        texto = str(texto)
    return texto if len(texto) <= limite else texto[:limite] + "\n  ... (recortado)"


def main():
    pregunta = " ".join(sys.argv[1:]).strip() or input("Pregunta: ").strip()
    project = AIProjectClient(endpoint=ENDPOINT, credential=DefaultAzureCredential())
    with project.get_openai_client() as openai:
        resp = openai.responses.create(
            input=pregunta,
            extra_body={"agent_reference": {"name": AGENTE, "type": "agent_reference"}},
        )
    print(f"\nPREGUNTA: {pregunta}")
    paso = 0
    for item in resp.output:
        tipo = getattr(item, "type", "")
        if tipo == "openapi_call":
            paso += 1
            print(f"\n[{paso}] El agente llama a: {getattr(item, 'name', '')}")
            print(f"    Parámetros: {getattr(item, 'arguments', '')}")
        elif tipo == "openapi_call_output":
            print("    La API devolvió:")
            for linea in bonito(getattr(item, "output", "")).splitlines():
                print("      " + linea)
    print(f"\nRESPUESTA DEL AGENTE:\n{resp.output_text}\n")


if __name__ == "__main__":
    sys.exit(main())
