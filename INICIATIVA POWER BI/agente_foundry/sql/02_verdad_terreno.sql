-- =====================================================================================
-- 02 · VERDAD DE TERRENO: la respuesta correcta de las 7 preguntas del ticket   (REQ-3741)
-- SOLO LECTURA. Ejecutar en MySQL Workbench y comparar UNA A UNA con lo que responde el agente
-- (y con la página equivalente del tablero de Power BI). Si una cifra no coincide, el error está
-- en el agente o en la consulta de la API (ConsultasMySql.cs), no en este script.
--
-- Definiciones, confirmadas con el esquema real (descubrimiento del 12-nov-2026):
--   · conexión = una fila de connected_date = un usuario en un día (hay UNA fila por usuario-día), cualquier rol
--   · docente = user.role_id 5 · estudiante = role_id 3
--   · área = materia (subject) [definición pendiente en el ticket]; universo = materias con pruebas asignadas alguna vez
--   · pruebas: UNA sola base de fechas, la de asignación (quiz_assigned_to_plan_date). pruebas_asignadas = asignadas en el
--     período; pruebas_completadas = de esas, las que ya tienen quiz_taken_date (a la fecha de los datos), así que
--     completadas <= asignadas; porcentaje_completado = completadas / asignadas
--   · "por escuela": solo escuelas con al menos un usuario (63 de 65)
--   · los timestamps están en UTC (servidor en +00:00) pero el "día" se lee en HORA DE PUERTO RICO (UTC-4, decisión del
--     12-nov-2026): medianoche local = 04:00 UTC. Por eso todas las fronteras se corren @off_horas = 4 horas.
--     Para volver a días UTC: SET @off_horas = 0;  (la API: Datos__OffsetUtcMinutos=0)
--   · "hasta" es INCLUSIVO: se filtra < hasta + 1 día
-- =====================================================================================
-- NOTA TÉCNICA: no usar `r.user_id IN (SELECT ...)` dentro del ON de un LEFT JOIN; en MySQL 8 dio resultados equivocados
-- (contaba filas de docentes como de estudiantes). Aquí se usa una tabla derivada con JOIN explícito.
-- EXCLUSIÓN de cuentas de prueba (la API las excluye por defecto): para igualar sus cifras agrega a cada consulta
--   · escuelas: AND COALESCE(sh.ore,'') <> 'DEMOSTRACION' AND sh.school_name <> 'Escuela Demo T3 School'
--   · usuarios: AND COALESCE(us.email,'') NOT LIKE '%@requiretechnology.com'
-- (o ejecuta la API con Datos__ExcluirCuentasDePrueba=false para comparar sin exclusión, como hace Power BI).
USE gs_data_analysis;

SET @off_horas      = 4;                 -- 4 = hora de Puerto Rico (UTC-4) · 0 = UTC
SET @desde          = '2026-09-01';      -- día local, inclusive
SET @hasta          = '2026-11-10';      -- día local, inclusive
SET @ini            = DATE_ADD(@desde, INTERVAL @off_horas HOUR);                          -- instante UTC de inicio
SET @fin            = DATE_ADD(DATE_ADD(@hasta, INTERVAL 1 DAY), INTERVAL @off_horas HOUR); -- instante UTC de fin (exclusivo)
-- cortes fijos usados en Q4, Q5 y Q6 (todos como instantes UTC)
SET @anio_ini = DATE_ADD('2026-01-01', INTERVAL @off_horas HOUR);
SET @anio_fin = DATE_ADD('2027-01-01', INTERVAL @off_horas HOUR);
SET @hoy_fin  = DATE_ADD('2026-11-11', INTERVAL @off_horas HOUR);   -- fin exclusivo de "hasta 2026-11-10"
SET @sep_ini  = DATE_ADD('2026-09-01', INTERVAL @off_horas HOUR);
SET @oct_ini  = DATE_ADD('2026-10-01', INTERVAL @off_horas HOUR);
SET @nov_ini  = DATE_ADD('2026-11-01', INTERVAL @off_horas HOUR);
SET @rol_estudiante = 3;
SET @rol_docente    = 5;

-- ── Frescura (lo que la API devuelve como datos_hasta) ───────────────────────────────
SELECT (SELECT DATE(DATE_SUB(MAX(connected_date), INTERVAL @off_horas HOUR)) FROM connected_date) AS datos_hasta_conexiones,
       (SELECT DATE(DATE_SUB(GREATEST(MAX(quiz_taken_date), MAX(quiz_assigned_to_plan_date)), INTERVAL @off_horas HOUR)) FROM result_generic) AS datos_hasta_pruebas;

-- ── Q1/Q2 · Áreas más / menos utilizadas entre @desde y @hasta ────────────────────────
SELECT s.subject_name AS area,
       COALESCE(SUM(r.quiz_assigned_to_plan_date >= @ini AND r.quiz_assigned_to_plan_date < @fin), 0) AS pruebas_asignadas,
       COALESCE(SUM(r.quiz_assigned_to_plan_date >= @ini AND r.quiz_assigned_to_plan_date < @fin AND r.quiz_taken_date IS NOT NULL), 0) AS pruebas_completadas,
       ROUND(100 * SUM(r.quiz_assigned_to_plan_date >= @ini AND r.quiz_assigned_to_plan_date < @fin AND r.quiz_taken_date IS NOT NULL)
                 / NULLIF(SUM(r.quiz_assigned_to_plan_date >= @ini AND r.quiz_assigned_to_plan_date < @fin), 0), 1) AS porcentaje_completado
