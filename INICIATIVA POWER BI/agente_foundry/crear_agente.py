"""Crea el agente de Foundry (REQ-3741, paso 3) con la herramienta OpenAPI y prueba las 7 preguntas.


Requisitos: az login hecho, azure-ai-projects instalado, API de ejemplo publica y conexion creada.
Uso:  python3 crear_agente.py            (crea una version nueva del agente y corre las 7 preguntas)
      python3 crear_agente.py --solo-crear
"""
import json
import os
import sys

from azure.ai.projects import AIProjectClient
from azure.ai.projects.models import (
    OpenApiFunctionDefinition,
    OpenApiProjectConnectionAuthDetails,
    OpenApiProjectConnectionSecurityScheme,
    OpenApiTool,
    PromptAgentDefinition,
)
from azure.identity import DefaultAzureCredential

from instrucciones import instrucciones

ENDPOINT = os.environ.get(
    "FOUNDRY_PROJECT_ENDPOINT",
    "https://chatbotaipowerbi.services.ai.azure.com/api/projects/proj-default",
)
MODELO = os.environ.get("FOUNDRY_MODEL_DEPLOYMENT_NAME", "gpt-4.1-mini")
CONEXION = os.environ.get("FOUNDRY_API_CONNECTION", "eya-api-key")
NOMBRE_AGENTE = "eya-asistente-uso"

PREGUNTAS = [
    "¿Cuál fue el área más utilizada entre el 1 de septiembre y el 10 de noviembre de 2026?",
    "¿Cuáles fueron las áreas menos utilizadas entre el 1 de septiembre y el 10 de noviembre de 2026?",
    "¿Cuáles fueron las escuelas con mayor cantidad de conexiones en lo corrido de 2026?",
    "¿Cuáles maestros registraron la mayor cantidad de conexiones durante el año 2026?",
    "¿Cómo se compara el uso de las áreas entre septiembre y octubre de 2026?",
    "¿Cuáles fueron las tendencias de uso por trimestre en 2026?",
    "¿Qué escuelas presentaron menor actividad entre el 1 de septiembre y el 10 de noviembre de 2026?",
]


def crear_agente(project):
    aqui = os.path.dirname(os.path.abspath(__file__))
    spec = json.load(open(os.path.join(aqui, "openapi.json"), encoding="utf-8"))
    conexion_id = project.connections.get(CONEXION).id
    herramienta = OpenApiTool(
        openapi=OpenApiFunctionDefinition(
            name="consultas_uso",
            description="Consultas de solo lectura sobre el uso de la plataforma Estudia y Avanza.",
            spec=spec,
            auth=OpenApiProjectConnectionAuthDetails(
                security_scheme=OpenApiProjectConnectionSecurityScheme(project_connection_id=conexion_id)
            ),
        )
    )
    return project.agents.create_version(
        agent_name=NOMBRE_AGENTE,
        definition=PromptAgentDefinition(
            model=MODELO,
            instructions=instrucciones(),
            tools=[herramienta],
        ),
    )


def main():
    project = AIProjectClient(endpoint=ENDPOINT, credential=DefaultAzureCredential())
    agente = crear_agente(project)
    print(f"Agente creado: {agente.name} versión {agente.version}")
    if "--solo-crear" in sys.argv:
        return
    with project.get_openai_client() as openai:
        for i, pregunta in enumerate(PREGUNTAS, 1):
            print(f"\n=== {i}. {pregunta}")
            resp = openai.responses.create(
                input=pregunta,
                extra_body={"agent_reference": {"name": agente.name, "type": "agent_reference"}},
            )
            llamadas = [o for o in resp.output if "call" in getattr(o, "type", "")]
            for o in llamadas:
                print(f"  -> {o.type}: {getattr(o, 'name', '')} {getattr(o, 'arguments', '') or ''}"[:220])
            if not llamadas:
                print("  -> (no llamó ninguna consulta)")
            print(f"  Respuesta: {resp.output_text}")


if __name__ == "__main__":
    sys.exit(main())
