"""Black-box verifier of the monolith gs-base-api (REQ-3741 · B13).

It calls the REAL monolith (usage insights routes in DataAnalysisAPI and the data assistant in AIServicesAPI) and compares what comes back with the
same figures RECOMPUTED BY BRUTE FORCE in Python over the same synthetic data (the generator of ../pruebas_mysql/datos_prueba.py, which is also the
source of the SQL loaded in MySQL). It is a second, independent implementation: if the SQL, the scope or the wiring of the monolith is wrong, the
difference shows here. The session comes from fake_users_api.py and the model from fake_model.py (Chat Completions), so nothing real is touched.

Usage:  python3 verify_monolith.py http://127.0.0.1:5101 0      (URL of the monolith, UTC offset in minutes that the monolith uses)
"""
import datetime as dt
import json
import os
import sys
import unicodedata
import urllib.error
import urllib.parse
import urllib.request
from functools import cmp_to_key

sys.path.insert(0, os.path.join(os.path.dirname(os.path.abspath(__file__)), "..", "pruebas_mysql"))
import datos_prueba as dp   # noqa: E402  (synthetic data generator shared with the MVP harness)
import leer_xlsx            # noqa: E402  (reads the sheets of an .xlsx file)

BASE = sys.argv[1].rstrip("/")
OFFSET = int(sys.argv[2]) if len(sys.argv) > 2 else 0
INSIGHTS = BASE + "/dataanalysisapi/usage-insights"
ASSISTANT = BASE + "/aiservicesapi/data-assistant"
MODEL = os.environ.get("FAKE_MODEL_URL", "http://127.0.0.1:5198")
USERS_API = os.environ.get("FAKE_USERS_API_URL", "http://127.0.0.1:5199")

USERS, CONNECTIONS, RESULTS = dp.generar()
ROLE = {user[0]: user[1] for user in USERS}
SCHOOL_OF = {user[0]: user[2] for user in USERS}
SCHOOL_NAME = dict(dp.ESCUELAS)
SUBJECT_NAME = dict(dp.MATERIAS)
# Test accounts excluded by the monolith in this run: schools of the ORE DEMOSTRACION and the school "Escuela Facilitadores MSJ" (no domain exclusion).
EXCLUDED_SCHOOLS = {school for school, name in dp.ESCUELAS if "DEMOSTRACION" in dp.ORE[school] or name == "Escuela Facilitadores MSJ"}
SCOPES = {"tok-admin": None, "tok-ore1": ("ore", 1), "tok-ore2": ("ore", 2), "tok-director-1": ("school", 1)}
FAILURES, TOTAL = [], 0


# ── helpers ─────────────────────────────────────────────────────────────────────────────

def request(url, token=None, body=None, binary=False):
    headers = {"Token": token} if token else {}
    data = None
    if body is not None:
        data = json.dumps(body).encode()
        headers["Content-Type"] = "application/json"
    try:
        with urllib.request.urlopen(urllib.request.Request(url, data=data, headers=headers), timeout=60) as response:
            payload = response.read()
            return response.status, dict(response.headers), (payload if binary else json.loads(payload))
    except urllib.error.HTTPError as error:
        payload = error.read()
        try:
            return error.code, dict(error.headers), (payload if binary else json.loads(payload))
        except ValueError:
            return error.code, dict(error.headers), payload


def get(path, token="tok-admin", **params):
    status, _, body = request(f"{INSIGHTS}/{path}?{urllib.parse.urlencode(params)}", token)
    return status, body


def check(name, got, expected):
    global TOTAL
    TOTAL += 1
    if got != expected:
        FAILURES.append(name)
        print(f"  ✗ {name}\n      GOT      : {json.dumps(got, ensure_ascii=False, default=str)[:300]}\n      EXPECTED : {json.dumps(expected, ensure_ascii=False, default=str)[:300]}")
    else:
        print(f"  ✓ {name}")


def fold(text):
    """Name key like the collation utf8mb4_0900_ai_ci: no accents, no case."""
    return "".join(c for c in unicodedata.normalize("NFD", text) if not unicodedata.combining(c)).lower()


def sort_rows(rows, value, name, descending, tie_breaker=lambda row: 0):
    sign = -1 if descending else 1

    def compare(a, b):
        for x, y, s in ((value(a), value(b), sign), (fold(name(a)), fold(name(b)), sign), (tie_breaker(a), tie_breaker(b), 1)):
            if x != y:
                return s * (-1 if x < y else 1)
        return 0
    return sorted(rows, key=cmp_to_key(compare))


