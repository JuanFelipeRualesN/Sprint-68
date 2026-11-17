# MSJ_Utilization: cómo se calcula cada cifra

Lectura completa del TMDL del modelo y del informe (19 páginas). No incluyo el host de MySQL, la cuenta del dueño del Excel, el nombre del gateway ni los nombres de personas que aparecen en los filtros de los visuales.

## Antes de usar estas cifras

- **“Usuarios Conectados (Activos por Rol)” no filtra por suscripción activa.** Cuenta 2.432 usuarios, pero los conectados con suscripción activa son 726. Por eso “Usuarios NO Conectados” sale subestimado.
- **El “Año académico” de las conexiones es el año calendario corrido.** Una conexión de noviembre de 2026 queda en “2025-2026”.
- **Los slicers guardados cambian mucho las cifras.** Por ejemplo, los usuarios conectados bajan de 2.433 (histórico) a 37 con el filtro de fecha guardado (1 jul–30 nov 2026). Para cada pregunta doy la cifra sin filtros y la que muestra la página.

---

## 1. Conexión

- **Origen de la lectura:** instancia local de Power BI Desktop (puerto 60054), archivo `MSJ_Utilization.pbix`. El modelo interno se llama “Model” y tiene compatibilidad 1606.
- **Último procesamiento local:** 2026-11-05 00:13 (UTC-5). La conexión más reciente en los datos es del 2026-11-04.
- **Último cambio de esquema:** 2026-06-26.
- **Fuente principal:** MySQL (Azure), base `gs_data_analysis`.
- **Servicio (vista de linaje del área de trabajo MSJ, 17-nov-2026):** `Reports_Semantic_Model` toma sus datos de dos orígenes, MySQL y el libro de Excel en SharePoint. Los dos pasan por el mismo gateway de datos on-premises (VPN), que está en línea y con la conexión correcta. Su último refresco en el servicio fue el 17/11/26 a las 00:15:38.

## 2. Tablas y relaciones

### Tablas (37)

**Vistas de MySQL (14 consultas, todas en modo Import)**

| Tabla | Filas | Columnas (tipo) | Power Query (M) |
|---|---|---|---|
| vw_connections_report | 6.994 (1 fila = usuario × día) | connected_date (fecha); user_id, role_id, school_id, ore_id (entero); username, email, first_name, last_name, role, sie, school_name, school_type, city, ore, state_iso_code, country, country_iso, school_identifier, subscription_status, Año académico (texto). Columna calculada: Full_Name | connected_date → date. subscription_status null → “sin matrícula”. Corrige “LUÍS RODRÍGUEZ CABRERO” → “LUIS RODRÍGUEZ CABRERO”. school_name con Trim + Upper. Año académico = `Text(Year(connected_date)-1) & "-" & Text(Year(connected_date))` |
| vw_not_connected | 6.262 | Las columnas base, sin connected_date, subscription_status ni Año académico. Columna calculada: Full_Name | Trim + Upper de school_name y corrección de LUIS RODRÍGUEZ CABRERO. Sin exclusiones |
| vw_users_report | 17.679 | Columnas base (user_id, username, email, first_name, last_name, role_id, role, school_id, sie, school_name, school_type, city, ore, ore_id, state_iso_code, country, country_iso, school_identifier) + Sistema educativo | Filtra role <> "client" y school_name <> "Escuela Facilitadores MSJ". Sistema educativo = “SEMI/Alianza” si school_identifier ∈ {2881, 2009, 2692, 07345, 7845, 6372}; si no, “ORE”. Trim + Upper de school_name y corrección de LUIS |
| vw_users_details_report | 72.575 (usuario × suscripción) | Columnas base + status, subscription_id (entero), plan_name, Sistema Educativo | Mismos filtros que vw_users_report. status null → “sin subscripción”. Misma regla de Sistema Educativo |
| vw_total_users_with_connection_details | 1 | total_connected (entero), total_users (entero), total_percent (decimal) | Sin transformaciones. Valores calculados en SQL: 726 / 6.988 / 10,389 |
| vw_student_teacher_relation | 1.675 | teacher_id, student_id, teacher_school_id, student_school_id (entero); el resto, texto (krono, nombres, email, school_identifier, school_name) | Trim + Upper. Excluye 9 escuelas de demostración o prueba del maestro (ver §6) y “ESCUELA DEMOSTRACIONES” del estudiante |
| vw_teacher_activities | 2.108 | Todo texto: teacher_name, sie, school, school_number, has_connections_current_period, has_created_instructional_plans_current_period, plans_have_data_current_period, has_created_groups_current_year, has_created_assignments_overall, invitation_code, ore, from_period, to_period (fechas guardadas como texto) + Período | Excluye 5 escuelas de prueba. sie null → “Unknown”. Trim + Upper. Corrige el nombre de EEMCyT. Período (regla en §2f) |
| vw_teacher_activities_Unpivot | 16.864 | Texto: teacher_name, sie, school, school_number, has_connections_current_period, Attribute_Old, Value, Attribute | Sale de vw_teacher_activities. Excluye 5 escuelas (en minúsculas, antes del Upper). Despivota los 4 indicadores. Renombra los atributos a etiquetas en español |
| vw_teacher_activities_Unpivot2 | 8.432 | Texto: teacher_name, invitation_code, sie, ore, school, school_number, from_period, to_period, has_connections_current_period, Attribute_OLD, Value, Attribute | Sale de vw_teacher_activities. Solo ore = “MUNICIPIO SAN JUAN” (filtro aplicado dos veces). Despivota y renombra |
| vw_student_active_courses | 8.491 | user_id, role, school_id (entero); invitation_code, given_name, surname, email, school_name, school_number, course, sie (texto) | Excluye 6 escuelas de prueba (en minúsculas, antes del Upper). Trim + Upper |
| vw_teacher_active_courses | 1.103 | Igual que la anterior | Excluye “Escuela” y las mismas 6 escuelas de prueba |
| vw_academics_average | 2.135 | user_id, period_id, period_no (entero); period_from, period_to, academic_year_start, academic_year_end (fecha/hora); average_score (decimal); el resto, texto | Trim + Upper y corrección de LUIS |
| vw_academics_details | 10.628 | quiz_taken_date, quiz_due_date, quiz_assigned_to_plan_date, plan_closed_date, period_from, period_to, academic_year_start, academic_year_end (fecha/hora); total_percent_full_quiz (decimal); period_no (texto); el resto, texto, incluidas Completado/No completado, total_percent_full_quiz - Copy, Sistema Educativo, academic_year y Period | Renombra note → “Completado/No completado” (valores: Completed / Not Completed). Duplica la nota. Sistema Educativo (misma regla). academic_year y Period (regla en §2f) |
| vw_subscription_profile_history_detail | 72.104 | user_id, school_id, subscription_id, plan_id (entero); activated_date, cancellation_date, user_attached_date (fecha/hora); username, user_token, given_name, surname, school_name, school_token, status, plan_name, Academic Year (texto) | Excluye “ESCUELA ESCALERA GONZÁLEZ”. Academic Year = `Year(cancellation_date)-1 & "-" & Year(cancellation_date)` |

