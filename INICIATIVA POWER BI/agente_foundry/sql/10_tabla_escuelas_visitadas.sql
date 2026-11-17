-- =====================================================================================
-- 10 · TABLA PARA EL EXCEL DE ESCUELAS VISITADAS   (REQ-3741)
-- ATENCIÓN: crea una tabla y un usuario. La ejecuta un administrador del MySQL de Azure, después de sql/09.
-- La API la lee en /escuelas-visitadas (P5). Mientras esté vacía o no exista, esa consulta responde 503 «fuente_no_disponible».
--
-- Por qué una tabla y no el Excel directo: el Excel vive hoy en el OneDrive personal de una persona y todas sus columnas
-- son texto (también las fechas). Aquí queda con tipos (DATE, booleano) y se cruza en SQL con usuarios y conexiones.
-- Cómo se llena (de más simple a más automático):
--   1) A mano: exportar Table1 del Excel a CSV y usar «Table Data Import Wizard» de Workbench sobre esta tabla
--      (o el INSERT de ejemplo del final, una fila por escuela).
--   2) Automático: un pipeline de Azure Data Factory o una Logic App que lea el Excel desde SharePoint y reemplace las filas
--      cada día, con el usuario agente_cargador (solo puede escribir en esta tabla).
-- =====================================================================================

SET NAMES utf8mb4;   -- obligatorio: con otro juego de caracteres los textos con tildes quedan mal guardados
CREATE TABLE IF NOT EXISTS agente.escuelas_visitadas (
    school_identifier    VARCHAR(20)  NOT NULL PRIMARY KEY,   -- clave que usa Power BI para cruzar con vw_users_report
    school_name          VARCHAR(250) NOT NULL,                -- en MAYÚSCULAS, como lo deja el Power Query del tablero
    school_id            INT          NULL,
    sistema_educativo    VARCHAR(30)  NULL,                    -- columna propia del Excel (ORE / SEMI/ALIANZA)
    visitada             TINYINT(1)   NOT NULL,                -- 1 = «Si», 0 = «No»
    fecha_visita_inicial DATE         NULL,
    fecha_ultima_visita  DATE         NULL,
    periodo_excel        VARCHAR(50)  NULL,                    -- texto escrito a mano en el Excel; solo de referencia (no cuadra con las fechas)
    cargado_en           DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Usuario para la carga automática: solo puede escribir en esta tabla.
CREATE USER IF NOT EXISTS 'agente_cargador'@'%' IDENTIFIED BY 'CAMBIAR_POR_CONTRASEÑA_DE_KEY_VAULT';
GRANT SELECT, INSERT, UPDATE, DELETE ON agente.escuelas_visitadas TO 'agente_cargador'@'%';

-- Ejemplo de carga a mano (las fechas del Excel vienen como texto M/D/AAAA):
-- INSERT INTO agente.escuelas_visitadas
--   (school_identifier, school_name, school_id, sistema_educativo, visitada, fecha_visita_inicial, fecha_ultima_visita, periodo_excel)
-- VALUES ('62984', 'ALBERT EINSTEIN', 392, 'ORE', 1, STR_TO_DATE('9/16/2025', '%c/%e/%Y'), STR_TO_DATE('12/3/2025', '%c/%e/%Y'), '1er Sem 2024-2025')
-- ON DUPLICATE KEY UPDATE school_name = VALUES(school_name), visitada = VALUES(visitada),
--   fecha_visita_inicial = VALUES(fecha_visita_inicial), fecha_ultima_visita = VALUES(fecha_ultima_visita), cargado_en = CURRENT_TIMESTAMP;

-- Verificación (esperado con el Excel actual: 17 visitadas, 30 no visitadas, 47 en total)
SELECT SUM(visitada = 1) AS visitadas, SUM(visitada = 0) AS no_visitadas, COUNT(*) AS total, MAX(cargado_en) AS cargado_en
FROM agente.escuelas_visitadas;