def period(first, last):
    start = dt.datetime.fromisoformat(first) - dt.timedelta(minutes=OFFSET)
    end = dt.datetime.fromisoformat(last) + dt.timedelta(days=1) - dt.timedelta(minutes=OFFSET)
    return start, end


def local_day(instant):
    return (instant + dt.timedelta(minutes=OFFSET)).date()


def in_scope(school, scope):
    if scope is None:
        return True
    if school is None:
        return False
    return dp.ORE_ID[school] == scope[1] if scope[0] == "ore" else school == scope[1]


def counts_as_connection(user, scope):
    school = SCHOOL_OF[user]
    return school is not None and school not in EXCLUDED_SCHOOLS and ROLE[user] != 1 and in_scope(school, scope)


def visible_student_school_user(user, scope):
    school = SCHOOL_OF[user]
    return school is not None and school not in EXCLUDED_SCHOOLS and in_scope(school, scope)


# ── oracles (brute force) ───────────────────────────────────────────────────────────────

def expected_connections_by_school(first, last, descending, limit, scope):
    start, end = period(first, last)
    count, users, latest = {}, {}, {}
    schools = {s for s, _ in dp.ESCUELAS if s not in EXCLUDED_SCHOOLS and in_scope(s, scope)
               and (any(SCHOOL_OF[u] == s and ROLE[u] == 3 for u in ROLE) or any(counts_as_connection(u, None) and SCHOOL_OF[u] == s for u, _ in CONNECTIONS))}
    for school in schools:
        count[school], users[school] = 0, set()
    for user, instant in CONNECTIONS:
        school = SCHOOL_OF[user]
        if school in count and ROLE[user] != 1 and start <= instant < end:
            count[school] += 1
            users[school].add(user)
            latest[school] = max(latest.get(school, local_day(instant)), local_day(instant))
    rows = [(SCHOOL_NAME[s], count[s], len(users[s]), latest.get(s)) for s in count]
    rows = sort_rows(rows, lambda r: r[1], lambda r: r[0], descending)[:limit]
    return [dict(Name=r[0], Connections=r[1], ConnectedUsers=r[2], **({"LastConnection": r[3].isoformat()} if r[3] else {})) for r in rows]


def expected_connections_by_teacher(first, last, descending, limit, scope, school_name=None):
    start, end = period(first, last)
    teachers = {u: 0 for u in ROLE if ROLE[u] == 5 and visible_student_school_user(u, scope) and (school_name is None or SCHOOL_NAME[SCHOOL_OF[u]] == school_name)}
    latest = {}
    for user, instant in CONNECTIONS:
        if user in teachers and start <= instant < end:
            teachers[user] += 1
            latest[user] = max(latest.get(user, local_day(instant)), local_day(instant))
    by_id = {u[0]: u for u in USERS}
    rows = [(f"{by_id[u][3]} {by_id[u][4]}", SCHOOL_NAME[SCHOOL_OF[u]], n, u) for u, n in teachers.items()]
    rows = sort_rows(rows, lambda r: r[2], lambda r: r[0], descending, lambda r: r[3])[:limit]
    return [dict(Name=r[0], School=r[1], Connections=r[2], **({"LastConnection": latest[r[3]].isoformat()} if r[3] in latest else {})) for r in rows]


def expected_subject_area_usage(first, last, descending, limit, scope):
    start, end = period(first, last)
    subjects = sorted({subject for _, subject, _, _ in RESULTS})
    rows = []
    for subject in subjects:
        taken = [r for r in RESULTS if r[1] == subject and ROLE[r[0]] == 3 and visible_student_school_user(r[0], scope)]
        assigned = sum(1 for r in taken if start <= r[2] < end)
        completed = sum(1 for r in taken if start <= r[2] < end and r[3])
        rows.append((SUBJECT_NAME[subject], assigned, completed))
    rows = sort_rows(rows, lambda r: r[2], lambda r: r[0], descending)[:limit]
    return [dict(Name=r[0], AssignedTests=r[1], CompletedTests=r[2], **({"CompletedPercentage": round(100.0 * r[2] / r[1], 1)} if r[1] else {})) for r in rows]


