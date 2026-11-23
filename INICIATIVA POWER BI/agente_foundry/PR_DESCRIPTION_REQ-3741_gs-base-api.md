# REQ-3741: Power BI data assistant (usage insights + assistant) — gs-base-api

Base: `main` (ac9c9d0). Target: `uat`. Branch `req-3741`, 20 commits, one or more per task (`REQ-3741 Bn: ...`).

## Summary

The MVP (`EyaInsightsApi`) moves into the monolith with no auth, hosting or CORS of its own:

* **DataAnalysisAPI** owns the data: 13 read-only usage queries (Dapper over `gs_data_analysis`), the data scope, Excel (NPOI) and chart export.
  Routes `dataanalysisapi/usage-insights/{query}`, `.../{query}/export?format=xlsx|chart`, `.../data-status`.
* **AIServicesAPI** owns the assistant: model client (`AzureOpenAIClient`), tool loop (max 6 steps), conversations, downloads, figure verifier.
  Routes `aiservicesapi/data-assistant/ask`, `downloads/{id}`, `charts/{id}`. `ask` has `[RequiresAiEnabled]`, `TextTokens` quota and prompt monitoring (`Subject = "data-assistant"`).
* They talk over internal HTTP (`DataAnalysisFeignAPI`, existing Feign pattern) **forwarding the user's `Token`**: DataAnalysisAPI validates the session again and applies the scope
  (all schools / ORE / school) inside the SQL. The scope is not an argument of any tool, so the model cannot widen it.
* Permission `embed-powerbi-info`. Roles: `usage_insights_*_role_ids` for the scope (2 / 8 / 6) and `data_assistant_allowed_role_ids` (`2,8,6`) for the assistant.
* No secrets added. Notes, errors, titles and Excel labels are localized (`Messages:{key}:{es|en}`); the code, routes, JSON fields, config keys and logs are English.

## Changes per task

| Task | What |
|---|---|
| B0 | `AWSSDK.Core 4.0.0.26` direct reference in `GSBaseAPI.csproj`: **`main` did not restore** (NU1107: MongoDB.Driver.Core needs AWSSDK.Core <4, AWSSDK.S3 needs >=4.0.0.26). The fix is the one NuGet itself suggests. |
| B1 | Domain models (`DataAnalysisAPI.Domain/Models/UsageInsights`): rows, `UsageInsightResponse` (`HasMoreRows`, `Notices`), `DataScope`, `TestAccountExclusion`, `ChartOptions`, `InvalidQueryParameterException`, `UsageInsightsQueryContext`. |
| B2 | `DataScopeResolver` (fails closed) + `UsageInsightsSettings`; new non-secret keys in `appsettings.json`. |
| B3a/b | Dapper queries (`Infrastructure/Dapper/UsageInsights.cs`): same SQL as the MVP, scope and test-account exclusion in the SQL, every value a parameter, sessions opened in `READ ONLY` and UTC (refuses to run otherwise). |
| B4 | The 13 queries (`Services/UsageInsights.cs`), `UsageInsightsQueryParser`, localized messages (`DataAnalysisAPI/appsettings.dataanalysis.json`, loaded in `Program.cs`). Nothing is detected by text: `Notices[]`, `HasMoreRows`, `Summary.Ties`. |
| B5 | `UsageInsightsController` (one parametrized action serves the 13 queries so they share validation) and `data-status`. |
| B6 | Excel (NPOI, sheets *Data* and *References*), `ChartSpecificationBuilder`, `UsageInsightsColumnFormatter`, export route. |
| B7 | `DataAnalysisAPI.Tests` (xUnit, real MySQL): 168 tests. |
| B8 | `OreId` in the AI `ValidationResponseModel`; `IDataAnalysisFeignAPI`/`DataAnalysisFeignAPI` (token and language forwarded, never logged). |
| B9 | `tool-definitions.json` (13 tools), English system instructions, `DataAssistantToolCatalog`. |
| B10 | `DataAssistantService`, `IDataAssistantChatClient` + Azure adapter, conversation store (`IMemoryCache`, 2 h, 40-message cap, bound to `UserId`), download registry (2 h, 1000 entries, bound to `UserId`), `AnswerFigureVerifier`. |
| B11 | `DataAssistantController`: session, role check, empty question, quota (reserve / commit with the sum of every model call / release), monitoring, 429 / 502 / 503 / `content_blocked`, owner check on downloads and charts. |
| B12 | `AIServicesAPI.Tests` (xUnit): 93 tests. |
| B14 | New keys in every `appsettings.{env}.json` and the template, README section. |
| B13 | Not in this repo: `INICIATIVA POWER BI/agente_foundry/pruebas_monolito/` in `Sprint-68` (branch `claude/req-3741-b13-monolith-checks`). |

## Findings the reviewer should know

1. **Typed Azure OpenAI chat calls fail at runtime in this solution.** With `Azure.AI.OpenAI 2.1.0` on top of the pinned `OpenAI 2.9.1`, `AzureOpenAIClient.GetChatClient(..).CompleteChat*` (streaming or not, with or without options) throws
   `MissingMethodException: ChatCompletionOptions.get_SerializedAdditionalRawData` (reproduced with a 10-line probe). `GenerativeTutorEngine`, which is the pattern rule 5 points to, uses exactly those calls, so it is very likely affected on `main` too (not verified against Azure).
   The assistant therefore calls the **protocol method** of the same chat client with the request JSON (same Azure pipeline: deployment route, api-version, `api-key`). Tested against a fake Azure endpoint (`AzureOpenAiAdapterTests`).
   Suggested follow-up ticket: align `Azure.AI.OpenAI` / `OpenAI` versions.
