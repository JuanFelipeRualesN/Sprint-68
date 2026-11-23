#!/bin/bash
# Black-box checks of the monolith gs-base-api (REQ-3741 · B13) against a FAKE UsersAPI and a FAKE Azure OpenAI.
# Requirements: .NET 8 SDK, Python 3 and a MySQL 8 with the synthetic data loaded:
#   python3 -c "import sys; sys.path.insert(0, '../pruebas_mysql'); import datos_prueba as d; print(d.a_sql(*d.generar()))" | mysql --default-character-set=utf8mb4
# (it creates the gs_data_analysis database) plus an AI services database migrated by EF (the tests of AIServicesAPI.Tests create one).
# Environment variables (all optional):
#   GS_BASE_API  path of the gs-base-api checkout (default ../repos/gs-base-api)
#   MYSQL_HOST / MYSQL_PORT / MYSQL_USER / MYSQL_PASSWORD   (default 127.0.0.1 / 3306 / tester / test-password)
#   AI_DB        AI services database name (default gs_ai_services_test)
# Usage:  ./run_monolith_checks.sh      (builds the monolith, starts it on :5101 and prints the summary)
set -uo pipefail
HERE="$(cd "$(dirname "$0")" && pwd)"
GS_BASE_API="${GS_BASE_API:-$HERE/../repos/gs-base-api}"
HOST="${MYSQL_HOST:-127.0.0.1}"; PORT="${MYSQL_PORT:-3306}"; DBUSER="${MYSQL_USER:-tester}"; DBPASS="${MYSQL_PASSWORD:-test-password}"; AI_DB="${AI_DB:-gs_ai_services_test}"
URL=http://127.0.0.1:5101
LOG="${TMPDIR:-/tmp}/monolith_checks.log"

(cd "$GS_BASE_API/GSBaseAPI" && dotnet build -v q --nologo >/dev/null) || { echo "The monolith does not build"; exit 1; }
python3 "$HERE/fake_users_api.py" 5199 & USERS_PID=$!
python3 "$HERE/fake_model.py" 5198 & MODEL_PID=$!
trap 'kill $USERS_PID $MODEL_PID ${API_PID:-} 2>/dev/null' EXIT

start_monolith() {   # $1... = extra VARIABLE=value for the monolith
  kill ${API_PID:-} 2>/dev/null; sleep 1
  ( cd "$GS_BASE_API/GSBaseAPI/bin/Debug/net8.0"   # the content root must hold the appsettings*.json files that the build copies there
    export ASPNETCORE_ENVIRONMENT=Development ASPNETCORE_URLS=$URL DOTNET_ROLL_FORWARD=Major
    # Nothing of the repository configuration is used for real services: every endpoint, connection and key below is local and fake.
    export ConnectionStrings__AnalysisConnectionStringReader="Server=$HOST;Port=$PORT;User ID=$DBUSER;Password=$DBPASS;Database=gs_data_analysis"
    export ConnectionStrings__AIServicesAPIDBConnection="Server=$HOST;Port=$PORT;User ID=$DBUSER;Password=$DBPASS;Database=$AI_DB"
    export AppSettings__gus_web_service_url=http://127.0.0.1:5199/ AppSettings__gus_endpoint_web_service_url=http://127.0.0.1:5199/
    export AppSettings__gus_endpoint_permission_session_api=usersapi/accounts/get_user_by_token
    export AppSettings__development_mode_session_off=0 AppSettings__cache_on_off=0
    export AppSettings__data_analysis_api_url=$URL AppSettings__azure_openai_deployment=fake-deployment AppSettings__ai_data_assistant_deployment_name=
    export AppSettings__usage_insights_excluded_email_domains= AppSettings__usage_insights_excluded_schools="Escuela Facilitadores MSJ" AppSettings__usage_insights_utc_offset_minutes=0
    export AzureOpenAI__Endpoint=http://127.0.0.1:5198 AzureOpenAI__ApiKey=fake-api-key-for-local-checks
    export MongoDb__ConnectionString="mongodb://127.0.0.1:27999/?serverSelectionTimeoutMS=300&connectTimeoutMS=300" MongoDb__DatabaseName=local-checks
    for variable in "$@"; do export "$variable"; done
    exec dotnet GSBaseAPI.dll >"$LOG" 2>&1 ) & API_PID=$!
  for _ in $(seq 1 60); do curl -s -H "Token: tok-admin" "$URL/dataanalysisapi/usage-insights/data-status" >/dev/null && return; sleep 1; done
  echo "The monolith did not start (see $LOG)"; exit 1
}
start_monolith
python3 "$HERE/verify_monolith.py" "$URL" 0
STATUS=$?
# The token must never reach the log of the monolith.
if grep -q "tok-admin\|tok-ore" "$LOG"; then echo "  ✗ a session token appears in the log of the monolith"; STATUS=1; else echo "  ✓ no session token in the log of the monolith"; fi
exit $STATUS
