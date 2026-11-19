import { ASSISTANT_URL } from "./config";

/**
 * Hace una pregunta al servicio del asistente. Devuelve el JSON del servicio:
 * { respuesta, conversacion_id, referencias[], avisos[], advertencias[], consultas[], descargas[]?, graficas[]?, error? }
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

// ---------------------------------------------------------------------------------------------------------------
// Adjuntos de la respuesta (Fase 4 · REQ-3741): botón de Excel y gráficas.
//
// Contrato (PLAN_FASE4.md D7/D8/D9; los campos los agrega la tarea T6 de la API, aún pendiente):
//   descargas[i] = { id, nombre, consulta, url: "/asistente/descargas/{id}", filas_estimadas?, destacar? }
//   graficas[i]  = { consulta, tipo: "line"|"bar"|"pie", layout?: "horizontal"|"vertical", titulo,
//                    eje_x: {clave, titulo}, eje_y: {titulo, unidad}, series: [{clave, nombre}],
//                    data: [{categoria, <serie>...}], hay_mas_filas, nota, destacar? }
// ÚNICO punto que lee esos campos: si T6 cambia nombres o forma, se ajusta solo aquí.
// ---------------------------------------------------------------------------------------------------------------
export function leerAdjuntos(d) {
  const lista = (x) => (Array.isArray(x) ? x : []);
  const descargas = lista(d && d.descargas).filter((x) => x && typeof x.url === "string" && x.url);
  const graficas = lista(d && d.graficas).filter((g) => g && Array.isArray(g.data) && g.data.length > 0 && Array.isArray(g.series) && g.series.length > 0);
  return { descargas, graficas };
}

function nombreDeContentDisposition(cd) {
  if (!cd) return "";
  const utf = /filename\*\s*=\s*UTF-8''([^;]+)/i.exec(cd);
  if (utf) { try { return decodeURIComponent(utf[1].trim()); } catch (_) { /* cae al siguiente */ } }
  const simple = /filename\s*=\s*"?([^";]+)"?/i.exec(cd);
  return simple ? simple[1].trim() : "";
}

/**
 * GET a `url` (ruta relativa al servicio o absoluta) y baja el archivo con el nombre de content-disposition
 * (o `nombreRespaldo`). Sin headers propios: un GET simple no dispara preflight de CORS.
 * Errores con `codigo`: descarga_expirada (404), sin_conexion, http_NNN, o el `error` del servicio (p. ej. parametros_invalidos).
 */
export async function descargarArchivo(url, nombreRespaldo, signal) {
  const destino = /^https?:\/\//i.test(url) ? url : `${ASSISTANT_URL}${url.startsWith("/") ? "" : "/"}${url}`;
  let r;
  try {
    r = await fetch(destino, { method: "GET", signal });
  } catch (e) {
    if (e && e.name === "AbortError") throw e;
    const err = new Error("sin conexión con el asistente");
    err.codigo = "sin_conexion";
    throw err;
  }
  if (!r.ok) {
    let datos = {};
    try { datos = await r.json(); } catch (_) { /* cuerpo vacío o no JSON */ }
    const err = new Error(datos.mensaje || `error ${r.status}`);
    err.codigo = r.status === 404 ? "descarga_expirada" : datos.error || `http_${r.status}`;
    err.estado = r.status;
    throw err;
  }
  const blob = await r.blob();
  const nombre = nombreDeContentDisposition(r.headers.get("content-disposition")) || nombreRespaldo || "datos.xlsx";
  const enlace = document.createElement("a");
  const href = URL.createObjectURL(blob);
  enlace.href = href;
  enlace.download = nombre;
  document.body.appendChild(enlace);
  enlace.click();
  document.body.removeChild(enlace);
  setTimeout(() => URL.revokeObjectURL(href), 1000);
  return nombre;
}

/** Descarga de un elemento de `descargas[]` (botón bajo la respuesta del asistente). */
export const descargarExcel = (descarga, signal) => descargarArchivo(descarga.url, descarga.nombre, signal);

/**
 * Exportación directa, sin pasar por el chat: GET /exportar/{consulta}?formato=xlsx&desde=…&hasta=…
 * (lista completa, hasta 5000 filas). `consulta` = ruta sin "/" (p. ej. "conexiones-por-maestro").
 */
export function descargarExportacion(consulta, parametros, signal) {
  const q = new URLSearchParams({ formato: "xlsx" });
  Object.entries(parametros || {}).forEach(([k, v]) => { if (v !== undefined && v !== null && v !== "") q.set(k, v); });
  return descargarArchivo(`/exportar/${encodeURIComponent(consulta)}?${q.toString()}`, `${consulta}.xlsx`, signal);
}
