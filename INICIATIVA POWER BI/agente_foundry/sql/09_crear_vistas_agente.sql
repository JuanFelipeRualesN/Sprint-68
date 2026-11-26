-- =====================================================================================
-- 09 · VISTAS DEL AGENTE: MISMAS REGLAS QUE POWER BI, SIN PASAR POR POWER BI   (REQ-3741)
-- ATENCIÓN: a diferencia de los scripts 01–08, la PARTE A NO es solo de lectura: crea un esquema, vistas y un usuario.
-- La ejecuta un administrador del MySQL de Azure. No toca ni modifica nada de gs_data_analysis: solo LEE sus vw_*.
--
-- Qué hace: crea el esquema `agente` con vistas que leen las mismas vw_* que el modelo Reports_Semantic_Model
-- y les aplican en SQL las reglas que Power BI aplica en Power Query (M). Referencia de cada regla:
-- ../../MSJ_Utilization_modelo_y_10_preguntas.md (§2 y §6).
-- Diferencias deliberadas con el M:
--   · Las comparaciones no distinguen mayúsculas (collation *_ci), así que no dependen de cómo venga el texto.
--   · Con null, el M deja pasar la fila en «role <> "client"»; aquí también (IS NULL OR …).
--   · El año académico va de agosto a julio (columna anio_academico). La versión del tablero se conserva en *_tablero.
--   · No se exponen nombres, apellidos, username ni tokens: el agente solo da agregados. email y user_id quedan para contar.
-- Si un CREATE VIEW falla por «Unknown column», la vista de origen tiene otro nombre de columna: mira
-- DESCRIBE <vista> (script 08) y ajusta ese nombre.
-- =====================================================================================

-- ─────────────────────────────── PARTE A · CREAR (administrador) ───────────────────────────────
SET NAMES utf8mb4;   -- obligatorio: con otro juego de caracteres el texto «sin subscripción» queda mal guardado en la vista
CREATE SCHEMA IF NOT EXISTS agente DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;

-- A1) Sistema Educativo: los school_identifier SEMI/Alianza (en Power BI están fijos en el M de 3 tablas)
CREATE OR REPLACE SQL SECURITY DEFINER VIEW agente.sistema_educativo AS
SELECT '2881' AS school_identifier, 'SEMI/Alianza' AS sistema_educativo
UNION ALL SELECT '2009',  'SEMI/Alianza'
UNION ALL SELECT '2692',  'SEMI/Alianza'
UNION ALL SELECT '07345', 'SEMI/Alianza'
UNION ALL SELECT '7845',  'SEMI/Alianza'
UNION ALL SELECT '6372',  'SEMI/Alianza';

-- A2) Usuarios (P1, P3). Origen: vw_users_report. Excluye client y Escuela Facilitadores MSJ.
CREATE OR REPLACE SQL SECURITY DEFINER VIEW agente.usuarios AS
SELECT u.user_id, u.email, u.role_id, u.role, u.school_id, u.sie,
       CASE WHEN UPPER(TRIM(u.school_name)) = 'LUÍS RODRÍGUEZ CABRERO' THEN 'LUIS RODRÍGUEZ CABRERO'
            ELSE UPPER(TRIM(u.school_name)) END AS school_name,
       u.school_type, u.city, u.ore, u.ore_id, u.state_iso_code, u.country, u.country_iso, u.school_identifier,
       COALESCE(se.sistema_educativo, 'ORE') AS sistema_educativo
FROM gs_data_analysis.vw_users_report u
LEFT JOIN agente.sistema_educativo se ON se.school_identifier = CAST(u.school_identifier AS CHAR)
WHERE (u.role IS NULL OR u.role <> 'client')
  AND (u.school_name IS NULL OR TRIM(u.school_name) <> 'Escuela Facilitadores MSJ');

