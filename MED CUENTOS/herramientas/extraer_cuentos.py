#!/usr/bin/env python3
"""Extrae el texto limpio de los cuentos de LecturIA (Word y PDF) y prepara el pareo EN/ES.

Estructura esperada de la carpeta raíz:
    Español/<Grado>/*.docx|*.pdf
    Inglés/<Grado>.docx            (o Inglés/<Grado>/*.docx)

Qué elimina de los Word:
  * el bloque de fluidez ("Total de palabras leídas...", "Número de errores", "PLCM")
  * los contadores de palabras al final de cada línea (10, 20, 30...)

Salida (carpeta --salida, por defecto <raíz>/salida):
    cuentos_es.json / cuentos_en.json   un registro por cuento
    cuentos_es.csv  / cuentos_en.csv    lo mismo, para revisar en Excel
    pareo_pendiente.csv                 estado del pareo EN/ES ANTES de traducir (usar unir_pareos.py para el final)
    reporte.txt                         advertencias a revisar a mano

Uso:
    python3 herramientas/extraer_cuentos.py "<carpeta raíz>"
Los PDF se leen con herramientas/pdf_paginas.swift (PDFKit + OCR de Vision; solo macOS).
"""
import argparse
import csv
import json
import re
import subprocess
import sys
import unicodedata
import zipfile
import xml.etree.ElementTree as ET
from pathlib import Path

W = "{http://schemas.openxmlformats.org/wordprocessingml/2006/main}"
RUTA_SWIFT = Path(__file__).with_name("pdf_paginas.swift")

GRADOS = ["Kinder", "Primero", "Segundo", "Tercero", "Cuarto", "Quinto"]
RUIDO = re.compile(r"^(total de palabras|n[uú]mero de errores|=\s*palabras le)", re.I)
FIN_ORACION = tuple('.!?…»”"\')')
reporte = []


def avisar(msg):
    reporte.append(msg)
    print("  ! " + msg)


def sin_acentos(s):
    return "".join(c for c in unicodedata.normalize("NFD", s) if unicodedata.category(c) != "Mn")


def grado_de(texto):
    t = sin_acentos(texto).lower()
    for g in GRADOS:
        if sin_acentos(g).lower() in t:
            return g
    if "kindergarten" in t:
        return "Kinder"
    return None


def limpiar(s):
    s = s.replace(" ", " ").replace("\t", " ")
    s = re.sub(r"[ ]{2,}", " ", s)
    return s.strip()


# --------------------------------------------------------------------------- DOCX
def parrafos_docx(ruta):
    """Devuelve [(estilo, [líneas])] por cada párrafo de Word, respetando saltos de línea (<w:br/>)."""
    raiz = ET.fromstring(zipfile.ZipFile(ruta).read("word/document.xml"))
    salida = []
    for p in raiz.find(W + "body").iter(W + "p"):
        st = p.find(f"{W}pPr/{W}pStyle")
        estilo = st.get(W + "val") if st is not None else ""
        trozos = []
        for el in p.iter():
            if el.tag == W + "t":
                trozos.append(el.text or "")
            elif el.tag == W + "tab":
                trozos.append("\t")
            elif el.tag in (W + "br", W + "cr"):
                trozos.append("\n")
        salida.append((estilo, [limpiar(x) for x in "".join(trozos).split("\n")]))
    return salida


def quitar_contadores(lineas):
    """Quita los números de conteo de palabras al final de las líneas (10, 20, 30... 147)."""
    finales = []
    for i, l in enumerate(lineas):
        m = re.search(r"\s(\d{1,4})$", l)
        if m:
            finales.append((i, int(m.group(1))))
    nums = [n for _, n in finales]
    if len(finales) >= 3 and all(a < b for a, b in zip(nums, nums[1:])):
        lineas = list(lineas)
        for i, _ in finales:
            lineas[i] = re.sub(r"\s\d{1,4}$", "", lineas[i]).strip()
    return lineas


