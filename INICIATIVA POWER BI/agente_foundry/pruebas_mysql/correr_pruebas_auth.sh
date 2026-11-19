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
arrancar() {   # $1 = modo de datos, $2 = local|plataforma
  kill ${API_PID:-} 2>/dev/null; sleep 1
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
