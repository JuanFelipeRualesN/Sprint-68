#!/bin/bash
# Corre el verificador completo (Fase 4: autenticación y alcance) en los tres modos de datos, contra UsersAPI y Foundry FALSOS.
# Requisitos: .NET 8, Python 3 y un MySQL 8 de pruebas con esquema_y_datos.sql cargado (mysql --default-character-set=utf8mb4 < esquema_y_datos.sql)
# y una cuenta de solo lectura (por defecto eya/eya en 127.0.0.1; EYA_MYSQL_USER / EYA_MYSQL_PASSWORD / EYA_MYSQL_HOST la cambian).
# Uso:  ./correr_pruebas_auth.sh            (compila la API, la arranca en :5101 y deja el resultado de cada modo; ~2 min)
set -uo pipefail
AQUI="$(cd "$(dirname "$0")" && pwd)"; API="$AQUI/../EyaInsightsApi"
(cd "$API" && dotnet build -v q --nologo >/dev/null) || { echo "No compila"; exit 1; }
python3 "$AQUI/usersapi_falso.py" 5199 & S1=$!; python3 "$AQUI/foundry_falso.py" 5198 & S2=$!
trap 'kill $S1 $S2 ${API_PID:-} 2>/dev/null' EXIT
arrancar() {   # $1 = modo de datos, $2 = local|plataforma, $3… = VARIABLE=valor extra para la API
  kill ${API_PID:-} 2>/dev/null; sleep 1
  local extras=("${@:3}")
  ( cd "$API"
    export ASPNETCORE_ENVIRONMENT=Development ASPNETCORE_URLS=http://127.0.0.1:5101 DOTNET_ROLL_FORWARD=Major
    export Datos__Origen=mysql EYA_MYSQL_HOST="${EYA_MYSQL_HOST:-127.0.0.1}" EYA_MYSQL_USER="${EYA_MYSQL_USER:-eya}" EYA_MYSQL_PASSWORD="${EYA_MYSQL_PASSWORD:-eya}" EYA_MYSQL_SSL=None Datos__OffsetUtcMinutos=0
    case $1 in dominio) export Datos__DominiosExcluidos=requiretechnology.com Datos__EscuelasExcluidas="Escuela Demo T3 School,Escuela Facilitadores MSJ";; incluir) export Datos__ExcluirCuentasDePrueba=false;; esac
    export Auth__Modo=$2
    if [ "$2" = plataforma ]; then
      export Auth__UsersApiUrl=http://127.0.0.1:5199 EYA_API_KEY=clave-de-servicio-de-prueba
      export Asistente__Habilitado=true Asistente__Credencial=prueba Foundry__Endpoint=http://127.0.0.1:5198/api/projects/p
      export Cors__Origenes=http://localhost:3000,https://front.ejemplo.invalid
    fi
    for v in "${extras[@]}"; do export "$v"; done        # al final: pueden reemplazar lo de arriba (p. ej. Asistente__Credencial=apikey)
    exec dotnet bin/Debug/net8.0/EyaInsightsApi.dll >/tmp/eya_api.log 2>&1 ) & API_PID=$!
  for _ in $(seq 1 40); do curl -s http://127.0.0.1:5101/salud >/dev/null && return; sleep 1; done
  echo "La API no arrancó (ver /tmp/eya_api.log)"; exit 1
}
fallo=0
# Arranque: «local» no puede escuchar fuera de loopback (ni por ASPNETCORE_HTTP_PORTS ni por Kestrel:Endpoints) y Production exige «plataforma».
revisar() { # $1 = descripción; el resto = variables de entorno; la API debe NO arrancar (salir con error)
  local d="$1"; shift
  if ( cd "$API" && env "$@" timeout 15 dotnet bin/Debug/net8.0/EyaInsightsApi.dll >/tmp/eya_arranque.log 2>&1 ); then echo "arranque ✗ $d (arrancó y no debía)"; else echo "arranque ✓ $d"; fi
}
revisar "local con ASPNETCORE_HTTP_PORTS no arranca" ASPNETCORE_ENVIRONMENT=Development ASPNETCORE_HTTP_PORTS=5110 Auth__Modo=local
revisar "local con Kestrel:Endpoints en 0.0.0.0 no arranca" ASPNETCORE_ENVIRONMENT=Development Kestrel__Endpoints__X__Url=http://0.0.0.0:5111 Auth__Modo=local
revisar "local en 0.0.0.0 no arranca" ASPNETCORE_ENVIRONMENT=Development ASPNETCORE_URLS=http://0.0.0.0:5112 Auth__Modo=local
revisar "Production con local no arranca" ASPNETCORE_ENVIRONMENT=Production ASPNETCORE_URLS=http://127.0.0.1:5113 Auth__Modo=local
revisar "Production sin Auth__UsersApiUrl no arranca" ASPNETCORE_ENVIRONMENT=Production ASPNETCORE_URLS=http://127.0.0.1:5114 Auth__Modo=plataforma
revisar "Production con Cors__Origenes=* no arranca" ASPNETCORE_ENVIRONMENT=Production ASPNETCORE_URLS=http://127.0.0.1:5115 Auth__Modo=plataforma Auth__UsersApiUrl=https://u.invalid Cors__Origenes='*'
for modo in paridad dominio incluir; do
  arrancar $modo local;       echo "local/$modo:      $(python3 "$AQUI/verificar_api_mysql.py" http://127.0.0.1:5101 0 $modo 2>&1 | tail -1)"
  arrancar $modo plataforma;  echo "plataforma/$modo: $(EYA_API_KEY=clave-de-servicio-de-prueba python3 "$AQUI/verificar_api_mysql.py" http://127.0.0.1:5101 0 $modo auth 2>&1 | tail -1)"
