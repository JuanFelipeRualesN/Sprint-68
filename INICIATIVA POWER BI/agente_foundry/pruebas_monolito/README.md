# Black-box checks of the monolith (REQ-3741 · B13)

Runs the REAL `gs-base-api` monolith (usage insights in `DataAnalysisAPI`, data assistant in `AIServicesAPI`) against fake services and compares it with an
independent brute-force implementation in Python over the same synthetic data. It lives in this repository, not in `gs-base-api`.

| File | What it is |
|---|---|
| `fake_users_api.py` | Fake UsersAPI (`get_user_by_token`) with fixed tokens: admin, ORE 1, ORE 2, ORE without ORE, teacher, directors. |
| `fake_model.py` | Fake Azure OpenAI (Chat Completions with tool calling). Keywords of the question choose the tool or the failure (see its header). |
| `verify_monolith.py` | The verifier. Data comes from `../pruebas_mysql/datos_prueba.py` (the generator of the MVP harness). |
| `run_monolith_checks.sh` | Builds the monolith, starts it with local configuration only and runs the verifier. |

## Run

1. MySQL 8 with the synthetic data and an AI services database migrated by EF (`AIServicesAPI.Tests` creates one):
   `python3 -c "import sys; sys.path.insert(0, '../pruebas_mysql'); import datos_prueba as d; print(d.a_sql(*d.generar()))" | mysql --default-character-set=utf8mb4`
2. `GS_BASE_API=/path/to/gs-base-api ./run_monolith_checks.sh` (see the header of the script for the connection variables).

No real service is touched: every endpoint, connection string and key is local and fake. The session token must never reach the log of the monolith; the script checks it.

## What changed against the MVP harness (`../pruebas_mysql`)

The MVP verifier had 351 checks in platform mode and 275 in local mode, most of them about things that do not exist in the monolith (own `x-api-key` identity,
own CORS, loopback guard, `/salud` without session, Foundry Responses protocol, text markers such as `AVISO:` or `HAY MÁS FILAS`). This verifier is new and
smaller on purpose; see the table of the pull request for what was dropped and why. The figures are compared in the same way (brute force over the same data).
