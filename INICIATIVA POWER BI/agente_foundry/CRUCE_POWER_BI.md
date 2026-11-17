# Cruce con Power BI (REQ-3741)

Objetivo: comprobar que lo que dice el agente (que lee `gs_data_analysis`) coincide con lo que muestra el tablero de Power BI.
Los valores de la columna «Base de datos» son los reales de la base, **sin excluir cuentas de prueba** (Power BI casi seguro las incluye).
Día contado en hora de Puerto Rico (UTC-4); si Power BI usa UTC, una diferencia de ±1–5 conexiones en un período corto es normal.

| # | Qué mirar en Power BI | Base de datos (sin exclusión) | Power BI muestra | ¿Coincide? |
|---|---|---|---|---|
| 1 | Total de usuarios | 18.610 (estudiantes 16.822 · docentes 1.408 · directores 137 · admin 16 · admin-ore 21 · client 206) | | |
| 2 | Conexiones, todo el histórico (conteo de filas de `connected_date`) | 23.993 | | |
| 3 | Usuarios distintos que se han conectado alguna vez («Conectado único») | 2.982 | | |
| 4 | Conexiones por año | 2022: 5.409 · 2023: 7.347 · 2024: 1.861 · 2025: 6.924 · 2026: 2.452 | | |
| 5 | Conexiones 2026 por trimestre | T1 1.458 · T2 555 · T3 237 · T4 202 (hasta 11-nov) | | |
| 6 | Conexiones 2026 por escuela (las 5 primeras) | Vimenti 364 · The School of San Juan 343 · Escuela Calidad Grupo Require 280 · Leap STEAM 262 · The United School of San Juan 195 | | |
| 7 | Quizzes: tomados vs. no tomados (todo el histórico) | tomados 611 · no tomados 11.662 · total 12.273 | | |
| 8 | Fecha de la última conexión / último quiz tomado | conexión: 11-nov-2026 · quiz tomado: 19-oct-2026 (con cuentas de prueba; sin ellas, 23-abr-2026) | | |

## Cómo interpretar el resultado
- **Coincide todo** → el agente y el tablero leen lo mismo; se puede confiar en las cifras.
- **Difiere un total pequeño (±1 %)** → casi seguro zona horaria (UTC vs. Puerto Rico) o la hora de corte.
- **Difiere mucho en conexiones** → revisar qué cuenta Power BI como «conectado» (¿solo estudiantes? ¿usuarios activos?) y si usa otra tabla.
- **Los quizzes no coinciden** → revisar el filtro de «Period» y si el tablero toma `result_generic` completo o solo parte.
- **Las escuelas coinciden solo si se incluyen las cuentas de prueba** → confirma que Power BI no las excluye; decidir si el tablero también debe excluirlas.
- Si el tablero dice que hay quizzes tomados después del 23-abr-2026, esos son de cuentas de prueba (o hay algo que la exclusión no captura): decirlo y revisamos.

---

# RESULTADO DEL CRUCE (12-nov-2026, 18:30) — leído directamente del informe de producción
Informe `MSJ_Utilization` del área de trabajo **MSJ** (`a4c05dbb…`, informe `a52c6997…`), «Datos actualizados el 12/11/26»; el modelo `Reports_Semantic_Model` se refresca una vez al día (último 12-nov 0:14, próximo 13-nov 0:00).
Nota: la tabla de arriba (valores «sin exclusión») quedó superada, porque **Power BI ya excluye las cuentas de prueba** (ver abajo); el cruce correcto es contra la API con la exclusión activada.