-- A3) Usuario × suscripción (P1, P2, P3, P6). Origen: vw_users_details_report. Mismos filtros. status null → «sin subscripción».
CREATE OR REPLACE SQL SECURITY DEFINER VIEW agente.usuarios_suscripciones AS
SELECT u.user_id, u.email, u.role_id, u.role, u.school_id, u.sie,
       CASE WHEN UPPER(TRIM(u.school_name)) = 'LUÍS RODRÍGUEZ CABRERO' THEN 'LUIS RODRÍGUEZ CABRERO'
            ELSE UPPER(TRIM(u.school_name)) END AS school_name,
       u.city, u.ore, u.ore_id, u.school_identifier,
       COALESCE(u.status, 'sin subscripción') AS status, u.subscription_id, u.plan_name,
       COALESCE(se.sistema_educativo, 'ORE') AS sistema_educativo
FROM gs_data_analysis.vw_users_details_report u
LEFT JOIN agente.sistema_educativo se ON se.school_identifier = CAST(u.school_identifier AS CHAR)
WHERE (u.role IS NULL OR u.role <> 'client')
  AND (u.school_name IS NULL OR TRIM(u.school_name) <> 'Escuela Facilitadores MSJ');

-- A4) Conexiones (P2, P3, P4, P9). Origen: vw_connections_report. 1 fila = usuario × día.
--     Sin exclusiones en el M; la página excluye Facilitadores con un filtro → aquí queda como columna es_facilitadores.
CREATE OR REPLACE SQL SECURITY DEFINER VIEW agente.conexiones AS
SELECT DATE(c.connected_date) AS connected_date,
       c.user_id, c.email, c.role_id, c.role, c.school_id, c.sie,
       CASE WHEN UPPER(TRIM(c.school_name)) = 'LUÍS RODRÍGUEZ CABRERO' THEN 'LUIS RODRÍGUEZ CABRERO'
            ELSE UPPER(TRIM(c.school_name)) END AS school_name,
       c.school_type, c.city, c.ore, c.ore_id, c.school_identifier,
       COALESCE(c.subscription_status, 'sin matrícula') AS subscription_status,
       CASE WHEN MONTH(c.connected_date) >= 8
            THEN CONCAT(YEAR(c.connected_date), '-', YEAR(c.connected_date) + 1)
            ELSE CONCAT(YEAR(c.connected_date) - 1, '-', YEAR(c.connected_date)) END AS anio_academico,
       CONCAT(YEAR(c.connected_date) - 1, '-', YEAR(c.connected_date))               AS anio_academico_tablero,
       (UPPER(TRIM(c.school_name)) = 'ESCUELA FACILITADORES MSJ')                     AS es_facilitadores
FROM gs_data_analysis.vw_connections_report c
WHERE c.connected_date IS NOT NULL;

-- A5) No conectados (P3, definición SQL). Origen: vw_not_connected. Sin exclusiones.
CREATE OR REPLACE SQL SECURITY DEFINER VIEW agente.no_conectados AS
SELECT n.user_id, n.email, n.role_id, n.role, n.school_id, n.sie,
       CASE WHEN UPPER(TRIM(n.school_name)) = 'LUÍS RODRÍGUEZ CABRERO' THEN 'LUIS RODRÍGUEZ CABRERO'
            ELSE UPPER(TRIM(n.school_name)) END AS school_name,
       n.city, n.ore, n.ore_id, n.school_identifier
FROM gs_data_analysis.vw_not_connected n;

-- A6) Pruebas (P7, P8). Origen: vw_academics_details. El M no la filtra: renombra note, calcula año académico,
--     período y Sistema Educativo. Se usa d.* para no depender de nombres de columna que no se ven en el modelo.
CREATE OR REPLACE SQL SECURITY DEFINER VIEW agente.pruebas AS
SELECT d.*,
       d.note                                         AS completado,      -- 'Completed' / 'Not Completed'
       CONCAT('P', d.period_no)                       AS periodo,
       CASE WHEN MONTH(d.academic_year_start) >= 8
            THEN CONCAT(YEAR(d.academic_year_start), '-', YEAR(d.academic_year_start) + 1)
            ELSE CONCAT(YEAR(d.academic_year_start) - 1, '-', YEAR(d.academic_year_start)) END AS anio_academico,
       COALESCE(se.sistema_educativo, 'ORE')          AS sistema_educativo,
       (COALESCE(d.quiz_title, '') = 'Posprueba')     AS es_posprueba
