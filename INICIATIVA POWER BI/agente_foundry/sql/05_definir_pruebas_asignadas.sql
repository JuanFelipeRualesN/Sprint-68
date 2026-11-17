-- =====================================================================================
-- 05 · ¿QUÉ CUENTA POWER BI COMO «PRUEBAS ASIGNADAS»?   (REQ-3741, cruce con Power BI del 12-nov-2026)
-- SOLO LECTURA. Power BI (página «Reporte Académico», Año académico 2025-2026, P1+P2+P3) muestra:
--     Pruebas asignadas 1.029 · completadas 206 · no completadas 823 · estudiantes que tomaron pruebas 135
-- La API (filas de result_generic de estudiantes, 1-jul-2025 → hoy) da: asignadas 7.117 · completadas 217.
-- Las completadas se parecen; las asignadas NO. Estas consultas buscan qué agrupación/filtro produce 1.029 / 206 / 135.
-- result_generic tiene ~12 mil filas: cada consulta recorre la tabla una vez (segundos). Solo estudiantes (role_id = 3).
-- Pegar las tres tablas (son conteos, sin nombres de personas).
-- =====================================================================================
USE gs_data_analysis;

-- ── A. Por año académico y período: ¿qué fila de aquí se parece a 1.029 / 206 / 135? ──────────────────
SELECT x.academic_year_id,
       x.inicio,
       x.fin,
       x.tipo,
       x.period_no,
       COUNT(*)                                                              AS filas,
       SUM(x.tomada)                                                         AS tomadas,
       COUNT(DISTINCT CASE WHEN x.tomada = 1 THEN x.user_id END)             AS estudiantes_que_tomaron
FROM (SELECT r.academic_year_id,
             DATE(r.academic_year_start) AS inicio,
             DATE(r.academic_year_end)   AS fin,
             r.academic_year_type        AS tipo,
             r.period_no,
             r.user_id,
             (r.quiz_taken_date IS NOT NULL) AS tomada
      FROM result_generic r
      JOIN user u ON u.user_id = r.user_id AND u.role_id = 3) x
GROUP BY x.academic_year_id, x.inicio, x.fin, x.tipo, x.period_no
ORDER BY x.inicio DESC, x.period_no;

-- ── B. Dentro del año académico más reciente: ¿filtran por tipo de asignación o por plan vigente? ─────
SELECT x.assignment_type,
       x.plan_currently_available,
       COUNT(*)      AS filas,
       SUM(x.tomada) AS tomadas
FROM (SELECT r.assignment_type, r.plan_currently_available, (r.quiz_taken_date IS NOT NULL) AS tomada
      FROM result_generic r
      JOIN user u ON u.user_id = r.user_id AND u.role_id = 3
      WHERE r.academic_year_start >= '2025-06-01') x
GROUP BY x.assignment_type, x.plan_currently_available
ORDER BY filas DESC;

-- ── C. ¿«Asignadas» son pruebas distintas y no filas por estudiante? (mismo año académico) ──────────
SELECT COUNT(*)                                       AS filas,
       COUNT(DISTINCT r.quiz_id)                      AS quizzes_distintos,
       COUNT(DISTINCT r.plan_quiz_id)                 AS plan_quiz_distintos,
       COUNT(DISTINCT r.user_quiz_result_id)          AS resultados_distintos,
       COUNT(DISTINCT r.plan_assignment_user_result_id) AS asignaciones_distintas,
       COUNT(DISTINCT r.user_id)                      AS estudiantes_distintos,
       SUM(r.quiz_taken_date IS NOT NULL)             AS tomadas
FROM result_generic r
JOIN user u ON u.user_id = r.user_id AND u.role_id = 3
WHERE r.academic_year_start >= '2025-06-01';
