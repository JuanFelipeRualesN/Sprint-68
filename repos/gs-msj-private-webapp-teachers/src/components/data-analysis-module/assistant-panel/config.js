// Configuración del asistente de datos junto a Power BI (REQ-3741). Todo es nuevo: NO usa ni cambia
// REACT_APP_AI_BOT_AVAILABLE / REACT_APP_AI_DASHBOARD_AVAILABLE ni el chatbot de planeación de lecciones.
//
//   REACT_APP_PBI_ASSISTANT_AVAILABLE=true            muestra el panel (por defecto, oculto)
//   REACT_APP_PBI_ASSISTANT_URL=http://127.0.0.1:5099  base del servicio (EyaInsightsApi) que expone POST /asistente/preguntar
//   REACT_APP_PBI_ASSISTANT_ROLES=admin,admin-ore      roles que ven el panel (por defecto solo admin y admin-ore).
//                                                      SOLO para pruebas locales: ampliarlo expone datos de todas las escuelas.
export const ASSISTANT_AVAILABLE = process.env.REACT_APP_PBI_ASSISTANT_AVAILABLE === "true";
export const ASSISTANT_URL = (process.env.REACT_APP_PBI_ASSISTANT_URL || "http://127.0.0.1:5099").replace(/\/+$/, "");
export const ASSISTANT_ROLES = (process.env.REACT_APP_PBI_ASSISTANT_ROLES || "admin,admin-ore")
  .split(",").map((r) => r.trim()).filter(Boolean);
