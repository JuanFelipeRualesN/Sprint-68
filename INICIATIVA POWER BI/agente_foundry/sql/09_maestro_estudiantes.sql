-- =====================================================================================
-- 09 · POR MAESTRO: estudiantes sin conexión, progreso y puntuaciones   (siguiente tarea indicada por Pedro Ortiz)
-- SOLO LECTURA. Caso de uso: «un maestro quiere saber quiénes de sus estudiantes no se están conectando, el progreso,
-- las puntuaciones». Reutiliza las definiciones ya validadas (CRUCE_POWER_BI.md / 02_verdad_terreno.sql):
--   conexión = una fila de connected_date (usuario-día) · estudiante = user.role_id 3 · maestro = role_id 5
--   prueba completada = quiz_taken_date no nulo · excluye ORE '%DEMOSTRACION%' (igual que las vistas vw_*)
-- RELACIÓN MAESTRO → ESTUDIANTE (supuesto a validar en el paso 0): gs_teachers_web_db_prod.teacher_groups (id, teacher_id,
--   available) + teacher_group_students (teacher_group_id, student_id), como usa TeachersAPI. Se asume que teacher_id y
--   student_id = gs_data_analysis.user.user_id. Si el paso 0 da 0 coincidencias, mirar la vista
--   vw_student_teacher_relationship (SHOW CREATE VIEW) y reemplazar la CTE «alumnos».
-- PRIVACIDAD: estas consultas devuelven user_id de estudiantes (no nombres ni correos). Correr en Workbench; no pegar
--   resultados con identificadores en chats externos.
-- Ejecutar de a una sección. Cambiar solo las variables de la sección 0.
-- =====================================================================================
USE gs_data_analysis;

-- ── 0. VARIABLES y validación del vínculo maestro-estudiante ─────────────────────────────────────────
SET @teacher_id = NULL;   -- ← poner el user_id de un maestro (elegirlo con la consulta 0.b)
SET @dias       = 30;     -- «sin conexión» = ninguna conexión en los últimos @dias días de datos
SET @hasta      = (SELECT MAX(connected_date) FROM connected_date);   -- los datos llegan hasta aquí, no hasta «hoy»

-- 0.a ¿Los ids de los grupos son usuarios de gs_data_analysis? (esperado: coincidencias > 0 en ambos)
SELECT COUNT(DISTINCT g.teacher_id)  AS maestros_en_grupos,
       COUNT(DISTINCT t.user_id)     AS de_ellos_son_role_5,
       COUNT(DISTINCT s.student_id)  AS estudiantes_en_grupos,
       COUNT(DISTINCT e.user_id)     AS de_ellos_son_role_3
FROM gs_teachers_web_db_prod.teacher_groups g
LEFT JOIN user t ON t.user_id = g.teacher_id AND t.role_id = 5
LEFT JOIN gs_teachers_web_db_prod.teacher_group_students s ON s.teacher_group_id = g.id
LEFT JOIN user e ON e.user_id = s.student_id AND e.role_id = 3
WHERE g.available = 1;

-- 0.b Maestros con más estudiantes (para elegir uno de prueba; fuera del ORE DEMOSTRACION)
SELECT g.teacher_id, COUNT(DISTINCT s.student_id) AS estudiantes, COUNT(DISTINCT g.id) AS grupos
FROM gs_teachers_web_db_prod.teacher_groups g
JOIN gs_teachers_web_db_prod.teacher_group_students s ON s.teacher_group_id = g.id
JOIN user t ON t.user_id = g.teacher_id AND t.role_id = 5
JOIN school sh ON sh.school_id = t.school_id AND NOT (sh.ore LIKE '%DEMOSTRACION%')
WHERE g.available = 1
GROUP BY g.teacher_id
ORDER BY estudiantes DESC
LIMIT 10;

-- ── 1. ESTUDIANTES SIN CONEXIÓN de un maestro (últimos @dias días de datos, o nunca) ─────────────────
SELECT a.student_id,
       a.grupo,
       c.ultima_conexion,
       COALESCE(DATEDIFF(@hasta, c.ultima_conexion), NULL) AS dias_sin_conectar,
       CASE WHEN c.ultima_conexion IS NULL THEN 'nunca se ha conectado' ELSE 'sin conexión reciente' END AS situacion
FROM (SELECT DISTINCT s.student_id, g.name AS grupo
      FROM gs_teachers_web_db_prod.teacher_groups g
      JOIN gs_teachers_web_db_prod.teacher_group_students s ON s.teacher_group_id = g.id
      JOIN user e ON e.user_id = s.student_id AND e.role_id = 3
      WHERE g.teacher_id = @teacher_id AND g.available = 1) a
LEFT JOIN (SELECT user_id, MAX(connected_date) AS ultima_conexion
           FROM connected_date GROUP BY user_id) c ON c.user_id = a.student_id
