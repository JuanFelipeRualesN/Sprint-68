"""Datos SINTÉTICOS con el esquema real de gs_data_analysis (columnas tomadas del descubrimiento del 12-nov-2026).
No contiene ni un dato real. Genera:
  - esquema_y_datos.sql   (DDL + INSERT, para cargar en un MySQL 8 de pruebas local)
y expone los datos en memoria para que verificar_api_mysql.py recalcule lo esperado por fuerza bruta.

Casos límite incluidos a propósito: conexiones a las 00:00:00 y 23:59:59 UTC en las fronteras de un período,
una escuela sin usuarios, una escuela con usuarios y cero conexiones, docentes sin escuela, filas de pruebas
de usuarios que NO son estudiantes (deben ignorarse), una materia con pruebas solo fuera del período.
"""
import datetime as dt
import os
import random
import sys

R = random.Random(2026)
D = dt.datetime
ULT_CONEXION = D(2026, 11, 11, 20, 14, 13)      # como en la base real
ULT_TOMADA = D(2026, 10, 19, 18, 18, 52)
ULT_ASIGNADA = D(2026, 10, 19, 12, 51, 17)

ROLES = [(1, "client"), (2, "admin"), (3, "student"), (5, "teacher"), (6, "director"), (8, "admin-ore")]
MATERIAS = [("EN", "English"), ("MAT-SP", "Matemáticas"), ("SP", "Español"), ("A1-SP", "Álgebra I"),
            ("A2", "Álgebra II"), ("BI-SP", "Biología"), ("CD", "BORRAR")]   # BI-SP y CD: sin pruebas nunca
ESCUELAS = [(i + 1, f"Escuela Prueba {chr(65 + i)}") for i in range(9)]       # la 9 (I) no tiene usuarios
ESCUELAS += [(10, "Escuela Demo Interna"), (11, "Escuela Demo T3 School"), (12, "Escuela Facilitadores MSJ")]    # de prueba: ORE DEMOSTRACION / excluida por nombre
ORE = {s: "MUNICIPIO PRUEBA" for s, _ in ESCUELAS}
ORE[10] = "DEMOSTRACION"
DOMINIO_INTERNO = "requiretechnology.com"


def email_de(uid, nombre, apellido):
    """Un docente de una escuela real con correo interno (debe excluirse por dominio)."""
    return f"usuario{uid}@{DOMINIO_INTERNO if (nombre, apellido) == ('Maestro', 'Prueba 01-1') else 'ejemplo.invalid'}"


