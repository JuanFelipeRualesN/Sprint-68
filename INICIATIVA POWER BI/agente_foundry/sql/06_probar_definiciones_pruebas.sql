-- =====================================================================================
-- 06 · PROBAR DEFINICIONES DE «PRUEBAS ASIGNADAS» CONTRA LO QUE MUESTRA POWER BI   (REQ-3741)
-- SOLO LECTURA. Objetivo de Power BI (página «Reporte Académico», Año académico 2025-2026, P1+P2+P3):
--        asignadas 1.029 · completadas 206 · no completadas 823 · estudiantes que tomaron 135
-- Cada fila de resultado es UNA definición candidata; la que produzca 1029 / 206 / 823 / 135 es la que usa el tablero.
-- Base común: estudiantes (role_id 3), fuera del ORE DEMOSTRACION, año académico que empieza en 2025 y termina en 2026.
-- Hallazgos previos (consulta 05): academic_year_type '' (vacío) vs NULL separan dos poblaciones muy distintas;
-- assignment_type 'L' (6.901 filas, 125 tomadas) vs 'H' (500 filas, 197 tomadas).
-- Una sola pasada por tabla, segundos de ejecución. Pegar el resultado (son conteos, sin nombres de personas).
-- =====================================================================================
USE gs_data_analysis;

WITH base AS (
  SELECT r.user_id, r.quiz_id, r.period_no, r.assignment_type, r.academic_year_type,
         r.plan_assignment_user_result_id,
         (r.quiz_taken_date IS NOT NULL) AS tomada,
         (r.quiz_due_date < UTC_TIMESTAMP()) AS vencida
  FROM result_generic r
  JOIN user u        ON u.user_id = r.user_id AND u.role_id = 3
  LEFT JOIN school sc ON sc.school_id = u.school_id
  WHERE YEAR(r.academic_year_start) = 2025
    AND YEAR(r.academic_year_end)   = 2026
    AND COALESCE(sc.ore, '') <> 'DEMOSTRACION'
)
SELECT 'D01 todas las filas, cualquier período' AS definicion,
       COUNT(*) AS asignadas, SUM(tomada) AS completadas, COUNT(*) - SUM(tomada) AS no_completadas,
       COUNT(DISTINCT CASE WHEN tomada = 1 THEN user_id END) AS estudiantes_que_tomaron
FROM base
UNION ALL
SELECT 'D02 todas las filas, período 1-3', COUNT(*), SUM(tomada), COUNT(*) - SUM(tomada),
       COUNT(DISTINCT CASE WHEN tomada = 1 THEN user_id END)
FROM base WHERE period_no BETWEEN 1 AND 3
UNION ALL
SELECT 'D03 tipo de año académico NO nulo (vacío), cualquier período', COUNT(*), SUM(tomada), COUNT(*) - SUM(tomada),
       COUNT(DISTINCT CASE WHEN tomada = 1 THEN user_id END)
FROM base WHERE academic_year_type IS NOT NULL
UNION ALL
SELECT 'D04 tipo de año académico NO nulo, período 1-3', COUNT(*), SUM(tomada), COUNT(*) - SUM(tomada),
       COUNT(DISTINCT CASE WHEN tomada = 1 THEN user_id END)
FROM base WHERE academic_year_type IS NOT NULL AND period_no BETWEEN 1 AND 3
UNION ALL
SELECT 'D05 assignment_type = H, cualquier período', COUNT(*), SUM(tomada), COUNT(*) - SUM(tomada),
       COUNT(DISTINCT CASE WHEN tomada = 1 THEN user_id END)
FROM base WHERE assignment_type = 'H'
UNION ALL
SELECT 'D06 assignment_type = H, período 1-3', COUNT(*), SUM(tomada), COUNT(*) - SUM(tomada),
       COUNT(DISTINCT CASE WHEN tomada = 1 THEN user_id END)
FROM base WHERE assignment_type = 'H' AND period_no BETWEEN 1 AND 3
UNION ALL
SELECT 'D07 solo vencidas (due < ahora), período 1-3', COUNT(*), SUM(tomada), COUNT(*) - SUM(tomada),
       COUNT(DISTINCT CASE WHEN tomada = 1 THEN user_id END)
FROM base WHERE vencida = 1 AND period_no BETWEEN 1 AND 3
UNION ALL
SELECT 'D08 pares distintos (estudiante, quiz), período 1-3',
       COUNT(DISTINCT user_id, quiz_id),
       COUNT(DISTINCT CASE WHEN tomada = 1 THEN CONCAT(user_id, '-', quiz_id) END),
       COUNT(DISTINCT user_id, quiz_id) - COUNT(DISTINCT CASE WHEN tomada = 1 THEN CONCAT(user_id, '-', quiz_id) END),
       COUNT(DISTINCT CASE WHEN tomada = 1 THEN user_id END)
FROM base WHERE period_no BETWEEN 1 AND 3
UNION ALL
SELECT 'D09 pares distintos (estudiante, quiz), tipo NO nulo',
       COUNT(DISTINCT user_id, quiz_id),
       COUNT(DISTINCT CASE WHEN tomada = 1 THEN CONCAT(user_id, '-', quiz_id) END),
       COUNT(DISTINCT user_id, quiz_id) - COUNT(DISTINCT CASE WHEN tomada = 1 THEN CONCAT(user_id, '-', quiz_id) END),
       COUNT(DISTINCT CASE WHEN tomada = 1 THEN user_id END)
FROM base WHERE academic_year_type IS NOT NULL
UNION ALL
SELECT 'D10 con asignación del docente (plan_assignment_user_result_id no nulo)', COUNT(*), SUM(tomada), COUNT(*) - SUM(tomada),
       COUNT(DISTINCT CASE WHEN tomada = 1 THEN user_id END)
FROM base WHERE plan_assignment_user_result_id IS NOT NULL;

-- Si ninguna fila da 1029 / 206 / 823 / 135, el siguiente paso es mirar la medida «Pruebas asignadas» en el modelo
-- semántico de Power BI (Reports_Semantic_Model → Ver modelo → medida) o el filtro de la página «Reporte Académico».
