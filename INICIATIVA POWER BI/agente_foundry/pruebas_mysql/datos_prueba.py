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
# A2 (alcance por ORE): tres escuelas de OTROS ORE. Se añaden al final y con su propio generador aleatorio para no alterar
# ni un número de las escuelas 1-12 (los datos de siempre son idénticos).
ESCUELAS += [(13, "Escuela Prueba J"), (14, "Escuela Prueba K"), (15, "Escuela Prueba L")]
ORE = {s: "MUNICIPIO PRUEBA" for s, _ in ESCUELAS}
ORE[10] = "DEMOSTRACION"
ORE_ID = {s: 1 for s, _ in ESCUELAS}                    # ore_id de cada escuela: 1 = MUNICIPIO PRUEBA (escuelas 1-12)
ORE.update({13: "MUNICIPIO SEGUNDO", 14: "MUNICIPIO SEGUNDO", 15: "MUNICIPIO TERCERO"})
ORE_ID.update({10: 9, 13: 2, 14: 2, 15: 3})      # ore_id ↔ nombre de ORE 1:1 (DEMOSTRACION = 9)
ESCUELAS_ORE2 = {13, 14}
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
    # ── A2: usuarios, conexiones y pruebas de las escuelas 13-15 (otros ORE), con un generador aparte ──
    R2 = random.Random(1313)
    base = len(usuarios)
    for sid, n_maestros, n_est in ((13, 3, 6), (14, 2, 4), (15, 2, 4)):
        usuarios.append((uid, 6, sid, "Director", f"Prueba {sid:02d}")); uid += 1
        for k in range(n_maestros):
            usuarios.append((uid, 5, sid, "Maestro", f"Prueba {sid:02d}-{k + 1}")); uid += 1
        for k in range(n_est):
            usuarios.append((uid, 3, sid, "Estudiante", f"Prueba {sid:02d}-{k + 1:02d}")); uid += 1
    extra = usuarios[base:]
    dia = dt.date(2026, 1, 12)
    while dia <= ULT_CONEXION.date():
        if dia.weekday() < 5:
            for u in extra:
                if R2.random() < prob[u[1]]:
                    conex[(u[0], dia)] = D.combine(dia, dt.time(R2.randint(0, 23), R2.randint(0, 59), R2.randint(0, 59)))
        dia += dt.timedelta(days=1)
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
    for e in (u[0] for u in extra if u[1] == 3):           # A2: pruebas de los estudiantes de las escuelas 13-15 (generador aparte)
        for _ in range(R2.randint(4, 9)):
            asig = D(2026, 2, 1) + dt.timedelta(seconds=R2.randint(0, int((D(2026, 10, 19) - D(2026, 2, 1)).total_seconds())))
            tom = None
            if R2.random() < 0.25:
                t = asig + dt.timedelta(days=R2.randint(0, 9), seconds=R2.randint(0, 80000))
                tom = t if t <= ULT_TOMADA else None
            resultados.append((e, R2.choice(["EN", "MAT-SP", "SP"]), asig, tom))
    return usuarios, conexiones, resultados


# ── Suscripciones y uso de docentes (generador aparte para no alterar los datos de arriba) ──
CALENDARIOS = {"A": [(D(2026, 8, 28), D(2026, 10, 23)), (D(2026, 10, 24), D(2027, 1, 8))],      # como las 40 escuelas ORE
               "B": [(D(2026, 9, 12), D(2026, 10, 15)), (D(2026, 10, 16), D(2027, 1, 21))]}     # como Escuela del Deporte
INDICADORES = ["has_connections_current_period", "has_created_instructional_plans_current_period",
               "plans_have_data_current_period", "has_created_groups_current_year", "has_created_assignments_overall"]


def suscripciones(usuarios):
    """(user_id, status). Como en la base real, solo existen 'active' y 'canceled'. Activo si user_id % 5 != 0; los múltiplos de 5 tienen 'canceled';
    los múltiplos de 15 no tienen ninguna fila (sin suscripción); los múltiplos de 7 tienen además una fila 'canceled' (activa + cancelada: gana activa, prueba el DISTINCT)."""
    filas = []
    for u in usuarios:
        uid = u[0]
        if uid % 15 == 0:
            continue                                  # sin suscripción (nunca fue activo: 15 es múltiplo de 5)
        filas.append((uid, "canceled" if uid % 5 == 0 else "active"))
        if uid % 7 == 0:
            filas.append((uid, "canceled"))
    return filas


def docentes(usuarios):
    """Filas de vw_teacher_activities: una por docente × período de su escuela (+ un duplicado con otros indicadores)."""
    r = random.Random(77)
    nom = dict(ESCUELAS)
    filas = []                                   # (sie, nombre, ore, escuela, desde, hasta, [5 x 'SI'/'NO'])
    for u in usuarios:
        if u[1] != 5 or u[2] is None:
            continue
        for d, h in CALENDARIOS["A" if u[2] <= 4 else "B"]:
            ind = ["SI" if r.random() < 0.4 else "NO" for _ in INDICADORES]
            filas.append((f"SIE{u[0]}", f"{u[3]} {u[4]}", ORE[u[2]], nom[u[2]], d, h, ind))
            if u[0] % 4 == 0:                    # misma persona y período otra vez: cuenta UNA vez y vale el 'SI' de cualquiera
                filas.append((f"SIE{u[0]}", f"{u[3]} {u[4]}", ORE[u[2]], nom[u[2]], d, h, ["SI" if x == "NO" else "NO" for x in ind]))
    return filas