FROM gs_data_analysis.vw_academics_details d
LEFT JOIN agente.sistema_educativo se ON se.school_identifier = CAST(d.school_identifier AS CHAR);

-- A7) Historial de suscripciones (P10). Origen: vw_subscription_profile_history_detail. Excluye Escalera González.
CREATE OR REPLACE SQL SECURITY DEFINER VIEW agente.suscripciones AS
SELECT s.user_id, s.school_id, s.subscription_id, s.plan_id, s.plan_name, s.status,
       UPPER(TRIM(s.school_name)) AS school_name,
       s.activated_date, s.cancellation_date, s.user_attached_date,
       CASE WHEN MONTH(s.activated_date) >= 8
            THEN CONCAT(YEAR(s.activated_date), '-', YEAR(s.activated_date) + 1)
            ELSE CONCAT(YEAR(s.activated_date) - 1, '-', YEAR(s.activated_date)) END AS anio_academico_activacion,
       CONCAT(YEAR(s.cancellation_date) - 1, '-', YEAR(s.cancellation_date))          AS anio_academico_fin   -- = «Academic Year» del tablero
FROM gs_data_analysis.vw_subscription_profile_history_detail s
WHERE s.school_name IS NULL OR UPPER(TRIM(s.school_name)) <> 'ESCUELA ESCALERA GONZÁLEZ';

-- A8) Calendario continuo 2022-01-01 → 2029-12-31 (Tbl_Fecha_Conexion solo tiene días con conexiones)
CREATE OR REPLACE SQL SECURITY DEFINER VIEW agente.digitos AS
SELECT 0 AS n UNION ALL SELECT 1 UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4
UNION ALL SELECT 5 UNION ALL SELECT 6 UNION ALL SELECT 7 UNION ALL SELECT 8 UNION ALL SELECT 9;

CREATE OR REPLACE SQL SECURITY DEFINER VIEW agente.calendario AS
SELECT f.fecha, YEAR(f.fecha) AS anio, MONTH(f.fecha) AS mes, DATE_FORMAT(f.fecha, '%Y-%m') AS anio_mes,
       CASE WHEN MONTH(f.fecha) >= 8 THEN CONCAT(YEAR(f.fecha), '-', YEAR(f.fecha) + 1)
            ELSE CONCAT(YEAR(f.fecha) - 1, '-', YEAR(f.fecha)) END AS anio_academico
FROM (SELECT DATE '2022-01-01' + INTERVAL (a.n + 10 * b.n + 100 * c.n + 1000 * d.n) DAY AS fecha
      FROM agente.digitos a CROSS JOIN agente.digitos b CROSS JOIN agente.digitos c CROSS JOIN agente.digitos d) f
WHERE f.fecha <= DATE '2029-12-31';

-- A9) Usuario de solo lectura para la API. Solo ve el esquema `agente`, no gs_data_analysis.
--     Las vistas son SQL SECURITY DEFINER: leen gs_data_analysis con los permisos de quien las creó.
--     Pon una contraseña fuerte y guárdala en Key Vault (no en appsettings.json ni en el repo).
CREATE USER IF NOT EXISTS 'agente_lector'@'%' IDENTIFIED BY 'CAMBIAR_POR_CONTRASEÑA_DE_KEY_VAULT';
GRANT SELECT, SHOW VIEW ON agente.* TO 'agente_lector'@'%';