**Tabla del Excel (1):** `Escuela_E&A`, 47 filas. El detalle está en §5.

**Tablas calculadas en DAX (5)**

| Tabla | Filas | Columnas | Definición |
|---|---|---|---|
| Valor_Si_No | 2 | Si_No (texto) | `UNION(ROW("Si_No","Si"), ROW("Si_No","No"))` |
| Tbl_Connections | 2 | Status (texto) | `DATATABLE("Status", STRING, {{"Active"},{"Canceled"}})` |
| Tbl_Fecha_Conexion | 514 | Fecha_Conexion (fecha), Año (entero), Mes (texto), Mes_Num (entero), YearMonth (texto) | `ADDCOLUMNS(DISTINCT(SELECTCOLUMNS(vw_connections_report,"Fecha_Conexion",[connected_date])), "Año",YEAR(..), "Mes",FORMAT(..,"MMMM"), "Mes_Num",MONTH(..), "YearMonth",FORMAT(..,"YYYY-MM"))`. Solo incluye los días con conexiones; no es un calendario continuo |
| TBL_All_Users_Connections | 8.695 | first_name, last_name, role, school_name, sie, email, Status (texto) | `DISTINCT(UNION(SELECTCOLUMNS(vw_connections_report, …, "Status","Connected"), SELECTCOLUMNS(vw_not_connected, …, "Status","Not Connected")))` |
| Tbl_Escuelas | 44 | Escuela (texto) | `DISTINCT(SELECTCOLUMNS(vw_connections_report,"Escuela",[school_name]))` |

**Tablas de fecha automáticas (17, ocultas):** 16 LocalDateTable_* y 1 DateTableTemplate_*, que crea la opción de fecha/hora automática. Ninguna tabla está marcada como tabla de fechas.

### Relaciones (36)

16 relaciones van de columnas de fecha a las LocalDateTable automáticas. Todas son 1:N, con filtro simple y activas. Salen de Tbl_Fecha_Conexion[Fecha_Conexion], de vw_academics_average (period_from, period_to, academic_year_start, academic_year_end), de vw_academics_details (sus 8 fechas) y de vw_subscription_profile_history_detail (activated_date, cancellation_date, user_attached_date). Estas son las otras 20:

