-- =====================================================================================
-- 11 · ¿QUÉ VISTA LEE LA PÁGINA «Teachers_Activity (admin)»?   (REQ-3741) · SOLO LECTURA
-- 18-nov: la página filtra por «Periodo desde/hasta» (2026-09-12 → 2026-10-15) y solo muestra Escuela del Deporte
-- (18 maestros: 8 con asignaciones, 7 con grupos). La vista de la wiki no tiene columnas de período y solo toma el
-- período vigente → la página lee OTRA variante. Copiar al chat el resultado de 1) y el «Create View» de cada vista de 2).
-- =====================================================================================
USE gs_data_analysis;

-- 1) Todas las vistas de docentes
SHOW FULL TABLES WHERE Table_type = 'VIEW' AND Tables_in_gs_data_analysis LIKE 'vw_teacher%';

-- 2) Definición de cada una (repetir con los nombres que salgan en 1)
SHOW CREATE VIEW vw_teacher_activities;
-- SHOW CREATE VIEW vw_teacher_activities_admin;   -- ← quitar el «-- » si existe
