#!/usr/bin/env python3
"""Genera la portada de cada cuento con la API de Gemini (Nano Banana 2 Lite) a partir de
salida/prompts_imagenes.csv y las guarda en salida/imagenes/<nombre>.png.

La API key NO va en el código: se lee de la variable de entorno GEMINI_API_KEY o se pide por teclado
(no se muestra). Se crea en AI Studio > ícono de llave > Create API key.

Sin --generar solo simula (no llama a la API). Lo que ya existe en salida/imagenes se salta, así que si
se corta por cuota se puede volver a correr y sigue donde se quedó.

Ejemplos:
    python3 -I herramientas/generar_imagenes.py                        # simula
    python3 -I herramientas/generar_imagenes.py --generar --limite 1   # prueba con una
    python3 -I herramientas/generar_imagenes.py --generar              # genera las que faltan
    python3 -I herramientas/generar_imagenes.py --generar --proporcion 16:9
"""
import argparse
import base64
import csv
import getpass
import json
import os
import sys
import time
import urllib.error
import urllib.request
from datetime import datetime
from pathlib import Path

MODELO = "gemini-3.1-flash-lite-image"
URL = "https://generativelanguage.googleapis.com/v1beta/models/{}:generateContent"
EXT = {"image/png": ".png", "image/jpeg": ".jpg", "image/webp": ".webp"}


def pedir_imagen(prompt, key, proporcion, modelo):
    cuerpo = {"contents": [{"parts": [{"text": prompt}]}],
              "generationConfig": {"responseModalities": ["TEXT", "IMAGE"]}}
    if proporcion:
        cuerpo["generationConfig"]["imageConfig"] = {"aspectRatio": proporcion}
    req = urllib.request.Request(
        URL.format(modelo), data=json.dumps(cuerpo).encode("utf-8"), method="POST",
        headers={"Content-Type": "application/json", "x-goog-api-key": key})
    try:
        with urllib.request.urlopen(req, timeout=120) as r:
            return r.status, json.loads(r.read().decode("utf-8"))
    except urllib.error.HTTPError as e:
        texto = e.read().decode("utf-8", "replace")
        try:
            return e.code, json.loads(texto)
        except ValueError:
            return e.code, {"error": texto[:300]}


def extraer(resp):
    """(bytes, mime) de la primera imagen de la respuesta, o (None, motivo)."""
    for cand in resp.get("candidates", []):
        for parte in cand.get("content", {}).get("parts", []):
            d = parte.get("inlineData") or parte.get("inline_data")
            if d and d.get("data"):
                return base64.b64decode(d["data"]), d.get("mimeType") or d.get("mime_type") or "image/png"
    motivo = resp.get("promptFeedback", {}).get("blockReason") or \
        (resp.get("candidates") or [{}])[0].get("finishReason") or "sin imagen en la respuesta"
    return None, motivo


def main():
    raiz = Path(__file__).resolve().parent.parent
    ap = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("--csv", type=Path, default=raiz / "salida" / "prompts_imagenes.csv")
    ap.add_argument("--destino", type=Path, default=raiz / "salida" / "imagenes")
    ap.add_argument("--generar", action="store_true", help="llamar a la API de verdad (sin esto solo simula)")
    ap.add_argument("--limite", type=int, help="generar como máximo N imágenes")
    ap.add_argument("--proporcion", help="por ejemplo 16:9, 4:3 o 1:1 (si no se indica, el modelo decide)")
    ap.add_argument("--modelo", default=MODELO)
    ap.add_argument("--pausa", type=float, default=1.0, help="segundos entre llamadas (por defecto 1)")
    a = ap.parse_args()

    with open(a.csv, encoding="utf-8-sig") as fh:
        filas = list(csv.DictReader(fh))
    a.destino.mkdir(parents=True, exist_ok=True)
    hechas = {p.stem for p in a.destino.iterdir() if p.suffix in EXT.values()}
    pendientes = [f for f in filas if Path(f["imagen"]).stem not in hechas]
    if a.limite:
        pendientes = pendientes[: a.limite]
    print(f"{len(filas)} prompts; ya generadas: {len(filas) - len([f for f in filas if Path(f['imagen']).stem not in hechas])}; "
          f"a procesar ahora: {len(pendientes)}  [{'GENERACIÓN REAL' if a.generar else 'SIMULACIÓN'}]  modelo {a.modelo}")

    if not a.generar:
        for f in pendientes[:5]:
            print(f"  [simulación] {f['imagen']}  <-  {f['prompt'][:90]}...")
        if len(pendientes) > 5:
            print(f"  ... y {len(pendientes) - 5} más")
        print("Listo.")
        return

    key = os.environ.get("GEMINI_API_KEY") or getpass.getpass("API key (no se muestra): ").strip()
    if not key:
        sys.exit("Falta la API key.")
    log_ruta = raiz / "salida" / "imagenes_log.csv"
    nuevo = not log_ruta.exists()
    log = open(log_ruta, "a", newline="", encoding="utf-8-sig")
    w = csv.writer(log)
    if nuevo:
        w.writerow(["imagen", "estado", "detalle", "fecha"])

    ok = 0
    for f in pendientes:
        estado, resp = pedir_imagen(f["prompt"], key, a.proporcion, a.modelo)
        if estado == 200:
            datos, mime = extraer(resp)
            if datos:
                nombre = Path(f["imagen"]).stem + EXT.get(mime, ".png")
                (a.destino / nombre).write_bytes(datos)
                ok += 1
                w.writerow([nombre, 200, f"{len(datos)} bytes {mime}", datetime.now().isoformat(timespec="seconds")])
                log.flush()
                print(f"  OK   {nombre}  ({len(datos) // 1024} KB)")
            else:
                w.writerow([f["imagen"], 200, f"sin imagen: {mime}", datetime.now().isoformat(timespec="seconds")])
                log.flush()
                print(f"  --   {f['imagen']}  no devolvió imagen ({mime}); se salta, revisar luego")
        else:
            detalle = json.dumps(resp, ensure_ascii=False)[:300]
            w.writerow([f["imagen"], estado, detalle, datetime.now().isoformat(timespec="seconds")])
            log.flush()
            print(f"  {estado}  {f['imagen']}  {detalle[:150]}")
            if estado in (401, 403):
                sys.exit("La API key no sirve o no tiene permiso. Revisa la key.")
            if estado == 429:
                sys.exit(f"Se llegó al límite de cuota. Llevas {ok} imágenes; espera y vuelve a correr (retoma donde quedó).")
            sys.exit(f"Se detuvo en {f['imagen']} (estado {estado}). Revisa la respuesta antes de seguir.")
        time.sleep(a.pausa)
    log.close()
    print(f"Listo. {ok} imágenes nuevas en {a.destino}")


if __name__ == "__main__":
    main()