| Desde (muchos) | Hacia | Cardinalidad | Dirección | Activa |
|---|---|---|---|---|
| vw_connections_report[connected_date] | Tbl_Fecha_Conexion[Fecha_Conexion] | N:1 | Simple | Sí |
| vw_connections_report[school_name] | Tbl_Escuelas[Escuela] | N:1 | Simple | Sí |
| vw_connections_report[school_name] | vw_users_report[school_name] | N:N | Simple (users_report filtra connections) | Sí |
| vw_users_details_report[user_id] | vw_users_report[user_id] | N:1 | Simple | Sí |
| vw_users_details_report[email] | vw_users_report[email] | N:N | Ambas | Sí |
| vw_users_report[school_identifier] | Escuela_E&A[school_identifier] | N:1 | Ambas | Sí |
| vw_not_connected[school_name] | Escuela_E&A[school_name] | N:1 | Simple | Sí |
| vw_not_connected[school_name] | Tbl_Escuelas[Escuela] | N:1 | Simple | Sí |
| vw_not_connected[role] | vw_users_report[role] | N:N | Simple (users_report filtra not_connected) | Sí |
| vw_student_active_courses[user_id] | vw_users_report[user_id] | N:1 | Simple | Sí |
| vw_teacher_active_courses[user_id] | vw_users_report[user_id] | N:1 | Simple | Sí |
| vw_subscription_profile_history_detail[user_id] | vw_users_details_report[user_id] | N:N | Ambas | Sí |
| vw_subscription_profile_history_detail[Academic Year] | vw_connections_report[Año académico] | N:N | Ambas | No |
| vw_teacher_activities[has_connections_current_period] | Valor_Si_No[Si_No] | N:1 | Simple | Sí |
| vw_teacher_activities[school_number] | vw_teacher_activities_Unpivot2[school_number] | N:N | Ambas | Sí |
| vw_teacher_activities_Unpivot2[has_connections_current_period] | Valor_Si_No[Si_No] | N:1 | Simple | Sí |
| vw_teacher_activities_Unpivot2[school] | Escuela_E&A[school_name] | N:1 | Simple | Sí |
| vw_teacher_activities_Unpivot[sie] | vw_not_connected[sie] | N:1 | Simple | Sí |
| vw_teacher_activities_Unpivot[school] | Tbl_Escuelas[Escuela] | N:1 | Simple | No |
| vw_teacher_activities_Unpivot[school] | vw_teacher_activities_Unpivot2[school] | N:N | Ambas | No |

Ninguna relación une vw_academics_details ni vw_academics_average con las tablas de usuarios.

### (e) RLS

Hay un solo rol, **ViewAll**, con permiso de lectura y sin filtros de tabla. No restringe ninguna fila.

### (f) Fechas, año académico y períodos

La jerarquía Año/Trimestre/Mes/Día del informe viene de las LocalDateTable automáticas y sigue el año calendario. Cada tabla define el año académico a su manera:

| Tabla.columna | Regla exacta |
|---|---|
| vw_connections_report[Año académico] | `(YEAR(connected_date)-1) & "-" & YEAR(connected_date)`. No usa el mes: todo 2026 queda en “2025-2026”, incluso de agosto a diciembre |
| vw_subscription_profile_history_detail[Academic Year] | `(YEAR(cancellation_date)-1) & "-" & YEAR(cancellation_date)`. Funciona porque cancellation_date es la fecha de fin programada (jun–jul, 23:59:59, siempre con valor). Las suscripciones activas terminan en 2027 y quedan en “2026-2027” |
| vw_academics_details[academic_year] | Sale de academic_year_start: si el mes es ≥ 8, Y-(Y+1); si no, (Y-1)-Y. El año académico va de agosto a julio |
| vw_academics_details[Period] | `"P" & period_no`. En 2025-26: P1 = 6 ago–24 oct 2025, P2 = 25 oct 2025–23 ene 2026, P3 = 24 ene–10 abr 2026, P4 = 11 abr–5 jun 2026 |
| vw_teacher_activities[Período] | Si from_period y to_period caen en el mismo año y entre julio y diciembre → “1er Sem AAAA”. Si caen entre enero y junio → “2do Sem AAAA”. En cualquier otro caso → null |
| Escuela_E&A[Período] | Texto escrito a mano en el Excel |

## 3. Medidas en DAX (31, completas)

### vw_connections_report

```dax
Conteo_Conectados_Si =
CALCULATE(
    DISTINCTCOUNT('gs_data_analysis vw_connections_report'[Email]),
    NOT(ISBLANK('gs_data_analysis vw_connections_report'[connected_date]))
)

Conteo_Conectados_Si_Total =
COUNTROWS(
    FILTER(
        'gs_data_analysis vw_connections_report',
        NOT(ISBLANK('gs_data_analysis vw_connections_report'[connected_date]))
    )
)

Diferencia Conectados =
VAR FechaActual = MAX ( TBL_Fecha_Conexion[Fecha_Conexion] )
VAR FechaPrev = FechaActual - 1
VAR ValorActual = [Conteo_Conectados_Si_Total]
VAR ValorPrev =
    CALCULATE (
        [Conteo_Conectados_Si_Total],
        REMOVEFILTERS ( TBL_Fecha_Conexion ),          -- quita Year/Qtr/Month/Day
        TBL_Fecha_Conexion[Fecha_Conexion] = FechaPrev -- fuerza día anterior
    )
RETURN
IF ( ISBLANK ( ValorPrev ), BLANK (), ValorActual - ValorPrev )

Usuarios Conectados (Activos por Rol) =
VAR EmailsActivosPorRol =
    CALCULATETABLE (
        VALUES ( 'gs_data_analysis vw_users_details_report'[email] ),
        NOT ISBLANK ( 'gs_data_analysis vw_users_details_report'[email] )
    )
RETURN
CALCULATE (
    DISTINCTCOUNT ( 'gs_data_analysis vw_connections_report'[email] ),
    TREATAS ( EmailsActivosPorRol, 'gs_data_analysis vw_connections_report'[email] )
)

Conectado único =
CALCULATE(DISTINCTCOUNT('gs_data_analysis vw_connections_report'[Email]))
```

### vw_not_connected

