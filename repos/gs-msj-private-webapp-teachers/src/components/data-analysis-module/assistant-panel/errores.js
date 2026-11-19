/** Texto para la persona según el `codigo` del error de assistant-api.js (sesión, permiso, descargas, gráficas…). */
export function mensajeDeError(e, t) {
  switch (e && e.codigo) {
    case "sin_conexion": return t.errRed;
    case "sin_acceso_al_modelo": return t.errModelo;
    case "modelo_no_disponible": return t.errNoDisponible;
    case "descarga_expirada": return t.descargaExpirada;
    case "no_autorizado": case "http_401": return t.errSesion;
    case "sin_permiso": case "http_403": return t.errPermiso;
    case "autenticacion_no_disponible": return t.errAutenticacion;
    case "sin_grafica": return t.errSinGrafica;
    case "url_no_permitida": return t.errDescarga;
    case "parametros_invalidos": return e.message || t.errDescarga;
    case "http_500": case "http_503": case "base_no_disponible": return t.errDescarga;
    default: return t.errGenerico;
  }
}