## Página «Reporte de Conexión» — filtro de fecha en **2026, trimestres 3 y 4** (1-jul → 11-nov-2026)
| Medida | Power BI | API (exclusión activada, día en hora de Puerto Rico) | |
|---|---|---|---|
| Total de conexiones | 113 | 113 (suma por escuela) | ✅ |
| Escuelas conectadas | 14 | 14 | ✅ |
| Conexiones por escuela | The School of San Juan 31 · Esc. Especializada en Matemáticas 24 · Esc. del Deporte 23 · The United School 15 · Gustavo A Becquer 5 · Arturo Morales Carrión 4 · Vimenti 2 · Leap STEAM 2 · Centro Eugenio María de Hostos 2 · Venus Gardens, Corretjer, Eleanor Roosevelt, Amalia Marín Castilla, Albert Einstein 1 c/u | idéntico, escuela por escuela | ✅ |
| Por rol (teacher / student / director) | 98 / 9 / 6 | (la API no desglosa por rol) | — |
| Total de usuarios con matrícula activa | 7.199 | (no se expone) | — |
| Total de escuelas | 47 | universo de la API ≥ 50 (63 con usuarios − 13 de prueba) | ⚠️ ver abajo |

## Hallazgos del cruce
1. ✅ **Las conexiones coinciden exactamente.** Valida en producción el SQL, la exclusión de cuentas de prueba y el «día» en hora de Puerto Rico.
2. ✅ **Power BI ya excluye las cuentas de prueba**: las 47 escuelas del tablero no incluyen ninguna de ORE DEMOSTRACION, y el informe tiene un filtro de página explícito «school_name no es ESCUELA FACILITADORES MSJ». La exclusión de la API coincide con la del tablero.
3. 🔧 **Inconsistencia propia corregida:** la tendencia trimestral de la API sumaba 120 (30 + 90) mientras la suma por escuela daba 113: los 7 de diferencia eran usuarios **sin escuela** (hay 186). Ahora todas las consultas de conexiones cuentan solo usuarios con escuela asignada, como Power BI, y hay una comprobación automática de que el total por escuela = por trimestre = por mes (37/37).
4. 🔧 **«Escuela Facilitadores MSJ» ahora se excluye por defecto**, igual que el tablero.
5. ⚠️ **Total de escuelas:** Power BI 47 vs. 50 de la API. Probablemente Power BI cuenta solo escuelas con usuarios de matrícula activa (p. ej. se caen las de 1 usuario: TCPSA, Salvador Brau, etc.). No afecta a las conexiones, solo a «escuelas con cero conexiones» (la API puede listar 3 más).
6. ❌ **Pruebas: NO coinciden las asignadas.** Página «Reporte Académico», Año académico 2025-2026 (P1+P2+P3): Power BI muestra **asignadas 1.029 · completadas 206 · no completadas 823 · 135 estudiantes**; la API (filas de `result_generic`, estudiantes, 1-jul-2025 →) da **7.117 asignadas · 217 completadas**. Las completadas se parecen (206 vs 217); las asignadas, no: «Pruebas asignadas» del tablero **no es** «una fila de `result_generic` por estudiante». Falta definirlo con `sql/05_definir_pruebas_asignadas.sql` (año académico, período, tipo de asignación, pruebas distintas…). Hasta entonces, el agente debe presentar las pruebas como «filas de asignación», no como «pruebas asignadas» a secas.

## Otras observaciones del tablero
- Todas las páginas tienen filtros de fecha propios; comparar siempre con el mismo período (aquí: 2026 Q3+Q4).
- En «Reporte de Conexión», el 99 % de los 7.199 usuarios con matrícula activa **no se conectó** en el período (84 conectados, 1,17 %).
- Los usuarios conectados listados tienen `subscription_status = inactive` (la columna existe en `connected_date`): el tablero cuenta «conectado» aunque la suscripción esté inactiva.

---

