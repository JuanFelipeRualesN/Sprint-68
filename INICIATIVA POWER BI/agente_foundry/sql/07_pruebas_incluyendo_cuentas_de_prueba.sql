-- =====================================================================================
-- 07 · ¿LA PÁGINA «REPORTE ACADÉMICO» DE POWER BI INCLUYE LAS CUENTAS DE PRUEBA?   (REQ-3741)
-- SOLO LECTURA. Objetivo de Power BI (Año académico 2025-2026, P1+P2+P3):
--        asignadas 1.029 · completadas 206 · no completadas 823 · estudiantes que tomaron 135
-- Resultado de la consulta 06 (SIN cuentas de prueba): tipo H = 465 asignadas · 189 completadas · 276 no · 130 estudiantes.
--   · tipo no nulo (vacío) y assignment_type = 'H' son LAS MISMAS filas → Power BI cuenta las tareas del docente.
--   · faltan ≈ 564 asignadas, 17 completadas y 5 estudiantes: hipótesis = vienen de las escuelas de prueba (ORE DEMOSTRACION).
-- Estas consultas dan tres pistas (todas son conteos y nombres de escuelas, sin nombres de personas). Pegar las TRES tablas.
-- =====================================================================================
USE gs_data_analysis;

-- ── A. Candidatas CON cuentas de prueba (compara con 1029 / 206 / 823 / 135) ──────────────────────────
WITH base AS (
  SELECT r.user_id, r.period_no, r.assignment_type,
         (r.quiz_taken_date IS NOT NULL) AS tomada,
         (COALESCE(sc.ore, '') = 'DEMOSTRACION') AS es_prueba
  FROM result_generic r
  JOIN user u         ON u.user_id = r.user_id AND u.role_id = 3
  LEFT JOIN school sc ON sc.school_id = u.school_id
  WHERE YEAR(r.academic_year_start) = 2025 AND YEAR(r.academic_year_end) = 2026
)
SELECT 'E01 tipo H, CON cuentas de prueba, cualquier período' AS definicion,
       COUNT(*) AS asignadas, SUM(tomada) AS completadas, COUNT(*) - SUM(tomada) AS no_completadas,
       COUNT(DISTINCT CASE WHEN tomada = 1 THEN user_id END) AS estudiantes_que_tomaron
FROM base WHERE assignment_type = 'H'
UNION ALL
SELECT 'E02 tipo H, CON cuentas de prueba, período 1-3', COUNT(*), SUM(tomada), COUNT(*) - SUM(tomada),
       COUNT(DISTINCT CASE WHEN tomada = 1 THEN user_id END)
FROM base WHERE assignment_type = 'H' AND period_no BETWEEN 1 AND 3
UNION ALL
SELECT 'E03 tipo H, SOLO cuentas de prueba, cualquier período', COUNT(*), SUM(tomada), COUNT(*) - SUM(tomada),
       COUNT(DISTINCT CASE WHEN tomada = 1 THEN user_id END)
FROM base WHERE assignment_type = 'H' AND es_prueba = 1
UNION ALL
SELECT 'E04 todas las filas (L+H), CON cuentas de prueba, período 1-3', COUNT(*), SUM(tomada), COUNT(*) - SUM(tomada),
       COUNT(DISTINCT CASE WHEN tomada = 1 THEN user_id END)
FROM base WHERE period_no BETWEEN 1 AND 3;

-- ── B. Tipo H por período y por «cuenta de prueba sí/no» (para sumar a mano las combinaciones) ─────────
WITH base AS (
  SELECT r.user_id, r.period_no,
         (r.quiz_taken_date IS NOT NULL) AS tomada,
         (COALESCE(sc.ore, '') = 'DEMOSTRACION') AS es_prueba
  FROM result_generic r
  JOIN user u         ON u.user_id = r.user_id AND u.role_id = 3
  LEFT JOIN school sc ON sc.school_id = u.school_id
  WHERE YEAR(r.academic_year_start) = 2025 AND YEAR(r.academic_year_end) = 2026
    AND r.assignment_type = 'H'
)
SELECT es_prueba, period_no,
       COUNT(*) AS asignadas, SUM(tomada) AS completadas,
       COUNT(DISTINCT CASE WHEN tomada = 1 THEN user_id END) AS estudiantes_que_tomaron
FROM base
GROUP BY es_prueba, period_no
ORDER BY es_prueba, period_no;

-- ── C. Tipo H por escuela (¿dónde están las ~1.000 asignadas? solo nombres de escuela y conteos) ────────
WITH base AS (
  SELECT r.user_id, u.school_id,
         (r.quiz_taken_date IS NOT NULL) AS tomada
  FROM result_generic r
  JOIN user u ON u.user_id = r.user_id AND u.role_id = 3
  WHERE YEAR(r.academic_year_start) = 2025 AND YEAR(r.academic_year_end) = 2026
    AND r.assignment_type = 'H'
)
SELECT COALESCE(sc.school_name, '(sin escuela)') AS escuela,
       COALESCE(sc.ore, '')                      AS ore,
       COUNT(*)                                  AS asignadas,
       SUM(b.tomada)                             AS completadas,
       COUNT(DISTINCT CASE WHEN b.tomada = 1 THEN b.user_id END) AS estudiantes_que_tomaron
FROM base b
LEFT JOIN school sc ON sc.school_id = b.school_id
GROUP BY sc.school_id, sc.school_name, sc.ore
ORDER BY asignadas DESC;