def period_key(day, granularity):
    return {"month": f"{day.year}-{day.month:02d}", "quarter": f"{day.year}-T{(day.month - 1) // 3 + 1}", "year": str(day.year)}[granularity]


def expected_trend(first, last, granularity, scope):
    start, end = period(first, last)
    series, day = {}, dt.date.fromisoformat(first)
    while day <= dt.date.fromisoformat(last):
        series.setdefault(period_key(day, granularity), 0)
        day += dt.timedelta(days=1)
    for user, instant in CONNECTIONS:
        if start <= instant < end and counts_as_connection(user, scope):
            series[period_key(local_day(instant), granularity)] += 1
    return [[key, series[key]] for key in sorted(series)]


def expected_subscription_total(scope):
    statuses = {}
    for user, status in dp.suscripciones(USERS):
        statuses.setdefault(user, []).append(status)
    totals = [0, 0, 0, 0]       # active, inactive, canceled, none
    for user in ROLE:
        if ROLE[user] == 1 or not visible_student_school_user(user, scope):
            continue
        found = statuses.get(user, [])
        category = 3 if not found else 0 if "active" in found else 2 if any("cancel" in s for s in found) else 1
        totals[category] += 1
    return totals


def expected_participation(first, last, scope):
    start, end = period(first, last)
    statuses = {}
    for user, status in dp.suscripciones(USERS):
        statuses.setdefault(user, []).append(status)
    rows = []
    for school, name in dp.ESCUELAS:
        if school in EXCLUDED_SCHOOLS or not in_scope(school, scope):
            continue
        active = sum(1 for u in ROLE if SCHOOL_OF[u] == school and ROLE[u] != 1 and "active" in statuses.get(u, []))
        connected = len({u for u, i in CONNECTIONS if SCHOOL_OF[u] == school and ROLE[u] != 1 and start <= i < end})
        if active > 0 or connected > 0:
            rows.append((name, active, connected))
    return sorted(rows)


def expected_compare_schools(first_a, last_a, first_b, last_b, scope):
    a = {r["Name"]: r["Connections"] for r in expected_connections_by_school(first_a, last_a, True, 1000, scope)}
    b = {r["Name"]: r["Connections"] for r in expected_connections_by_school(first_b, last_b, True, 1000, scope)}
    rows = [(n, a.get(n, 0), b.get(n, 0)) for n in set(a) | set(b) if a.get(n, 0) > 0 or b.get(n, 0) > 0]
    return sort_rows(rows, lambda r: r[2] - r[1], lambda r: r[0], True)


# ── 1. session and role ─────────────────────────────────────────────────────────────────
print(f"Monolith {BASE}  offset={OFFSET} min")
print("\n[session and role]")
FIRST, LAST = "2026-09-01", "2026-11-10"
check("no token -> 401", get("connections-by-school", None, **{"from": FIRST, "to": LAST})[0], 401)
check("unknown token -> 401", get("connections-by-school", "nobody", **{"from": FIRST, "to": LAST})[0], 401)
check("teacher -> 403", get("connections-by-school", "tok-teacher", **{"from": FIRST, "to": LAST})[0], 403)
check("admin-ore without ORE -> 403 (never everything)", get("connections-by-school", "tok-ore-without-ore", **{"from": FIRST, "to": LAST})[0], 403)
check("director whose school is not in the database -> 403", get("connections-by-school", "tok-director-unknown", **{"from": FIRST, "to": LAST})[0], 403)
status, body = get("connections-by-school", "nobody", **{"from": FIRST, "to": LAST})
check("401 body has the error code and a localized message", (body.get("ErrorCode"), "error" in body), ("unauthorized", True))
check("unknown query -> 404 unknown_query", (get("conexiones-por-escuela", **{"from": FIRST, "to": LAST})[0], get("nope")[1].get("ErrorCode")), (404, "unknown_query"))
status, body = get("data-status")
check("data-status: last day of connections and tests", (status, body["ConnectionsDataUntil"], body["TestsDataUntil"]),
      (200, (max(i for u, i in CONNECTIONS if counts_as_connection(u, None)) + dt.timedelta(minutes=OFFSET)).date().isoformat(),
       (max(max(r[2], r[3] or r[2]) for r in RESULTS if ROLE[r[0]] == 3 and visible_student_school_user(r[0], None)) + dt.timedelta(minutes=OFFSET)).date().isoformat()))

