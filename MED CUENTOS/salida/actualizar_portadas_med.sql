-- Portadas de los 108 cuentos nuevos de LecturIA (Ids 48 a 155).
-- Pone la URL de la imagen en stories.image_url y en TODAS las diapositivas del cuento (story_slides.image_url).
-- Base de datos: gs_reading_ai_db_prod (PRODUCCIÓN).
-- Las imágenes DEBEN estar ya subidas al storage (https://stpubliceastus001.blob.core.windows.net/public-assets/stories/) antes de correr esto.
-- Cómo usarlo en MySQL Workbench:
--   0) Ejecuta el bloque "ANTES" y guarda el resultado (por si hay que volver atrás).
--   1) Selecciona desde START TRANSACTION hasta el último UPDATE y ejecútalo (queda sin confirmar).
--   2) Ejecuta el bloque VERIFICAR. Si dan 108 y el total de slides esperado, ejecuta COMMIT; si no, ROLLBACK;
--   Cada UPDATE de stories incluye el título: si un Id no coincide con su cuento, no cambia nada (0 rows).

USE gs_reading_ai_db_prod;

-- ===== ANTES (solo lectura) =====
SELECT id, title_en, image_url FROM stories WHERE id BETWEEN 48 AND 155 ORDER BY id;
SELECT story_id, COUNT(*) AS slides, MIN(image_url) AS image_url_min FROM story_slides
 WHERE story_id BETWEEN 48 AND 155 GROUP BY story_id ORDER BY story_id;

-- ===== UPDATE (sin confirmar hasta el COMMIT) =====
START TRANSACTION;

-- stories
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_48.jpg' WHERE id = 48 AND title_en = 'The Memory Tree';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_49.jpg' WHERE id = 49 AND title_en = 'The Chair by the Window';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_50.jpg' WHERE id = 50 AND title_en = 'The Mangrove Map';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_51.jpg' WHERE id = 51 AND title_en = 'The Bottle Cap Machine';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_52.jpg' WHERE id = 52 AND title_en = 'The Friday Menu';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_53.jpg' WHERE id = 53 AND title_en = 'An Unsigned Piece of News';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_54.jpg' WHERE id = 54 AND title_en = 'The Plaza of Sounds';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_55.jpg' WHERE id = 55 AND title_en = 'The Rainbow Ramp';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_56.jpg' WHERE id = 56 AND title_en = 'The Neighborhood Water';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_57.jpg' WHERE id = 57 AND title_en = 'The Idea Bank';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_58.jpg' WHERE id = 58 AND title_en = 'Samuel''s Race';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_59.jpg' WHERE id = 59 AND title_en = 'The Festival Photo';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_60.jpg' WHERE id = 60 AND title_en = 'Grandma''s Gift';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_61.jpg' WHERE id = 61 AND title_en = 'The Last Bottle';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_62.jpg' WHERE id = 62 AND title_en = 'Toñito the Invisible';
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
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_78.jpg' WHERE id = 78 AND title_en = 'The Lighthouse and the Two Proposals';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_79.jpg' WHERE id = 79 AND title_en = 'The Hard Promise';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_80.jpg' WHERE id = 80 AND title_en = 'The Voices of the Mangrove';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_81.jpg' WHERE id = 81 AND title_en = 'The Notebook of Trials';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_82.jpg' WHERE id = 82 AND title_en = 'The Screen and the Rumor';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_83.jpg' WHERE id = 83 AND title_en = 'The Recipe of Three Homes';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_84.jpg' WHERE id = 84 AND title_en = 'The Price of the Shortcut';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_85.jpg' WHERE id = 85 AND title_en = 'The Mural of Differences';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_86.jpg' WHERE id = 86 AND title_en = 'The Extra Light';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_87.jpg' WHERE id = 87 AND title_en = 'The Seed Business';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_88.jpg' WHERE id = 88 AND title_en = 'The Incomplete Photograph';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_89.jpg' WHERE id = 89 AND title_en = 'The Garden That Listens';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_90.jpg' WHERE id = 90 AND title_en = 'The Story Behind the Drum';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_91.jpg' WHERE id = 91 AND title_en = 'The Ending That Wasn''t Written';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_92.jpg' WHERE id = 92 AND title_en = 'A Day at the Beach';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_93.jpg' WHERE id = 93 AND title_en = 'Taking Care of Our Pets';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_94.jpg' WHERE id = 94 AND title_en = 'The Tadpole and the Little Fish';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_95.jpg' WHERE id = 95 AND title_en = 'The Legend of El Dorado';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_96.jpg' WHERE id = 96 AND title_en = 'The Secret of the Jamaican Sea';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_97.jpg' WHERE id = 97 AND title_en = 'The Coquí';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_98.jpg' WHERE id = 98 AND title_en = 'The Maga: National Tree';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_99.jpg' WHERE id = 99 AND title_en = 'Cuba';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_100.jpg' WHERE id = 100 AND title_en = 'Fable: The Tortoise and the Rabbit of the Panama Canal';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_101.jpg' WHERE id = 101 AND title_en = 'Pedro''s Dream';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_102.jpg' WHERE id = 102 AND title_en = 'The Lady of Liberty';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_103.jpg' WHERE id = 103 AND title_en = 'United States, Land of Dreams';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_104.jpg' WHERE id = 104 AND title_en = 'The Shared Umbrella';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_105.jpg' WHERE id = 105 AND title_en = 'The Seed from the Stream';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_106.jpg' WHERE id = 106 AND title_en = 'Alberto and the Secret of the Water';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_107.jpg' WHERE id = 107 AND title_en = 'The Grocery List';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_108.jpg' WHERE id = 108 AND title_en = 'Nocturnal Animals';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_109.jpg' WHERE id = 109 AND title_en = 'The House of Instruments';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_110.jpg' WHERE id = 110 AND title_en = 'Egypt';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_111.jpg' WHERE id = 111 AND title_en = 'Wildlife';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_112.jpg' WHERE id = 112 AND title_en = 'Madagascar';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_113.jpg' WHERE id = 113 AND title_en = 'The Adventurous Lemur of Madagascar';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_114.jpg' WHERE id = 114 AND title_en = 'The Fox and the Crow';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_115.jpg' WHERE id = 115 AND title_en = 'Christmas and the Three Kings';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_116.jpg' WHERE id = 116 AND title_en = 'The Maga: National Tree';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_117.jpg' WHERE id = 117 AND title_en = 'The Rolling Cheese Festival';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_118.jpg' WHERE id = 118 AND title_en = 'Bao the Panda Bear';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_119.jpg' WHERE id = 119 AND title_en = 'Little Ravi and the Light of Diwali';
UPDATE stories SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_120.jpg' WHERE id = 120 AND title_en = 'Alexei''s Dream';
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

