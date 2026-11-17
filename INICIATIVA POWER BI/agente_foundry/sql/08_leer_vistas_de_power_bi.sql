-- =====================================================================================
-- 08 · LEER LAS VISTAS QUE USA POWER BI   (REQ-3741)
-- SOLO LECTURA (SHOW / DESCRIBE / SELECT COUNT). Hallazgo del 12-nov-2026: el modelo de Power BI NO lee las tablas
-- directamente; lee VISTAS de gs_data_analysis (vw_academics_details, vw_academics_average, vw_connections_report,
-- vw_not_connected, vw_teacher_activities, vw_users_details_report…). La medida del tablero es:
--     Quizz_taken = CALCULATE ( COUNTROWS ( vw_academics_details ), vw_academics_details[Completado/No completado] = "Completed" )
-- Es decir: «Pruebas asignadas» = filas de vw_academics_details. La lógica de negocio (qué cuenta y qué no) está en la vista.
-- Una definición de vista es solo SQL (no trae datos de personas), se puede pegar en el chat.
--
-- CÓMO COPIAR la definición: en la pestaña de resultado de SHOW CREATE VIEW, clic derecho en la celda «Create View»
-- → «Copy Field» → pegar en el chat (si es muy larga, pegar en dos mensajes).
-- =====================================================================================
USE gs_data_analysis;

-- 1) Columnas de la vista de pruebas
DESCRIBE vw_academics_details;

-- 2) LA definición (lo más importante)
SHOW CREATE VIEW vw_academics_details;

-- 3) Cuántas filas tiene y cómo se reparten por estado (Power BI: completadas 206 + no completadas 823 = 1.029 para 2025-2026)
SELECT COUNT(*) AS filas_totales FROM vw_academics_details;

-- 4) La vista de promedios y la de conexiones (para dejar la API idéntica al tablero también ahí)
SHOW CREATE VIEW vw_academics_average;
SHOW CREATE VIEW vw_connections_report;
