"""UsersAPI FALSA para probar la autenticación de EyaInsightsApi (REQ-3741 · Fase 4 · A4). Solo biblioteca estándar.

Imita POST /usersapi/accounts/get_user_by_token de UsersAPI: 200 con {SessionValid, UserId, RoleId, OreId, ...} si el token está en la
tabla fija de abajo (o empieza por «tok-admin-», que se toma como admin: sirve para tokens nuevos); 401 si no. Cuenta las llamadas
(GET /llamadas) para comprobar la caché; POST /reiniciar las pone en 0. POST /caer simula UsersAPI caída (responde 500); POST /levantar la repone.

Uso:  python3 usersapi_falso.py [puerto]     (por defecto 5199, solo en 127.0.0.1)
      y arrancar la API con  Auth__Modo=plataforma  Auth__UsersApiUrl=http://127.0.0.1:5199
"""
import json
import sys
from http.server import BaseHTTPRequestHandler, ThreadingHTTPServer

# token → (UserId, RoleId, OreId)   (RoleId 2 admin · 5 teacher · 6 director · 8 admin-ore, como en el front)
TOKENS = {
    "tok-admin":       ("900", "2", None),
    "tok-ore1":        ("901", "8", "1"),      # admin-ore del ORE 1 (escuelas 1-3 y 7 de los datos de prueba)
    "tok-ore2":        ("902", "8", "2"),      # admin-ore del ORE 2 (escuelas nuevas de A2)
    "tok-ore-sin-ore": ("903", "8", None),     # admin-ore SIN OreId → debe dar 403 (nunca «todo»)
    "tok-teacher":     ("5", "5", None),       # user_id 5 = un maestro
    "tok-director-1":  ("1", "6", None),       # user_id 1 = director de la escuela 1
    "tok-director-x":  ("999999", "6", None),  # director cuyo user_id no está en la base → 403
}
llamadas = {"n": 0, "caida": False}


class H(BaseHTTPRequestHandler):
    def _json(self, estado, cuerpo=None):
        datos = json.dumps(cuerpo).encode() if cuerpo is not None else b""
        self.send_response(estado)
        self.send_header("Content-Type", "application/json")
        self.send_header("Content-Length", str(len(datos)))
        self.end_headers()
        self.wfile.write(datos)

    def do_GET(self):
        if self.path == "/llamadas":
            return self._json(200, {"llamadas": llamadas["n"]})
        self._json(404, {})

    def do_POST(self):
        n = int(self.headers.get("Content-Length") or 0)
        cuerpo = self.rfile.read(n) if n else b""
        if self.path == "/reiniciar":
            llamadas["n"] = 0
            return self._json(200, {"llamadas": 0})
        if self.path in ("/caer", "/levantar"):
            llamadas["caida"] = self.path == "/caer"
            return self._json(200, {"caida": llamadas["caida"]})
        if self.path != "/usersapi/accounts/get_user_by_token":
            return self._json(404, {})
        llamadas["n"] += 1
        if llamadas["caida"]:
            return self._json(500)
        try:
            d = json.loads(cuerpo)
        except ValueError:
            return self._json(400)
        if not d.get("Token") or not d.get("PermissionName") or not d.get("IpAddress"):
            return self._json(400)        # como UsersAPI: los tres campos son obligatorios
        t = TOKENS.get(d["Token"]) or (("900", "2", None) if d["Token"].startswith("tok-admin-") else None)
        if t is None:
            return self._json(401)
        uid, rol, ore = t
        self._json(200, {"IpAddress": d["IpAddress"], "SessionValid": True, "UserId": uid, "RoleId": rol, "OreId": ore,
                         "IsStaff": False, "IsDirector": rol == "6", "Messages": []})

    def log_message(self, *a):
        pass


if __name__ == "__main__":
    puerto = int(sys.argv[1]) if len(sys.argv) > 1 else 5199
    ThreadingHTTPServer(("127.0.0.1", puerto), H).serve_forever()