```dax
Conteo_Conectados_No =
DISTINCTCOUNT('gs_data_analysis vw_not_connected'[email])

Usuarios NO Conectados (Activos por Rol) =
VAR TotalActivos = [Total Usuarios Activos (por Rol)]
VAR Conectados = [Usuarios Conectados (Activos por Rol)]
RETURN
MAX ( 0, TotalActivos - Conectados )
```

### vw_total_users_with_connection_details

```dax
% Usuarios Conectados =
DIVIDE ( [Usuarios Conectados (Activos por Rol)], [Total Usuarios Activos (por Rol)] )

% usuarios conectados por escuela =
('gs_data_analysis vw_connections_report'[Conteo_Conectados_Si])/('gs_data_analysis vw_users_details_report'[Usuarios Activos])*100
```

### vw_users_report

```dax
distinct_escuela = DISTINCTCOUNT('gs_data_analysis vw_users_report'[school_identifier])

distinct_user = DISTINCTCOUNT('gs_data_analysis vw_users_report'[user_id])

distinct_sin_subscripción_null =
CALCULATE(
    DISTINCTCOUNT('gs_data_analysis vw_users_details_report'[user_id]),
    'gs_data_analysis vw_users_details_report'[status] = "sin subscripción"
)

distinct_Actives =
CALCULATE(
    DISTINCTCOUNT('gs_data_analysis vw_users_details_report'[user_id]),
    'gs_data_analysis vw_users_details_report'[status] = "active"
)

distinct_Inactives_canceled =
CALCULATE(
    DISTINCTCOUNT('gs_data_analysis vw_users_details_report'[user_id]),
    'gs_data_analysis vw_users_details_report'[status] = "canceled"
)
```

### vw_users_details_report

```dax
Total Usuarios Activos (por Rol) =
CALCULATE (
    DISTINCTCOUNT ( 'gs_data_analysis vw_users_details_report'[email] ),
    'gs_data_analysis vw_users_details_report'[status] = "active",
    NOT ISBLANK ( 'gs_data_analysis vw_users_details_report'[email] ),
    REMOVEFILTERS ( 'gs_data_analysis vw_connections_report' ),
    REMOVEFILTERS ( 'gs_data_analysis vw_not_connected' )
)

Total Usuarios = DISTINCTCOUNT('gs_data_analysis vw_users_details_report'[user_id])

Usuarios Activos =
CALCULATE(
    DISTINCTCOUNT('gs_data_analysis vw_users_details_report'[user_id]),
    'gs_data_analysis vw_users_details_report'[status] = "active"
)

Usuarios Inactivos_canceled =
CALCULATE(
    DISTINCTCOUNT('gs_data_analysis vw_users_details_report'[user_id]),
    'gs_data_analysis vw_users_details_report'[status] = "canceled"
)

Usuarios Sin subscripción_null =
CALCULATE(
    DISTINCTCOUNT('gs_data_analysis vw_users_details_report'[user_id]),
    'gs_data_analysis vw_users_details_report'[status] = "sin subscripción"
)

Total Escuelas = DISTINCTCOUNT('gs_data_analysis vw_users_details_report'[school_identifier])
```

### vw_teacher_activities_Unpivot / Unpivot2

```dax
Conteo_No =
CALCULATE(
    COUNT('gs_data_analysis vw_teacher_activities_Unpivot'[SIE]),
    'gs_data_analysis vw_teacher_activities_Unpivot'[Value] = "No")

Conteo_SI =
CALCULATE(
    COUNT('gs_data_analysis vw_teacher_activities_Unpivot'[SIE]),
    'gs_data_analysis vw_teacher_activities_Unpivot'[Value] = "Si")

Conteo_SI2 =
CALCULATE(
    COUNT('gs_data_analysis vw_teacher_activities_Unpivot2'[school_number]),
    'gs_data_analysis vw_teacher_activities_Unpivot2'[Value] = "Si")

Conteo_No2 =
CALCULATE(
    COUNT('gs_data_analysis vw_teacher_activities_Unpivot2'[school_number]),
    'gs_data_analysis vw_teacher_activities_Unpivot2'[Value] = "No")
```

### Escuela_E&A

```dax
Visitadas_Si = CALCULATE ( COUNTROWS ( 'Escuela_E&A' ), 'Escuela_E&A'[Visitada (Si/No)] = "Si" )

Visitadas_No = CALCULATE ( COUNTROWS ( 'Escuela_E&A' ), 'Escuela_E&A'[Visitada (Si/No)] = "No" )

Escuela Visitada (Texto) =
IF ( SELECTEDVALUE ( 'Escuela_E&A'[Visitada (Si/No)]) = "Si", "Visitada", "No visitada" )

Total escuelas fijo = CALCULATE( [distinct_escuela], REMOVEFILTERS() )
```

### vw_academics_details

```dax
% Full Quiz (promedio) = AVERAGE ( 'gs_data_analysis vw_academics_details'[total_percent_full_quiz] )

Quizz_taken =
CALCULATE (
    COUNTROWS ( 'gs_data_analysis vw_academics_details' ),
    'gs_data_analysis vw_academics_details'[Completado/No completado] = "Completed"
)

Quizz_not_taken =
CALCULATE (
    COUNTROWS ( 'gs_data_analysis vw_academics_details' ),
    'gs_data_analysis vw_academics_details'[Completado/No completado] = "Not Completed"
)
```