2. The Responses API probe asked by the plan: `GetResponsesClient()` exists on `OpenAIClient 2.9.1`, but `AzureOpenAIClient 2.1.0` is built against an older OpenAI and nothing in the repo exercises it against an Azure OpenAI deployment; it could not be tested here, so Chat Completions is used (history kept on the server).
3. `IPromptMonitoringService.CaptureAsync` **can throw** (Mongo down: 10 s timeout, then `TimeoutException`), contrary to its description. Found by running the real monolith; the controller swallows it (logs only the exception type) so an answer already produced is not lost.
4. The Ubuntu SDK 8.0.134 fails to compile `PlanPromptBuilder.cs` (CS0121 on a collection expression); the build was done with SDK 8.0.428. Not changed.
5. `obj/*.nuget.*` files are tracked in the repository; they were not committed.

## Test results (real runs, this environment)

| Suite | Result |
|---|---|
| `dotnet build GSBaseAPI/GSBaseAPI.sln` | 0 errors, **0 new warnings** (compared unique warnings with the baseline of `main`; only line numbers of existing warnings in `Program.cs` moved) |
| `DataAnalysisAPI.Tests` | 168 / 168 (real MySQL 8.0.46: every query, scope, exclusion, boundaries of the local day, Excel read back with NPOI, charts, controller 401/403/400/404/422, messages) |
| `AIServicesAPI.Tests` | 93 / 93 (tool loop, 6-step cap, dropped arguments, token forwarded, owner checks, figure verifier, conversations, 2 h expiry, quota 429 and commit with the sum of tokens on a real MySQL with the EF migrations, AI switch 503, `content_blocked`, monitoring `Subject`, Azure adapter) |
| `ReadingAPI.Tests` (existing) | 42 / 42 |
| `GSWebAPI.Tests` (existing) | 53 / 194 — the 141 failures are Testcontainers without Docker (same on `main`) |
| Black-box (`pruebas_monolito`, real monolith + fake UsersAPI + fake Chat Completions) | 116 / 116, and the session token does not appear in the monolith log |

Not run: real Azure OpenAI, real UsersAPI, the real database, Docker/Testcontainers (a local MySQL 8.0.46 was used through `USAGE_INSIGHTS_TEST_CONNECTION_STRING` and `AI_SERVICES_TEST_CONNECTION_STRING`), the UAT battery of real questions.

**About B13 vs the MVP baseline (274/275 local, 350/351 platform):** the numbers are not comparable. The MVP verifier was dropped as a whole because most of its checks were about things the monolith no longer has:
own `x-api-key` identity, own CORS and loopback guard, `Auth:Modo`, `/salud` without session, the Foundry Responses protocol, text markers (`AVISO:`, `HAY MÁS FILAS`) that are now structured fields, Spanish route/field/sheet names.
The new verifier recomputes by brute force the same figures for the main queries (all schools, ORE 1, ORE 2, director), plus session/role, invalid parameters, export, and the assistant end to end. The compact fixture of `DataAnalysisAPI.Tests` is a hand-checkable SQL file, not the 275 KB MVP dataset (which B13 does use).

## Configuration and secrets per environment

* **Every environment file:** `AppSettings:data_analysis_api_url` (base URL of this same monolith, empty in the repo; without it every assistant tool call answers 502 `data_unavailable`) and `AppSettings:ai_data_assistant_deployment_name` (falls back to `azure_openai_deployment`).
* `appsettings.json` (non-sensitive): `data_analysis_api_timeout_seconds`, `data_assistant_allowed_role_ids`, `usage_insights_*` (role ids, test-account exclusion, subscription columns, `utc_offset_minutes` -240, timeout).
* **Key Vault:** nothing new. Reuses `AzureOpenAI--ApiKey`, `AzureOpenAI:Endpoint` and `ConnectionStrings--AnalysisConnectionStringReader`.
* The localized message files are copied to the output (`appsettings.dataanalysis.json`, `appsettings.aiservices.json`).

## Open items

* **P6 (unconfirmed):** the `AnalysisConnectionStringReader` account should only have `SELECT` on `gs_data_analysis` (the UAT user name does not look read-only). The code opens every session `READ ONLY` and refuses to work otherwise.
* **P7 (unconfirmed):** conversations and downloads are in memory (behind `IDataAssistantConversationStore` / `IDataAssistantDownloadRegistry`). With several App Service instances an id can land on another instance and give 404 / a new conversation: use ARR affinity or move the state to a shared store.
* Run the battery of real questions (`preguntas_fase1.txt`, `preguntas_trampa.txt`) in UAT with the real model before merging (English instructions and renamed tools may change the quality of Spanish answers).
* Existing plaintext secrets in `appsettings*.json` (not touched, out of scope): rotate and move them to Key Vault.
* Fate of the MVP App Service `app-eya-msj-uat-eastus-001` (P9).
* Suggested follow-up: the package versions problem of finding 1.

🤖 Generated with [Claude Code](https://claude.com/claude-code)

https://claude.ai/code/session_01TzS2k1TbKGyCooLCTRCqFe
