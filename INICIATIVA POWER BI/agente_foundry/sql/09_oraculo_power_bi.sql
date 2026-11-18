-- =====================================================================================
-- 09 · ORÁCULO DE POWER BI SIN POWER BI   (REQ-3741)
-- SOLO LECTURA. Reproduce las medidas del modelo `Reports_Semantic_Model` leyendo las MISMAS vistas que usa el tablero
-- (vw_connections_report, vw_academics_details, vw_academics_average). Sirve para validar al agente/API sin abrir
-- ni consumir capacidad de Power BI: si la API da lo mismo que este script, da lo mismo que el tablero.
-- Fórmulas tomadas del modelo (INFO.MEASURES / Power Query), ver CRUCE_POWER_BI.md:
--   Conteo_Conectados_Si_Total = COUNTROWS(vw_connections_report) con connected_date no vacío   → «conexiones»
--   Conectado único            = DISTINCTCOUNT(email)                                            → «usuarios conectados»
--   Quizz_taken / not_taken    = COUNTROWS(vw_academics_details) por note = Completed / Not Completed
--   % Full Quiz (promedio)     = AVERAGE sobre vw_academics_average
--   academic_year (Power Query)= mes(academic_year_start) >= 8 ? año-(año+1) : (año-1)-año
--   Period                     = 'P' + period_no
-- Filtro de página de «Reporte de Conexión»: school_name <> 'ESCUELA FACILITADORES MSJ' (el modelo pone el nombre en mayúsculas y sin espacios sobrantes).
-- Ya incluido en las vistas: ORE NOT LIKE '%DEMOSTRACION%', usuarios sin escuela fuera, rol client fuera (conexiones), solo estudiantes (pruebas).
-- Si algún nombre de columna no existe, correr antes `DESCRIBE vw_connections_report; DESCRIBE vw_academics_details;` (sql/08).
-- =====================================================================================
USE gs_data_analysis;

-- Parámetros (mismo criterio que la API: día en hora de Puerto Rico; poner 0 para UTC)
SET @desde = '2026-07-01', @hasta = '2026-11-11', @off_horas = 4;
SET @anio_academico = '2025-2026', @periodo_min = 1, @periodo_max = 3;

-- A) Página «Reporte de Conexión»  (12-nov: Power BI 2026 T3+T4 = 113 conexiones · 14 escuelas · 84 usuarios)
SELECT COUNT(*)                      AS conexiones,
       COUNT(DISTINCT c.school_name) AS escuelas_conectadas,
       COUNT(DISTINCT c.email)       AS usuarios_conectados
FROM vw_connections_report c
WHERE c.connected_date IS NOT NULL
  AND DATE(c.connected_date - INTERVAL @off_horas HOUR) BETWEEN @desde AND @hasta
  AND UPPER(TRIM(c.school_name)) <> 'ESCUELA FACILITADORES MSJ';

-- B) Conexiones por escuela (ranking del tablero)
SELECT UPPER(TRIM(c.school_name)) AS escuela, COUNT(*) AS conexiones, COUNT(DISTINCT c.email) AS usuarios_conectados
FROM vw_connections_report c
WHERE c.connected_date IS NOT NULL
  AND DATE(c.connected_date - INTERVAL @off_horas HOUR) BETWEEN @desde AND @hasta
  AND UPPER(TRIM(c.school_name)) <> 'ESCUELA FACILITADORES MSJ'
GROUP BY escuela ORDER BY conexiones DESC, escuela;

-- C) Conexiones por rol (tarjetas teacher / student / director)
SELECT c.role, COUNT(*) AS conexiones, COUNT(DISTINCT c.email) AS usuarios
FROM vw_connections_report c
WHERE c.connected_date IS NOT NULL
  AND DATE(c.connected_date - INTERVAL @off_horas HOUR) BETWEEN @desde AND @hasta
  AND UPPER(TRIM(c.school_name)) <> 'ESCUELA FACILITADORES MSJ'
GROUP BY c.role ORDER BY conexiones DESC;

-- D) Página «Reporte Académico»: asignadas / completadas / no completadas / estudiantes
--    (12-nov: modelo 2025-2026 P1–P3 = 6.106 / 217 / 5.889 / 146)
WITH a AS (
  SELECT d.*,
         CASE WHEN MONTH(d.academic_year_start) >= 8
              THEN CONCAT(YEAR(d.academic_year_start), '-', YEAR(d.academic_year_start) + 1)
              ELSE CONCAT(YEAR(d.academic_year_start) - 1, '-', YEAR(d.academic_year_start)) END AS anio_pbi
  FROM vw_academics_details d
)
SELECT COUNT(*)                                       AS asignadas,
       SUM(a.note = 'Completed')                      AS completadas,
       SUM(a.note = 'Not Completed')                  AS no_completadas,
       COUNT(DISTINCT a.sie)                          AS estudiantes
FROM a
WHERE a.anio_pbi = @anio_academico AND a.period_no BETWEEN @periodo_min AND @periodo_max;

-- E) Lo mismo por período (debe dar P1 51/1.333 · P2 152/1.831 · P3 14/2.725 · P4 0/1.011)
WITH a AS (
  SELECT d.*,
         CASE WHEN MONTH(d.academic_year_start) >= 8
              THEN CONCAT(YEAR(d.academic_year_start), '-', YEAR(d.academic_year_start) + 1)
              ELSE CONCAT(YEAR(d.academic_year_start) - 1, '-', YEAR(d.academic_year_start)) END AS anio_pbi
  FROM vw_academics_details d
)
SELECT CONCAT('P', a.period_no) AS periodo, SUM(a.note = 'Completed') AS tomadas,
       SUM(a.note = 'Not Completed') AS no_tomadas, COUNT(*) AS filas
FROM a WHERE a.anio_pbi = @anio_academico
GROUP BY a.period_no ORDER BY a.period_no;

-- F) Total histórico de la vista de pruebas (debe dar 10.628 filas)
SELECT COUNT(*) AS filas_vw_academics_details FROM vw_academics_details;
