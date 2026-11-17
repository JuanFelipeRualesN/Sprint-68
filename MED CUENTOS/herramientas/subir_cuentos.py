#!/usr/bin/env python3
"""Inserta los JSON de cuentos en LecturIA/MED, uno por uno.

Endpoint: POST https://med-rd-baseapi.genialskillsweb.com/readingapi/stories/create
Header:   Token: <token>      (el token que genera el login en med-rd-maestros.genialskillsweb.com)

El token NO va en el código. Se lee de la variable de entorno MED_TOKEN o, si no existe, se pide por teclado.
Para sacarlo: iniciar sesión en la página, abrir las herramientas del navegador > Application > Local Storage
y copiar el valor del token.

Por seguridad, sin --enviar solo simula (no manda nada). Cada envío exitoso queda en
salida/subidos.csv y no se repite si el script se corre otra vez.

Ejemplos:
    python3 herramientas/subir_cuentos.py                          # simula con los 73 de Primero a Quinto
    python3 herramientas/subir_cuentos.py --enviar --limite 1      # envía solo el primero (prueba)
    python3 herramientas/subir_cuentos.py --enviar                 # envía todos los pendientes
    python3 herramientas/subir_cuentos.py --carpeta salida/json_med_kinder --enviar
"""
import argparse
import csv
import getpass
import json
import os
import sys
import urllib.error
import urllib.request
from datetime import datetime
from pathlib import Path

URL = "https://med-rd-baseapi.genialskillsweb.com/readingapi/stories/create"
CAMPOS = ["LevelId", "CategoryId", "TitleEn", "TitleEs", "ImageUrl", "FluencyGoal", "AccuracyGoal",
          "CompletenessGoal", "PronunciationGoal", "Slides"]


def validar(doc):
    faltan = [c for c in CAMPOS if c not in doc]
    if faltan:
        return f"faltan campos: {faltan}"
    if not doc["Slides"] or not all(s.get("TextEn") and s.get("TextEs") for s in doc["Slides"]):
        return "algún slide no tiene TextEn/TextEs"
    return None


def enviar(doc, token):
    req = urllib.request.Request(
        URL, data=json.dumps(doc, ensure_ascii=False).encode("utf-8"), method="POST",
        headers={"Content-Type": "application/json", "Token": token, "Accept": "application/json"})
    try:
        with urllib.request.urlopen(req, timeout=60) as r:
            return r.status, r.read().decode("utf-8", "replace")
    except urllib.error.HTTPError as e:
        return e.code, e.read().decode("utf-8", "replace")


def main():
    raiz = Path(__file__).resolve().parent.parent
    ap = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("--carpeta", type=Path, default=raiz / "salida" / "json_med")
    ap.add_argument("--enviar", action="store_true", help="enviar de verdad (sin esto solo simula)")
    ap.add_argument("--limite", type=int, help="enviar como máximo N cuentos")
    ap.add_argument("--registro", type=Path, default=raiz / "salida" / "subidos.csv")
    ap.add_argument("--reenviar", nargs="+", metavar="ARCHIVO",
                    help="volver a enviar estos archivos aunque ya consten como subidos (bórralos antes en la página)")
    a = ap.parse_args()

    archivos = sorted(a.carpeta.glob("*.json"))
    if not archivos:
        sys.exit(f"No hay JSON en {a.carpeta}")
    hechos = set()
    if a.registro.exists():
        with open(a.registro, encoding="utf-8-sig") as fh:
            hechos = {r["archivo"] for r in csv.DictReader(fh) if r["estado"].startswith("2")}
    hechos -= set(a.reenviar or [])
    pendientes = [f for f in archivos if f.name not in hechos]
    if a.limite:
        pendientes = pendientes[: a.limite]
    print(f"{len(archivos)} JSON en {a.carpeta.name}; ya subidos: {len(archivos) - len([f for f in archivos if f.name not in hechos])}; "
          f"a procesar ahora: {len(pendientes)}  [{'ENVÍO REAL' if a.enviar else 'SIMULACIÓN'}]")

    token = None
    if a.enviar:
        token = os.environ.get("MED_TOKEN") or getpass.getpass("Token (no se muestra): ").strip()
        if not token:
            sys.exit("Falta el token.")
        nuevo = not a.registro.exists()
        log = open(a.registro, "a", newline="", encoding="utf-8-sig")
        w = csv.writer(log)
        if nuevo:
            w.writerow(["archivo", "estado", "respuesta", "fecha"])

    for f in pendientes:
        doc = json.loads(f.read_text(encoding="utf-8"))
        error = validar(doc)
        if error:
            sys.exit(f"{f.name}: {error}")
        if not a.enviar:
            print(f"  [simulación] {f.name}  nivel {doc['LevelId']} cat {doc['CategoryId']}  {doc['TitleEn']!r}")
            continue
        estado, cuerpo = enviar(doc, token)
        w.writerow([f.name, estado, cuerpo[:300].replace("\n", " "), datetime.now().isoformat(timespec="seconds")])
        log.flush()
        print(f"  {estado}  {f.name}  {cuerpo[:120]!r}")
        if estado in (401, 403):
            sys.exit("El token no sirve o venció. Inicia sesión de nuevo, copia el token nuevo y vuelve a correr.")
        if not 200 <= estado < 300:
            sys.exit(f"Se detuvo en {f.name} (estado {estado}). Revisa la respuesta antes de seguir.")
    if a.enviar:
        log.close()
    print("Listo.")


if __name__ == "__main__":
    main()
