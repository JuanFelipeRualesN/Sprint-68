-- =====================================================================================
-- 12 · ORÁCULO «USO DE DOCENTES» SOBRE LA VISTA REAL   (REQ-3741) · SOLO LECTURA
-- Definición real de vw_teacher_activities (SHOW CREATE VIEW, 18-nov), distinta de la wiki:
--   · NO filtra por período vigente: una fila por docente × período académico de su escuela
--     (columnas from_period / to_period) → por eso salen ~4 filas por docente (P1–P4).
--   · Los indicadores «del período» se calculan dentro de ESE período (conexiones en gus_activity_date_reports
--     entre from_period y to_period; planes de teacher_plans con ese period_id).
--   · Trae la columna `ore`; `sie` = us.identifier e `invitation_code` = usi.sie.
--   · Pruebas excluidas solo por school_number ('00000X','REQ120349DM','w45567','TRAININGSCHOOLS','12.01.2023').
-- Tablero 18-nov, período 2026-09-12 → 2026-10-15: solo Escuela del Deporte, 18 maestros, 8 con asignaciones,
-- 7 con grupos, 0 con planes, 0 con planes con contenido.
-- =====================================================================================
USE gs_data_analysis;

-- 1) Qué períodos hay y cuántos docentes y escuelas tiene cada uno (¿por qué el tablero solo ofrece 2026-09-12?)
SELECT from_period, to_period, COUNT(DISTINCT sie) AS docentes, COUNT(DISTINCT school) AS escuelas,
       SUM(ore LIKE '%DEMOSTRACION%') AS filas_ore_demostracion
FROM vw_teacher_activities
GROUP BY from_period, to_period ORDER BY from_period DESC;

-- 2) Réplica de la página para un período (maestros DISTINTOS por escuela, como el tablero)
SET @desde = '2026-09-12';
SELECT school, COUNT(DISTINCT sie) AS maestros,
       COUNT(DISTINCT CASE WHEN has_connections_current_period = 'SI'                 THEN sie END) AS con_conexiones,
       COUNT(DISTINCT CASE WHEN has_created_instructional_plans_current_period = 'SI' THEN sie END) AS con_planes,
       COUNT(DISTINCT CASE WHEN plans_have_data_current_period = 'SI'                 THEN sie END) AS planes_con_datos,
       COUNT(DISTINCT CASE WHEN has_created_groups_current_year = 'SI'                THEN sie END) AS con_grupos,
       COUNT(DISTINCT CASE WHEN has_created_assignments_overall = 'SI'                THEN sie END) AS con_asignaciones
FROM vw_teacher_activities
WHERE DATE(from_period) = @desde
GROUP BY school ORDER BY maestros DESC;
