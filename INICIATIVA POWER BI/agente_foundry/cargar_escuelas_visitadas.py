"""Genera el SQL para cargar el Excel de escuelas visitadas en agente.escuelas_visitadas (REQ-3741, P5).

Uso:
    pip install openpyxl
    python3 cargar_escuelas_visitadas.py Escuelas_EA_Visitadas.xlsx [salida.sql]

No se conecta a la base: escribe un .sql que se abre y ejecuta en MySQL Workbench (después de sql/10).
La carga REEMPLAZA la tabla completa dentro de una transacción, así que una escuela que se borre del Excel también
desaparece de la tabla. Lee la tabla «Table1» del libro (la misma que lee Power BI); si no existe, la primera hoja.
Antes de escribir, valida los datos e informa los problemas; con errores graves (escuela sin school_identifier,
identificador repetido, Visitada distinta de Si/No) no genera nada.
"""
import sys
from datetime import date, datetime

try:
    from openpyxl import load_workbook
except ImportError:
    sys.exit("Falta openpyxl: ejecuta  pip install openpyxl")

COLUMNAS = {  # encabezado del Excel (sin distinguir mayúsculas) → columna de la tabla
    "school_name": "school_name",
    "school_identifier": "school_identifier",
    "school_id": "school_id",
    "sistema educativo": "sistema_educativo",
    "visitada (si/no)": "visitada",
    "fecha visita inicial": "fecha_visita_inicial",
    "fecha ultima visita": "fecha_ultima_visita",
    "período": "periodo_excel",
    "periodo": "periodo_excel",
}


def filas_de_la_tabla(ruta):
    libro = load_workbook(ruta, data_only=True)
    for hoja in libro.worksheets:
        if "Table1" in hoja.tables:
            celdas = hoja[hoja.tables["Table1"].ref]
            return [[c.value for c in fila] for fila in celdas], f"tabla Table1 de la hoja «{hoja.title}»"
    hoja = libro.worksheets[0]
    return [list(f) for f in hoja.iter_rows(values_only=True)], f"hoja «{hoja.title}» (no se encontró Table1)"


def texto(v):
    if v is None:
        return None
    if isinstance(v, float) and v.is_integer():
        v = int(v)
    s = str(v).strip()
    return s or None


def fecha(v, donde, problemas):
    if v is None or (isinstance(v, str) and not v.strip()):
        return None
    if isinstance(v, datetime):
        return v.date()
    if isinstance(v, date):
        return v
    try:
        return datetime.strptime(str(v).strip(), "%m/%d/%Y").date()   # el Excel guarda M/D/AAAA como texto
    except ValueError:
        problemas.append(f"{donde}: fecha «{v}» no tiene el formato M/D/AAAA; se deja vacía")
        return None


def sql(v):
    if v is None:
        return "NULL"
    if isinstance(v, (int, bool)):
        return str(int(v))
    if isinstance(v, date):
        return f"'{v.isoformat()}'"
    return "'" + str(v).replace("\\", "\\\\").replace("'", "''") + "'"


def main():
    if len(sys.argv) < 2:
        sys.exit(__doc__)
    ruta, salida = sys.argv[1], (sys.argv[2] if len(sys.argv) > 2 else "escuelas_visitadas_carga.sql")
    filas, origen = filas_de_la_tabla(ruta)
    filas = [f for f in filas if any(c not in (None, "") for c in f)]
    if not filas:
        sys.exit("El Excel no tiene filas.")
    encabezado = [str(c).strip().lower() if c is not None else "" for c in filas[0]]
    idx = {COLUMNAS[h]: i for i, h in enumerate(encabezado) if h in COLUMNAS}
    faltan = {"school_name", "school_identifier", "visitada"} - idx.keys()
    if faltan:
        sys.exit(f"Faltan columnas en el Excel: {', '.join(sorted(faltan))}. Encabezados leídos: {encabezado}")

    errores, avisos, registros, vistos = [], [], [], set()
    for n, f in enumerate(filas[1:], start=2):
        val = lambda col: f[idx[col]] if col in idx and idx[col] < len(f) else None
        nombre, ident = texto(val("school_name")), texto(val("school_identifier"))
        donde = f"fila {n} ({nombre or 'sin nombre'})"
        if not nombre or not ident:
            errores.append(f"{donde}: falta school_name o school_identifier")
            continue
        if ident in vistos:
            errores.append(f"{donde}: school_identifier {ident} repetido")
            continue
        vistos.add(ident)
        vis = (texto(val("visitada")) or "").lower().replace("í", "i")
        if vis not in ("si", "no"):
            errores.append(f"{donde}: «Visitada (Si/No)» = «{val('visitada')}»; debe ser Si o No")
            continue
        ini = fecha(val("fecha_visita_inicial"), donde, avisos)
        ult = fecha(val("fecha_ultima_visita"), donde, avisos)
        if ini and ult and ult < ini:
            avisos.append(f"{donde}: la última visita ({ult}) es anterior a la inicial ({ini})")
        if vis == "si" and not ult:
            avisos.append(f"{donde}: marcada como visitada pero sin fecha de última visita")
        sid = texto(val("school_id"))
        registros.append({
            "school_identifier": ident, "school_name": nombre.upper(),
            "school_id": int(sid) if sid and sid.isdigit() else None,
            "sistema_educativo": texto(val("sistema_educativo")), "visitada": vis == "si",
            "fecha_visita_inicial": ini, "fecha_ultima_visita": ult, "periodo_excel": texto(val("periodo_excel")),
        })

    print(f"Leído: {origen} · {len(registros)} escuelas · "
          f"{sum(r['visitada'] for r in registros)} visitadas · {sum(not r['visitada'] for r in registros)} no visitadas")
    for a in avisos:
        print("  AVISO:", a)
    if errores:
        for e in errores:
            print("  ERROR:", e)
        sys.exit("No se generó el SQL: corrige los errores en el Excel y vuelve a ejecutar.")

    cols = list(registros[0].keys())
    with open(salida, "w", encoding="utf-8") as out:
        out.write(f"-- Carga de agente.escuelas_visitadas generada el {datetime.now():%Y-%m-%d %H:%M} desde {origen}\n")
        out.write("-- Reemplaza la tabla completa. Ejecutar en Workbench con el usuario administrador o agente_cargador.\n")
        out.write("SET NAMES utf8mb4;\nSTART TRANSACTION;\nDELETE FROM agente.escuelas_visitadas;\n")
        out.write(f"INSERT INTO agente.escuelas_visitadas ({', '.join(cols)}) VALUES\n")
        out.write(",\n".join("  (" + ", ".join(sql(r[c]) for c in cols) + ")" for r in registros) + ";\n")
        out.write("COMMIT;\n\n")
        out.write("SELECT SUM(visitada = 1) AS visitadas, SUM(visitada = 0) AS no_visitadas, COUNT(*) AS total, MAX(cargado_en) AS cargado_en\n"
                  "FROM agente.escuelas_visitadas;\n")
    print(f"SQL escrito en {salida}. Ábrelo en Workbench y ejecútalo.")


if __name__ == "__main__":
    main()