# Resultados de `sql/05_definir_pruebas_asignadas.sql` (Workbench, 12-nov-2026)
- **Dos poblaciones de filas en `result_generic`.** Con `academic_year_type` vacío (`''`, no `NULL`) las tasas de realización son muy altas (p. ej. año 1652, período 2: 240 filas, 131 tomadas, 83 estudiantes; año 1644, período 1: 137 filas, 41 tomadas, 32 estudiantes). Con `academic_year_type = NULL` hay miles de filas y casi ninguna tomada (1652 P2: 600 filas, 0 tomadas; 1656 P3: 1.170 filas, 0 tomadas; 62 P4: 1.911 filas, 0 tomadas).
- **Tipo de asignación** (desde 2025-06, solo estudiantes): `L` = 6.901 filas / 125 tomadas; `H` = 500 filas / 197 tomadas. Todas con `plan_currently_available = 1`. Hipótesis: `L` = lecciones automáticas, `H` = tareas puestas por el docente (por confirmar).
- **Totales desde 2025-06:** 7.401 filas · 322 tomadas · 868 estudiantes distintos · 450 quizzes distintos · 43 `plan_quiz` distintos · 197 asignaciones distintas (`plan_assignment_user_result_id`, que coincide con las tomadas de tipo `H`).
- **Los años académicos están desordenados**: ids con fechas que se solapan (1644 y 1650–1654 casi idénticos, 1603 hasta 2033, 1 hasta 2030…). Un año académico «2025-2026» en Power BI agrupa varios ids.
- Ninguna agrupación sola reproduce 1.029 / 206 / 135 a simple vista → `sql/06_probar_definiciones_pruebas.sql` prueba 10 definiciones candidatas lado a lado (tipo nulo/vacío, tipo H, período 1–3, vencidas, pares distintos…). **Pendiente: que Juan lo ejecute y pegue la tabla.**
- Si ninguna coincide, el siguiente paso es leer la medida «Pruebas asignadas» en el modelo semántico (`Reports_Semantic_Model` → Ver modelo semántico).

---

# Resultados de `sql/06_probar_definiciones_pruebas.sql` (Workbench, 12-nov-2026, 18:01)
Objetivo de Power BI (Año académico 2025-2026, P1+P2+P3): **1.029 asignadas · 206 completadas · 823 no completadas · 135 estudiantes**. Base: estudiantes, SIN escuelas del ORE DEMOSTRACION, año académico que empieza en 2025 y termina en 2026.

| Definición | asignadas | completadas | no completadas | estudiantes |
|---|---|---|---|---|
| D01 todas las filas, cualquier período | 7.117 | 217 | 6.900 | 146 |
| D02 todas las filas, período 1–3 | 6.106 | 217 | 5.889 | 146 |
| D03 / D04 tipo de año académico NO nulo (vacío) | 465 | 189 | 276 | 130 |
| D05 / D06 `assignment_type = 'H'` | 465 | 189 | 276 | 130 |
| D07 solo vencidas, período 1–3 | 6.106 | 217 | 5.889 | 146 |
| D08 pares distintos (estudiante, quiz), período 1–3 | 5.881 | 217 | 5.664 | 146 |
| D09 pares distintos, tipo NO nulo | 465 | 189 | 276 | 130 |
| D10 con asignación del docente | 189 | 189 | 0 | 130 |

**Conclusiones:**
1. **«Tipo de año académico no nulo» ≡ `assignment_type = 'H'` ≡ 465 filas / 189 completadas**: son exactamente las mismas filas → lo que Power BI cuenta como «pruebas» son las **tareas asignadas por el docente (H)**, no las lecciones automáticas (L, ~6.600 filas con casi 0 realización).
2. `H` sin cuentas de prueba queda **cerca** de Power BI en completadas y estudiantes (189 vs 206; 130 vs 135), pero lejos en asignadas (465 vs 1.029). Faltan ≈ **564 asignadas, 17 completadas, 5 estudiantes**.
3. Hipótesis: esa página **no excluye** las cuentas de prueba (ORE DEMOSTRACION; «Escuela Demostraciones» tiene 377 estudiantes) y por eso suma más. → `sql/07_pruebas_incluyendo_cuentas_de_prueba.sql` lo comprueba (E01–E04, desglose por período y por escuela). Si E01/E02 dan 1.029 / 206 / 823 / 135, queda resuelto.
4. Consecuencia para el agente y la API (a aplicar cuando se confirme): las «pruebas» deben ser las de tipo H (tareas del docente) y no filas de lecciones automáticas; con ese cambio, «completadas ≈ 20 %» se vuelve cierto en vez de ~3 %.

---

# Resultados de `sql/07` y hallazgo del modelo de Power BI (12-nov-2026)