### Columnas calculadas en DAX

```dax
vw_connections_report[Full_Name] =
TRIM('gs_data_analysis vw_connections_report'[last_name] & ", " & 'gs_data_analysis vw_connections_report'[first_name])

vw_not_connected[Full_Name] =
TRIM(COALESCE('gs_data_analysis vw_not_connected'[last_name], "") & ", " & COALESCE('gs_data_analysis vw_not_connected'[first_name], ""))
```

### Cálculos visuales (no son medidas del modelo)

- **“% Conexión”** (página Usuarios_Conexiones): `DISTINCTCOUNT(connections[email]) / SUM(vw_total[total_users])`.
- **“% Conexión Global”:** `[Conectado único] / [Total Usuarios Activos (por Rol)]`.
- **“Pruebas asignadas”:** `[Quizz_taken] + [Quizz_not_taken]`.

## 4. Las 10 preguntas

“Sin filtros” quiere decir sin ningún slicer. “Página” quiere decir con los filtros y slicers guardados en el .pbix.

### P1. Total de usuarios y usuarios con suscripción activa

- **Medidas:** `Total Usuarios` (DISTINCTCOUNT de user_id en vw_users_details_report) y `Usuarios Activos` (lo mismo, con status = “active”).
- **Filtros implícitos en M:** se excluyen role = client y la escuela “Escuela Facilitadores MSJ”.
- **Sin filtros:** Total Usuarios = **17.679**. Usuarios Activos = **6.988**. `Total Usuarios Activos (por Rol)`, que cuenta emails, también da 6.988.
- **Por status (usuarios distintos / filas):** active 6.988 / 9.674 · canceled 14.479 / 62.412 · sin subscripción 489 / 489. Las categorías se solapan: 4.277 usuarios tienen a la vez una suscripción activa y una cancelada.
- **En el informe:** las tarjetas “Total de usuarios” y “Total de usuarios matrícula activa” de la página Usuarios_Conexiones usan `vw_total_users_with_connection_details[total_users]` = 6.988, un valor fijo calculado en SQL que no responde a los slicers. En “Reporte de Usuarios”, con los slicers guardados (status = active y Academic Year 2026-2027), la tarjeta Total Usuarios muestra 6.988.

### P2. % de usuarios conectados por escuela: las 10 más bajas

- **Medida:** `% usuarios conectados por escuela` = Conteo_Conectados_Si / Usuarios Activos × 100. Ningún visual del informe la usa.
- **Visual equivalente:** la barra 100 % apilada “Cantidad y porcentaje de usuarios conectados por Escuela”. Su eje es vw_users_report[school_name] y usa las medidas Conectados (Activos por Rol) / NO Conectados (Activos por Rol). Da los mismos porcentajes.
- **Filtros:** el numerador cuenta usuarios conectados de cualquier status y en cualquier fecha; el denominador cuenta solo los activos.

| Escuela | Conectados | Activos | % |
|---|---|---|---|
| ELEMENTAL BERWIND | 0 (vacío) | 123 | vacío (0 %) |
| GABRIELA MISTRAL - 61382 | 0 | 138 | vacío (0 %) |
| INSTITUTO LOAIZA CORDERO | 0 | 228 | vacío (0 %) |
| INÉS M MENDOZA VDA. DE MUÑOZ MARIN62901 | 0 | 124 | vacío (0 %) |
| PACHÍN MARÍN | 1 | 250 | 0,40 % |
| UNIVERSITY GARDENS | 2 | 378 | 0,53 % |
| AMALIA MARÍN ELEMENTAL | 1 | 172 | 0,58 % |
| RAMÓN VILA MAYO | 1 | 152 | 0,66 % |
| ARTURO MORALES CARRIÓN | 1 | 150 | 0,67 % |
| LAS AMÉRICAS | 1 | 147 | 0,68 % |

**En la página:** “% de Conexiones redesign” tiene guardado el slicer de fecha 2026 T3–T4 sin diciembre (1 jul–30 nov 2026). Con ese filtro casi todas las escuelas quedan cerca de 0 %. No lo calculé escuela por escuela.

### P3. Usuarios activos que no se han conectado, por rol

- **Medida:** `Usuarios NO Conectados (Activos por Rol)` = MAX(0, Total Usuarios Activos (por Rol) − Usuarios Conectados (Activos por Rol)). El eje es vw_users_report[role].
- **Ojo:** el término “Conectados” no filtra por status = active.

| Rol | Activos | Conectados | NO conectados (sin filtros) | NO conectados (página, 1 jul–30 nov 2026) |
|---|---|---|---|---|
| student | 6.393 | 1.928 | 4.465 | 6.385 |
| teacher | 527 | 483 | 44 | 500 |
| director | 68 | 18 | 50 | 66 |
| admin-ore | (vacío) | 3 | 0 | 0 |
| **Total** | **6.988** | **2.432** | **4.556** | **6.951** |

Si solo se cuentan los conectados con suscripción activa (726), los no conectados reales son **6.262**. Esa cifra coincide con Conteo_Conectados_No y con el número de filas de vw_not_connected.

### P4. Evolución de las conexiones por fecha en el último año académico

