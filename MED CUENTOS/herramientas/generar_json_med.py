#!/usr/bin/env python3
"""Genera un JSON por cuento con el formato de carga de LecturIA/MED.

Lee salida/pareo_final.json (lo produce unir_pareos.py) y herramientas/categorias.json.
Escribe salida/json_med/<grado>-<nn>-<titulo-en>.json y salida/json_med/indice.csv.

Reglas acordadas:
  * Slides: si el cuento tiene hasta 110 palabras (en el idioma más largo) va en un solo slide; si no,
    se agrupan los párrafos en slides de ~90 palabras y nunca más de 110, sin cortar oraciones. Un párrafo
    que por sí solo pasa de 110 se corta por oraciones (EN y ES a la par). Dentro de un slide los
    párrafos van separados por salto de línea.
  * LevelId = número del grado (Primero=1 ... Quinto=5), provisional.
  * Kinder queda fuera salvo con --kinder (se evaluará aparte, son lecturas de sílabas):
    con la bandera genera solo Kinder en salida/json_med_kinder con LevelId=7 (NIVEL_KINDER; el
    endpoint no acepta 0). El frontend los mostrará en un módulo aparte llamando a ese nivel.
  * Metas (fluidez, exactitud, completitud, pronunciación) = 70.

Uso:  python3 herramientas/generar_json_med.py "<carpeta raíz>" [--kinder]
"""
import csv
import json
import re
import sys
import unicodedata
from pathlib import Path

NIVEL = {"Primero": 1, "Segundo": 2, "Tercero": 3, "Cuarto": 4, "Quinto": 5}
NIVEL_KINDER = 7       # el endpoint rechaza LevelId 0 ("LevelId is required."); 7 es el nivel creado para Kinder
CATEGORIA_ID = {"animals": 1, "scifi": 2, "history": 3, "adventure": 4}
BASE_IMG = "https://stpubliceastus001.blob.core.windows.net/public-assets/categories/{}.png"
META = 70
MAX_PALABRAS = 110     # tope por slide (en el idioma más largo)
OBJETIVO = 90          # tamaño aproximado al dividir
ORACION = re.compile(r"(?:(?<=[.!?…])|(?<=[.!?…][\"”»]))\s+(?=[A-ZÁÉÍÓÚÑ¿¡“\"«—−-])")


def pal(s):
    return len(s.split())


