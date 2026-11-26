Soft change to .Net
- Remove ConfigurationManager
- Convert API Controllers to Mvc Controllers.
- Register the controllers in Program.cs 
- Create an AppConfiguration file for each API library
- Forcing deployment.
- Forcing deployment 2.

## Data assistant and usage insights (REQ-3741)

The Power BI dashboard has a data assistant. Its queries live in `DataAnalysisAPI` and the assistant (model, tools, conversations) in `AIServicesAPI`.
They talk over HTTP, like every other module of the monolith (`DataAnalysisFeignAPI`), forwarding the `Token` header of the person: the data module validates
the session again and applies the data scope, so the model can never see more than the person who asks.

### Routes

All of them need the `Token` header (platform session, permission `embed-powerbi-info`) and accept `Accept-Language` (`es` or `en`). JSON uses PascalCase.
Errors: `{ "error": "<localized message>", "ErrorCode": "<code>" }`.

| Route | What it does |
|---|---|
| `GET dataanalysisapi/usage-insights/{query}` | One of 13 queries: `subject-area-usage`, `search-schools`, `connections-by-school`, `connections-by-teacher`, `compare-subject-areas`, `compare-schools`, `full-quiz-average`, `connections-trend`, `cumulative-connections`, `teacher-activity`, `connection-patterns`, `school-participation`, `users-by-subscription`. Parameters: `from`, `to`, `order`, `limit` (1-50), `school` and the ones of each query (`groupBy`, `granularity`, `fromA`, `toA`, `fromB`, `toB`, `minConnections`, `maxConnections`, `text`, `date`, `periodFrom`). |
| `GET dataanalysisapi/usage-insights/{query}/export?format=xlsx\|chart` | Excel workbook (sheets Data and References) or chart specification, with every row (up to 5000). |
| `GET dataanalysisapi/usage-insights/data-status` | Last day with data of the tests and of the connections. |
| `POST aiservicesapi/data-assistant/ask` | Question for the assistant (`Question`, `ConversationId`, `Context{Report, Page}`, `Language`). Needs the AI switch on and consumes `TextTokens` quota. |
| `GET aiservicesapi/data-assistant/downloads/{id}` | Excel of a query of an answer, rebuilt with the token of whoever asks. |
| `GET aiservicesapi/data-assistant/charts/{id}` | Expanded chart (`all`, `order`, `includeZeros`, `limit`). |

Data scope (taken from the session, applied in the SQL): administrators (role ids in `usage_insights_all_schools_role_ids`) see every school, ORE administrators
(`usage_insights_ore_role_ids`) the schools of their ORE, directors (`usage_insights_school_role_ids`) their school. Any other role, or an ORE administrator without ORE, gets 403.
The assistant also checks `data_assistant_allowed_role_ids` (default `2,8,6`).

### Configuration (`AppSettings`)

| Key | Where it is set | Meaning |
|---|---|---|
| `data_analysis_api_url` | **every environment file**, with the same value as that file's `gus_web_service_url` (the base URL of this same monolith, like the other internal API URLs) | Base URL used by `DataAnalysisFeignAPI`. Without it the assistant answers 502 `data_unavailable` to every tool call. |
| `data_analysis_api_timeout_seconds` | `appsettings.json` | Timeout of that call (30). |
| `ai_data_assistant_deployment_name` | **every environment file** (empty in the repository) | Azure OpenAI deployment of the assistant; when empty, `azure_openai_deployment` is used. |
| `data_assistant_allowed_role_ids` | `appsettings.json` | Roles that can use the assistant (`2,8,6`). |
| `usage_insights_*_role_ids`, `usage_insights_exclude_test_accounts`, `usage_insights_excluded_ores`, `usage_insights_excluded_email_domains`, `usage_insights_excluded_schools`, `usage_insights_subscription_status_column`, `usage_insights_subscription_cancellation_column`, `usage_insights_utc_offset_minutes`, `usage_insights_command_timeout_seconds` | `appsettings.json` | Scope roles, test account exclusion, subscription columns, local day offset (-240 = Puerto Rico) and query timeout. None of them is a secret. |

Localized texts: `DataAnalysisAPI/appsettings.dataanalysis.json` (usage insights) and `AIServicesAPI/appsettings.aiservices.json` (assistant), both under `Messages:{key}:{es|en}`.

### Secrets

No new secret. The ticket reuses `AzureOpenAI--ApiKey` and `AzureOpenAI:Endpoint` (the same `AzureOpenAIClient` of the AI services) and
`ConnectionStrings--AnalysisConnectionStringReader` (the account of that connection string should only have `SELECT` on `gs_data_analysis`; the queries
also open every session in `READ ONLY` mode and refuse to run if it is not).

### How to test

```
dotnet build GSBaseAPI/GSBaseAPI.sln
dotnet test DataAnalysisAPI.Tests       # needs Docker (Testcontainers) or USAGE_INSIGHTS_TEST_CONNECTION_STRING pointing to an empty MySQL 8 database
dotnet test AIServicesAPI.Tests         # needs Docker (Testcontainers) or AI_SERVICES_TEST_CONNECTION_STRING pointing to an empty MySQL 8 database
```