FROM subject s
LEFT JOIN (SELECT rr.subject_code, rr.quiz_assigned_to_plan_date, rr.quiz_taken_date
           FROM result_generic rr
           JOIN user us ON us.user_id = rr.user_id AND us.role_id = @rol_estudiante) r ON r.subject_code = s.subject_code
WHERE EXISTS (SELECT 1 FROM result_generic x WHERE x.subject_code = s.subject_code)
GROUP BY s.subject_code, s.subject_name
ORDER BY pruebas_completadas DESC, s.subject_name DESC;      -- Q1: primera fila · Q2: leer de abajo hacia arriba

-- ── Q3 · Escuelas con más conexiones (lo corrido de 2026) ─────────────────────────────
-- Cambiar @desde/@hasta (y recalcular @ini/@fin) a '2026-01-01' / '2026-11-10' para "lo corrido de 2026".
SELECT sh.school_name AS escuela,
       COUNT(dt.id)                  AS conexiones,          -- lo que cuenta la API
       COUNT(DISTINCT dt.user_id)    AS usuarios_distintos   -- para cruzar con "Conectado único" del tablero
FROM school sh
LEFT JOIN user us ON us.school_id = sh.school_id
LEFT JOIN connected_date dt ON dt.user_id = us.user_id AND dt.connected_date >= @ini AND dt.connected_date < @fin
WHERE EXISTS (SELECT 1 FROM user u2 WHERE u2.school_id = sh.school_id)
GROUP BY sh.school_id, sh.school_name
ORDER BY conexiones DESC, sh.school_name DESC;

-- ── Q4 · Maestros con más conexiones (año 2026) ───────────────────────────────────────
-- PRIVACIDAD: devuelve nombres de docentes; usar solo para validar, no pegar en chats ni documentos.
SELECT TRIM(CONCAT_WS(' ', us.first_name, us.last_name)) AS maestro,
       COALESCE(sh.school_name, '(sin escuela)')          AS escuela,
       COUNT(dt.id)                                       AS conexiones
FROM user us
LEFT JOIN school sh ON sh.school_id = us.school_id
LEFT JOIN connected_date dt ON dt.user_id = us.user_id AND dt.connected_date >= @anio_ini AND dt.connected_date < @hoy_fin
WHERE us.role_id = @rol_docente
GROUP BY us.user_id, us.first_name, us.last_name, sh.school_name
ORDER BY conexiones DESC, maestro DESC, us.user_id
LIMIT 10;

-- ── Q5 · Comparación de áreas: septiembre vs octubre 2026 (completadas de las asignadas en cada mes) ──
SELECT s.subject_name AS area,
       COALESCE(SUM(r.quiz_assigned_to_plan_date >= @sep_ini AND r.quiz_assigned_to_plan_date < @oct_ini AND r.quiz_taken_date IS NOT NULL), 0) AS septiembre,
       COALESCE(SUM(r.quiz_assigned_to_plan_date >= @oct_ini AND r.quiz_assigned_to_plan_date < @nov_ini AND r.quiz_taken_date IS NOT NULL), 0) AS octubre,
       COALESCE(SUM(r.quiz_assigned_to_plan_date >= @oct_ini AND r.quiz_assigned_to_plan_date < @nov_ini AND r.quiz_taken_date IS NOT NULL), 0)
     - COALESCE(SUM(r.quiz_assigned_to_plan_date >= @sep_ini AND r.quiz_assigned_to_plan_date < @oct_ini AND r.quiz_taken_date IS NOT NULL), 0) AS diferencia
FROM subject s
LEFT JOIN (SELECT rr.subject_code, rr.quiz_assigned_to_plan_date, rr.quiz_taken_date
           FROM result_generic rr
           JOIN user us ON us.user_id = rr.user_id AND us.role_id = @rol_estudiante) r ON r.subject_code = s.subject_code
WHERE EXISTS (SELECT 1 FROM result_generic x WHERE x.subject_code = s.subject_code)
GROUP BY s.subject_code, s.subject_name
ORDER BY s.subject_name;

-- ── Q6 · Tendencia de conexiones por trimestre en 2026 ────────────────────────────────
SELECT t.trimestre, COUNT(*) AS conexiones
FROM (SELECT CONCAT(YEAR(DATE_SUB(connected_date, INTERVAL @off_horas HOUR)), '-T',
                    QUARTER(DATE_SUB(connected_date, INTERVAL @off_horas HOUR))) AS trimestre   -- el trimestre se lee en hora local
      FROM connected_date
      WHERE connected_date >= @anio_ini AND connected_date < @anio_fin) t
GROUP BY t.trimestre
ORDER BY t.trimestre;

-- ── Q7 · Escuelas con MENOR actividad entre @desde y @hasta (incluye las de 0 conexiones) ─
-- Es la misma consulta de Q3 leída de abajo hacia arriba (ORDER BY conexiones ASC).
SELECT sh.school_name AS escuela, COUNT(dt.id) AS conexiones
FROM school sh
LEFT JOIN user us ON us.school_id = sh.school_id
LEFT JOIN connected_date dt ON dt.user_id = us.user_id AND dt.connected_date >= @ini AND dt.connected_date < @fin
WHERE EXISTS (SELECT 1 FROM user u2 WHERE u2.school_id = sh.school_id)
GROUP BY sh.school_id, sh.school_name
ORDER BY conexiones ASC, sh.school_name ASC
LIMIT 10;