## Consulta 07: la hipótesis de las cuentas de prueba queda REFUTADA
Tipo H con cuentas de prueba: 474 asignadas · 194 completadas · 280 no · 131 estudiantes (cualquier período); período 1–3: 471 / 192 / 279 / 131. Las cuentas de prueba aportan solo 9 filas de tipo H (Equipo Require 6, Calidad IT Require 3). Por escuela (tipo H, 2025-2026): Vimenti 240/131/83 · Esc. Especializada en Matemáticas 161/48/39 · The United School 56/7/5 · Ramón Power y Giralt 7/3/3 · Equipo Require 6/5/1 · Calidad IT Require 3/0/0 · The School of San Juan 1/0/0. Eso sigue lejos de Power BI (1.029 / 206 / 823 / 135), y además «The School of San Juan» aparece en el gráfico del tablero con puntuación 92 % pese a tener 1 sola fila H → **el tablero incluye filas que no son H**. Conclusión: adivinar definiciones sobre `result_generic` no sirve.

## Hallazgo clave: Power BI lee VISTAS de la base, no tablas
Leído directamente del modelo semántico `Reports_Semantic_Model` (vista de modelo + consulta DAX de solo lectura):
- Las tablas del modelo son vistas de `gs_data_analysis`: `vw_academics_average`, `vw_academics_details`, `vw_connections_report`, `vw_not_connected`, `vw_student_active_courses`, `vw_student_teacher_relationship`, `vw_subscription_profile…`, `vw_teacher_active_courses`, `vw_teacher_activities` (+ variantes), `vw_total_users_with_connections`, `vw_users_details_report`, `vw_users_report`, y tablas propias del modelo (`Escuela_E&A`, `TBL_All_Users_Connections`, `Tbl_Connections`, `Tbl_Escuelas`, `Tbl_Fecha_Conexion`, `Valor_Si_No`).
- Medidas de quizzes del modelo (solo 3): `% Full Quiz (promedio)` = AVERAGE(…), `Quizz_taken` = CALCULATE(COUNTROWS(`vw_academics_details`), `vw_academics_details`[Completado/No completado] = "Completed"), `Quizz_not_taken` = igual con el otro estado.
- **Por tanto «Pruebas asignadas» del tablero = nº de filas de la vista `vw_academics_details`** (completadas + no completadas). La lógica de qué se cuenta vive en la definición de esa vista, no en `result_generic`.
- Las conexiones sí coincidieron con la API: la vista `vw_connections_report` evidentemente cuenta lo mismo que `connected_date` + usuarios con escuela.

## Siguiente paso
`sql/08_leer_vistas_de_power_bi.sql`: `DESCRIBE` + `SHOW CREATE VIEW` de `vw_academics_details` (y de `vw_academics_average` y `vw_connections_report`). Con esa definición se reescribe la consulta de pruebas de la API **sobre la misma vista** que usa Power BI (o replicando su SQL), de modo que las cifras coincidan por construcción.

---

# Definiciones reales de las vistas y del modelo de Power BI (12-nov-2026, 18:30–19:30)
Fuentes: `sql/08` (SHOW CREATE VIEW, Workbench) y el modelo semántico `Reports_Semantic_Model` leído con consultas DAX de solo lectura (`INFO.MEASURES()`, `INFO.PARTITIONS()`).

## Vistas de MySQL (lo que Power BI realmente lee)
- **`vw_academics_details`** = `result_generic` ⨝ `user` (role_id **= 3**, estudiantes) ⨝ `school` ⨝ `subject`, **`WHERE NOT (ore LIKE '%DEMOSTRACION%')`**. Trae `note` = «Completed» si `quiz_taken_date` no es nulo, «Not Completed» si es nulo. **10.628 filas** en total = exactamente las «10.628 asignadas» que daba mi API para todo el histórico → la API y Power BI parten de las mismas filas de pruebas.
- **`vw_academics_average`** = lo mismo agrupado por estudiante/materia/período: `ROUND(AVG(COALESCE(total_percent_full_quiz, 0)), 2)`.
- **`vw_connections_report`** = `connected_date` ⨝ `user` (**`role_id <> 1`**, excluye el rol *client*) ⨝ `school` (LEFT), **`WHERE NOT (ore LIKE '%DEMOSTRACION%')`**. Como `NOT(NULL)` es NULL, **los usuarios sin escuela quedan fuera** (por eso el cruce de conexiones solo cuadró al excluirlos).

