-- =====================================================================================
-- 04 · DIAGNÓSTICO: escuelas y cuentas internas / de prueba que distorsionan los rankings   (REQ-3741)
-- SOLO LECTURA. En las respuestas reales aparecen "Escuela Calidad Grupo Require", "Escuela DEMO",
-- "Escuela 12.1.2023a", "Escuela Pruebas Rezago", docentes internos... y dominan "más conexiones" y "menor actividad".
-- Estas dos consultas muestran SOLO conteos y nombres de escuelas (sin nombres de personas ni emails completos):
-- se pueden pegar en el chat. Con ellas se decide qué excluir.
--
-- v2: cada tabla se recorre UNA sola vez (tablas derivadas agrupadas + JOIN). La v1 usaba subconsultas
-- correlacionadas por escuela y era muy lenta en producción. Ejecutar y ESPERAR: debe tardar segundos.
-- Si pasa de ~20 s, pulsar Stop (es solo lectura, no pasa nada) y avisar.
-- =====================================================================================
USE gs_data_analysis;

-- ── A. Las 65 escuelas: tipo, usuarios y actividad (decidir cuáles son de prueba/internas) ──
SELECT sh.school_id,
       sh.school_name,
       sh.school_type,
       sh.ore,
       COALESCE(u.usuarios, 0)         AS usuarios,
       COALESCE(u.estudiantes, 0)      AS estudiantes,
       COALESCE(u.docentes, 0)         AS docentes,
       COALESCE(c.conexiones_2026, 0)  AS conexiones_2026,
       c.ultima_conexion
FROM school sh
LEFT JOIN (SELECT school_id,
                  COUNT(*)          AS usuarios,
                  SUM(role_id = 3)  AS estudiantes,
                  SUM(role_id = 5)  AS docentes
           FROM user
           GROUP BY school_id) u ON u.school_id = sh.school_id
LEFT JOIN (SELECT us.school_id,
                  SUM(d.connected_date >= '2026-01-01') AS conexiones_2026,
                  DATE(MAX(d.connected_date))           AS ultima_conexion
           FROM connected_date d
           JOIN user us ON us.user_id = d.user_id
           GROUP BY us.school_id) c ON c.school_id = sh.school_id
ORDER BY conexiones_2026 DESC, sh.school_name;

-- ── B. Dominio de correo de los usuarios (solo el dominio, agregado): detecta cuentas internas ──
SELECT x.dominio,
       COUNT(*)                          AS usuarios,
       COALESCE(SUM(x.conexiones_2026), 0) AS conexiones_2026
FROM (SELECT SUBSTRING_INDEX(u.email, '@', -1) AS dominio,
             u.user_id,
             COALESCE(c.n, 0)                  AS conexiones_2026
      FROM user u
      LEFT JOIN (SELECT user_id, COUNT(*) AS n
                 FROM connected_date
                 WHERE connected_date >= '2026-01-01'
                 GROUP BY user_id) c ON c.user_id = u.user_id) x
GROUP BY x.dominio
ORDER BY conexiones_2026 DESC
LIMIT 15;