def armar_parrafos(lineas):
    """Une las líneas cortadas a mano por el ancho de página; respeta el fin de oración."""
    parrafos, actual = [], ""
    for l in lineas:
        if not l:
            continue
        actual = f"{actual} {l}".strip() if actual else l
        if actual.endswith(FIN_ORACION) or actual.endswith(":"):
            parrafos.append(actual)
            actual = ""
    if actual:
        parrafos.append(actual)
    return parrafos


def posible_verso(lineas):
    """Heurística: líneas cortas = rima/poema; se conservan los saltos de línea. Es solo una pista."""
    n = len(lineas)
    if n < 3:
        return False
    media, maximo = sum(map(len, lineas)) / n, max(map(len, lineas))
    puntuadas = sum(1 for l in lineas if l[-1] in ",.;!?") / n
    return media <= 26 or (n <= 6 and media <= 38) or (n >= 4 and puntuadas >= 0.75 and media <= 44 and maximo <= 50)


def cuentos_de_docx(ruta, idioma, grado):
    ps = parrafos_docx(ruta)
    # una "línea" = un párrafo de Word (o un salto de línea dentro de uno); None marca un párrafo vacío
    lineas = []
    for estilo, ls in ps:
        if not any(ls):
            lineas.append((estilo, None))
        else:
            lineas.extend((estilo, l) for l in ls if l)
    hay_ruido = any(l and RUIDO.match(l) for _, l in lineas)
    n_encab = sum(1 for e, l in lineas if l and e.lower().startswith("heading"))

    segmentos = []  # cada uno: (titulo, [líneas de cuerpo])
    if hay_ruido:                               # fichas de fluidez: el cuento termina en "Total de palabras..."
        actual, en_ruido = [], False
        for _, l in lineas:
            if l and RUIDO.match(l):
                if actual and not en_ruido:
                    segmentos.append(actual)
                    actual = []
                en_ruido = True
                continue
            en_ruido = False
            if l:
                actual.append(l)
        if actual:
            avisar(f"{ruta.name}: texto sin cerrar después del último cuento: {actual[0][:50]!r}")
        segmentos = [(s[0], s[1:]) for s in segmentos]
    elif n_encab >= 2:                          # "Cuentos IA": título con estilo Heading
        titulo, cuerpo = None, []
        for e, l in lineas:
            if not l:
                continue
            if e.lower().startswith("heading"):
                if titulo and cuerpo:
                    segmentos.append((titulo, cuerpo))
                titulo, cuerpo = l, []
            else:
                cuerpo.append(l)
        if titulo and cuerpo:
            segmentos.append((titulo, cuerpo))
    else:                                       # bloques separados por líneas en blanco: título + cuerpo
        bloque = []
        for _, l in lineas + [("", None)]:
            if l:
                bloque.append(l)
            elif bloque:
                if len(bloque) >= 2:
                    segmentos.append((bloque[0], bloque[1:]))
                else:
                    avisar(f"{ruta.name}: bloque sin cuerpo ignorado: {bloque[0][:50]!r}")
                bloque = []

    cuentos = []
    for orden, (titulo, cuerpo) in enumerate(segmentos, 1):
        cuerpo = quitar_contadores(cuerpo)
        titulo = titulo.strip().strip('"“”«»').strip()
        verso = posible_verso(cuerpo)
        parrafos = list(cuerpo) if verso else armar_parrafos(cuerpo)
        if not parrafos:
            avisar(f"{ruta.name}: '{titulo}' quedó sin texto")
            continue
        cuentos.append({
            "grado": grado, "idioma": idioma, "orden": orden, "titulo": titulo,
            "autor": None, "origen": f"{ruta.parent.name}/{ruta.name}",
            "tipo": "verso?" if verso else "prosa",
            "parrafos": parrafos, "texto": "\n".join(parrafos), "diapositivas": None,
            "lineas_originales": cuerpo,
        })
    return cuentos