# ── 2. parity of the queries (admin: every school) ──────────────────────────────────────
print("\n[connections-by-school]")
for first, last, descending, limit in [("2026-01-01", LAST, True, 50), (FIRST, LAST, False, 3), ("2026-09-01", "2026-09-01", True, 50), ("2026-12-01", "2026-12-31", True, 50)]:
    status, body = get("connections-by-school", **{"from": first, "to": last, "order": "desc" if descending else "asc", "limit": limit})
    rows = [{k: v for k, v in row.items() if k not in ("PercentageOfTotal", "ConnectionsPerUser")} for row in body["Results"]]
    check(f"{first}..{last} {'desc' if descending else 'asc'} limit {limit}", rows, expected_connections_by_school(first, last, descending, limit, None))
status, body = get("connections-by-school", **{"from": "2026-01-01", "to": LAST, "limit": 50})
total = sum(r["Connections"] for r in body["Results"])
check("summary TotalConnections equals the sum of every school", body["Summary"]["TotalConnections"], total)
check("percentages are over the total", [r["PercentageOfTotal"] for r in body["Results"][:3]], [round(100.0 * r["Connections"] / total, 1) for r in body["Results"][:3]])

print("\n[connections-by-teacher]")
for first, last, descending, limit in [("2026-01-01", LAST, True, 50), (FIRST, LAST, False, 7)]:
    status, body = get("connections-by-teacher", **{"from": first, "to": last, "order": "desc" if descending else "asc", "limit": limit})
    rows = [{k: v for k, v in row.items() if k != "PercentageOfTotal"} for row in body["Results"]]
    check(f"{first}..{last} {'desc' if descending else 'asc'} limit {limit}", rows, expected_connections_by_teacher(first, last, descending, limit, None))
status, body = get("connections-by-teacher", **{"from": "2026-01-01", "to": LAST, "school": "Escuela Prueba A", "limit": 50})
check("filtered by school", [{k: v for k, v in row.items() if k != "PercentageOfTotal"} for row in body["Results"]],
      expected_connections_by_teacher("2026-01-01", LAST, True, 50, None, "Escuela Prueba A"))

print("\n[subject-area-usage]")
for first, last, descending, limit in [(FIRST, LAST, True, 50), (FIRST, LAST, False, 2), ("2026-12-01", "2026-12-31", True, 50)]:
    status, body = get("subject-area-usage", **{"from": first, "to": last, "order": "desc" if descending else "asc", "limit": limit})
    rows = [{k: v for k, v in row.items() if k != "PercentageOfTotalCompleted"} for row in body["Results"]]
    check(f"{first}..{last} {'desc' if descending else 'asc'} limit {limit}", rows, expected_subject_area_usage(first, last, descending, limit, None))
status, body = get("subject-area-usage", **{"from": "2026-01-01", "to": LAST, "limit": 50})
check("completed never exceeds assigned", all(r["CompletedTests"] <= r["AssignedTests"] for r in body["Results"]), True)
check("the data-until notice appears when the period passes the last test", "2026-10-19" in " ".join(get("subject-area-usage", **{"from": FIRST, "to": LAST})[1]["Notices"]), True)
check("no notice in connections when the data reaches the end of the period", get("connections-by-school", **{"from": FIRST, "to": "2026-11-10"})[1]["Notices"], [])

print("\n[connections-trend / coherence]")
for first, last, granularity in [("2026-01-01", "2026-11-10", "month"), ("2026-01-01", "2026-12-31", "quarter"), ("2025-12-01", "2026-12-31", "year")]:
    status, body = get("connections-trend", **{"from": first, "to": last, "granularity": granularity})
    check(f"{first}..{last} by {granularity}", [[r["Period"], r["Connections"]] for r in body["Results"]], expected_trend(first, last, granularity, None))
