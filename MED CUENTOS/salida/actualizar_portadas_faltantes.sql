-- Portadas faltantes: 51 cuentos (Ids 63-77, 92 y 121-155) que seguían con categories/scifi.png.
-- Base de datos: gs_reading_ai_db_prod (PRODUCCIÓN). Las imágenes ya están en el storage.
-- Cómo usarlo: 1) Selecciona desde START TRANSACTION hasta el último UPDATE y ejecútalo.
--              2) Ejecuta las 3 consultas de VERIFICAR. 3) Si cuadran, selecciona COMMIT; y ejecútalo.

USE gs_reading_ai_db_prod;

START TRANSACTION;

-- stories
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_141.jpg' WHERE id = 141 AND title_en = 'The Moon';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_142.jpg' WHERE id = 142 AND title_en = 'My Little Parrot';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_143.jpg' WHERE id = 143 AND title_en = 'Honorato the Duck';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_144.jpg' WHERE id = 144 AND title_en = 'The Flamingo';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_145.jpg' WHERE id = 145 AND title_en = 'The Little Ant';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_146.jpg' WHERE id = 146 AND title_en = 'The Lion';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_147.jpg' WHERE id = 147 AND title_en = 'Mimi and Mila';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_148.jpg' WHERE id = 148 AND title_en = 'Susi Washes Up';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_149.jpg' WHERE id = 149 AND title_en = 'The Moon and the Lagoon';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_150.jpg' WHERE id = 150 AND title_en = 'Tito and Tita';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_151.jpg' WHERE id = 151 AND title_en = 'Pepa and the Paper';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_152.jpg' WHERE id = 152 AND title_en = 'Nina and the Pineapple';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_153.jpg' WHERE id = 153 AND title_en = 'Fina and the Fruit';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_154.jpg' WHERE id = 154 AND title_en = 'Coco Creates';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_155.jpg' WHERE id = 155 AND title_en = 'Sol and the Tablet';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_63.jpg' WHERE id = 63 AND title_en = 'My Grandma Inés';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_64.jpg' WHERE id = 64 AND title_en = 'Ants';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_65.jpg' WHERE id = 65 AND title_en = 'Sofía''s Rabbit';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_66.jpg' WHERE id = 66 AND title_en = 'Mumo the Monkey';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_67.jpg' WHERE id = 67 AND title_en = 'My Friend Popi';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_68.jpg' WHERE id = 68 AND title_en = 'Little Kangaroo';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_69.jpg' WHERE id = 69 AND title_en = 'The Puerto Rican Flag';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_70.jpg' WHERE id = 70 AND title_en = 'Lía''s Snack';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_71.jpg' WHERE id = 71 AND title_en = 'A Place for Tomás';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_72.jpg' WHERE id = 72 AND title_en = 'The Toad and the Can';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_73.jpg' WHERE id = 73 AND title_en = 'Bruno''s Drum';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_74.jpg' WHERE id = 74 AND title_en = 'Clara''s Box';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_75.jpg' WHERE id = 75 AND title_en = 'Diego Looks for a Source';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_76.jpg' WHERE id = 76 AND title_en = 'The Rain and the Garden';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_77.jpg' WHERE id = 77 AND title_en = 'An Afternoon Without Screens';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_92.jpg' WHERE id = 92 AND title_en = 'A Day at the Beach';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_121.jpg' WHERE id = 121 AND title_en = 'We All Play';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_122.jpg' WHERE id = 122 AND title_en = 'My New Pal';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_123.jpg' WHERE id = 123 AND title_en = 'A Turn for Sam';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_124.jpg' WHERE id = 124 AND title_en = 'The Red Drum';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_125.jpg' WHERE id = 125 AND title_en = 'My Home Song';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_126.jpg' WHERE id = 126 AND title_en = 'The Flag Art';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_127.jpg' WHERE id = 127 AND title_en = 'The Clean Sand';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_128.jpg' WHERE id = 128 AND title_en = 'The Little Seed';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_129.jpg' WHERE id = 129 AND title_en = 'The Blue Sea';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_130.jpg' WHERE id = 130 AND title_en = 'A Box Boat';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_131.jpg' WHERE id = 131 AND title_en = 'My Big Fix';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_132.jpg' WHERE id = 132 AND title_en = 'The Cup Tower';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_133.jpg' WHERE id = 133 AND title_en = 'Wash, Wash!';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_134.jpg' WHERE id = 134 AND title_en = 'I Can Move';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_135.jpg' WHERE id = 135 AND title_en = 'My Red Apple';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_136.jpg' WHERE id = 136 AND title_en = 'Time to Rest';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_137.jpg' WHERE id = 137 AND title_en = 'The Class Tablet';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_138.jpg' WHERE id = 138 AND title_en = 'Ask First';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_139.jpg' WHERE id = 139 AND title_en = 'The Star Screen';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_140.jpg' WHERE id = 140 AND title_en = 'Tap and Stop';

