-- =====================================================================================
-- 01 · DESCUBRIR ESQUEMA Y FRESCURA DE gs_data_analysis   (REQ-3741)
-- SOLO LECTURA: únicamente SHOW / DESCRIBE / SELECT. Ejecutar en MySQL Workbench con el
-- usuario de SOLO LECTURA (ver PLAN_PRUEBAS_REALES_Y_PANEL.md, paso 1) y conectado a la VPN.
-- Ejecuta por secciones (Ctrl+Shift+Enter ejecuta la selección) y copia/pega los resultados
-- de A, B, C, D y G de vuelta: con eso se escribe ConsultasMySql sin adivinar nada.
-- =====================================================================================
USE gs_data_analysis;

-- ── A. Tablas y columnas (para confirmar nombres y tipos reales) ──────────────────────
SHOW TABLES;
DESCRIBE connected_date;
DESCRIBE user;
DESCRIBE school;
DESCRIBE result_generic;
DESCRIBE subject;
DESCRIBE role;

-- ── B. FRESCURA: hasta qué día llegan los datos (→ "datos_hasta" de cada respuesta) ───
SELECT 'connected_date.connected_date'  AS columna, MIN(connected_date)  AS primera, MAX(connected_date)  AS ultima, COUNT(*) AS filas FROM connected_date
UNION ALL
SELECT 'result_generic.quiz_taken_date',          MIN(quiz_taken_date),          MAX(quiz_taken_date),          COUNT(quiz_taken_date) FROM result_generic
UNION ALL
SELECT 'result_generic.quiz_assigned_to_plan_date', MIN(quiz_assigned_to_plan_date), MAX(quiz_assigned_to_plan_date), COUNT(quiz_assigned_to_plan_date) FROM result_generic
UNION ALL
SELECT 'result_generic.period_from',              MIN(period_from),              MAX(period_from),              COUNT(period_from) FROM result_generic;

-- ── C. connected_date: ¿es DATE o DATETIME? ¿una fila por conexión o por usuario-día? ─
SELECT COLUMN_NAME, DATA_TYPE
FROM information_schema.COLUMNS
WHERE TABLE_SCHEMA = 'gs_data_analysis' AND TABLE_NAME = 'connected_date';

SELECT COUNT(*)                                        AS filas,
       COUNT(DISTINCT user_id)                         AS usuarios_distintos,
       COUNT(DISTINCT user_id, DATE(connected_date))   AS usuario_dia_distintos
FROM connected_date;
-- Si filas = usuario_dia_distintos → hay una fila por usuario y día; "conexiones" = filas.
-- Si filas > usuario_dia_distintos → hay varias por día; decidir si "conexión" = fila o usuario-día
-- (comparar con la medida "Conteo_Conectados_Si" / "Conectado único" del tablero de Power BI).

-- ── D. Roles: ¿cuál es "maestro/docente" y cuál "estudiante"? ─────────────────────────
SELECT role_id, role, COUNT(*) AS usuarios FROM user GROUP BY role_id, role ORDER BY usuarios DESC;
-- Anota el role_id de docente: se usa en 02_verdad_terreno.sql (@rol_docente).
-- El API existente filtra result_generic con role_id = 3; confirmar que 3 = estudiante.

-- ── E. "Área" = ¿materia? Qué valores existen y cuántas pruebas tiene cada una ────────
SELECT s.subject_code, s.subject_name,
       COUNT(r.id)                          AS filas_result_generic,
       SUM(r.quiz_taken_date IS NOT NULL)   AS completadas
FROM subject s
LEFT JOIN result_generic r ON r.subject_code = s.subject_code
GROUP BY s.subject_code, s.subject_name
ORDER BY filas_result_generic DESC;

-- Códigos de materia en result_generic que NO existen en subject (saldrían sin nombre):
SELECT r.subject_code, COUNT(*) AS filas
FROM result_generic r LEFT JOIN subject s ON s.subject_code = r.subject_code
WHERE s.subject_code IS NULL
GROUP BY r.subject_code;

-- ── F. Escuelas: calidad del catálogo (el agente las identifica por NOMBRE) ───────────
SELECT COUNT(*) AS escuelas_total,
       COUNT(DISTINCT school_name) AS nombres_distintos,
       SUM(school_name IS NULL OR school_name = '') AS sin_nombre
FROM school;

-- Nombres repetidos con ids distintos (ambigüedad: el agente tendría que preguntar cuál):
SELECT school_name, COUNT(*) AS veces, GROUP_CONCAT(school_id ORDER BY school_id) AS ids
FROM school GROUP BY school_name HAVING COUNT(*) > 1 ORDER BY veces DESC LIMIT 20;

-- Escuelas con y sin usuarios (para "escuelas con menor actividad" ¿incluir las sin usuarios?):
SELECT SUM(u.n > 0) AS con_usuarios, SUM(u.n = 0) AS sin_usuarios
FROM (SELECT sh.school_id, COUNT(us.user_id) AS n
      FROM school sh LEFT JOIN user us ON us.school_id = sh.school_id GROUP BY sh.school_id) u;

-- Usuarios sin escuela (quedarían fuera de "por escuela"):
SELECT COUNT(*) AS usuarios_sin_escuela FROM user WHERE school_id IS NULL;

-- ── G. Zona horaria (un día "10-nov" ¿es hora local o UTC?) ───────────────────────────
SELECT @@global.time_zone AS tz_global, @@session.time_zone AS tz_sesion, NOW() AS ahora_servidor, UTC_TIMESTAMP() AS ahora_utc;

-- ── H. Volumen (para dimensionar límites y tiempos de respuesta) ──────────────────────
SELECT 'connected_date' AS tabla, COUNT(*) AS filas FROM connected_date
UNION ALL SELECT 'user', COUNT(*) FROM user
UNION ALL SELECT 'school', COUNT(*) FROM school
UNION ALL SELECT 'result_generic', COUNT(*) FROM result_generic;

-- NOTA DE PRIVACIDAD: user trae username, email, first_name y last_name. La API del agente NUNCA debe
-- devolver emails ni datos de estudiantes; solo agregados (y nombre de docente solo si se decide así).
