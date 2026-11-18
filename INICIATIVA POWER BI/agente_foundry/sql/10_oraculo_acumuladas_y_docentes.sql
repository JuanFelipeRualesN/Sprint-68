-- =====================================================================================
-- 10 · ORÁCULO: «CONEXIONES ACUMULADAS» Y «USO DE DOCENTES»   (REQ-3741)
-- SOLO LECTURA. Páginas de MSJ_Utilization que el admin ve en Reportes → Administrativo.
-- Las vistas salen del DDL de la wiki («PowerBI Data Analysis Dashboard Setup and Breakdown»).
-- ⚠️ No se sabe aún qué medida usa cada tarjeta de «Conexiones acumuladas»: se calculan las 3 lecturas posibles
--    (A, B, C). La correcta se confirma con dax/medidas_y_particiones.dax o comparando una vez con el tablero.
-- ⚠️ Ambas páginas dependen de NOW()/SYSDATE() (suscripción vigente, período actual): Power BI guarda la foto
--    del refresco diario (~0:00), así que hay que comparar el MISMO día.
-- =====================================================================================
USE gs_data_analysis;
SET @desde = '2026-01-01', @hasta = '2026-11-18', @off_horas = 4;

-- ---------------------------------------------------------------- CONEXIONES ACUMULADAS
-- C) Vista vw_total_users_with_connection_details: usuarios con suscripción activa que se conectaron ALGUNA VEZ
--    / total de usuarios con suscripción activa (rol ≠ client, fuera de ORE DEMOSTRACION). Es un acumulado histórico.
--    Ojo: aquí «activa» = status 'active' SIN mirar cancellation_date (en vw_not_connected sí se mira).
SELECT total_connected, total_users, ROUND(total_percent, 2) AS porcentaje
FROM vw_total_users_with_connection_details;

-- A) Acumulado mes a mes de conexiones (suma corrida) en el rango
WITH m AS (
  SELECT DATE_FORMAT(c.connected_date - INTERVAL @off_horas HOUR, '%Y-%m') AS mes, COUNT(*) AS conexiones
  FROM vw_connections_report c
  WHERE c.connected_date IS NOT NULL
    AND DATE(c.connected_date - INTERVAL @off_horas HOUR) BETWEEN @desde AND @hasta
    AND UPPER(TRIM(c.school_name)) <> 'ESCUELA FACILITADORES MSJ'
  GROUP BY mes
)
SELECT mes, conexiones, SUM(conexiones) OVER (ORDER BY mes) AS conexiones_acumuladas FROM m ORDER BY mes;

-- B) Usuarios distintos acumulados: cada usuario cuenta en el mes de su PRIMERA conexión del rango
WITH p AS (
  SELECT c.email, MIN(DATE(c.connected_date - INTERVAL @off_horas HOUR)) AS primera
  FROM vw_connections_report c
  WHERE c.connected_date IS NOT NULL
    AND DATE(c.connected_date - INTERVAL @off_horas HOUR) BETWEEN @desde AND @hasta
    AND UPPER(TRIM(c.school_name)) <> 'ESCUELA FACILITADORES MSJ'
  GROUP BY c.email
), m AS (SELECT DATE_FORMAT(primera, '%Y-%m') AS mes, COUNT(*) AS nuevos FROM p GROUP BY mes)
SELECT mes, nuevos, SUM(nuevos) OVER (ORDER BY mes) AS usuarios_acumulados FROM m ORDER BY mes;

-- ---------------------------------------------------------------- USO DE DOCENTES (vw_teacher_activities)
-- Docentes con suscripción activa en el período académico VIGENTE (SYSDATE entre period_from y period_to),
-- sin las escuelas de prueba por número ('00000X','REQ120349DM','w45567','TRAININGSCHOOLS','12.01.2023').
-- Cada indicador es 'SI'/'NO':
--   has_connections_current_period                  se conectó en el período actual
--   has_created_instructional_plans_current_period  creó planes instruccionales en el período
--   plans_have_data_current_period                  esos planes tienen quizzes o lecciones
--   has_created_groups_current_year                 creó grupos en el año académico
--   has_created_assignments_overall                 ha creado alguna asignación (histórico)
-- La vista hace DISTINCT de la fila completa: un docente con 2 suscripciones o 2 escuelas sale 2 veces.
-- Se cuentan filas (lo más probable en COUNTROWS) y docentes distintos (sie) para detectar la diferencia.

-- D) Totales de la página
SELECT COUNT(*)                                                   AS filas,
       COUNT(DISTINCT sie)                                        AS docentes,
       SUM(has_connections_current_period = 'SI')                 AS conectados_periodo,
       SUM(has_created_instructional_plans_current_period = 'SI') AS con_planes,
       SUM(plans_have_data_current_period = 'SI')                 AS planes_con_contenido,
       SUM(has_created_groups_current_year = 'SI')                AS con_grupos,
       SUM(has_created_assignments_overall = 'SI')                AS con_asignaciones
FROM vw_teacher_activities;

-- E) Por escuela
SELECT school, school_number, COUNT(*) AS filas, COUNT(DISTINCT sie) AS docentes,
       SUM(has_connections_current_period = 'SI')                 AS conectados_periodo,
       SUM(has_created_instructional_plans_current_period = 'SI') AS con_planes,
       SUM(plans_have_data_current_period = 'SI')                 AS planes_con_contenido,
       SUM(has_created_groups_current_year = 'SI')                AS con_grupos,
       SUM(has_created_assignments_overall = 'SI')                AS con_asignaciones
FROM vw_teacher_activities
GROUP BY school, school_number ORDER BY docentes DESC, school;

-- F) Docentes duplicados (explica una diferencia filas vs. docentes)
SELECT sie, teacher_name, COUNT(*) AS veces
FROM vw_teacher_activities GROUP BY sie, teacher_name HAVING COUNT(*) > 1 ORDER BY veces DESC LIMIT 20;
