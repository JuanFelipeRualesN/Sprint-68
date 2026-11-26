"""Lector mínimo de .xlsx con la biblioteca estándar (zipfile + xml.etree): sin openpyxl, para que el verificador no dependa de nada.

leer(bytes) → {nombre_de_hoja: [[valor, ...], ...]}
  · número  → int (si no tiene decimales) o float
  · texto   → str (sharedStrings, inlineStr y t="str")
  · booleano→ bool
  · celda vacía/ausente → None
Una fórmula se lee como su valor guardado (v); el verificador además usa `hay_formulas` para exigir que no haya ninguna.
"""
import io
import re
import xml.etree.ElementTree as ET
import zipfile

NS = {"m": "http://schemas.openxmlformats.org/spreadsheetml/2006/main",
      "r": "http://schemas.openxmlformats.org/officeDocument/2006/relationships",
      "p": "http://schemas.openxmlformats.org/package/2006/relationships"}


def _col(ref):
    letras = re.match(r"[A-Z]+", ref).group(0)
    n = 0
    for ch in letras:
        n = n * 26 + (ord(ch) - 64)
    return n - 1


def _texto(si):
    return "".join(t.text or "" for t in si.iter("{%s}t" % NS["m"]))


def _numero(s):
    f = float(s)
    return int(f) if f.is_integer() and "." not in s and "E" not in s.upper() else f


def leer(datos, con_formulas=False):
    z = zipfile.ZipFile(io.BytesIO(datos))
    compartidas = []
    if "xl/sharedStrings.xml" in z.namelist():
        compartidas = [_texto(si) for si in ET.fromstring(z.read("xl/sharedStrings.xml")).findall("m:si", NS)]
    libro = ET.fromstring(z.read("xl/workbook.xml"))
    rels = {r.get("Id"): r.get("Target") for r in ET.fromstring(z.read("xl/_rels/workbook.xml.rels")).findall("p:Relationship", NS)}
    hojas, formulas = {}, 0
    for h in libro.find("m:sheets", NS).findall("m:sheet", NS):
        destino = rels[h.get("{%s}id" % NS["r"])].lstrip("/")
        ruta = destino if destino.startswith("xl/") else "xl/" + destino
        filas = []
        for fila in ET.fromstring(z.read(ruta)).iter("{%s}row" % NS["m"]):
            idx = int(fila.get("r")) - 1
            while len(filas) <= idx:
                filas.append([])
            for c in fila.findall("m:c", NS):
                j, t = _col(c.get("r")), c.get("t")
                v, f = c.find("m:v", NS), c.find("m:f", NS)
                if f is not None:
                    formulas += 1
                if t == "inlineStr":
                    valor = _texto(c.find("m:is", NS))
                elif v is None:
                    valor = None
                elif t == "s":
                    valor = compartidas[int(v.text)]
                elif t in ("str", "e"):
                    valor = v.text or ""
                elif t == "b":
                    valor = v.text == "1"
                else:
                    valor = _numero(v.text)
                while len(filas[idx]) <= j:
                    filas[idx].append(None)
                filas[idx][j] = valor
        hojas[h.get("name")] = filas
    return (hojas, formulas) if con_formulas else hojas