-- ───────────────────── PARTE B · VERIFICAR (solo lectura; cualquiera con acceso a `agente`) ─────────────────────
-- Las cifras de referencia vienen del .pbix procesado el 2026-11-05 (MSJ_Utilization_modelo_y_10_preguntas.md).
-- Las conexiones se cortan al 2026-11-04 para comparar con el mismo corte. Usuarios y suscripciones no se pueden
-- llevar a esa fecha: una diferencia pequeña es normal; una grande indica que una regla no coincide.

-- B1) P1/P6 · esperado: 17.679 usuarios · active 6.988 / 9.674 · canceled 14.479 / 62.412 · sin subscripción 489 / 489
SELECT COUNT(DISTINCT user_id) AS total_usuarios FROM agente.usuarios_suscripciones;
SELECT status, COUNT(DISTINCT user_id) AS usuarios, COUNT(*) AS filas
FROM agente.usuarios_suscripciones GROUP BY status;

-- B2) P4/P9 · esperado: 6.994 conexiones / 2.433 usuarios; calendario 2026 («2025-2026» del tablero) 1.481 / 751;
--     año académico real 2025-2026: 4.078 / 1.819
SELECT COUNT(*) AS conexiones, COUNT(DISTINCT email) AS usuarios
FROM agente.conexiones WHERE connected_date <= '2026-11-04';
SELECT 'tablero' AS regla, COUNT(*) AS conexiones, COUNT(DISTINCT email) AS usuarios
FROM agente.conexiones WHERE connected_date <= '2026-11-04' AND anio_academico_tablero = '2025-2026'
UNION ALL
SELECT 'ago-jul', COUNT(*), COUNT(DISTINCT email)
FROM agente.conexiones WHERE connected_date <= '2026-11-04' AND anio_academico = '2025-2026';

-- B3) P3 · esperado: conectados según el tablero 2.432; conectados con suscripción activa 726; activos 6.988
SELECT
  (SELECT COUNT(DISTINCT c.email) FROM agente.conexiones c
    WHERE c.connected_date <= '2026-11-04'
      AND c.email IN (SELECT email FROM agente.usuarios_suscripciones WHERE email IS NOT NULL))      AS conectados_tablero,
  (SELECT COUNT(DISTINCT c.email) FROM agente.conexiones c
    WHERE c.connected_date <= '2026-11-04'
      AND c.email IN (SELECT email FROM agente.usuarios_suscripciones WHERE status = 'active'))      AS conectados_activos,
  (SELECT COUNT(DISTINCT email) FROM agente.usuarios_suscripciones
    WHERE status = 'active' AND email IS NOT NULL)                                                   AS activos;

-- B4) P8 · esperado: 10.628 filas; total 271 tomados / 10.357 no tomados; página (2025-2026 sin Posprueba) 206 / 6.883
SELECT anio_academico, periodo,
       SUM(completado = 'Completed') AS tomados, SUM(completado = 'Not Completed') AS no_tomados,
       SUM(completado = 'Completed' AND NOT es_posprueba)       AS tomados_sin_posprueba,
       SUM(completado = 'Not Completed' AND NOT es_posprueba)   AS no_tomados_sin_posprueba
FROM agente.pruebas GROUP BY anio_academico, periodo WITH ROLLUP;

-- B5) P7 · esperado: promedio global de la página 60,19 (Completed, sin Posprueba, 2025-2026)
SELECT ROUND(AVG(total_percent_full_quiz), 2) AS promedio_full_quiz
FROM agente.pruebas
WHERE completado = 'Completed' AND NOT es_posprueba AND anio_academico = '2025-2026';

-- B6) P10 · esperado por año de fin (suscripciones / usuarios / filas): 2022-23 24/1.035/1.241 · 2023-24 424/4.677/9.239 ·
--     2024-25 945/10.327/47.077 · 2025-26 131/4.534/4.873 · 2026-27 290/6.988/9.674
SELECT anio_academico_fin, COUNT(DISTINCT subscription_id) AS suscripciones,
       COUNT(DISTINCT user_id) AS usuarios, COUNT(*) AS filas
FROM agente.suscripciones GROUP BY anio_academico_fin ORDER BY anio_academico_fin;