## Power Query (M) de `vw_academics_details` en el modelo
- Recorta y pone en mayúsculas `school_name`; renombra `note` → «Completado/No completado».
- **`Sistema Educativo`** = «SEMI/Alianza» si `school_identifier` ∈ {2881, 2009, 2692, 07345, 7845, 6372}, si no «ORE».
- **`academic_year`** = si el **mes de `academic_year_start` ≥ 8** → «año-(año+1)», si no → «(año−1)-año». **Es por la fecha de INICIO** (el «2025-2026» = inicio entre 1-ago-2025 y 31-jul-2026), no por inicio y fin.
- **`Period`** = «P» + `period_no`.

## Medidas relevantes
- `Quizz_taken` = COUNTROWS(`vw_academics_details`) donde «Completado/No completado» = «Completed»; `Quizz_not_taken` = ídem «Not Completed».
- `Conteo_Conectados_Si_Total` = COUNTROWS de `vw_connections_report` con `connected_date` no vacío (= «conexiones», lo mismo que cuenta la API).
- `Conectado único` / `Conteo_Conectados_Si` = **DISTINCTCOUNT del correo** (usuarios distintos).
- `% Escuela conectada` = escuelas conectadas / escuelas de la hoja Excel `Escuelas_EA_Visitadas.xlsx` (SharePoint de Require).

## Conteos del propio modelo para el año académico 2025-2026
| Período | Tomadas | No tomadas | Filas |
|---|---|---|---|
| P1 | 51 | 1.333 | 1.384 |
| P2 | 152 | 1.831 | 1.983 |
| P3 | 14 | 2.725 | 2.739 |
| P4 | — | 1.011 | 1.011 |
| **P1–P3** | **217** | **5.889** | **6.106** |
→ **idéntico a mi consulta D02** (6.106 / 217 / 5.889, 146 estudiantes): el SQL de la API y el modelo de Power BI coinciden fila por fila.

## Lo que sigue sin explicarse: las tarjetas de la página «Reporte Académico (end user-demo)»
Muestran 1.029 asignadas · 206 completadas · 823 no completadas · 135 estudiantes, con año 2025-2026, P1–P3 y todos los filtros en «(todos)» salvo «Completado/No completado = Completed». Con exactamente esos filtros el modelo da 6.106 / 217 / 5.889 (146), no 1.029 / 206 / 823 (135). Ni los distintos de `quiz_title` (194), `plan_name` (69) ni de `sie` (146) lo explican. Pista numérica: **217 → 206 y 146 → 135 restan exactamente 11** (11 estudiantes con 1 prueba completada cada uno desaparecen) y las no tomadas pasan de 5.889 a 823. Hay un filtro adicional (de visual, de tarjeta o una medida que no pude listar: el grid del editor DAX solo mostró 17 de las medidas) que no he localizado. **Ojo: es una página «demo»**, no necesariamente la que usa la plataforma.

## Diferencias de criterio entre Power BI y la API (decisión de negocio)
| | Power BI | API |
|---|---|---|
| Escuelas de prueba | excluye solo `ore LIKE '%DEMOSTRACION%'` (+ «Facilitadores MSJ» en una página) | excluye ORE DEMOSTRACION, «Escuela Demo T3 School» y «Facilitadores MSJ» |
| Correos `@requiretechnology.com` | **no los excluye** | los excluye (33 % de las conexiones de 2026) |
| Rol *client* | excluido en conexiones | **excluido desde hoy** (antes se contaba) |
| Usuarios sin escuela | fuera | fuera |
| «Usuarios conectados» | usuarios distintos por correo | la API cuenta filas (usuario-día); no expone usuarios distintos |

---

