-- Actualiza image_url en story_slides (ids 1-76) y stories (ids 1-38), según el listado de Pedro.
-- Base de datos: gs_reading_ai_db_prod (PRODUCCIÓN). Cada UPDATE lleva WHERE id = N.
-- Cómo usarlo en MySQL Workbench:
--   1) Ejecuta SOLO el bloque "ANTES" y exporta cada resultado a CSV (copia de seguridad).
--   2) Ejecuta el bloque de UPDATE (queda dentro de una transacción SIN confirmar).
--   3) Ejecuta el bloque "VERIFICAR". Si los conteos dan 76 y 38, ejecuta COMMIT;  si no, ROLLBACK;
--   No cierres la conexión entre el paso 2 y el 3: una transacción abierta se pierde al cerrar.

USE gs_reading_ai_db_prod;

-- ===== ANTES (solo lectura) =====
SELECT id, image_url FROM story_slides WHERE id BETWEEN 1 AND 76 ORDER BY id;
SELECT id, image_url FROM stories      WHERE id BETWEEN 1 AND 38 ORDER BY id;

-- ===== UPDATE (sin confirmar hasta el COMMIT) =====
START TRANSACTION;

-- story_slides
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/story-id-1.mp4' WHERE id = 1;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/story-id-1.mp4' WHERE id = 2;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/2_3.png' WHERE id = 3;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/2_4.png' WHERE id = 4;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/3_5.png' WHERE id = 5;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/3_6.png' WHERE id = 6;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/4_7.png' WHERE id = 7;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/4_8.png' WHERE id = 8;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/5_9.png' WHERE id = 9;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/5_10.mp4' WHERE id = 10;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/6_11.png' WHERE id = 11;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/6_12.png' WHERE id = 12;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/video_story_7.mp4' WHERE id = 13;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/video_story_7.mp4' WHERE id = 14;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/8_15.png' WHERE id = 15;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/8_16.mp4' WHERE id = 16;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/9_17.png' WHERE id = 17;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/9_18.png' WHERE id = 18;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/10_19.png' WHERE id = 19;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/10_20.png' WHERE id = 20;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/11_21.png' WHERE id = 21;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/11_22.png' WHERE id = 22;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/12_23.png' WHERE id = 23;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/12_24.png' WHERE id = 24;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/13_25.png' WHERE id = 25;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/13_26.png' WHERE id = 26;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/14_27.png' WHERE id = 27;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/14_28.png' WHERE id = 28;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/video-15-part-1.mp4' WHERE id = 29;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/video-15-part-2.mp4' WHERE id = 30;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/16_31.png' WHERE id = 31;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/16_32.png' WHERE id = 32;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/17_33.png' WHERE id = 33;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/17_34.png' WHERE id = 34;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/18_35.png' WHERE id = 35;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/18_36.png' WHERE id = 36;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/19_37.png' WHERE id = 37;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/19_38.png' WHERE id = 38;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/20_39.png' WHERE id = 39;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/20_40.png' WHERE id = 40;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/21_41.png' WHERE id = 41;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/21_42.png' WHERE id = 42;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/22_43.png' WHERE id = 43;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/22_44.png' WHERE id = 44;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/23_45.png' WHERE id = 45;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/23_46.png' WHERE id = 46;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/24_47.png' WHERE id = 47;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/24_48.png' WHERE id = 48;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/25_49.png' WHERE id = 49;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/25_50.png' WHERE id = 50;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/26_51.png' WHERE id = 51;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/26_52.png' WHERE id = 52;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/27_53.png' WHERE id = 53;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/27_54.png' WHERE id = 54;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/28_55.png' WHERE id = 55;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/28_56.png' WHERE id = 56;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/29_57.png' WHERE id = 57;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/29_58.png' WHERE id = 58;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/30_59.png' WHERE id = 59;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/30_60.png' WHERE id = 60;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/31_61.png' WHERE id = 61;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/31_62.png' WHERE id = 62;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/32_63.png' WHERE id = 63;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/32_64.png' WHERE id = 64;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/33_65.png' WHERE id = 65;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/33_66.png' WHERE id = 66;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/34_67.png' WHERE id = 67;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/34_68.png' WHERE id = 68;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/35_69.png' WHERE id = 69;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/35_70.png' WHERE id = 70;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/36_71.png' WHERE id = 71;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/36_72.png' WHERE id = 72;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/37_73.png' WHERE id = 73;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/37_74.png' WHERE id = 74;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/38_75.png' WHERE id = 75;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/38_76.png' WHERE id = 76;

-- stories
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/cover_story_1_c.png' WHERE id = 1;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/2_4.png' WHERE id = 2;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/3_6.png' WHERE id = 3;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/4_8.png' WHERE id = 4;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/5_10.png' WHERE id = 5;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/6_12.png' WHERE id = 6;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/cover_story_7_c.png' WHERE id = 7;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/8_16.png' WHERE id = 8;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/9_18.png' WHERE id = 9;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/10_20.png' WHERE id = 10;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/11_22.png' WHERE id = 11;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/12_24.png' WHERE id = 12;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/13_26.png' WHERE id = 13;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/14_28.png' WHERE id = 14;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/cover-15.jpg' WHERE id = 15;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/16_32.png' WHERE id = 16;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/17_34.png' WHERE id = 17;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/18_36.png' WHERE id = 18;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/19_38.png' WHERE id = 19;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/20_40.png' WHERE id = 20;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/21_42.png' WHERE id = 21;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/22_44.png' WHERE id = 22;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/23_46.png' WHERE id = 23;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/24_48.png' WHERE id = 24;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/25_50.png' WHERE id = 25;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/26_52.png' WHERE id = 26;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/27_54.png' WHERE id = 27;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/28_56.png' WHERE id = 28;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/29_58.png' WHERE id = 29;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/30_60.png' WHERE id = 30;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/31_62.png' WHERE id = 31;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/32_64.png' WHERE id = 32;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/33_66.png' WHERE id = 33;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/34_68.png' WHERE id = 34;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/35_70.png' WHERE id = 35;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/36_72.png' WHERE id = 36;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/37_74.png' WHERE id = 37;
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/38_76.png' WHERE id = 38;

-- ===== VERIFICAR (esperado: 76 y 38) =====
SELECT COUNT(*) AS slides_actualizados FROM story_slides
 WHERE id BETWEEN 1 AND 76 AND image_url LIKE 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/%';
SELECT COUNT(*) AS stories_actualizadas FROM stories
 WHERE id BETWEEN 1 AND 38 AND image_url LIKE 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/%';

-- Si todo está bien, ejecuta (y solo entonces se guarda de verdad):
-- COMMIT;
-- Si algo no cuadra:
-- ROLLBACK;