def unidades(parrafos, avisos, cid):
    """Convierte los párrafos pareados en unidades (en, es, separador) que caben en un slide.
    Un párrafo que supera MAX_PALABRAS se corta por oraciones, EN y ES a la par."""
    out = []
    for d in parrafos:
        en, es = d["en"], d["es"]
        if max(pal(en), pal(es)) <= MAX_PALABRAS:
            out.append((en, es, "\n"))
            continue
        se, ss = ORACION.split(en), ORACION.split(es)
        if len(se) == len(ss):
            pares = list(zip(se, ss))
        else:                       # oraciones desiguales: se reparten por posición relativa
            k = -(-max(pal(en), pal(es)) // OBJETIVO)
            pares = []
            for i in range(k):
                def trozo(oraciones):
                    tot, acum, sel = sum(map(pal, oraciones)), 0, []
                    for o in oraciones:
                        mitad = acum + pal(o) / 2
                        if i / k <= mitad / tot < (i + 1) / k:
                            sel.append(o)
                        acum += pal(o)
                    return " ".join(sel)
                pares.append((trozo(se), trozo(ss)))
            pares = [p for p in pares if p[0] and p[1]]
            avisos.append(f"{cid}: párrafo {d['orden']} con oraciones desiguales ({len(se)} EN / {len(ss)} ES); cortes aproximados, revisar")
        for i, (a, b) in enumerate(pares):
            out.append((a, b, "\n" if i == 0 else " "))
    return out


def agrupar(unids):
    """Reparte las unidades en slides de ~OBJETIVO palabras, sin pasar de MAX_PALABRAS."""
    tot = max(sum(pal(u[0]) for u in unids), sum(pal(u[1]) for u in unids))
    if tot <= MAX_PALABRAS:
        return [unids]
    k = -(-tot // OBJETIVO)
    meta = tot / k
    slides, cur, ce, cs = [], [], 0, 0
    for u in unids:
        we, ws = pal(u[0]), pal(u[1])
        excede = cur and max(ce + we, cs + ws) > MAX_PALABRAS
        llena = cur and max(ce, cs) >= meta and len(slides) < k - 1
        if excede or llena:
            slides.append(cur)
            cur, ce, cs = [], 0, 0
        cur.append(u)
        ce, cs = ce + we, cs + ws
    if cur:
        slides.append(cur)
    return slides


def texto(unids, i):
    return "".join((u[2] if j else "") + u[i] for j, u in enumerate(unids))


def slug(s):
    s = "".join(c for c in unicodedata.normalize("NFD", s) if unicodedata.category(c) != "Mn")
    return re.sub(r"[^a-z0-9]+", "-", s.lower()).strip("-")


def main():
    args = [a for a in sys.argv[1:] if a != "--kinder"]
    kinder = "--kinder" in sys.argv
    raiz = Path(args[0] if args else ".").expanduser().resolve()
    salida = raiz / "salida"
    destino = salida / ("json_med_kinder" if kinder else "json_med")
    destino.mkdir(parents=True, exist_ok=True)
    for viejo in destino.glob("*.json"):
        viejo.unlink()
    cats = json.loads((Path(__file__).with_name("categorias.json")).read_text(encoding="utf-8"))
    pares = json.loads((salida / "pareo_final.json").read_text(encoding="utf-8"))

    indice, usados, avisos = [], set(), []
    for p in pares:
        if (p["grado"] == "Kinder") != kinder or (p["grado"] not in NIVEL and not kinder):
            continue
        cat = cats.get(p["id"], "adventure")
        if cat not in CATEGORIA_ID:
            sys.exit(f"{p['id']}: categoría desconocida {cat!r}")
        doc = {
            "LevelId": NIVEL.get(p["grado"], NIVEL_KINDER),   # Kinder: nivel propio (módulo aparte en el frontend)
            "CategoryId": CATEGORIA_ID[cat],
            "TitleEn": p["titulo_en"],
            "TitleEs": p["titulo_es"],
            "ImageUrl": BASE_IMG.format(cat),
            "FluencyGoal": META,
            "AccuracyGoal": META,
            "CompletenessGoal": META,
            "PronunciationGoal": META,
            "Slides": [{
                "SlideOrder": n,
                "TextEn": texto(g, 0),
                "TextEs": texto(g, 1),
                "ImageUrl": None,
            } for n, g in enumerate(agrupar(unidades(p["parrafos"], avisos, p["id"])), 1)],
        }
        base = p["id"] if kinder else f"{p['grado'].lower()}-{p['orden']:02d}"
        nombre = f"{base}-{slug(p['titulo_en'])}.json"
        assert nombre not in usados, nombre
        usados.add(nombre)
        (destino / nombre).write_text(json.dumps(doc, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
        sl = doc["Slides"]
        mayor = max(max(pal(s["TextEn"]), pal(s["TextEs"])) for s in sl)
        if mayor > MAX_PALABRAS:
            avisos.append(f"{p['id']}: un slide tiene {mayor} palabras (> {MAX_PALABRAS})")
        indice.append([nombre, p["grado"], doc["LevelId"], cat, doc["CategoryId"], p["titulo_es"], p["titulo_en"],
                       sum(pal(s["TextEs"]) for s in sl), len(sl), mayor, p["origen_en"]])

    with open(destino / "indice.csv", "w", newline="", encoding="utf-8-sig") as fh:
        w = csv.writer(fh)
        w.writerow(["archivo", "grado", "LevelId", "categoria", "CategoryId", "TitleEs", "TitleEn", "palabras_es",
                    "slides", "max_palabras_slide", "origen_TextEn"])
        w.writerows(indice)
    print(f"{len(indice)} JSON en {destino}  ({sum(r[8] for r in indice)} slides)")
    for a in avisos:
        print("  !", a)
    for g in (["Kinder"] if kinder else NIVEL):
        print(f"  {g:8} nivel {NIVEL.get(g, NIVEL_KINDER)}: {sum(1 for r in indice if r[1] == g)}")
    por_cat = {}
    for r in indice:
        por_cat[r[3]] = por_cat.get(r[3], 0) + 1
    print("  categorías:", por_cat)


if __name__ == "__main__":
    main()