-- story_slides (todas las diapositivas de cada cuento)
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_48.jpg' WHERE story_id = 48;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_49.jpg' WHERE story_id = 49;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_50.jpg' WHERE story_id = 50;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_51.jpg' WHERE story_id = 51;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_52.jpg' WHERE story_id = 52;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_53.jpg' WHERE story_id = 53;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_54.jpg' WHERE story_id = 54;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_55.jpg' WHERE story_id = 55;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_56.jpg' WHERE story_id = 56;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_57.jpg' WHERE story_id = 57;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_58.jpg' WHERE story_id = 58;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_59.jpg' WHERE story_id = 59;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_60.jpg' WHERE story_id = 60;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_61.jpg' WHERE story_id = 61;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_62.jpg' WHERE story_id = 62;
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
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_78.jpg' WHERE story_id = 78;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_79.jpg' WHERE story_id = 79;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_80.jpg' WHERE story_id = 80;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_81.jpg' WHERE story_id = 81;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_82.jpg' WHERE story_id = 82;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_83.jpg' WHERE story_id = 83;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_84.jpg' WHERE story_id = 84;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_85.jpg' WHERE story_id = 85;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_86.jpg' WHERE story_id = 86;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_87.jpg' WHERE story_id = 87;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_88.jpg' WHERE story_id = 88;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_89.jpg' WHERE story_id = 89;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_90.jpg' WHERE story_id = 90;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_91.jpg' WHERE story_id = 91;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_92.jpg' WHERE story_id = 92;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_93.jpg' WHERE story_id = 93;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_94.jpg' WHERE story_id = 94;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_95.jpg' WHERE story_id = 95;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_96.jpg' WHERE story_id = 96;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_97.jpg' WHERE story_id = 97;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_98.jpg' WHERE story_id = 98;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_99.jpg' WHERE story_id = 99;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_100.jpg' WHERE story_id = 100;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_101.jpg' WHERE story_id = 101;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_102.jpg' WHERE story_id = 102;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_103.jpg' WHERE story_id = 103;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_104.jpg' WHERE story_id = 104;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_105.jpg' WHERE story_id = 105;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_106.jpg' WHERE story_id = 106;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_107.jpg' WHERE story_id = 107;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_108.jpg' WHERE story_id = 108;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_109.jpg' WHERE story_id = 109;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_110.jpg' WHERE story_id = 110;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_111.jpg' WHERE story_id = 111;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_112.jpg' WHERE story_id = 112;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_113.jpg' WHERE story_id = 113;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_114.jpg' WHERE story_id = 114;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_115.jpg' WHERE story_id = 115;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_116.jpg' WHERE story_id = 116;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_117.jpg' WHERE story_id = 117;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_118.jpg' WHERE story_id = 118;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_119.jpg' WHERE story_id = 119;
UPDATE story_slides SET image_url = 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_120.jpg' WHERE story_id = 120;
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

-- ===== VERIFICAR =====
SELECT COUNT(*) AS stories_actualizadas FROM stories
 WHERE id BETWEEN 48 AND 155 AND image_url LIKE 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_%';
SELECT COUNT(*) AS slides_actualizados, COUNT(DISTINCT story_id) AS cuentos_con_slides FROM story_slides
 WHERE story_id BETWEEN 48 AND 155 AND image_url LIKE 'https://stpubliceastus001.blob.core.windows.net/public-assets/stories/med_story_%';
SELECT COUNT(*) AS slides_totales_de_estos_cuentos FROM story_slides
 WHERE story_id BETWEEN 48 AND 155;

-- Si stories_actualizadas = 108 y slides_actualizados = slides_totales_de_estos_cuentos, ejecuta:
-- COMMIT;
-- Si algo no cuadra:
-- ROLLBACK;