- **Medidas:** Conteo_Conectados_Si (usuarios distintos) y Conteo_Conectados_Si_Total (filas). Esta última es la que muestra el gráfico “Conexiones Por Fecha”. El eje es la jerarquía de fecha de Tbl_Fecha_Conexion.
- **Página:** el slicer guardado es 2026 T3–T4, y “Diferencia en Conectados” tiene el filtro Año = 2026.
- **Según la columna del modelo:** el último año es “2025-2026”, que en realidad es el año calendario 2026 (4 ene–4 nov). Da **1.481 conexiones / 751 usuarios distintos**.
- **Según el año académico real (ago–jul):** 2025-26 tiene 4.078 conexiones / 1.819 usuarios. El 2026-27 en curso (ago–nov 2026) lleva 46 / 35.
- **Mes a mes (conexiones / usuarios distintos):**
  - 2025: ago 278/243 · sep 343/235 · oct 817/506 · nov 679/612 · dic 526/409.
  - 2026: ene 450/346 · feb 411/290 · mar 244/181 · abr 302/149 · may 19/18 · jun 7/6 · jul 2/2 · ago 11/11 · sep 10/8 · oct 19/18 · nov 6/6.
- **Histórico total:** 6.994 conexiones / 2.433 usuarios, desde el 2023-06-06.

### P5. Escuelas visitadas y no visitadas, con la última visita

- **Medidas:** Visitadas_Si y Visitadas_No (COUNTROWS sobre Escuela_E&A). La columna es Fecha Ultima Visita (texto con formato M/D/AAAA).
- **Resultado:** **17 visitadas, 30 no visitadas, 47 en total.**

| Escuela visitada | Última visita |
|---|---|
| ALBERT EINSTEIN | 12/3/2025 |
| CARMEN GÓMEZ TEJERA | 9/25/2025 |
| CENTRO EUGENIO MARÍA DE HOSTOS | 12/5/2025 |
| ESCUELA DEL DEPORTE DE SAN JUAN | 1/21/2026 |
| EEMCyT | 9/17/2025 |
| JOSÉ JULIÁN ACOSTA | 9/19/2025 |
| JUAN ANTONIO CORRETJER | 1/26/2026 |
| LEAP STEAM | 9/24/2025 (anterior a su visita inicial, 9/26/2025) |
| LUIS RODRÍGUEZ CABRERO | 2/12/2026 |
| MANUEL BOADA | (vacía) |
| RAFAEL MARÍA DE LABRA | 10/7/2025 |
| RAMÓN POWER Y GIRALT | 3/5/2026 |
| REPÚBLICA DEL PERÚ | 10/29/2025 |
| SUPERIOR BERWIND | 11/6/2025 |
| THE SCHOOL OF SAN JUAN | 12/8/2025 |
| THE UNITED SCHOOL OF SAN JUAN | 12/9/2025 |
| VIMENTI | 2/10/2026 |

Las 30 escuelas no visitadas tienen la fecha vacía.

### P6. Usuarios con suscripción cancelada y usuarios sin suscripción

- **Medidas:** `Usuarios Inactivos_canceled` (equivale a distinct_Inactives_canceled, la que usa la tarjeta) y `Usuarios Sin subscripción_null`.
- **Sin filtros:** cancelados = **14.479**. Incluye 4.277 usuarios que además tienen una suscripción activa; los que solo tienen canceladas son 10.202. Sin suscripción = **489**.
- **En la página “Reporte de Usuarios”** (slicers guardados: Academic Year 2026-2027 y status = active): la tarjeta “Inactivos” muestra 4.277 y la tarjeta “Sin Subscripción” sale vacía.

### P7. Promedio de % Full Quiz por escuela y por materia

- **Medida:** `% Full Quiz (promedio)` = AVERAGE(total_percent_full_quiz). Los visuales usan el promedio implícito de esa columna, que da lo mismo.
- **Filtros de la página “Reporte Académico”:** Completado = “Completed”; quiz_title ≠ “Posprueba”; slicer academic_year = 2025-2026. El filtro ore = MUNICIPIO SAN JUAN sobre vw_users_details_report no tiene efecto, porque esa tabla no se relaciona con la académica.
- **Sin filtros**, el promedio global es 1,52, porque los 10.357 quizzes “Not Completed” tienen nota 0. **Con los filtros de la página**, el global es **60,19**.
- **Por escuela (página):** THE SCHOOL OF SAN JUAN 91,89 (13 quizzes) · EEMCyT 86,53 (40) · VIMENTI 52,82 (143) · RAMÓN POWER Y GIRALT 30,00 (3) · THE UNITED SCHOOL OF SAN JUAN 14,29 (7). Las otras 9 escuelas quedan vacías.
- **Por materia (página):** Álgebra II 91,71 · English 86,40 · Geometría 61,25 · Matemáticas 51,13 · Preálgebra 40,00 · Español 25,00 · Álgebra I 14,28. Quedan vacías Preparación al Cálculo, Matemáticas Actualizadas y Trigonometría.

### P8. Quizzes tomados y no tomados por período

- **Medidas:** Quizz_taken y Quizz_not_taken, ambas con COUNTROWS.
- El filtro de página “Completed” no afecta a Quizz_not_taken, porque su CALCULATE reemplaza el filtro de esa misma columna. El filtro de Posprueba sí la afecta.