def porcentaje_quiz(user, asignada, tomada):
    """% Full Quiz sintético y determinista (solo de las pruebas ya realizadas; NULL si no): 0.00-100.00 con dos decimales. Lo usa el verificador de promedio-full-quiz."""
    if tomada is None:
        return "NULL"
    return f"{((user * 31 + asignada.day * 7 + asignada.hour * 3 + asignada.month) % 101) + (user % 4) * 0.25:.2f}"


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
           " last_name VARCHAR(155), role_id INT NOT NULL, role VARCHAR(45) NOT NULL, school_id INT NULL, sie VARCHAR(50), ore_id INT, invitation_code VARCHAR(100));",
           "CREATE TABLE connected_date (id INT AUTO_INCREMENT PRIMARY KEY, user_id INT, connected_date TIMESTAMP NULL, subscription_status VARCHAR(25));",
           "CREATE TABLE subject (id INT PRIMARY KEY, subject_code VARCHAR(15), subject_name VARCHAR(100));",
           "CREATE TABLE result_generic (id INT AUTO_INCREMENT PRIMARY KEY, user_id INT, subject_code VARCHAR(15),"
           " quiz_assigned_to_plan_date TIMESTAMP NULL, quiz_taken_date TIMESTAMP NULL, quiz_title VARCHAR(1000), total_percent_full_quiz DECIMAL(6,2) NULL);",
           "CREATE TABLE etl_last_date (id INT PRIMARY KEY, etl_last_date DATETIME(6));",
           # Esquema REAL (confirmado 19-nov): SIN columna id; solo una clave invisible my_row_id que no se debe usar (el 19-nov una consulta que contaba sph.id falló con la base real).
           "CREATE TABLE subscription_profile_history (my_row_id BIGINT NOT NULL AUTO_INCREMENT INVISIBLE PRIMARY KEY, subscription_id INT NOT NULL, status VARCHAR(20) NOT NULL,"
           " activated_date DATETIME NULL, cancellation_date DATETIME NULL, user_attached_date DATETIME NULL, user_id INT NOT NULL, plan_name VARCHAR(100) NULL, plan_id INT NULL);",
           # En producción es una VISTA que cruza otras bases; aquí una tabla con las mismas columnas.
           "CREATE TABLE vw_teacher_activities (teacher_name VARCHAR(200), invitation_code VARCHAR(100), sie VARCHAR(100), ore VARCHAR(100),"
           " school VARCHAR(250), school_number VARCHAR(50), from_period DATETIME, to_period DATETIME, "
           + ", ".join(f"{c} VARCHAR(2)" for c in INDICADORES) + ");"]
    out.append("INSERT INTO role VALUES " + ",".join(f"({i},{q(n)})" for i, n in ROLES) + ";")
    out.append("INSERT INTO school (school_id, school_name, school_type, city, ore, ore_id, country, country_iso) VALUES "
               + ",".join(f"({i},{q(n)},'Public','Ciudad Prueba',{q(ORE[i])},{ORE_ID[i]},'Puerto Rico','PR')" for i, n in ESCUELAS) + ";")
    out.append("INSERT INTO user (user_id, username, email, first_name, last_name, role_id, role, school_id) VALUES "
               + ",".join(f"({u[0]},'usuario{u[0]}',{q(email_de(u[0], u[3], u[4]))},{q(u[3])},{q(u[4])},{u[1]},{q(rol[u[1]])},{q(u[2])})" for u in usuarios) + ";")
    out.append("INSERT INTO subject (id, subject_code, subject_name) VALUES "
               + ",".join(f"({i + 1},{q(c)},{q(n)})" for i, (c, n) in enumerate(MATERIAS)) + ";")
    for i in range(0, len(conexiones), 500):
        out.append("INSERT INTO connected_date (user_id, connected_date, subscription_status) VALUES "
                   + ",".join(f"({u},{f(h)},'active')" for u, h in conexiones[i:i + 500]) + ";")
    for i in range(0, len(resultados), 500):
        out.append("INSERT INTO result_generic (user_id, subject_code, quiz_assigned_to_plan_date, quiz_taken_date, quiz_title, total_percent_full_quiz) VALUES "
                   + ",".join(f"({u},{q(m)},{f(a)},{f(t)},'Prueba sintetica',{porcentaje_quiz(u, a, t)})" for u, m, a, t in resultados[i:i + 500]) + ";")
    out.append("INSERT INTO etl_last_date VALUES (1, '2026-11-12 03:00:00.000000');")
    out.append("INSERT INTO subscription_profile_history (user_id, subscription_id, status, cancellation_date) VALUES "
               + ",".join(f"({u},1,{q(st)},'2027-06-30 00:00:00')" for u, st in suscripciones(usuarios)) + ";")
    out.append(f"INSERT INTO vw_teacher_activities (sie, teacher_name, ore, school, from_period, to_period, {', '.join(INDICADORES)}) VALUES "
               + ",".join(f"({q(a)},{q(b)},{q(c)},{q(e)},{f(d)},{f(h)}," + ",".join(q(x) for x in ind) + ")"
                          for a, b, c, e, d, h, ind in docentes(usuarios)) + ";")
    return "\n".join(out) + "\n"


if __name__ == "__main__":
    destino = os.path.join(os.path.dirname(os.path.abspath(__file__)), "esquema_y_datos.sql")
    datos = generar()
    open(destino, "w", encoding="utf-8").write(a_sql(*datos))
    print(f"{destino}: {len(datos[0])} usuarios, {len(datos[1])} conexiones, {len(datos[2])} pruebas", file=sys.stderr)
