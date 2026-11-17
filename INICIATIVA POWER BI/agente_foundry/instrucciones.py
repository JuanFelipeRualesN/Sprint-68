"""Instrucciones del agente. UNA sola fuente de verdad: EyaInsightsApi/instrucciones.txt (con el marcador {hoy}),
que lee tanto este arnés (crear_agente.py, probar_agente_local.py) como el servicio /asistente/preguntar de la API."""
import os
from datetime import date

_ARCHIVO = os.path.join(os.path.dirname(os.path.abspath(__file__)), "EyaInsightsApi", "instrucciones.txt")


def instrucciones(hoy=None):
    hoy = hoy or date.today().isoformat()
    with open(_ARCHIVO, encoding="utf-8") as f:
        return f.read().strip().replace("{hoy}", hoy)