| Año | Período | Tomados | No tomados | Página (2025-26, sin Posprueba): tomados / no tomados |
|---|---|---|---|---|
| 2024-25 | P2 | 0 | 117 | – |
| 2024-25 | P3 | 19 | 393 | – |
| 2024-25 | P4 | 35 | 2.947 | – |
| 2025-26 | P1 | 51 | 1.333 | 46 / 1.316 |
| 2025-26 | P2 | 152 | 1.831 | 146 / 1.831 |
| 2025-26 | P3 | 14 | 2.725 | 14 / 2.725 |
| 2025-26 | P4 | 0 | 1.011 | 0 / 1.011 |
| **Total** | | **271** | **10.357** | **206 / 6.883** |

### P9. Usuarios conectados por ciudad y por ORE

- **Medida:** `Conectado único` = DISTINCTCOUNT(email) en vw_connections_report. Las columnas son city y ore de esa misma tabla.
- **Cada dimensión tiene un solo valor:** city = San Juan, con 2.433 usuarios / 6.994 conexiones, y ore = MUNICIPIO SAN JUAN, con las mismas cifras. Con un solo valor no hay ranking que hacer.
- **Con el slicer guardado de la página** (1 jul–30 nov 2026): 37.

### P10. Suscripciones activadas y canceladas por año académico

- **Tabla:** vw_subscription_profile_history_detail. Cada fila es un usuario × una suscripción.
- **Visual:** cuenta usuarios distintos por Academic Year, la columna que sale de cancellation_date.

| Año académico | Activadas (regla ago–jul sobre activated_date): suscripciones / usuarios / filas | Por el Academic Year del modelo (fin o cancelación): suscripciones / usuarios / filas |
|---|---|---|
| 2022-2023 | 73 / 1.328 / 5.559 | 24 / 1.035 / 1.241 (canceled) |
| 2023-2024 | 375 / 3.585 / 4.921 | 424 / 4.677 / 9.239 (canceled) |
| 2024-2025 | 945 / 10.327 / 47.077 | 945 / 10.327 / 47.077 (canceled) |
| 2025-2026 | 131 / 4.534 / 4.873 | 131 / 4.534 / 4.873 (canceled) |
| 2026-2027 | 290 / 6.988 / 9.674 | 290 / 6.988 / 9.674 (status active; fin programado entre el 31 may y el 31 jul de 2027) |

- cancellation_date siempre tiene valor: es la fecha de fin de la suscripción, no un evento de cancelación.
- Las páginas de historial tienen guardado el slicer Academic Year = 2026-2027.

## 5. Excel: Escuela_E&A

- **Origen:** `Web.Contents("https://requiretechnology-my.sharepoint.com/personal/<usuario>_requiretechnology_com/Documents/Microsoft%20Teams%20Chat%20Files/Escuelas_EA_Visitadas.xlsx")`. Oculté el segmento de usuario porque identifica a una persona. El archivo está en el OneDrive personal de alguien, dentro de “Microsoft Teams Chat Files”.
- **Objeto leído:** la tabla `Table1` (Kind = Table).
- **Transformación:** solo pasa school_name a mayúsculas. No cambia los tipos, así que todas las columnas son texto, incluidas las fechas y school_id.
- **Columnas:** school_name, school_identifier, school_id, Sistema Educativo, Visitada (Si/No), Fecha Visita Inicial, Fecha Ultima Visita, Período. **Filas:** 47.

| school_name | school_identifier | school_id | Sistema Educativo | Visitada | Fecha Visita Inicial | Fecha Ultima Visita | Período |
|---|---|---|---|---|---|---|---|
| ALBERT EINSTEIN | 62984 | 392 | ORE | Si | 9/16/2025 | 12/3/2025 | 1er Sem 2024-2025 |
| ESCUELA DEL DEPORTE DE SAN JUAN | 2692 | 4 | SEMI/ALIANZA | Si | 10/10/2025 | 1/21/2026 | 2do Sem 2025-2026 |
| VIMENTI | 6372 | 44 | SEMI/ALIANZA | Si | 8/22/2025 | 2/10/2026 | 2do Sem 2025-2026 |
| ABRAHAM LINCOLN | 61598 | 24 | ORE | No | | | |
| UNIVERSITY GARDENS | 61531 | 16 | ORE | No | | | |

- **Relaciones:** vw_users_report[school_identifier] → Escuela_E&A[school_identifier] (N:1, ambas direcciones); vw_not_connected[school_name] → [school_name] (N:1); vw_teacher_activities_Unpivot2[school] → [school_name] (N:1).
- **Refresco y gateway:** según la vista de linaje del servicio (17-nov-2026), el Excel pasa por el mismo gateway on-premises (VPN) que MySQL, y la conexión está bien.

## 6. Reglas de negocio ocultas