-- story_slides
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_141.jpg' WHERE story_id = 141;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_142.jpg' WHERE story_id = 142;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_143.jpg' WHERE story_id = 143;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_144.jpg' WHERE story_id = 144;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_145.jpg' WHERE story_id = 145;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_146.jpg' WHERE story_id = 146;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_147.jpg' WHERE story_id = 147;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_148.jpg' WHERE story_id = 148;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_149.jpg' WHERE story_id = 149;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_150.jpg' WHERE story_id = 150;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_151.jpg' WHERE story_id = 151;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_152.jpg' WHERE story_id = 152;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_153.jpg' WHERE story_id = 153;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_154.jpg' WHERE story_id = 154;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_155.jpg' WHERE story_id = 155;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_63.jpg' WHERE story_id = 63;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_64.jpg' WHERE story_id = 64;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_65.jpg' WHERE story_id = 65;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_66.jpg' WHERE story_id = 66;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_67.jpg' WHERE story_id = 67;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_68.jpg' WHERE story_id = 68;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_69.jpg' WHERE story_id = 69;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_70.jpg' WHERE story_id = 70;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_71.jpg' WHERE story_id = 71;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_72.jpg' WHERE story_id = 72;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_73.jpg' WHERE story_id = 73;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_74.jpg' WHERE story_id = 74;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_75.jpg' WHERE story_id = 75;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_76.jpg' WHERE story_id = 76;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_77.jpg' WHERE story_id = 77;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_92.jpg' WHERE story_id = 92;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_121.jpg' WHERE story_id = 121;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_122.jpg' WHERE story_id = 122;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_123.jpg' WHERE story_id = 123;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_124.jpg' WHERE story_id = 124;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_125.jpg' WHERE story_id = 125;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_126.jpg' WHERE story_id = 126;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_127.jpg' WHERE story_id = 127;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_128.jpg' WHERE story_id = 128;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_129.jpg' WHERE story_id = 129;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_130.jpg' WHERE story_id = 130;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_131.jpg' WHERE story_id = 131;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_132.jpg' WHERE story_id = 132;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_133.jpg' WHERE story_id = 133;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_134.jpg' WHERE story_id = 134;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_135.jpg' WHERE story_id = 135;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_136.jpg' WHERE story_id = 136;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_137.jpg' WHERE story_id = 137;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_138.jpg' WHERE story_id = 138;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_139.jpg' WHERE story_id = 139;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_140.jpg' WHERE story_id = 140;

-- ===== VERIFICAR (esperado: 51 y 56 slides) =====
SELECT COUNT(*) AS stories_actualizadas FROM stories
 WHERE id IN (63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 92, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155) AND image_url LIKE '%/stories/med_story_%';
SELECT COUNT(*) AS slides_actualizados FROM story_slides
 WHERE story_id IN (63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 92, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155) AND image_url LIKE '%/stories/med_story_%';
SELECT COUNT(*) AS slides_totales FROM story_slides WHERE story_id IN (63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 92, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155);

-- Si stories_actualizadas = 51 y slides_actualizados = slides_totales, ejecuta:
-- COMMIT;
-- Si algo no cuadra:
-- ROLLBACK;