_, by_school = get("connections-by-school", **{"from": "2026-07-01", "to": "2026-12-31", "limit": 50})
_, by_quarter = get("connections-trend", **{"from": "2026-07-01", "to": "2026-12-31", "granularity": "quarter"})
_, by_month = get("connections-trend", **{"from": "2026-07-01", "to": "2026-12-31", "granularity": "month"})
sums = (sum(r["Connections"] for r in by_school["Results"]), sum(r["Connections"] for r in by_quarter["Results"]), sum(r["Connections"] for r in by_month["Results"]))
check("same total by school, by quarter and by month", sums[0] == sums[1] == sums[2] > 0, True)
_, cumulative = get("cumulative-connections", **{"from": "2026-01-01", "to": "2026-11-10", "granularity": "month"})
check("cumulative: the last running total is the total of the trend", cumulative["Results"][-1]["CumulativeConnections"], sum(c for _, c in expected_trend("2026-01-01", "2026-11-10", "month", None)))

print("\n[users-by-subscription / school-participation / compare-schools]")
_, body = get("users-by-subscription")
by_status = {r["Name"]: r["Users"] for r in body["Results"]}
check("total grouping: active, canceled, without subscription (inactive omitted when 0)",
      [by_status.get("Active"), by_status.get("Inactive", 0), by_status.get("Canceled"), by_status.get("Without subscription")] if "Active" in by_status else by_status,
      expected_subscription_total(None))
_, body = get("school-participation", **{"from": FIRST, "to": LAST, "limit": 50})
check("participation: active and connected users per school", sorted((r["Name"], r["ActiveUsers"], r["ConnectedUsers"]) for r in body["Results"]), expected_participation(FIRST, LAST, None))
_, body = get("compare-schools", **{"fromA": "2026-09-01", "toA": "2026-09-30", "fromB": "2026-10-01", "toB": "2026-10-31", "limit": 50})
check("compare-schools: September vs October", [(r["Name"], r["ConnectionsA"], r["ConnectionsB"]) for r in body["Results"]],
      expected_compare_schools("2026-09-01", "2026-09-30", "2026-10-01", "2026-10-31", None))
_, body = get("search-schools", text="prueba a")
check("search-schools finds by part of the name, without case or accents", [r["Name"] for r in body["Results"]], ["Escuela Prueba A"])
check("search-schools hides the test schools", get("search-schools", text="facilitadores")[1]["Results"], [])

print("\n[other queries answer with the contract fields]")
for query, params in [("teacher-activity", {"date": "2026-09-20"}), ("connection-patterns", {"from": FIRST, "to": LAST}), ("full-quiz-average", {"from": FIRST, "to": LAST}),
                      ("compare-subject-areas", {"fromA": "2026-09-01", "toA": "2026-09-30", "fromB": "2026-10-01", "toB": "2026-10-31"})]:
    status, body = get(query, **params)
    check(f"{query}: 200 with the references to cite", (status, sorted(k for k in ("Query", "Source", "DateColumn", "Period", "Filters", "Scope", "Results", "Note", "TotalRows", "GeneratedAt", "HasMoreRows", "Notices") if k in body)),
          (200, sorted(["Query", "Source", "DateColumn", "Period", "Filters", "Scope", "Results", "Note", "TotalRows", "GeneratedAt", "HasMoreRows", "Notices"])))
