# Semántica de Reportes de la plataforma (REQ-3741)

Objetivo: que el agente responda **lo mismo que el tablero** y poder probarlo **sin abrir Power BI**.
Fuentes: `repos/gs-msj-private-webapp-teachers` (frontend), `repos/gs-base-api/DataAnalysisAPI` (backend), modelo `Reports_Semantic_Model` (ver `CRUCE_POWER_BI.md`).
Wiki `msj-and-private.wiki`, página «PowerBI Data Analysis Dashboard Setup and Breakdown» (leída el 18-nov-2026) y `gs-base-api` rama `main` en Azure DevOps (`PowerBIController.cs`, `PowerBI.cs`).
⚠️ La copia local `repos/gs-base-api` está desactualizada (equivale a `master`): no trae `PowerBIController.cs` ni `PowerBI.cs`. Hay que sincronizarla desde `main`.

## 1. Cómo llega el reporte al usuario
| Capa | Qué hace | Dónde |
|---|---|---|
| Menú de reportes | 5 categorías y quién las ve: **Usuarios** (admin, admin-ORE, director, employee), **Conexiones** (admin, admin-ORE, director; el docente ve «actividad de estudiantes»), **Académico** (admin, admin-ORE para la página PBI), **Administrativo** (solo admin) | `src/reportConfig.js` |
| Embed de Power BI | El front envía `reportId` + `workspaceId` a `GET /dataanalysisapi/embed-powerbi-info`. El backend obtiene un token del service principal y llama a `GenerateToken` con una identidad **RLS** que depende de la sesión (ver §1b). Panel de filtros oculto, sin navegación entre páginas | `powerbi-embed/index.js` → `PowerBIController.cs` → `PowerBI.cs` |
| Descargas Excel | Componentes `REPORT_USER / CONNECTIONS / ACADEMICS / STUDENT_ACTIVITIES_TEACHER` → DataAnalysisAPI (SQL directo a tablas, no a vistas) | `DataAnalysisAPI/*Infrastructure/Dapper` |
| Asistente | Panel junto al tablero → `EyaInsightsApi /asistente/preguntar` | `assistant-panel/` |

## 1a. Qué se embebe realmente en producción MSJ (`.env.msj` + linaje del área de trabajo MSJ)
- Área de trabajo `a4c05dbb…`. Las categorías Conexiones y Administrativo usan el informe `a52c6997…` = **MSJ_Utilization**, sobre **Reports_Semantic_Model** (MySQL `gs_data_analysis` prod + Excel de SharePoint, por la puerta de enlace `gw-pbi-hub-vpn-001`, con refresco diario).
- **Solo la categoría Administrativo muestra Power BI, y solo al admin**, con 3 páginas: Reporte de Conexión, Conexiones acumuladas y Uso de docentes (`vw_teacher_activities`).
- Usuarios y Académico **no tienen informe configurado** (IDs vacíos), y Conexiones no tiene página: para admin-ORE, director y docente la sección es **solo descargas de Excel** (DataAnalysisAPI, que filtra por ORE en el backend).
- Por eso la página «Reporte Académico» de Power BI no se ve en la plataforma, y el problema de RLS de admin-ORE (§1b) hoy no afecta a ningún usuario.
- «EyA Propuesto» (modelo e informe a partir de Excel: Access Logs, Connections, Average, Users, Escuelas_EA_Visitadas) es una propuesta y la plataforma no lo usa.

## 1b. Alcance de los datos (RLS): confirmado en wiki y código
- Si la sesión tiene `OreId` (admin-ORE): rol RLS **`Admin_ORE_Filter`**, regla `[ore] = USERNAME()`. Solo ve su ORE.
- Si no lo tiene (admin, director, docente, employee): rol **`ViewAll`**. Ve **todos** los datos. El menú limita qué páginas ve cada rol, pero no filtra filas: un director no queda limitado a su escuela.
- ⚠️ **Discrepancia wiki vs. código:** la wiki dice `username = oreId`, pero en `main` el código envía `username = "ORE"` fijo. Con la regla `[ore] = USERNAME()`, el admin-ORE solo vería filas cuyo ORE se llame literalmente «ORE», o sea nada o un filtro equivocado. Hay que revisar la regla real en *Manage roles* del modelo y probar el embed con un usuario admin-ORE.
- **Para el agente:** las cifras validadas (113 / 14 / 84…) corresponden a `ViewAll`. Para un admin-ORE, el agente tiene que recibir el ORE del usuario de sesión y filtrar igual que la regla RLS; hoy no lo hace.

## 2. Tres fuentes y sus criterios
| Criterio | Power BI (vistas `vw_*`) | Excel DataAnalysisAPI (tablas) | API del agente (modo paridad) |
|---|---|---|---|
| Escuelas de prueba (ORE `DEMOSTRACION`) | fuera | **dentro** | fuera |
| «Escuela Facilitadores MSJ» | fuera (filtro de página de conexiones) | dentro | fuera |
| Usuarios sin escuela | fuera (`NOT(NULL)`) | dentro (`LEFT JOIN`) | fuera |
| Rol client en conexiones | fuera (`role_id <> 1`) | dentro | fuera |
| Pruebas | solo estudiantes (`role_id = 3`) | solo estudiantes | solo estudiantes |
| Filtro de fecha en pruebas | año académico por **mes de inicio ≥ 8** + `period_no` | `period_from` entre fechas, o `academic_year_id` | igual que Power BI |
| «Conectados» | `DISTINCTCOUNT(email)` | usuarios distintos (`UserId`) | `usuarios_conectados` |
| Alcance por rol | RLS: admin-ORE → su ORE; resto → todo | ORE → escuelas del ORE (`AutoFilters.GetOreSchoolIds`); docente → `user_relation` | no aplica alcance por rol (vista de administrador) |

