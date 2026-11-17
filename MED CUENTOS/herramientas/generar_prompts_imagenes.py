#!/usr/bin/env python3
"""Arma el listado de prompts para generar la portada (una imagen por cuento).

Lee salida/pareo_final.json, herramientas/escenas_imagenes.json y salida/subidos.csv (para el Id que
la API le dio a cada cuento). Escribe salida/prompts_imagenes.csv y salida/prompts_imagenes.txt.

Formato del prompt (el que pidió el jefe, más la escena del cuento):
    Pixar style, generate an image for "<título en inglés>": <escena>. No text or letters in the image.

Uso:  python3 -I herramientas/generar_prompts_imagenes.py
"""
import csv
import json
import re
from pathlib import Path

NIVEL = {"Primero": 1, "Segundo": 2, "Tercero": 3, "Cuarto": 4, "Quinto": 5, "Kinder": 7}


def slug(s):
    import unicodedata
    s = "".join(c for c in unicodedata.normalize("NFD", s) if unicodedata.category(c) != "Mn")
    return re.sub(r"[^a-z0-9]+", "-", s.lower()).strip("-")


def main():
    raiz = Path(__file__).resolve().parent.parent
    pares = json.loads((raiz / "salida" / "pareo_final.json").read_text(encoding="utf-8"))
    escenas = json.loads((Path(__file__).with_name("escenas_imagenes.json")).read_text(encoding="utf-8"))

    ids = {}
    with open(raiz / "salida" / "subidos.csv", encoding="utf-8-sig") as fh:
        for r in csv.DictReader(fh):
            m = re.search(r'"Id":\s*(\d+)', r["respuesta"])
            if r["estado"].startswith("2") and m:
                ids[r["archivo"]] = int(m.group(1))      # el último envío exitoso manda

    filas = []
    for p in pares:
        escena = escenas.get(p["id"])
        if not escena:
            raise SystemExit(f"{p['id']}: falta la escena en escenas_imagenes.json")
        base = p["id"] if p["grado"] == "Kinder" else f"{p['grado'].lower()}-{p['orden']:02d}"
        nombre = f"{base}-{slug(p['titulo_en'])}"
        if escena.startswith("="):      # prompt completo, sin el título (para los que el filtro bloquea)
            prompt = escena[1:].strip()
        else:
            prompt = f'Pixar style, generate an image for "{p["titulo_en"]}": {escena}. No text or letters in the image.'
        filas.append([nombre + ".png", ids.get(nombre + ".json", ""), p["grado"], NIVEL[p["grado"]],
                      p["titulo_en"], p["titulo_es"], prompt])

    with open(raiz / "salida" / "prompts_imagenes.csv", "w", newline="", encoding="utf-8-sig") as fh:
        w = csv.writer(fh)
        w.writerow(["imagen", "id_cuento", "grado", "LevelId", "TitleEn", "TitleEs", "prompt"])
        w.writerows(filas)
    with open(raiz / "salida" / "prompts_imagenes.txt", "w", encoding="utf-8") as fh:
        for f in filas:
            fh.write(f"{f[0]}\t{f[6]}\n")
    sin_id = [f[0] for f in filas if f[1] == ""]
    print(f"{len(filas)} prompts; sin Id de cuento: {len(sin_id)} {sin_id[:3]}")


if __name__ == "__main__":
    main()