def generar():
    usuarios, uid = [], 1                      # (user_id, role_id, school_id, nombre, apellido)
    for sid in [1, 2, 3, 4, 5, 6, 7, 8, 10]:                  # 8 = sin conexiones; 10 = escuela de prueba (ORE DEMOSTRACION)
        usuarios.append((uid, 6, sid, "Director", f"Prueba {sid:02d}")); uid += 1
        for k in range(3):
            usuarios.append((uid, 5, sid, "Maestro", f"Prueba {sid:02d}-{k + 1}")); uid += 1
        for k in range(10 if sid != 10 else 5):
            usuarios.append((uid, 3, sid, "Estudiante", f"Prueba {sid:02d}-{k + 1:02d}")); uid += 1
    usuarios.append((uid, 5, 11, "Maestro", "Demo T3")); uid += 1                # escuela 11: excluida por nombre
    usuarios.append((uid, 5, 12, "Maestro", "Facilitador")); uid += 1            # escuela 12: excluida por nombre (como en Power BI)
    usuarios.append((uid, 1, 1, "Cliente", "Prueba")); uid += 1                  # rol client (1) con escuela y conexiones: la vista de Power BI lo excluye
    usuarios.append((uid, 5, None, "Maestro", "SinEscuela")); uid += 1
    usuarios.append((uid, 2, None, "Admin", "Prueba")); uid += 1

    # ── conexiones: una fila por usuario y día (UTC) ──
    prob = {6: 0.40, 5: 0.35, 3: 0.08, 2: 0.2, 1: 0.5}
    conex = {}
    dia = dt.date(2026, 1, 12)
    while dia <= ULT_CONEXION.date():
        if dia.weekday() < 5:
            for u in usuarios:
                if u[2] == 8:                          # escuela H: usuarios pero CERO conexiones
                    continue
                if R.random() < prob[u[1]]:
                    h = D.combine(dia, dt.time(R.randint(0, 23), R.randint(0, 59), R.randint(0, 59)))
                    conex[(u[0], dia)] = h
        dia += dt.timedelta(days=1)
    maestro_b = next(u[0] for u in usuarios if u[4] == "Prueba 02-1")
    for h in (D(2026, 8, 31, 23, 59, 59), D(2026, 9, 1, 0, 0, 0), D(2026, 10, 31, 23, 59, 59), D(2026, 11, 1, 0, 0, 0)):
        conex[(maestro_b, h.date())] = h               # fronteras de septiembre/octubre (en UTC y en UTC-4)
    conex[(maestro_b, ULT_CONEXION.date())] = ULT_CONEXION
    conexiones = sorted((u, h) for (u, _), h in conex.items())

    # ── pruebas: solo estudiantes (más unas filas de docentes que deben ignorarse) ──
    estudiantes = [u[0] for u in usuarios if u[1] == 3]
    resultados = []                                    # (user_id, subject_code, asignada, tomada|None)

    def fila(user, mat, asig):
        tom = None
        if R.random() < 0.25:
            t = asig + dt.timedelta(days=R.randint(0, 9), seconds=R.randint(0, 80000))
            tom = t if t <= ULT_TOMADA else None
        resultados.append((user, mat, asig, tom))

    for e in estudiantes:
        for _ in range(R.randint(4, 9)):
            mat = R.choices(["EN", "MAT-SP", "SP", "A1-SP"], weights=[4, 3, 3, 1])[0]
            asig = D(2026, 2, 1) + dt.timedelta(seconds=R.randint(0, int((D(2026, 10, 19) - D(2026, 2, 1)).total_seconds())))
            fila(e, mat, asig)
    for e in estudiantes[:6]:                          # A2: solo en feb-mar (cero en sep-oct)
        fila(e, "A2", D(2026, 2, 10) + dt.timedelta(hours=R.randint(0, 600)))
    resultados.append((estudiantes[0], "EN", ULT_ASIGNADA, None))
    resultados.append((estudiantes[1], "EN", D(2026, 10, 18, 9, 0, 0), ULT_TOMADA))
    # Casos que separan «cohorte por fecha de asignación» de «por fecha de realización»:
    resultados.append((estudiantes[2], "EN", D(2026, 8, 30, 12, 0, 0), D(2026, 9, 3, 12, 0, 0)))    # asignada ago, realizada sep
    resultados.append((estudiantes[3], "SP", D(2026, 9, 30, 12, 0, 0), D(2026, 10, 3, 12, 0, 0)))   # asignada sep, realizada oct
    resultados.append((estudiantes[4], "SP", D(2026, 9, 10, 12, 0, 0), None))                       # asignada sep, sin realizar
    docentes = [u[0] for u in usuarios if u[1] == 5][:3]
    for k, d in enumerate(docentes):                   # no son estudiantes: la consulta debe ignorarlas
        resultados.append((d, "EN", D(2026, 9, 5 + k, 10, 0, 0), D(2026, 9, 6 + k, 10, 0, 0)))
    return usuarios, conexiones, resultados