# --------------------------------------------------------------------------- PDF
def paginas_pdf(ruta, cache):
    cache.mkdir(parents=True, exist_ok=True)
    f = cache / (ruta.stem + ".json")
    if not f.exists() or f.stat().st_mtime < ruta.stat().st_mtime:
        print(f"  leyendo PDF (puede tardar, usa OCR si hace falta): {ruta.name}")
        r = subprocess.run(["swift", str(RUTA_SWIFT), str(ruta)], capture_output=True, text=True)
        if r.returncode != 0:
            avisar(f"{ruta.name}: no se pudo leer el PDF: {r.stderr.strip()[:200]}")
            return []
        f.write_text(r.stdout, encoding="utf-8")
    return json.loads(f.read_text(encoding="utf-8"))


def unir_lineas(txt):
    t = " ".join(l.strip() for l in txt.split("\n") if l.strip())
    return re.sub(r"\s+([.,;:!?])", r"\1", t)


def cuento_de_pdf(ruta, idioma, grado, cache):
    paginas = paginas_pdf(ruta, cache)
    if not paginas:
        return []
    primera = [l.strip() for l in paginas[0]["text"].split("\n") if l.strip()]
    autor, titulo_l, usadas = None, [], len(primera)
    for i, l in enumerate(primera):          # el título son las líneas anteriores a "por <autor>"
        m = re.match(r"^por\s+(.+)$", l, re.I)
        if m:
            autor = m.group(1).strip()
            if autor.isupper():
                autor = autor.title()
            usadas = i + 1
            break
        titulo_l.append(l)
    if autor is None:                        # sin autor: el título es la primera línea
        titulo_l, usadas = primera[:1], 1
    titulo = " ".join(titulo_l).strip()
    if titulo.isupper():
        titulo = titulo.capitalize()
    if len(paginas[0]["text"]) < 200 and len(paginas) > 1:   # portada sola
        resto = paginas[1:]
    else:                                                    # la página 1 trae título y cuerpo
        resto = [{"page": 1, "method": paginas[0]["method"], "text": "\n".join(primera[usadas:])}] + paginas[1:]
    resto = [p for p in resto if p["text"].strip()]
    medio = sum(len(p["text"]) for p in resto) / max(len(resto), 1)
    libro_ilustrado = len(resto) >= 10 and medio < 260   # una página = una diapositiva
    if libro_ilustrado:
        diapos = [{"pagina": p["page"], "texto": unir_lineas(p["text"]), "metodo": p["method"]} for p in resto]
        parrafos = [d["texto"] for d in diapos]
        if any(p["method"] == "ocr" for p in resto):
            avisar(f"{ruta.name}: páginas leídas con OCR {[p['page'] for p in resto if p['method']=='ocr']}; revisar ortografía")
    else:
        diapos = None
        parrafos = armar_parrafos([l.strip() for p in resto for l in p["text"].split("\n")])
    for i, p in enumerate(parrafos):   # las "notas aclaratorias" al final no forman parte del cuento
        if re.match(r"^nota aclaratoria", p, re.I):
            avisar(f"{ruta.name}: se descartó la nota final ({len(parrafos) - i} párrafos): {p[:40]!r}...")
            parrafos = parrafos[:i]
            break
    return [{
        "grado": grado, "idioma": idioma, "orden": 1, "titulo": titulo, "autor": autor,
        "origen": f"{ruta.parent.name}/{ruta.name}",
        "tipo": "libro ilustrado (1 página = 1 diapositiva)" if libro_ilustrado else "prosa",
        "parrafos": parrafos, "texto": "\n".join(parrafos), "diapositivas": diapos,
        "lineas_originales": None,
    }]


# --------------------------------------------------------------------------- pareo
def clave(s):
    return re.sub(r"[^a-z0-9]+", " ", sin_acentos(s).lower()).strip()


