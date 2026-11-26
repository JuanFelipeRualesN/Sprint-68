"""Instrucciones del agente. UNA sola fuente de verdad: EyaInsightsApi/instrucciones.txt (con el marcador {hoy}),
que lee tanto este arnés (crear_agente.py, probar_agente_local.py) como el servicio /asistente/preguntar de la API."""
import os
from datetime import date

_ARCHIVO = os.path.join(os.path.dirname(os.path.abspath(__file__)), "EyaInsightsApi", "instrucciones.txt")


def instrucciones(hoy=None):
    hoy = hoy or date.today().isoformat()
    with open(_ARCHIVO, encoding="utf-8") as f:
        texto = f.read().strip().replace("{hoy}", hoy)
    # Mismas fechas relativas que calcula el servicio (Asistente.cs): el arnés de Python ve las mismas instrucciones.
    h = date.fromisoformat(hoy)
    mes = h.replace(day=1)
    def mas_meses(d, n):
        m = d.year * 12 + d.month - 1 + n
        return date(m // 12, m % 12 + 1, 1)
    inicio = h.year if h.month >= 8 else h.year - 1
    reemplazos = {
        "{trimestre_desde}": mas_meses(mes, -3), "{trimestre_hasta}": mes.fromordinal(mes.toordinal() - 1),
        "{mes_pasado_desde}": mas_meses(mes, -1), "{mes_pasado_hasta}": mes.fromordinal(mes.toordinal() - 1),
        "{inicio_anio}": date(h.year, 1, 1),
        "{anio_academico_desde}": date(inicio - 1, 8, 1), "{anio_academico_hasta}": date(inicio, 7, 31), "{anio_academico_actual_desde}": date(inicio, 8, 1),
    }
    for marca, valor in reemplazos.items():
        texto = texto.replace(marca, valor.isoformat())
    return texto