done

# Diagnóstico y robustez de usuarios-por-suscripcion (ajuste del 19-nov, base real):
#  · si la columna de cancelación no existe, la consulta sigue y activas_con_fecha_de_cancelacion_vencida no aparece (mismos conteos);
#  · si la consulta falla, el log dice consulta, tipo, código y mensaje de MySQL, sin Token ni parámetros, y al cliente solo le llega base_no_disponible.
arrancar paridad plataforma Datos__ColCancelacionSuscripcion=columna_inexistente
base=$(curl -s -H "Token: tok-admin" "http://127.0.0.1:5101/usuarios-por-suscripcion?agrupar=escuela&limite=50")
python3 - "$base" <<'PY' && echo "suscripciones: sin columna de cancelación ✓" || { echo "suscripciones: sin columna de cancelación ✗"; fallo=1; }
import json, sys
d = json.loads(sys.argv[1])
assert "error" not in d, d
assert "activas_con_fecha_de_cancelacion_vencida" not in d["resumen"], d["resumen"]
assert d["resumen"]["total_usuarios"] == sum(f["total_usuarios"] for f in d["resultados"]) > 0
PY
arrancar paridad plataforma Datos__ColEstadoSuscripcion=columna_inexistente
resp=$(curl -s -o /tmp/eya_resp.json -w "%{http_code}" -H "Token: tok-admin" "http://127.0.0.1:5101/usuarios-por-suscripcion")
if [ "$resp" = 503 ] && grep -q '"base_no_disponible"' /tmp/eya_resp.json && ! grep -q "columna_inexistente\|1054" /tmp/eya_resp.json \
   && grep -q 'consulta_fallida consulta=usuarios-por-suscripcion tipo=MySqlException mysql_codigo=1054' /tmp/eya_api.log && ! grep -q 'tok-admin' /tmp/eya_api.log; then
  echo "suscripciones: log de diagnóstico (consulta, MySqlException, código 1054; sin Token; al cliente solo el error genérico) ✓"
else
  echo "suscripciones: log de diagnóstico ✗ (HTTP $resp)"; grep -m2 "consulta_fallida" /tmp/eya_api.log; fallo=1
fi