Consecuencia: **el Excel descargado NO es una referencia válida** para la paridad (incluye cuentas de prueba y usuarios sin escuela). La referencia es el tablero, y el tablero es reproducible con SQL sobre las vistas.

## 2b. Vistas del modelo (DDL en la wiki)
`vw_connections_report`, `vw_not_connected` (usuarios con suscripción activa y `cancellation_date >= NOW()` sin ninguna conexión histórica), `vw_teacher_activities` (cruza `gs_users_db_prod`, `gs_teachers_web_db_prod`, `gs_subscription_api_db_prod` y `gs_schools_api_db_prod`, período académico vigente), `vw_total_users_with_connection_details` (conectados / total con suscripción activa → %), `vw_users_report`, `vw_users_details_report`, `vw_academics_average`, `vw_academics_details`. Todas excluyen `ore LIKE '%DEMOSTRACION%'`. Las dos de suscripción dependen de `NOW()`, así que su resultado cambia con el día en que se consultan.

## 2c. Cambios de `main` (16-nov-2026) que no están en la copia local
- `PowerBIController.cs` + `PowerBI.cs`: endpoint del embed con RLS (§1b).
- **Filtro de suscripción** en las descargas (`status=active|inactive`):
  - Usuarios: **activo** = `subscription_profile_history.status = 'active'` y `cancellation_date >= NOW()`; el resto es inactivo.
  - Conexiones: usan `connected_date.subscription_status` (el estado que tenía el usuario el día que se conectó).
  - Es la misma definición que la tarjeta «usuarios con matrícula activa» (7.199) de Power BI y que `vw_not_connected`.
- Paginación de resultados académicos: `Skip(fromPage)` en lugar de `Skip(fromPage * limiter)` (offset en filas, no en páginas).
- La copia local tiene `assistant-panel/` (trabajo de REQ-3741), que no está en `main`.

## 3. Glosario (definición operativa = la del tablero)
- **Conexión**: fila de `connected_date` (1 por usuario y día) de un usuario con escuela, rol ≠ client, fuera de ORE DEMOSTRACION. Día en hora de Puerto Rico (UTC-4).
- **Usuario con matrícula activa**: suscripción `active` con `cancellation_date >= NOW()`. Cambia cada día.
- **Usuario conectado**: correo distinto con al menos una conexión en el período.
- **Escuela conectada**: escuela distinta con al menos una conexión. **Universo de escuelas**: 47 (lista mantenida en `Escuelas_EA_Visitadas.xlsx`; la API la aproxima: escuelas con estudiantes o con conexiones).
- **Prueba asignada**: fila de `vw_academics_details` (`result_generic` de estudiantes). **Completada**: `quiz_taken_date` no nulo.
- **Año académico «2025-2026»**: `academic_year_start` entre el 1-ago-2025 y el 31-jul-2026. **Período**: `P` + `period_no`.
- **% Full Quiz**: `AVG(COALESCE(total_percent_full_quiz,0))` por estudiante/materia/período (`vw_academics_average`).

## 4. Probar sin Power BI
1. `sql/09_oraculo_power_bi.sql` en Workbench (solo lectura): reproduce las medidas del tablero sobre las vistas. Valores de control: 113 / 14 / 84 conexiones (2026 T3+T4); 6.106 / 217 / 5.889 / 146 pruebas (2025-2026 P1–P3); 10.628 filas históricas.
1b. `sql/10_oraculo_acumuladas_y_docentes.sql`: páginas «Conexiones acumuladas» (3 lecturas posibles: total histórico de `vw_total_users_with_connection_details`, suma corrida de conexiones, usuarios nuevos acumulados) y «Uso de docentes» (`vw_teacher_activities`: 5 indicadores SI/NO, totales, por escuela y docentes duplicados). Compararlo con el tablero **el mismo día**, porque ambas vistas dependen de la fecha actual. `dax/medidas_y_particiones.dax` lee las fórmulas reales del modelo para decidir cuál lectura usa cada tarjeta.
2. Levantar la API con `correr_api_mysql.sh` y comparar sus respuestas con el oráculo para el mismo período.
3. `pruebas_mysql/verificar_api_mysql.py`: regresión offline (Docker, datos sintéticos) que debe seguir en 37/37 tras cada cambio de SQL.
4. Power BI solo se abre para casos que el oráculo no cubre (pendiente: tarjetas de la página demo 1.029 / 206 / 823 / 135).

## 5. Pendientes que cambian cifras
- RLS de admin-ORE: confirmar la regla real (`username = "ORE"` en el código vs. `oreId` en la wiki) y pasar el ORE de la sesión al agente.
- Confirmar con negocio si directores y docentes deben ver todo (`ViewAll`) o solo su escuela.
- El filtro oculto de las tarjetas de la página «Reporte Académico (end user-demo)».
- Confirmar con el equipo de datos la fecha del último ETL de `result_generic` (la actividad real de pruebas termina el 23-abr-2026).