def parear(es, en):
    filas = []
    for g in GRADOS:
        e = [c for c in es if c["grado"] == g]
        i = [c for c in en if c["grado"] == g]
        if not e and not i:
            continue
        if e and i and len(e) == len(i):
            for a, b in zip(e, i):
                filas.append((g, a["orden"], a["titulo"], b["titulo"], "pareado por orden (mismo número de cuentos): CONFIRMAR"))
            continue
        usados = set()
        for a in e:
            b = next((x for x in i if clave(x["titulo"]) == clave(a["titulo"]) and id(x) not in usados), None)
            if b:
                usados.add(id(b))
                filas.append((g, a["orden"], a["titulo"], b["titulo"], "pareado por título"))
            else:
                filas.append((g, a["orden"], a["titulo"], "", "SIN INGLÉS" if i or not en else "SIN INGLÉS (no hay Word en inglés para este grado)"))
        for b in i:
            if id(b) not in usados:
                filas.append((g, "", "", b["titulo"], "SIN ESPAÑOL"))
        if e and i:
            avisar(f"{g}: {len(e)} cuentos en español y {len(i)} en inglés; no son equivalentes uno a uno")
    return filas


# --------------------------------------------------------------------------- main
def main():
    ap = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("raiz", type=Path)
    ap.add_argument("--salida", type=Path)
    a = ap.parse_args()
    raiz = a.raiz.expanduser().resolve()
    salida = (a.salida or raiz / "salida").resolve()
    salida.mkdir(parents=True, exist_ok=True)

    por_idioma = {"es": [], "en": []}
    for idioma, carpeta in (("es", "Español"), ("en", "Inglés")):
        base = raiz / carpeta
        if not base.is_dir():
            avisar(f"No existe la carpeta {base}")
            continue
        for f in sorted(base.rglob("*")):
            if f.name.startswith(("~", ".")) or f.suffix.lower() not in (".docx", ".pdf"):
                continue
            grado = grado_de(f.parent.name) or grado_de(f.stem)
            if not grado:
                avisar(f"{f.name}: no se pudo deducir el grado, se omite")
                continue
            print(f"[{idioma}] {grado:8} {f.name}")
            if f.suffix.lower() == ".docx":
                cs = cuentos_de_docx(f, idioma, grado)
            else:
                cs = cuento_de_pdf(f, idioma, grado, salida / "cache_pdf")
            por_idioma[idioma].extend(cs)
            print(f"      {len(cs)} cuento(s)")

    for idioma, cs in por_idioma.items():
        # orden global dentro del grado (si hay varios archivos por grado)
        for g in GRADOS:
            n = 0
            for c in cs:
                if c["grado"] == g:
                    n += 1
                    c["orden"] = n
                    c["id"] = f"{sin_acentos(g).lower()}-{idioma}-{n:02d}"
                    c["palabras"] = len(c["texto"].split())
        (salida / f"cuentos_{idioma}.json").write_text(json.dumps(cs, ensure_ascii=False, indent=2), encoding="utf-8")
        with open(salida / f"cuentos_{idioma}.csv", "w", newline="", encoding="utf-8-sig") as fh:
            w = csv.writer(fh)
            w.writerow(["id", "grado", "orden", "titulo", "autor", "tipo", "palabras", "origen", "texto"])
            for c in cs:
                w.writerow([c["id"], c["grado"], c["orden"], c["titulo"], c["autor"] or "", c["tipo"], c["palabras"], c["origen"], c["texto"]])

    filas = parear(por_idioma["es"], por_idioma["en"])
    with open(salida / "pareo_pendiente.csv", "w", newline="", encoding="utf-8-sig") as fh:
        w = csv.writer(fh)
        w.writerow(["grado", "orden_es", "titulo_es", "titulo_en", "estado"])
        w.writerows(filas)

    (salida / "reporte.txt").write_text("\n".join(reporte) or "Sin advertencias", encoding="utf-8")
    print("\nResumen:")
    for g in GRADOS:
        n_es = sum(c["grado"] == g for c in por_idioma["es"])
        n_en = sum(c["grado"] == g for c in por_idioma["en"])
        print(f"  {g:8} ES={n_es:3}  EN={n_en:3}")
    print(f"Archivos en {salida}")


if __name__ == "__main__":
    sys.exit(main())
