-- =====================================================================================
-- 03 · TODO EL DESCUBRIMIENTO EN UN SOLO RESULTADO (un JSON)   (REQ-3741)
-- SOLO LECTURA (un único SELECT). Evita copiar 18 pestañas: devuelve UNA celda con todo.
-- Cómo usarlo: ejecuta TODO el archivo (⌘+Enter con nada seleccionado ejecuta la instrucción
-- donde está el cursor; usa ⌘+Shift+Enter para ejecutar todo). En la pestaña de resultado haz
-- clic derecho sobre la celda "descubrimiento" → "Copy Field" y pégalo en el chat.
-- No contiene emails ni nombres de personas: solo conteos, fechas, estructura y nombres de escuelas repetidos.
-- Requiere MySQL 8.0.14+ (tu servidor es 8.0.46).
-- =====================================================================================
USE gs_data_analysis;
SET SESSION group_concat_max_len = 1000000;

SELECT JSON_OBJECT(
  'columnas', (SELECT JSON_ARRAYAGG(JSON_OBJECT('tabla', TABLE_NAME, 'columna', COLUMN_NAME, 'tipo', COLUMN_TYPE))
               FROM information_schema.COLUMNS
               WHERE TABLE_SCHEMA = 'gs_data_analysis'
                 AND TABLE_NAME IN ('connected_date','user','school','result_generic','subject','role','level','etl_last_date')),

  'frescura', JSON_OBJECT(
      'connected_date',            (SELECT JSON_OBJECT('primera', MIN(connected_date), 'ultima', MAX(connected_date), 'filas', COUNT(*)) FROM connected_date),
      'quiz_taken_date',           (SELECT JSON_OBJECT('primera', MIN(quiz_taken_date), 'ultima', MAX(quiz_taken_date), 'filas_no_nulas', COUNT(quiz_taken_date)) FROM result_generic),
      'quiz_assigned_to_plan_date',(SELECT JSON_OBJECT('primera', MIN(quiz_assigned_to_plan_date), 'ultima', MAX(quiz_assigned_to_plan_date)) FROM result_generic),
      'period_from',               (SELECT JSON_OBJECT('primera', MIN(period_from), 'ultima', MAX(period_from)) FROM result_generic)),

  'conexiones_granularidad', (SELECT JSON_OBJECT('filas', COUNT(*),
                                                 'usuarios_distintos', COUNT(DISTINCT user_id),
                                                 'usuario_dia_distintos', COUNT(DISTINCT user_id, DATE(connected_date)))
                              FROM connected_date),

  'roles', (SELECT JSON_ARRAYAGG(JSON_OBJECT('role_id', role_id, 'role', role, 'usuarios', n))
            FROM (SELECT role_id, role, COUNT(*) AS n FROM user GROUP BY role_id, role) r),

  'materias', (SELECT JSON_ARRAYAGG(JSON_OBJECT('codigo', subject_code, 'nombre', subject_name, 'filas', filas, 'completadas', completadas))
               FROM (SELECT s.subject_code, s.subject_name, COUNT(r.id) AS filas, SUM(r.quiz_taken_date IS NOT NULL) AS completadas
                     FROM subject s LEFT JOIN result_generic r ON r.subject_code = s.subject_code
                     GROUP BY s.subject_code, s.subject_name) m),

  'materias_sin_catalogo', (SELECT JSON_ARRAYAGG(JSON_OBJECT('codigo', subject_code, 'filas', filas))
                            FROM (SELECT r.subject_code, COUNT(*) AS filas
                                  FROM result_generic r LEFT JOIN subject s ON s.subject_code = r.subject_code
                                  WHERE s.subject_code IS NULL GROUP BY r.subject_code) x),

  'escuelas', JSON_OBJECT(
      'total',               (SELECT COUNT(*) FROM school),
      'nombres_distintos',   (SELECT COUNT(DISTINCT school_name) FROM school),
      'sin_nombre',          (SELECT COUNT(*) FROM school WHERE school_name IS NULL OR school_name = ''),
      'nombres_repetidos',   (SELECT COUNT(*) FROM (SELECT school_name FROM school GROUP BY school_name HAVING COUNT(*) > 1) d),
      'con_usuarios',        (SELECT COUNT(*) FROM school sh WHERE EXISTS (SELECT 1 FROM user us WHERE us.school_id = sh.school_id)),
      'usuarios_sin_escuela',(SELECT COUNT(*) FROM user WHERE school_id IS NULL)),

  'zona_horaria', JSON_OBJECT('global', @@global.time_zone, 'sesion', @@session.time_zone,
                              'ahora_servidor', NOW(), 'ahora_utc', UTC_TIMESTAMP()),

  'volumen', JSON_OBJECT('connected_date', (SELECT COUNT(*) FROM connected_date),
                         'user', (SELECT COUNT(*) FROM user),
                         'school', (SELECT COUNT(*) FROM school),
                         'result_generic', (SELECT COUNT(*) FROM result_generic))
) AS descubrimiento;

-- Aparte (tabla pequeña, no se sabe su estructura): carga más reciente del ETL → sirve para "datos_hasta".
SELECT * FROM etl_last_date;
