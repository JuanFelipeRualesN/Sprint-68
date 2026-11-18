# Panel del asistente junto a Power BI (REQ-3741) — qué se construyó y cómo correrlo

Estado: **13-nov-2026**. Funciona de punta a punta con datos de ejemplo (probado en navegador, español e inglés, escritorio y móvil). Falta probarlo dentro de la plataforma real (requiere tu inicio de sesión) y con el MySQL real (requiere reiniciar la API con tu clave).

## Qué es
Un botón **«Asistente de datos»** en el dashboard de Power BI de la plataforma. Abre un panel lateral (400 px; pantalla completa en móvil) donde un administrador pregunta en lenguaje natural sobre el uso de la plataforma. Cada respuesta trae sus **referencias** (período, fecha filtrada, escuela, cuentas de prueba, datos hasta, filas) y la consulta exacta que se ejecutó.

```
 Panel React (front, rama nueva)  ──POST /asistente/preguntar──►  EyaInsightsApi (servicio nuevo)
                                                                      │  bucle de herramientas
                                                                      ▼
   Foundry (gpt-4.1-mini, token de az login) ◄── instrucciones + 6 consultas (openapi.json)
                                                                      │  el modelo pide consultas
                                                                      ▼
                                              las mismas rutas de la API (MySQL, solo lectura, criterio de Power BI)
```
El modelo **nunca ve la base**: solo el JSON agregado que devuelven las consultas. Las **referencias y avisos del panel las construye el servidor** con lo que devolvió la API, no el modelo.

## Servicio — `EyaInsightsApi` (agente_foundry/EyaInsightsApi)
| Archivo | Qué hace |
|---|---|
| `Asistente.cs` | Bucle del agente contra Foundry (Responses API, token `https://ai.azure.com/.default`), herramientas leídas de `openapi.json`, referencias/avisos deterministas, verificador anti-invención (cifras que no salen de las consultas → `advertencias`), conversaciones con `previous_response_id` (2 h), manejo de errores (filtro de contenido de Azure, 401/403, 5xx). |
| `instrucciones.txt` | **Única fuente** de las instrucciones del agente (la leen el servicio y los scripts de Python). Incluye reglas de idioma (ES/EN), contexto del tablero, período, empates, exclusión de cuentas de prueba… |
| `Program.cs` | `POST /asistente/preguntar`, `GET /asistente/descargas/{id}` y CORS por origen (`Cors__Origenes`). **Apagado por defecto** (`Asistente__Habilitado=true` lo activa). |
| `Auth.cs` | Autenticación (header `Token` ↔ UsersAPI), alcance por rol, caché, CORS y comprobaciones de arranque. |

Contrato (JSON en snake_case): petición `{ pregunta, conversacion_id?, contexto?: {reporte, pagina}, idioma? }` → respuesta `{ respuesta, conversacion_id, referencias[], avisos[], advertencias[], consultas[], error?, descargas[]?, graficas[]? }`. Todas las llamadas llevan el header `Token` de la sesión. `descargas[i] = {id, nombre, consulta, url: "/asistente/descargas/{id}", destacar}` (el enlace baja el `.xlsx` con el `Token`; 404 `descarga_expirada` si venció o es de otra persona); `graficas[i]` = especificación Recharts (`tipo`, `layout`, `eje_x`, `series`, `data`, `destacar`…). Errores de sesión: 401 `no_autorizado`, 403 `sin_permiso`, 503 `autenticacion_no_disponible`.

## Front — rama `REQ-3741-panel-asistente` del repo `gs-msj-private-webapp-teachers` (sin commit ni push)
**Archivos nuevos** (`src/components/data-analysis-module/assistant-panel/`): `index.js` (panel), `assistant-api.js`, `textos.js` (ES/EN), `config.js`, `style.css`.
**Cambios en archivos existentes (mínimos):**
- `dashboard/index.js` (+36 −3): importa el panel, guarda el informe de Power BI en el estado, pasa `onReportReady`, añade la clase `assistant-open` al layout y renderiza el panel **solo si** la bandera está activa **y** el rol es `admin` o `admin-ore`.
- `powerbi-embed/index.js` (+2 −1): nuevo prop opcional `onReportReady(report)`.
No se toca el chatbot de planeación, `AI_BOT_AVAILABLE`, `AI_DASHBOARD_AVAILABLE`, `gs-base-api` ni `settings.js`.

**Contexto de Power BI:** el panel lee la **página activa** del informe (`getActivePage()` y el evento `pageChanged`) y la manda como *orientación*; las instrucciones prohíben usarla como dato o período.

## Cómo correrlo
1. **API real con el asistente** (VPN activa; `az login` vigente): `correr_api_mysql.sh` ya exporta `Asistente__Habilitado=true`, `Asistente__Credencial=azcli`, `ASPNETCORE_ENVIRONMENT=Development`, `Auth__Modo=local` (sin validación, solo loopback) y `Cors__Origenes=http://localhost:3000`.
2. **Front** (otra terminal; entras con tu usuario administrador):
   ```bash
   cd "/Users/juanfelipe/Desktop/REQUIRE SPRINT 1/repos/gs-msj-private-webapp-teachers"
   REACT_APP_PBI_ASSISTANT_AVAILABLE=true REACT_APP_PBI_ASSISTANT_URL=http://127.0.0.1:5099 npm run start:msj-uat
   ```
3. Abre el dashboard de reportes: aparece el botón **Asistente de datos** abajo a la derecha.

## Qué se probó (13-nov)
- **Servicio** (modelo real de Foundry, datos de ejemplo): pregunta con período → referencias; seguimiento en la misma conversación; contexto del tablero; «lo corrido de 2026» y «último trimestre» resueltos con la fecha de hoy; mensaje de jailbreak bloqueado con aviso amable; pregunta vacía → 400; CORS rechaza orígenes ajenos; respuesta en inglés cuando `idioma=en`; sin cifras sin respaldo (`advertencias` vacías).
- **Panel** (empaquetado aparte con un informe simulado, en el navegador integrado): botón, apertura, sugerencias, envío con clic y con Enter, lista numerada, aviso amarillo de «datos hasta», referencias desplegables con chips y «Ver consulta», cambio de página del informe → el contexto se actualiza, estado de error con el servicio caído, inglés, móvil.
- **Sintaxis**: los 6 archivos del front compilan (esbuild).

## Lo que falta (en orden)
1. **Probarlo dentro de la plataforma** con tu sesión y el MySQL real (reiniciar la API con el script nuevo).
2. ~~Seguridad antes de desplegar~~ **Hecho (Fase 4 · A1–A6, ver `DISENO_AUTH.md`):** el servicio valida el `Token` de la plataforma en UsersAPI, impone el alcance (admin: todo · admin-ore: su ORE · director: su escuela) en SQL, reenvía la identidad en las llamadas internas, liga descargas y conversaciones a su dueño y usa identidad administrada hacia Foundry (`Asistente__Credencial=managed`). **Pendiente del usuario** para desplegar: URL de UsersAPI por ambiente, orígenes CORS del front y la identidad administrada en Azure (§7 de `DISENO_AUTH.md`). Sin probar contra UsersAPI ni Foundry reales.
3. Decidir si el panel se muestra a otros roles (hoy solo `admin` y `admin-ore`).
4. Opcional: botón «Ver en el tablero» (cambiar de página / aplicar filtro de escuela con `powerbi-client`) y respuestas en streaming.
5. Commit y PR de la rama cuando lo decidas (hoy los cambios están sin commit).
