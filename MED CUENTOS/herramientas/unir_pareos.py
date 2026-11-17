#!/usr/bin/env python3
"""Une los cuentos extraídos con sus traducciones y genera el pareo EN/ES por diapositiva.

Entradas (en <salida>):
    cuentos_es.json, cuentos_en.json      (los genera extraer_cuentos.py)
    traducciones/*.json                   {id_del_cuento: {"titulo": ..., "parrafos": [...], "notas": ...}}
                                          la traducción va en el idioma contrario al del cuento de origen

Salidas (en <salida>):
    pareo_final.json     un registro por cuento con los párrafos alineados es/en
    cuentos_pareados.csv una fila por cuento
    diapositivas.csv     una fila por párrafo, con las columnas de story_slides
                         (slide_order, text_en, text_es, image_url) más grado/título para ubicarlo
    reporte_pareo.txt    problemas de validación

Uso:  python3 herramientas/unir_pareos.py "<carpeta raíz>"
"""
import csv
import json
import sys
from pathlib import Path

ORIGINAL = "original"
TRADUCIDO = "traducción automática"


def main():
    raiz = Path(sys.argv[1] if len(sys.argv) > 1 else ".").expanduser().resolve()
    salida = raiz / "salida"
    es = json.loads((salida / "cuentos_es.json").read_text(encoding="utf-8"))
    en = json.loads((salida / "cuentos_en.json").read_text(encoding="utf-8"))
    trad = {}
    for f in sorted((salida / "traducciones").glob("*.json")):
        d = json.loads(f.read_text(encoding="utf-8"))
        repetidos = set(d) & set(trad)
        if repetidos:
            sys.exit(f"{f.name}: ids repetidos en otro archivo: {sorted(repetidos)}")
        trad.update(d)

    problemas, pares = [], []
    for c in es + en:
        t = trad.get(c["id"])
        if t is None:
            problemas.append(f"{c['id']} ({c['titulo']}): falta la traducción")
            continue
        if len(t["parrafos"]) != len(c["parrafos"]):
            problemas.append(f"{c['id']}: {len(c['parrafos'])} párrafos en el original y {len(t['parrafos'])} en la traducción")
            continue
        if not t["titulo"].strip() or any(not p.strip() for p in t["parrafos"]):
            problemas.append(f"{c['id']}: hay texto vacío en la traducción")
            continue
        es_origen = c["idioma"] == "es"
        par = {
            "id": c["id"],
            "grado": c["grado"],
            "orden": c["orden"],
            "idioma_original": c["idioma"],
            "origen_es": ORIGINAL if es_origen else TRADUCIDO,
            "origen_en": TRADUCIDO if es_origen else ORIGINAL,
            "titulo_es": c["titulo"] if es_origen else t["titulo"],
            "titulo_en": t["titulo"] if es_origen else c["titulo"],
            "autor": c["autor"],
            "tipo": c["tipo"],
            "archivo_origen": c["origen"],
            "notas": t.get("notas", ""),
            "parrafos": [
                {"orden": i, "es": (o if es_origen else x), "en": (x if es_origen else o)}
                for i, (o, x) in enumerate(zip(c["parrafos"], t["parrafos"]), 1)
            ],
        }
        pares.append(par)

    sobrantes = set(trad) - {c["id"] for c in es + en}
    for s in sorted(sobrantes):
        problemas.append(f"{s}: hay traducción pero el cuento no existe en la extracción")

    (salida / "pareo_final.json").write_text(json.dumps(pares, ensure_ascii=False, indent=2), encoding="utf-8")
    with open(salida / "cuentos_pareados.csv", "w", newline="", encoding="utf-8-sig") as fh:
        w = csv.writer(fh)
        w.writerow(["id", "grado", "orden", "titulo_es", "titulo_en", "origen_es", "origen_en", "diapositivas", "notas"])
        for p in pares:
            w.writerow([p["id"], p["grado"], p["orden"], p["titulo_es"], p["titulo_en"], p["origen_es"], p["origen_en"], len(p["parrafos"]), p["notas"]])
    with open(salida / "diapositivas.csv", "w", newline="", encoding="utf-8-sig") as fh:
        w = csv.writer(fh)
        w.writerow(["story_key", "grado", "slide_order", "text_en", "text_es", "image_url"])
        for p in pares:
            for d in p["parrafos"]:
                w.writerow([p["id"], p["grado"], d["orden"], d["en"], d["es"], ""])
    (salida / "reporte_pareo.txt").write_text("\n".join(problemas) or "Sin problemas", encoding="utf-8")

    print(f"{len(pares)} cuentos pareados, {sum(len(p['parrafos']) for p in pares)} diapositivas")
    for g in ["Kinder", "Primero", "Segundo", "Tercero", "Cuarto", "Quinto"]:
        ps = [p for p in pares if p["grado"] == g]
        print(f"  {g:8} {len(ps):3} cuentos  ({sum(len(p['parrafos']) for p in ps)} diapositivas)")
    print("Problemas:" if problemas else "Sin problemas de validación")
    for p in problemas:
        print("  !", p)


if __name__ == "__main__":
    main()