_, patterns = get("connection-patterns", **{"from": FIRST, "to": LAST})
check("connection-patterns: seven weekdays Monday to Sunday", [r["Name"] for r in patterns["Results"]], ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"])
_, patterns_es = request(f"{INSIGHTS}/connection-patterns?from={FIRST}&to={LAST}", "tok-admin")[0], None
status, _, spanish = request(f"{INSIGHTS}/connection-patterns?from={FIRST}&to={LAST}", "tok-admin")
req = urllib.request.Request(f"{INSIGHTS}/connection-patterns?from={FIRST}&to={LAST}", headers={"Token": "tok-admin", "Accept-Language": "es"})
with urllib.request.urlopen(req, timeout=30) as response:
    spanish = json.loads(response.read())
check("Accept-Language es localizes the weekday names, the scope and the notes", (spanish["Results"][0]["Name"], spanish["Scope"]), ("lunes", "todas las escuelas"))

# ── 3. scope ────────────────────────────────────────────────────────────────────────────
print("\n[scope comes from the session and is applied in the SQL]")
for token, scope in SCOPES.items():
    label = token
    _, body = get("connections-by-school", token, **{"from": "2026-01-01", "to": LAST, "limit": 50})
    check(f"{label}: connections by school", [{k: v for k, v in r.items() if k not in ("PercentageOfTotal", "ConnectionsPerUser")} for r in body["Results"]],
          expected_connections_by_school("2026-01-01", LAST, True, 50, scope))
    _, body = get("connections-by-teacher", token, **{"from": "2026-01-01", "to": LAST, "limit": 50})
    check(f"{label}: connections by teacher", [{k: v for k, v in r.items() if k != "PercentageOfTotal"} for r in body["Results"]], expected_connections_by_teacher("2026-01-01", LAST, True, 50, scope))
    _, body = get("subject-area-usage", token, **{"from": "2026-01-01", "to": LAST, "limit": 50})
    check(f"{label}: subject area usage", [{k: v for k, v in r.items() if k != "PercentageOfTotalCompleted"} for r in body["Results"]], expected_subject_area_usage("2026-01-01", LAST, True, 50, scope))
    _, body = get("connections-trend", token, **{"from": "2026-01-01", "to": "2026-11-10", "granularity": "month"})
    check(f"{label}: trend by month", [[r["Period"], r["Connections"]] for r in body["Results"]], expected_trend("2026-01-01", "2026-11-10", "month", scope))
    _, body = get("users-by-subscription", token)
    names = {r["Name"]: r["Users"] for r in body["Results"]}
    check(f"{label}: users by subscription", [names.get("Active", 0), names.get("Inactive", 0), names.get("Canceled", 0), names.get("Without subscription", 0)], expected_subscription_total(scope))
    _, body = get("school-participation", token, **{"from": FIRST, "to": LAST, "limit": 50})
    check(f"{label}: participation", sorted((r["Name"], r["ActiveUsers"], r["ConnectedUsers"]) for r in body["Results"]), expected_participation(FIRST, LAST, scope))
_, body = get("connections-by-school", "tok-ore2", **{"from": FIRST, "to": LAST})
check("ORE 2 declares its scope", body["Scope"], "ORE " + dp.ORE[13])
_, body = get("connections-by-school", "tok-director-1", **{"from": FIRST, "to": LAST})
check("the director declares the school", body["Scope"], "school " + SCHOOL_NAME[1])
status_out, body_out = get("connections-by-school", "tok-ore2", **{"from": FIRST, "to": LAST, "school": "Escuela Prueba A"})
status_missing, body_missing = get("connections-by-school", "tok-ore2", **{"from": FIRST, "to": LAST, "school": "Escuela Que No Existe"})
check("a school out of scope answers like a school that does not exist", (status_out, status_missing, body_out["ErrorCode"] == body_missing["ErrorCode"]), (400, 400, True))
check("parameters that try to widen the scope are ignored", [r["Name"] for r in get("connections-by-school", "tok-ore2", **{"from": FIRST, "to": LAST, "ore": 1, "oreId": 1, "scope": "all"})[1]["Results"]],
      [r["Name"] for r in get("connections-by-school", "tok-ore2", **{"from": FIRST, "to": LAST})[1]["Results"]])
check("ORE 2 never sees a school of ORE 1", set(r["Name"] for r in get("connections-by-school", "tok-ore2", **{"from": FIRST, "to": LAST, "limit": 50})[1]["Results"]) <= {SCHOOL_NAME[s] for s in dp.ESCUELAS_ORE2}, True)

# ── 4. invalid parameters ───────────────────────────────────────────────────────────────
print("\n[invalid parameters -> 400 with a code and a message]")
for label, params in [("missing to", {"from": FIRST}), ("bad date", {"from": "2026-13-01", "to": LAST}), ("from after to", {"from": LAST, "to": FIRST}),
                      ("bad order", {"from": FIRST, "to": LAST, "order": "sideways"}), ("limit too big", {"from": FIRST, "to": LAST, "limit": 51}),
                      ("old spanish granularity", {"from": FIRST, "to": LAST, "granularity": "mes"}),
                      ("hostile school", {"from": FIRST, "to": LAST, "school": "x' OR '1'='1"})]:
    query = "connections-trend" if "granularity" in params else "connections-by-school"
    status, body = get(query, **params)
    check(f"{label}", (status, body.get("ErrorCode"), isinstance(body.get("error"), str)), (400, "invalid_parameters", True))
check("hostile search text is data, not SQL", get("search-schools", text="'; DROP TABLE school; --")[1]["Results"], [])
check("the schools are still there after the hostile inputs", len(get("connections-by-school", **{"from": FIRST, "to": LAST, "limit": 50})[1]["Results"]) > 0, True)

# ── 5. exports ──────────────────────────────────────────────────────────────────────────
print("\n[export: Excel and chart]")
status, headers, content = request(f"{INSIGHTS}/connections-by-school/export?format=xlsx&from=2026-01-01&to={LAST}", "tok-admin", binary=True)
sheets, formulas = leer_xlsx.leer(content, con_formulas=True) if status == 200 else ({}, -1)
check("xlsx: file with the Data and References sheets and no formulas", (status, sorted(sheets), formulas), (200, ["Data", "References"], 0))
check("xlsx: content type and file name", (headers.get("Content-Type"), "connections-by-school_2026-01-01_2026-11-10.xlsx" in headers.get("Content-Disposition", "")),
      ("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", True))
_, full = get("connections-by-school", **{"from": "2026-01-01", "to": LAST, "limit": 50})
check("xlsx: one row per school plus the header", len(sheets.get("Data", [])), len(full["Results"]) + 1)
check("xlsx: the same figures as the JSON", [int(float(r[1])) for r in sheets["Data"][1:]], [r["Connections"] for r in full["Results"]])
status, _, content = request(f"{INSIGHTS}/connections-by-school/export?format=chart&from=2026-01-01&to={LAST}", "tok-admin", binary=True)
chart = json.loads(content) if status == 200 else {}
non_zero = [r["Name"] for r in full["Results"] if r["Connections"] > 0][:10]
check("chart: bar chart of the schools with connections (zeros left out, 10 in the preview)", (status, chart.get("Type"), [p["Category"] for p in chart.get("Data", [])]), (200, "bar", non_zero))
check("chart: counts the zero categories", chart.get("ZeroCategories"), sum(1 for r in full["Results"] if r["Connections"] == 0))
check("export of the school search -> 404", request(f"{INSIGHTS}/search-schools/export?format=xlsx&text=prueba", "tok-admin", binary=True)[0], 404)
check("export with an unknown format -> 400", request(f"{INSIGHTS}/connections-by-school/export?format=pdf&from={FIRST}&to={LAST}", "tok-admin", binary=True)[0], 400)
check("export without a token -> 401", request(f"{INSIGHTS}/connections-by-school/export?format=xlsx&from={FIRST}&to={LAST}", None, binary=True)[0], 401)
check("a chart with fewer than two categories -> 422 chart_unavailable", (lambda s, b: (s, b.get("ErrorCode")))(*get("connections-by-school/export", "tok-director-1", format="chart", **{"from": FIRST, "to": LAST})), (422, "chart_unavailable"))

# ── 6. data assistant ───────────────────────────────────────────────────────────────────
print("\n[data assistant with a fake Chat Completions model]")


def ask(question, token="tok-admin", conversation=None, language="en"):
    body = {"Question": question, "ConversationId": conversation, "Context": {"Report": "Usage", "Page": "Connections"}, "Language": language}
    status, _, answer = request(f"{ASSISTANT}/ask", token, body)
    return status, answer


status, answer = ask("TREND please, and an Excel")
check("ask: 200 with the contract fields", (status, sorted(answer)), (200, sorted(["Answer", "ConversationId", "References", "Notices", "Warnings", "ExecutedQueries", "ErrorCode", "Downloads", "Charts"])))
check("ask: the answer and no error code", (answer["Answer"], answer["ErrorCode"], answer["ConversationId"].startswith("c_")), ("Done.", None, True))
check("ask: the executed query is the tool of the model with its status", [(q["Tool"], q["Status"]) for q in answer["ExecutedQueries"]], [("get_connections_trend", 200)])
check("ask: references copied from the data API", [(r["Query"], r["Scope"]) for r in answer["References"]], [("connections-trend", "all schools")])
check("ask: a download and a line chart with the same id", (len(answer["Downloads"]), answer["Charts"][0]["Type"], answer["Charts"][0]["Id"] == answer["Downloads"][0]["Id"]), (1, "line", True))
check("ask: the question asked for a file, so the download is highlighted", answer["Downloads"][0]["Highlighted"], True)
check("ask: no unverified figures", answer["Warnings"], [])
download = answer["Downloads"][0]
status, headers, content = request(BASE + download["Url"], "tok-admin", binary=True)
check("download: the owner gets an .xlsx file", (status, content[:2], download["FileName"].endswith(".xlsx") and download["FileName"] in headers.get("Content-Disposition", "")), (200, b"PK", True))
check("download: another person gets the same 404 as an unknown id", (request(BASE + download["Url"], "tok-ore1", binary=True)[0], request(ASSISTANT + "/downloads/d_unknown", "tok-admin", binary=True)[0]), (404, 404))
status, _, body = request(BASE + download["Url"], "tok-ore1")
check("download: 404 body says download_expired", (body.get("ErrorCode")), "download_expired")
check("download without a token -> 401", request(BASE + download["Url"], None, binary=True)[0], 401)
status, _, expanded = request(BASE + answer["Charts"][0]["Url"] + "?all=true", "tok-admin")
check("expanded chart: same id, owner only", (status, expanded.get("Id") == download["Id"], request(BASE + answer["Charts"][0]["Url"], "tok-ore1", binary=True)[0]), (200, True, 404))

status, answer = ask("SUBSCRIPTION in English")
check("ask: the total subscription chart is a donut", (answer["Charts"][0]["Type"], answer["Charts"][0].get("Variant")), ("pie", "donut"))
status, answer = ask("DUP the same query twice")
check("ask: the same query twice gives two references and one download", (len(answer["References"]), len(answer["Downloads"])), (2, 1))
status, answer = ask("SEARCH a school")
check("ask: the school search has no download and no chart", (answer["Downloads"], answer["Charts"]), (None, None))
status, answer = ask("INVENT a figure")
check("ask: a figure that no query returned is flagged", (len(answer["Warnings"]), "987654" in answer["Warnings"][0]), (1, True))

status, answer = ask("OTHER_SCHOOL", "tok-ore1")
tool = answer["ExecutedQueries"][0]
check("scope: forged scope arguments of the model are dropped", (tool["Status"], sorted(tool["Arguments"])), (200, ["from", "limit", "school", "to"]))
check("scope: ORE 1 gets the school of ORE 1", answer["References"][0]["Scope"], "ORE " + dp.ORE[1])
status, answer = ask("OTHER_SCHOOL", "tok-ore2")
check("scope: the model asks for a school of another ORE and the data API says it does not exist", (answer["ExecutedQueries"][0]["Status"], answer["References"], answer["Downloads"]), (400, [], None))
check("scope: the monolith never sent the token of the person to the model", ("tok-" in json.dumps(request(MODEL + "/last")[2])), False)

status, first = ask("First question TREND")
status, second = ask("And the next one?", conversation=first["ConversationId"])
last_request = request(MODEL + "/last")[2]
check("conversation: the id is kept and the history reaches the model", (second["ConversationId"] == first["ConversationId"], len(last_request["messages"]) > 4), (True, True))
status, other = ask("Hello", "tok-ore1", conversation=first["ConversationId"])
check("conversation: the id of another person starts a new conversation", other["ConversationId"] != first["ConversationId"], True)
sent = request(MODEL + "/last")[2]
check("model request: Azure route with the deployment, api-key header, 13 tools and the instructions with today's date",
      (sent["path"].startswith("/openai/deployments/fake-deployment/chat/completions?api-version="), sent["headers"]["api-key"] is not None, len(sent["tools"]),
       "Today is " in sent["messages"][0]["content"]), (True, True, 13, True))
last_user_message = next(m["content"] for m in reversed(sent["messages"]) if m["role"] == "user")
check("model request: the question carries the panel language and the dashboard context", ("[Panel language: English]" in last_user_message, "Usage" in last_user_message), (True, True))

status, answer = ask("FILTERED message")
check("ask: content filter -> 200 with ErrorCode content_blocked", (status, answer["ErrorCode"]), (200, "content_blocked"))
status, answer = ask("BROKEN model")
check("ask: model failure -> 502 model_unavailable", (status, answer.get("ErrorCode")), (502, "model_unavailable"))
check("ask: no token -> 401", ask("TREND", None)[0], 401)
check("ask: teacher -> 403", ask("TREND", "tok-teacher")[0], 403)
check("ask: empty question -> 400 empty_question", (lambda s, a: (s, a.get("ErrorCode")))(*ask("   ")), (400, "empty_question"))

print(f"\nSummary: {TOTAL - len(FAILURES)}/{TOTAL} checks pass.")
if FAILURES:
    print("Failed: " + "; ".join(FAILURES))
sys.exit(1 if FAILURES else 0)