# ── Asistente:Credencial=apikey (Foundry con API key; el Foundry falso devuelve las cabeceras que recibió) ──────────────────────────────────
CLAVE=clave-foundry-de-prueba-7f3a9
cabeceras() { python3 - "$1" <<'PY'
import json, sys, urllib.request
print(json.dumps(json.loads(urllib.request.urlopen(sys.argv[1] + "/instrucciones", timeout=10).read())["cabeceras"]))
PY
}
preguntar() { curl -s -o /tmp/eya_resp.json -w "%{http_code}" -H "Token: tok-ore2" -H "Content-Type: application/json" -d '{"pregunta":"hola"}' http://127.0.0.1:5101/asistente/preguntar; }
arrancar paridad plataforma Asistente__Credencial=apikey Foundry__ApiKey=$CLAVE Foundry__EndpointRecurso=http://127.0.0.1:5198
resp=$(preguntar); cab=$(cabeceras http://127.0.0.1:5198)
if [ "$resp" = 200 ] && python3 - "$cab" "$CLAVE" <<'PY'
import json, sys
c = json.loads(sys.argv[1]); assert c["api_key"] == sys.argv[2] and c["authorization"] is None and c["ruta"] == "/openai/v1/responses", c
PY
then echo "apikey: llega api-key al endpoint del recurso (/openai/v1/responses), sin Authorization ✓"; else echo "apikey: cabeceras ✗ (HTTP $resp, $cab)"; fallo=1; fi
if ! grep -rq "$CLAVE" /tmp/eya_api.log /tmp/eya_resp.json; then echo "apikey: la clave no aparece en el log ni en la respuesta ✓"; else echo "apikey: la clave se filtró ✗"; fallo=1; fi
# clave rechazada: un Foundry falso que exige otra clave y contesta 401 repitiendo la que recibió
FOUNDRY_FALSO_CLAVE=$CLAVE python3 "$AQUI/foundry_falso.py" 5197 & S3=$!
arrancar paridad plataforma Asistente__Credencial=apikey Foundry__ApiKey=clave-mala-que-no-debe-salir-9x2 Foundry__EndpointRecurso=http://127.0.0.1:5197
resp=$(preguntar)
if [ "$resp" = 503 ] && grep -q '"sin_acceso_al_modelo"' /tmp/eya_resp.json && ! grep -rq "clave-mala-que-no-debe-salir-9x2" /tmp/eya_api.log /tmp/eya_resp.json; then
  echo "apikey: clave rechazada por Foundry (401 que repite la clave) → 503 sin_acceso_al_modelo y la clave no sale en el log ni en la respuesta ✓"
else echo "apikey: error de Foundry ✗ (HTTP $resp)"; cat /tmp/eya_resp.json; fallo=1; fi
arrancar paridad plataforma Asistente__Credencial=apikey Foundry__ApiKey=$CLAVE Foundry__EndpointRecurso=http://127.0.0.1:5197
resp=$(preguntar)
[ "$resp" = 200 ] && echo "apikey: con la clave correcta el Foundry que la exige responde ✓" || { echo "apikey: clave correcta ✗ (HTTP $resp)"; fallo=1; }
kill $S3 2>/dev/null
# los otros modos siguen igual: Bearer y SIN api-key (el modo «prueba» usa el mismo camino que managed/azcli)
arrancar paridad plataforma
resp=$(preguntar); cab=$(cabeceras http://127.0.0.1:5198)
if [ "$resp" = 200 ] && python3 - "$cab" <<'PY'
import json, sys
c = json.loads(sys.argv[1]); assert c["api_key"] is None and (c["authorization"] or "").startswith("Bearer ") and c["ruta"] == "/api/projects/p/openai/v1/responses", c
PY
then echo "credencial prueba/managed/azcli: Bearer, sin api-key y endpoint del proyecto (sin cambios) ✓"; else echo "otros modos ✗ (HTTP $resp, $cab)"; fallo=1; fi
revisar_falla() { # $1 = descripción, $2 = texto que debe decir el motivo; el resto = variables: no debe arrancar y debe explicar por qué
  local d="$1" t="$2"; shift 2
  if ( cd "$API" && env "$@" timeout 15 dotnet bin/Debug/net8.0/EyaInsightsApi.dll >/tmp/eya_arranque.log 2>&1 ); then echo "arranque ✗ $d (arrancó y no debía)"; fallo=1
  elif grep -q "$t" /tmp/eya_arranque.log; then echo "arranque ✓ $d (dice por qué)"; else echo "arranque ✗ $d (no explica el motivo)"; fallo=1; fi
}
BASE_PROD="ASPNETCORE_ENVIRONMENT=Production Auth__Modo=plataforma Auth__UsersApiUrl=https://u.invalid Cors__Origenes=https://front.ejemplo.invalid Asistente__Habilitado=true"
revisar_falla "apikey sin clave no arranca" "Foundry__ApiKey" ASPNETCORE_URLS=http://127.0.0.1:5116 $BASE_PROD Asistente__Credencial=apikey
revisar_falla "apikey con EndpointRecurso no https no arranca en Production" "EndpointRecurso" ASPNETCORE_URLS=http://127.0.0.1:5117 $BASE_PROD Asistente__Credencial=apikey Foundry__ApiKey=clave-prod-xyz-5521 Foundry__EndpointRecurso=http://127.0.0.1:5198
( cd "$API" && env ASPNETCORE_URLS=http://127.0.0.1:5118 $BASE_PROD Asistente__Credencial=apikey Foundry__ApiKey=clave-prod-xyz-5521 timeout 8 dotnet bin/Debug/net8.0/EyaInsightsApi.dll >/tmp/eya_arranque.log 2>&1 ); rc=$?
if [ $rc = 124 ] && ! grep -q "clave-prod-xyz-5521" /tmp/eya_arranque.log; then echo "arranque ✓ Production con apikey y clave arranca (y no escribe la clave)"; else echo "arranque ✗ Production con apikey (código $rc)"; tail -3 /tmp/eya_arranque.log; fallo=1; fi
exit $fallo
