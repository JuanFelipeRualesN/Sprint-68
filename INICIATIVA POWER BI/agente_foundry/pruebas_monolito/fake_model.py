"""Fake Azure OpenAI (Chat Completions with tool calling) to test the data assistant of the monolith without a real model (REQ-3741 · B13).
Standard library only.

It imitates POST /openai/deployments/{deployment}/chat/completions: according to a keyword of the last user message it asks for a tool
(tool_calls) and, when it receives the result of the tool, it answers "Done." with token usage. It does NOT test the quality of the model.

Keywords of the question:
  OTHER_SCHOOL  -> connections-by-teacher of "Escuela Prueba A" (a school of ORE 1) plus forged scope arguments (ore, scope), to prove they are dropped
  TREND         -> get_connections_trend by month
  DUP           -> get_connections_by_teacher TWICE with the same arguments (one download only)
  SUBSCRIPTION  -> get_users_by_subscription (total)
  SEARCH        -> search_schools
  FILTERED      -> answers 400 with the content filter body of Azure
  BROKEN        -> answers 500
  INVENT        -> asks for a tool and then answers with a figure that no query returned
  (other)       -> get_connections_by_teacher of every school, limit 5
GET /last returns the last request (messages, tools, headers) to check what the monolith sends.

Usage:  python3 fake_model.py [port]    (default 5198)
"""
import json
import sys
import uuid
from http.server import BaseHTTPRequestHandler, ThreadingHTTPServer

PERIOD = {"from": "2026-01-01", "to": "2026-11-10"}
LAST = {"messages": [], "tools": [], "headers": {}, "path": ""}


def calls_for(question):
    if "SUBSCRIPTION" in question:
        return [("get_users_by_subscription", {})]
    if "SEARCH" in question:
        return [("search_schools", {"text": "Prueba"})]
    if "OTHER_SCHOOL" in question:
        return [("get_connections_by_teacher", {**PERIOD, "school": "Escuela Prueba A", "limit": 5, "ore": "1", "oreId": 1, "scope": "all"})]
    if "TREND" in question:
        return [("get_connections_trend", {**PERIOD, "granularity": "month"})]
    if "DUP" in question:
        call = ("get_connections_by_teacher", {**PERIOD, "limit": 5})
        return [call, call]
    return [("get_connections_by_teacher", {**PERIOD, "limit": 5})]


def usage(prompt=100, completion=20):
    return {"prompt_tokens": prompt, "completion_tokens": completion, "total_tokens": prompt + completion}


class Handler(BaseHTTPRequestHandler):
    def _send(self, status, body):
        data = json.dumps(body).encode()
        self.send_response(status)
        self.send_header("Content-Type", "application/json")
        self.send_header("Content-Length", str(len(data)))
        self.end_headers()
        self.wfile.write(data)

    def do_GET(self):
        self._send(200, LAST)

    def do_POST(self):
        length = int(self.headers.get("Content-Length") or 0)
        request = json.loads(self.rfile.read(length) or b"{}")
        messages = request.get("messages", [])
        LAST.update(messages=messages, tools=request.get("tools", []), path=self.path,
                    headers={"api-key": self.headers.get("api-key"), "authorization": self.headers.get("Authorization")})
        question = next((m["content"] for m in reversed(messages) if m["role"] == "user"), "")
        if "FILTERED" in question:
            return self._send(400, {"error": {"code": "content_filter", "message": "The response was filtered due to the prompt triggering the content management policy."}})
        if "BROKEN" in question:
            return self._send(500, {"error": {"message": "internal error"}})
        last_role = messages[-1]["role"] if messages else "user"
        if last_role == "tool":
            text = "There were 987654 connections." if "INVENT" in question else "Done."
            message = {"role": "assistant", "content": text}
            finish = "stop"
        else:
            tool_calls = [{"id": f"call_{i}", "type": "function", "function": {"name": name, "arguments": json.dumps(args)}}
                          for i, (name, args) in enumerate(calls_for(question if "INVENT" not in question else "TREND"))]
            message = {"role": "assistant", "content": None, "tool_calls": tool_calls}
            finish = "tool_calls"
        self._send(200, {"id": "chatcmpl-" + uuid.uuid4().hex, "object": "chat.completion", "created": 1, "model": request.get("model", "fake"),
                         "choices": [{"index": 0, "finish_reason": finish, "message": message}], "usage": usage()})

    def log_message(self, *args):
        pass


if __name__ == "__main__":
    ThreadingHTTPServer(("127.0.0.1", int(sys.argv[1]) if len(sys.argv) > 1 else 5198), Handler).serve_forever()