# Decisión de Juan (12-nov, 18:25): «que coincida con Power BI» → valores por defecto de la API
Desde este cambio **la API, sin configurar nada, aplica el criterio de Power BI**:
- Excluye solo escuelas cuyo ORE **contiene** «DEMOSTRACION» (misma semántica que las vistas `vw_*`: si la escuela o su ORE son NULL, la fila queda fuera) y «Escuela Facilitadores MSJ» (filtro de la página de conexiones).
- **Ya no excluye** los correos `@requiretechnology.com` ni «Escuela Demo T3 School» (Power BI los cuenta). Siguen disponibles como opción: `Datos__DominiosExcluidos=requiretechnology.com` y `Datos__EscuelasExcluidas="Escuela Demo T3 School,Escuela Facilitadores MSJ"`.
- Conexiones: solo usuarios con escuela y rol ≠ client (igual que `vw_connections_report`).
- Nuevo campo `usuarios_conectados` por escuela (usuarios distintos con conexión en el período): equivale a la tarjeta «Usuarios conectados» de Power BI.
- `Datos__ExcluirCuentasDePrueba=false` desactiva toda exclusión por ORE/escuela/dominio.
Verificación independiente (base de prueba en Docker, `pruebas_mysql/verificar_api_mysql.py`): **37/37 en los tres modos** (`paridad` por defecto, `dominio`, `incluir`).

---

# Prueba con la API reiniciada en modo paridad (12-nov, 18:28) y último ajuste
**Cruce numérico con la página «Reporte de Conexión» (2026, trimestres 3 y 4):** conexiones **113 = 113**, escuelas conectadas **14 = 14**, usuarios conectados **84 = 84** (suma de `usuarios_conectados` por escuela); la tendencia trimestral suma 23 + 90 = 113 (consistente). El «datos hasta» de pruebas sigue siendo el 23-abr-2026 con el criterio exacto de Power BI (ORE DEMOSTRACION), lo que confirma que la actividad de pruebas posterior es de cuentas de prueba.
**Agente con datos reales:** 7/7 preguntas del ticket y 9/9 trampas sin observaciones.
**Desajuste restante → corregido en código (pendiente de reinicio):** la pregunta «escuelas con menor actividad» listaba «Escuela Demo T3 School» (1 usuario, nunca conectada). La lista de escuelas de Power BI (`Tbl_Escuelas` = `DISTINCT school_name` de `vw_connections_report`) incluye solo escuelas con **al menos una conexión**: por eso el tablero tiene **47 escuelas** y la API ~50. Ahora el universo «por escuela» de la API es el mismo: escuelas con al menos una conexión (de usuarios con escuela, rol ≠ client, fuera de ORE DEMOSTRACION). Verificado 37/37 en los tres modos. **Por confirmar tras reiniciar la API:** que el universo dé 47 escuelas, como el tablero.

## Corrección del universo de escuelas (12-nov, 18:45)
La primera hipótesis («escuelas con al menos una conexión») dio **43** escuelas, no las 47 de Power BI, y **estaba equivocada**: la lista de Power BI incluye escuelas que nunca se conectaron (Elemental Berwind, Instituto Loaiza Cordero) pero no incluye «Demo T3 School», «TCPSA» ni «Salvador Brau» (1 usuario, sin estudiantes). La lista «Escuelas en Estudia y Avanza» del tablero sale de una hoja de Excel (`Escuelas_EA_Visitadas.xlsx` en SharePoint de Require, tabla `Escuela_E&A`), no solo de MySQL. Aproximación aplicada: **escuelas con al menos un estudiante o con alguna conexión** (de usuarios con escuela y rol ≠ client), fuera de ORE DEMOSTRACION y «Facilitadores MSJ». Cuenta esperada con tu tabla 04: 63 con usuarios − 13 de prueba − Facilitadores − TCPSA − Salvador Brau = **47**. Verificado 37/37 en los tres modos. **Por confirmar al reiniciar la API:** que el universo dé 47.

