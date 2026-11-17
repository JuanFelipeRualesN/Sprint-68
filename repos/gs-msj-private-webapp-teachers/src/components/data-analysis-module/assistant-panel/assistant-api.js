import { ASSISTANT_URL } from "./config";

/**
 * Hace una pregunta al servicio del asistente. Devuelve el JSON del servicio:
 * { respuesta, conversacion_id, referencias[], avisos[], advertencias[], consultas[], error? }
 * Si falla, lanza un Error con `codigo` (sin_conexion, sin_acceso_al_modelo, modelo_no_disponible, pregunta_vacia, http_NNN).
 * Usa fetch directo (no el axios de la plataforma): el servicio es independiente de gs-base-api.
 */
export async function preguntar({ pregunta, conversacionId, contexto, idioma, signal }) {
  let respuesta;
  try {
    respuesta = await fetch(`${ASSISTANT_URL}/asistente/preguntar`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        pregunta,
        conversacion_id: conversacionId || undefined,
        contexto: contexto || undefined,
        idioma: idioma || undefined,
      }),
      signal,
    });
  } catch (e) {
    if (e && e.name === "AbortError") throw e;
    const err = new Error("sin conexión con el asistente");
    err.codigo = "sin_conexion";
    throw err;
  }

  let datos = {};
  try {
    datos = await respuesta.json();
  } catch (_) {
    /* cuerpo vacío o no JSON */
  }
  if (!respuesta.ok) {
    const err = new Error(datos.mensaje || `error ${respuesta.status}`);
    err.codigo = datos.error || `http_${respuesta.status}`;
    err.estado = respuesta.status;
    throw err;
  }
  return datos;
}
