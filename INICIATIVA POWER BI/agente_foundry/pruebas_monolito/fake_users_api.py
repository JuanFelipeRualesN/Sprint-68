"""Fake UsersAPI to run the black-box checks of the monolith (REQ-3741 · B13). Standard library only.

It imitates POST /usersapi/accounts/get_user_by_token: 200 with {SessionValid, UserId, RoleId, OreId, Token, ...} when the token is in the fixed table
below (a token starting with "tok-admin-" is taken as an administrator, so new tokens can be created at will); 401 otherwise.
GET /calls returns how many validations were received; POST /reset sets the counter to 0.

Usage:  python3 fake_users_api.py [port]     (default 5199, only on 127.0.0.1)
"""
import json
import sys
from http.server import BaseHTTPRequestHandler, ThreadingHTTPServer

# token -> (UserId, RoleId, OreId)   (RoleId 2 admin, 5 teacher, 6 director, 8 admin-ore, as in the front end)
TOKENS = {
    "tok-admin": ("900", "2", None),
    "tok-ore1": ("901", "8", "1"),             # admin-ore of ORE 1 (schools 1-12 of the synthetic data)
    "tok-ore2": ("902", "8", "2"),             # admin-ore of ORE 2 (schools 13 and 14)
    "tok-ore-without-ore": ("903", "8", None),  # admin-ore WITHOUT OreId: must get 403 (never "everything")
    "tok-teacher": ("5", "5", None),           # user_id 5 = a teacher
    "tok-director-1": ("1", "6", None),        # user_id 1 = director of school 1
    "tok-director-unknown": ("999999", "6", None),  # director whose user_id is not in the database: 403
}
STATE = {"calls": 0}


class Handler(BaseHTTPRequestHandler):
    def _json(self, status, body=None):
        data = json.dumps(body).encode() if body is not None else b""
        self.send_response(status)
        self.send_header("Content-Type", "application/json")
        self.send_header("Content-Length", str(len(data)))
        self.end_headers()
        self.wfile.write(data)

    def do_GET(self):
        self._json(200, {"calls": STATE["calls"]}) if self.path == "/calls" else self._json(404, {})

    def do_POST(self):
        length = int(self.headers.get("Content-Length") or 0)
        raw = self.rfile.read(length) if length else b""
        if self.path == "/reset":
            STATE["calls"] = 0
            return self._json(200, {"calls": 0})
        if self.path != "/usersapi/accounts/get_user_by_token":
            return self._json(404, {})
        STATE["calls"] += 1
        try:
            request = json.loads(raw)
        except ValueError:
            return self._json(400)
        if not request.get("Token") or not request.get("PermissionName") or not request.get("IpAddress"):
            return self._json(400)         # like UsersAPI: the three fields are mandatory
        found = TOKENS.get(request["Token"]) or (("900", "2", None) if request["Token"].startswith("tok-admin-") else None)
        if found is None:
            return self._json(401)
        user_id, role_id, ore_id = found
        self._json(200, {"IpAddress": request["IpAddress"], "SessionValid": True, "UserId": user_id, "RoleId": role_id, "OreId": ore_id,
                         "Token": request["Token"], "IsStaff": False, "IsDirector": role_id == "6", "Messages": []})

    def log_message(self, *args):
        pass


if __name__ == "__main__":
    ThreadingHTTPServer(("127.0.0.1", int(sys.argv[1]) if len(sys.argv) > 1 else 5199), Handler).serve_forever()