## CONFIRMADO con la API real (12-nov, 18:36) — paridad de conexiones con Power BI
- Universo de escuelas: **47 = 47** (sin «Demo T3 School», «TCPSA», «Salvador Brau» ni «Facilitadores MSJ»).
- Reporte de Conexión (2026, Q3+Q4): **113 / 14 / 84** (conexiones / escuelas conectadas / usuarios conectados) = Power BI; tendencia 23 + 90 = 113.
- «Escuelas con menor actividad» (1-sep → 10-nov): 35 de 47 escuelas con 0 conexiones; el agente lo dice («hay 25 filas más con cero conexiones, el listado no es exhaustivo»).
- Agente con datos reales: **7/7** preguntas del ticket y **9/9** trampas sin observaciones.
- Pruebas: mismas filas que el modelo de Power BI (10.628; 2025-2026 P1–P3 = 6.106 / 217 / 5.889). Sigue sin explicarse solo la tarjeta de la página demo (1.029 / 206 / 823 / 135).

---

# Resultados de `sql/09_crear_vistas_agente.sql` (Workbench, 17-nov-2026)
Se ejecutaron las 20 sentencias sin errores: el esquema `agente`, 9 vistas, el usuario `agente_lector` y su permiso, y las 8 consultas de verificación. La referencia es el .pbix procesado el 5-nov (`../MSJ_Utilization_modelo_y_10_preguntas.md`).

| Comprobación | Power BI (5-nov) | Vistas `agente` (17-nov) | |
|---|---|---|---|
| B1 cancelados · sin suscripción (usuarios / filas) | 14.479 / 62.412 · 489 / 489 | igual | ✅ |
| B1 activos (usuarios / filas) | 6.988 / 9.674 | 7.277 / 9.982 | ⚠️ datos nuevos |
| B2 conexiones / usuarios (corte 4-nov) | 6.994 / 2.433 | igual | ✅ |
| B2 año 2025-2026: regla del tablero · regla ago–jul | 1.481 / 751 · 4.078 / 1.819 | igual | ✅ |
| B3 conectados (definición del tablero) | 2.432 | 2.432 | ✅ |
| B3 conectados con suscripción activa | 726 | 935 | ⚠️ datos nuevos |
| B4 pruebas por año y período (tomadas / no tomadas) | 271 / 10.357 · página 206 / 6.883 | igual, fila por fila | ✅ |
| B6 suscripciones 2022-23 a 2025-26 | 24 · 424 · 945 · 131 | igual (suscripciones, usuarios y filas) | ✅ |
| B6 suscripciones 2026-27 (suscripciones / usuarios / filas) | 290 / 6.988 / 9.674 | 301 / 7.277 / 9.982 | ⚠️ datos nuevos |

**Conclusión:** las reglas coinciden con las de Power BI. Las únicas diferencias están en la suscripción activa 2026-27, y salen del crecimiento de los datos después del corte del 5-nov: el total de activos fue 6.988 el 5-nov, 7.199 el 12-nov (servicio) y 7.277 el 17-nov. Los cancelados y los años cerrados coinciden exactos.
Cierre de la verificación (17-nov):
- #13 total de usuarios: 17.744 en la base hoy (17.679 en el .pbix del 5-nov; +65 usuarios nuevos).
- #19 promedio de % Full Quiz (Completed, sin Posprueba, 2025-2026): **60,19**, igual que el tablero ✅.
- `vw_total_users_with_connection_details` hoy: 949 conectados / 7.277 usuarios / 13,04 %. Los 7.277 coinciden con las vistas `agente` ✅. Las vistas dan 935 conectados porque B3 corta las conexiones al 4-nov; sin ese corte dan **949**, igual que la vista SQL ✅. **Las vistas `agente` quedan validadas.**

# Carga del Excel de escuelas visitadas (17-nov-2026)
Se creó `agente.escuelas_visitadas` (`sql/10`) y se cargó el Excel con `cargar_escuelas_visitadas.py`: **21 visitadas · 26 no visitadas · 47 en total**.
El Excel que leía Power BI tenía 17 visitadas. Las 4 de diferencia (Amalia Marín Castilla, Amalia Marín Elemental, Antonio Sarriera Egozcue y Arturo Morales Carrión) están marcadas como visitadas pero no tienen fecha, igual que Manuel Boada. Falta confirmar que el archivo cargado es la versión vigente del Excel.
