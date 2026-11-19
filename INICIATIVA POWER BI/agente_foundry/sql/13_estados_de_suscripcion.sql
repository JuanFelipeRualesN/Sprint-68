-- REQ-3741 · Definición de «usuario activo» y estados de suscripción (SOLO LECTURA; correr en gs_data_analysis con la cuenta de solo lectura).
-- (Confirmado el 19-nov: solo existen 'active' y 'canceled'; la tabla no tiene columna id, solo la clave invisible my_row_id.)
-- 1) ¿Qué valores reales tiene subscription_profile_history.status y cuántos usuarios hay en cada uno?
SELECT COALESCE(sph.status, '(NULL)') AS status, COUNT(*) AS filas, COUNT(DISTINCT sph.user_id) AS usuarios,
       MIN(sph.cancellation_date) AS cancelacion_minima, MAX(sph.cancellation_date) AS cancelacion_maxima
FROM subscription_profile_history sph
GROUP BY sph.status ORDER BY usuarios DESC;

-- 2) Las dos definiciones de «activo» sobre los mismos usuarios (rol ≠ client, fuera de ORE DEMOSTRACION), para ver cuántos las separan:
--    A = status 'active' SIN mirar la fecha  (vw_total_users_with_connection_details; es la que valida el tablero: 7.277 el 18-nov)
--    B = status 'active' Y cancellation_date >= NOW()  (filtro de descargas de gs-base-api y vw_not_connected; 7.199 el 12-nov)
SELECT COUNT(DISTINCT CASE WHEN sph.status = 'active' THEN us.user_id END)                                              AS activos_A_sin_fecha,
       COUNT(DISTINCT CASE WHEN sph.status = 'active' AND sph.cancellation_date >= NOW() THEN us.user_id END)           AS activos_B_con_fecha,
       COUNT(DISTINCT CASE WHEN sph.status = 'active' AND (sph.cancellation_date < NOW() OR sph.cancellation_date IS NULL)
                           THEN us.user_id END)                                                                          AS activos_A_con_fecha_vencida_o_vacia
FROM user us
LEFT JOIN school sh ON sh.school_id = us.school_id
JOIN subscription_profile_history sph ON sph.user_id = us.user_id
WHERE us.role_id <> 1 AND COALESCE(sh.ore, '') NOT LIKE '%DEMOSTRACION%';

-- 3) Lo mismo por escuela para las de la validación (p. ej. Leap STEAM: 14 activos con la definición A, 14,3 % de participación):
SELECT sh.school_name,
       COUNT(DISTINCT CASE WHEN sph.status = 'active' THEN us.user_id END)                                       AS activos_A,
       COUNT(DISTINCT CASE WHEN sph.status = 'active' AND sph.cancellation_date >= NOW() THEN us.user_id END)    AS activos_B
FROM user us
JOIN school sh ON sh.school_id = us.school_id
JOIN subscription_profile_history sph ON sph.user_id = us.user_id
WHERE us.role_id <> 1 AND sh.school_name LIKE '%Leap STEAM%'
GROUP BY sh.school_name;