WHERE c.ultima_conexion IS NULL OR c.ultima_conexion < DATE_SUB(@hasta, INTERVAL @dias DAY)
ORDER BY c.ultima_conexion IS NULL DESC, c.ultima_conexion;

-- ── 2. CONEXIONES por estudiante del maestro en los últimos @dias días (todos, conectados o no) ──────
SELECT a.student_id,
       COALESCE(c.dias_conectado, 0) AS dias_conectado,
       c.ultima_conexion
FROM (SELECT DISTINCT s.student_id
      FROM gs_teachers_web_db_prod.teacher_groups g
      JOIN gs_teachers_web_db_prod.teacher_group_students s ON s.teacher_group_id = g.id
      JOIN user e ON e.user_id = s.student_id AND e.role_id = 3
      WHERE g.teacher_id = @teacher_id AND g.available = 1) a
LEFT JOIN (SELECT user_id, COUNT(*) AS dias_conectado, MAX(connected_date) AS ultima_conexion
           FROM connected_date
           WHERE connected_date >  DATE_SUB(@hasta, INTERVAL @dias DAY) AND connected_date <= @hasta
           GROUP BY user_id) c ON c.user_id = a.student_id
ORDER BY dias_conectado, a.student_id;

-- ── 3. PROGRESO por estudiante: pruebas asignadas / completadas / % ──────────────────────────────────
SELECT a.student_id,
       COALESCE(p.asignadas, 0)   AS asignadas,
       COALESCE(p.completadas, 0) AS completadas,
       CASE WHEN COALESCE(p.asignadas, 0) = 0 THEN NULL
            ELSE ROUND(100 * p.completadas / p.asignadas, 2) END AS pct_completado
FROM (SELECT DISTINCT s.student_id
      FROM gs_teachers_web_db_prod.teacher_groups g
      JOIN gs_teachers_web_db_prod.teacher_group_students s ON s.teacher_group_id = g.id
      JOIN user e ON e.user_id = s.student_id AND e.role_id = 3
      WHERE g.teacher_id = @teacher_id AND g.available = 1) a
LEFT JOIN (SELECT user_id, COUNT(*) AS asignadas, SUM(quiz_taken_date IS NOT NULL) AS completadas
           FROM result_generic GROUP BY user_id) p ON p.user_id = a.student_id
ORDER BY pct_completado IS NULL, pct_completado, a.student_id;

-- ── 4. PUNTUACIONES por estudiante y materia (solo pruebas completadas; misma métrica que vw_academics_average) ──
-- Nota: la vista promedia COALESCE(total_percent_full_quiz, 0); aquí se promedian solo las completadas para no
-- penalizar pruebas no realizadas. Si el tablero debe coincidir, quitar el filtro quiz_taken_date IS NOT NULL.
SELECT a.student_id,
       r.subject_code,
       COUNT(*)                                       AS pruebas_completadas,
       ROUND(AVG(COALESCE(r.total_percent_full_quiz, 0)), 2) AS promedio_pct,
       MAX(r.quiz_taken_date)                         AS ultima_prueba
FROM (SELECT DISTINCT s.student_id
      FROM gs_teachers_web_db_prod.teacher_groups g
      JOIN gs_teachers_web_db_prod.teacher_group_students s ON s.teacher_group_id = g.id
      JOIN user e ON e.user_id = s.student_id AND e.role_id = 3
      WHERE g.teacher_id = @teacher_id AND g.available = 1) a
JOIN result_generic r ON r.user_id = a.student_id AND r.quiz_taken_date IS NOT NULL
GROUP BY a.student_id, r.subject_code
ORDER BY a.student_id, r.subject_code;

-- ── 5. RESUMEN del maestro (una fila): sirve para comparar con lo que responda el agente ─────────────
SELECT COUNT(*)                                          AS estudiantes,
       SUM(c.ultima_conexion IS NULL)                    AS nunca_conectados,
       SUM(c.ultima_conexion < DATE_SUB(@hasta, INTERVAL @dias DAY)) AS sin_conexion_reciente,
       SUM(COALESCE(p.asignadas, 0))                     AS pruebas_asignadas,
       SUM(COALESCE(p.completadas, 0))                   AS pruebas_completadas
FROM (SELECT DISTINCT s.student_id
      FROM gs_teachers_web_db_prod.teacher_groups g
      JOIN gs_teachers_web_db_prod.teacher_group_students s ON s.teacher_group_id = g.id
      JOIN user e ON e.user_id = s.student_id AND e.role_id = 3
      WHERE g.teacher_id = @teacher_id AND g.available = 1) a
LEFT JOIN (SELECT user_id, MAX(connected_date) AS ultima_conexion FROM connected_date GROUP BY user_id) c ON c.user_id = a.student_id
LEFT JOIN (SELECT user_id, COUNT(*) AS asignadas, SUM(quiz_taken_date IS NOT NULL) AS completadas
           FROM result_generic GROUP BY user_id) p ON p.user_id = a.student_id;
