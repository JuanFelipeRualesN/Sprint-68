#!/usr/bin/env python3
"""Prepara las portadas para subirlas al storage y enlazarlas en la base de datos.

Lee salida/prompts_imagenes.csv (id del cuento en el sistema + título) y las imágenes de salida/imagenes.
Escribe:
  * salida/imagenes_subir/med_story_<Id>.jpg   copia de cada imagen con el nombre que irá en el storage
  * salida/imagenes_subir/mapa.csv             Id, archivo original, archivo a subir, URL final
  * salida/actualizar_portadas_med.sql         UPDATE de stories.image_url y story_slides.image_url

Cada UPDATE de stories lleva WHERE id = N AND title_en = '<título>': si el Id no corresponde al cuento,
no cambia nada. Los slides se actualizan por story_id.

Uso:  python3 -I herramientas/preparar_portadas.py [--base URL_DEL_STORAGE/]
"""
import argparse
import csv
import shutil
from pathlib import Path

BASE = "https://stpubliceastus001.blob.core.windows.net/public-assets/stories/"
EXT = (".jpg", ".png", ".webp")


def sql_txt(s):
    return "'" + s.replace("\\", "\\\\").replace("'", "''") + "'"


def main():
    raiz = Path(__file__).resolve().parent.parent
    ap = argparse.ArgumentParser()
    ap.add_argument("--base", default=BASE, help="URL base donde quedarán las imágenes (termina en /)")
    a = ap.parse_args()
    base = a.base if a.base.endswith("/") else a.base + "/"

    with open(raiz / "salida" / "prompts_imagenes.csv", encoding="utf-8-sig") as fh:
        filas = list(csv.DictReader(fh))
    orig, dest = raiz / "salida" / "imagenes", raiz / "salida" / "imagenes_subir"
    dest.mkdir(exist_ok=True)
    for viejo in dest.iterdir():
        viejo.unlink()

    mapa, sql_st, sql_sl = [], [], []
    for f in filas:
        if not f["id_cuento"]:
            raise SystemExit(f"{f['imagen']}: sin Id de cuento")
        cid = int(f["id_cuento"])
        stem = Path(f["imagen"]).stem
        src = next((orig / (stem + e) for e in EXT if (orig / (stem + e)).exists()), None)
        if not src:
            raise SystemExit(f"{stem}: no existe la imagen en {orig}")
        nombre = f"med_story_{cid}{src.suffix}"
        shutil.copy2(src, dest / nombre)
        url = base + nombre
        mapa.append([cid, src.name, nombre, url])
        sql_st.append(f"UPDATE stories SET image_url = {sql_txt(url)} WHERE id = {cid} AND title_en = {sql_txt(f['TitleEn'])};")
        sql_sl.append(f"UPDATE story_slides SET image_url = {sql_txt(url)} WHERE story_id = {cid};")
    mapa.sort()
    ids = sorted(m[0] for m in mapa)
    assert len(set(ids)) == len(ids), "Ids repetidos"

    with open(dest / "mapa.csv", "w", newline="", encoding="utf-8-sig") as fh:
        w = csv.writer(fh)
        w.writerow(["id_cuento", "archivo_original", "archivo_a_subir", "url_final"])
        w.writerows(mapa)

    n = len(mapa)
    sql = f"""-- Portadas de los {n} cuentos nuevos de LecturIA (Ids {ids[0]} a {ids[-1]}).
-- Pone la URL de la imagen en stories.image_url y en TODAS las diapositivas del cuento (story_slides.image_url).
-- Base de datos: gs_reading_ai_db_prod (PRODUCCIÓN).
-- Las imágenes DEBEN estar ya subidas al storage ({base}) antes de correr esto.
-- Cómo usarlo en MySQL Workbench:
--   0) Ejecuta el bloque "ANTES" y guarda el resultado (por si hay que volver atrás).
--   1) Selecciona desde START TRANSACTION hasta el último UPDATE y ejecútalo (queda sin confirmar).
--   2) Ejecuta el bloque VERIFICAR. Si dan {n} y el total de slides esperado, ejecuta COMMIT; si no, ROLLBACK;
--   Cada UPDATE de stories incluye el título: si un Id no coincide con su cuento, no cambia nada (0 rows).

USE gs_reading_ai_db_prod;

-- ===== ANTES (solo lectura) =====
SELECT id, title_en, image_url FROM stories WHERE id BETWEEN {ids[0]} AND {ids[-1]} ORDER BY id;
SELECT story_id, COUNT(*) AS slides, MIN(image_url) AS image_url_min FROM story_slides
 WHERE story_id BETWEEN {ids[0]} AND {ids[-1]} GROUP BY story_id ORDER BY story_id;

-- ===== UPDATE (sin confirmar hasta el COMMIT) =====
START TRANSACTION;

-- stories
{chr(10).join(sql_st)}

-- story_slides (todas las diapositivas de cada cuento)
{chr(10).join(sql_sl)}

-- ===== VERIFICAR =====
SELECT COUNT(*) AS stories_actualizadas FROM stories
 WHERE id BETWEEN {ids[0]} AND {ids[-1]} AND image_url LIKE '{base}med_story_%';
SELECT COUNT(*) AS slides_actualizados, COUNT(DISTINCT story_id) AS cuentos_con_slides FROM story_slides
 WHERE story_id BETWEEN {ids[0]} AND {ids[-1]} AND image_url LIKE '{base}med_story_%';
SELECT COUNT(*) AS slides_totales_de_estos_cuentos FROM story_slides
 WHERE story_id BETWEEN {ids[0]} AND {ids[-1]};

-- Si stories_actualizadas = {n} y slides_actualizados = slides_totales_de_estos_cuentos, ejecuta:
-- COMMIT;
-- Si algo no cuadra:
-- ROLLBACK;
"""
    (raiz / "salida" / "actualizar_portadas_med.sql").write_text(sql, encoding="utf-8")
    print(f"{n} imágenes copiadas a {dest} (Ids {ids[0]}..{ids[-1]}); SQL: {n} UPDATE stories + {n} UPDATE story_slides")


if __name__ == "__main__":
    main()