1. **Exclusiones en Power Query.**
   - Usuarios (vw_users_report y vw_users_details_report): se excluyen role = “client” y la escuela “Escuela Facilitadores MSJ”. La comparación distingue mayúsculas y se hace antes de pasar el texto a mayúsculas.
   - Conexiones y no conectados: no hay exclusiones en M. La escuela Facilitadores solo se quita con un filtro de la página Usuarios_Conexiones; tiene 1 conexión.
   - Escuelas de prueba excluidas, con variaciones según la tabla: ESCUELA, CALIDAD GRUPO REQUIRE, CALIDAD IT REQUIRE PUERTO RICO, DEMO, DEMOSTRACIONES, EQUIPO REQUIRE, ESCALERA GONZÁLEZ, PRUEBAS REZAGO, SCHOOL SUBJECT MATTER EXPERTS (SMEs). En vw_teacher_activities la comparación usa literales en MAYÚSCULAS antes del Upper; en Unpivot y en active_courses usa minúsculas. Que la exclusión funcione depende de cómo venga el texto de la fuente.
   - vw_subscription_profile_history_detail excluye ESCUELA ESCALERA GONZÁLEZ.
   - vw_teacher_activities_Unpivot2 solo incluye el ORE MUNICIPIO SAN JUAN.
2. **“Activo” quiere decir status = “active” en vw_users_details_report**, que es el estado de la suscripción. Un status null pasa a “sin subscripción”. En conexiones, subscription_status es el estado en el momento de conectarse: active 6.651 filas, inactive 335, null → “sin matrícula” 8.
3. **“Usuarios Conectados (Activos por Rol)” no filtra activos, aunque el nombre lo diga.** Cuenta los emails conectados de cualquier usuario (2.432). La vista SQL vw_total_users_with_connection_details sí lo hace bien: 726 conectados activos de 6.988 (10,39 %). Esas tarjetas no tienen relaciones, así que no responden a ningún slicer.
4. **Usuarios conectados frente a conexiones.** Conteo_Conectados_Si y Conectado único cuentan emails distintos (2.433). Conteo_Conectados_Si_Total cuenta filas (6.994); cada fila es un usuario en un día, no una sesión. “Escuelas conectadas” = DISTINCTCOUNT(school_identifier) = 44.
5. **Sistema Educativo:** los school_identifier 2881, 2009, 2692, 07345, 7845 y 6372 son “SEMI/Alianza”; el resto es “ORE”. Esta regla está fija en el M de tres tablas. El Excel tiene su propia columna.
6. **Slicers y filtros guardados que cambian las cifras.**
   - Usuarios_Conexiones: Fecha = 2026 T3+T4 y Escuela ≠ FACILITADORES. Con eso la página muestra 48 conexiones, 37 usuarios, 10 escuelas, % Conexión 0,53 % y NO conectados 6.951.
   - % de Conexiones redesign: 2026 T3–T4 sin diciembre.
   - Conexiones Acumulativo - redesign: jul–oct y Año 2026.
   - Teachers_Activity (administrative): ore = MUNICIPIO SAN JUAN y período del 2026-09-12 al 2026-10-15.
   - Reporte de Usuarios: status = active (desde un slicer titulado “Año académico”) y Academic Year 2026-2027.
   - Reporte Académico: Completed, sin Posprueba, AY 2025-2026.
   - La página oculta Teachers_Activity excluye por nombre 5 cuentas de maestro de prueba. No incluyo los nombres.
7. **Relaciones bidireccionales N:N** (users_details↔users_report por email y subscription_history↔users_details por user_id). Por ellas, el slicer Academic Year filtra a los usuarios. La relación entre Academic Year y el Año académico de conexiones está inactiva, así que ese slicer no filtra las conexiones.
8. **“Diferencia Conectados” compara con el día calendario anterior.** Como Tbl_Fecha_Conexion solo tiene los días con conexiones, la medida da vacío después de cualquier día sin conexiones.
9. **Datos sospechosos.** LEAP STEAM tiene la última visita antes que la inicial. El Período del Excel (“1er Sem 2024-2025” para visitas de 2025) no cuadra con las fechas. El Año académico de conexiones no usa el mes (ver §2f).

## 7. Lo que no pude leer y por qué

| Punto | Estado |
|---|---|
| Programación de refresco | Pendiente. Solo está en la configuración del dataset en el servicio. La vista de linaje sugiere un refresco diario de madrugada (último: 17/11/26 00:15:38), pero no muestra la programación |
| Gateway del dataset y del Excel | **Resuelto con la vista de linaje:** los dos orígenes (MySQL y Excel) pasan por el mismo gateway on-premises (VPN), que está en línea y con la conexión correcta |
| Fecha del último refresco en el servicio | **Resuelto:** 17/11/26 00:15:38. El procesamiento local del .pbix es del 2026-11-05 00:13, así que las cifras de este documento corresponden a ese corte |
| ¿Este .pbix es el `Reports_Semantic_Model` del área MSJ? | Probable, pero sin verificar: los orígenes del linaje (MySQL `gs_data_analysis` y el mismo Excel de SharePoint) coinciden con los del TMDL. Para confirmarlo hay que comparar el esquema en el servicio |
| El SQL de las vistas `vw_*` | Pendiente. Las reglas que viven en MySQL (por ejemplo, cómo se calcula total_connected o qué es una “conexión”) no se ven desde el modelo. Ver `agente_foundry/sql/08_leer_vistas_de_power_bi.sql` |
| % por escuela de P2 con el slicer de fecha de la página | No lo calculé escuela por escuela; solo está la cifra sin ese filtro |