def a_sql(usuarios, conexiones, resultados):
    q = lambda s: "NULL" if s is None else "'" + str(s).replace("'", "''") + "'"
    f = lambda d: "NULL" if d is None else f"'{d:%Y-%m-%d %H:%M:%S}'"
    rol = dict(ROLES)
    out = ["DROP DATABASE IF EXISTS gs_data_analysis;",
           "CREATE DATABASE gs_data_analysis CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;",
           "USE gs_data_analysis;",
           "SET time_zone = '+00:00';",
           "CREATE TABLE role (role_id INT PRIMARY KEY, role VARCHAR(45));",
           "CREATE TABLE school (school_id INT PRIMARY KEY, school_name VARCHAR(250), school_type VARCHAR(30), city VARCHAR(100),"
           " ore VARCHAR(100), ore_id INT, school_identifier VARCHAR(100), state_iso_code VARCHAR(45), country VARCHAR(150), country_iso VARCHAR(45));",
           "CREATE TABLE user (user_id INT PRIMARY KEY, username VARCHAR(100), email VARCHAR(200), first_name VARCHAR(100),"
           " last_name VARCHAR(155), role_id INT, role VARCHAR(45), school_id INT NULL, sie VARCHAR(50), ore_id INT, invitation_code VARCHAR(100));",
           "CREATE TABLE connected_date (id INT AUTO_INCREMENT PRIMARY KEY, user_id INT, connected_date TIMESTAMP NULL, subscription_status VARCHAR(25));",
           "CREATE TABLE subject (id INT PRIMARY KEY, subject_code VARCHAR(15), subject_name VARCHAR(100));",
           "CREATE TABLE result_generic (id INT AUTO_INCREMENT PRIMARY KEY, user_id INT, subject_code VARCHAR(15),"
           " quiz_assigned_to_plan_date TIMESTAMP NULL, quiz_taken_date TIMESTAMP NULL, quiz_title VARCHAR(1000));",
           "CREATE TABLE etl_last_date (id INT PRIMARY KEY, etl_last_date DATETIME(6));"]
    out.append("INSERT INTO role VALUES " + ",".join(f"({i},{q(n)})" for i, n in ROLES) + ";")
    out.append("INSERT INTO school (school_id, school_name, school_type, city, ore, ore_id, country, country_iso) VALUES "
               + ",".join(f"({i},{q(n)},'Public','Ciudad Prueba',{q(ORE[i])},1,'Puerto Rico','PR')" for i, n in ESCUELAS) + ";")
    out.append("INSERT INTO user (user_id, username, email, first_name, last_name, role_id, role, school_id) VALUES "
               + ",".join(f"({u[0]},'usuario{u[0]}',{q(email_de(u[0], u[3], u[4]))},{q(u[3])},{q(u[4])},{u[1]},{q(rol[u[1]])},{q(u[2])})" for u in usuarios) + ";")
    out.append("INSERT INTO subject (id, subject_code, subject_name) VALUES "
               + ",".join(f"({i + 1},{q(c)},{q(n)})" for i, (c, n) in enumerate(MATERIAS)) + ";")
    for i in range(0, len(conexiones), 500):
        out.append("INSERT INTO connected_date (user_id, connected_date, subscription_status) VALUES "
                   + ",".join(f"({u},{f(h)},'active')" for u, h in conexiones[i:i + 500]) + ";")
    for i in range(0, len(resultados), 500):
        out.append("INSERT INTO result_generic (user_id, subject_code, quiz_assigned_to_plan_date, quiz_taken_date, quiz_title) VALUES "
                   + ",".join(f"({u},{q(m)},{f(a)},{f(t)},'Prueba sintetica')" for u, m, a, t in resultados[i:i + 500]) + ";")
    out.append("INSERT INTO etl_last_date VALUES (1, '2026-11-12 03:00:00.000000');")
    return "\n".join(out) + "\n"


if __name__ == "__main__":
    destino = os.path.join(os.path.dirname(os.path.abspath(__file__)), "esquema_y_datos.sql")
    datos = generar()
    open(destino, "w", encoding="utf-8").write(a_sql(*datos))
    print(f"{destino}: {len(datos[0])} usuarios, {len(datos[1])} conexiones, {len(datos[2])} pruebas", file=sys.stderr)
