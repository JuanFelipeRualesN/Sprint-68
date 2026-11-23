# REQ-3741: Power BI data assistant panel (gs-msj-private-webapp-teachers)

Branch `req-3741` (from `main` 5504a3d) → target `uat`.

## Summary
Adds the **Data assistant** side panel to the reports dashboard. Administrators ask questions about platform usage in natural language and get answers with references (period, filtered date, school, data-up-to, rows), charts and Excel downloads. It is visible only to the `admin` and `admin-ore` roles, behind a feature flag. It calls the new backend routes using the existing API base URLs and the session `Token` header. No secrets, keys or queries live in the front.

## Changes
- **F1** Panel ported from the MVP to `data-analysis-module/data-assistant-panel/` with English identifiers, JSDoc and `data-assistant-*` CSS classes. Recharts only, no new dependencies.
- **F2** Texts moved to the i18next namespace `data-assistant` (`translations/i18next/{es,en}/data-assistant.json`), registered in `src/i18n.js`.
- **F3** URL builders in `settings.js` (`getDataAssistantAskAPI`, `getDataAssistantResourceAPI`, `getUsageInsightsExportAPI`) and `src/api/data-assistant.js` on axios. Resource URLs go through an allow-list (assistant paths and the usage insights export only).
- **F4** `src/api/data-assistant-contract.js` is the single module that knows the API field names (PascalCase). It builds the ask request and maps answers to the camelCase view model the components read. Notices, the more-rows flag and ties come from structured fields, never from texts.
- **F5** Optional `onReportReady` prop on `powerbi-embed`; panel mounted in `dashboard/index.js` only when the flag is `true` and the role is allowed. `REACT_APP_DATA_ASSISTANT_AVAILABLE = false` added to every `.env.*`; README section.
- **F6** Tests completed; build verified.

Routes used: `POST aiservicesapi/data-assistant/ask`, `GET aiservicesapi/data-assistant/downloads/{id}`, `GET aiservicesapi/data-assistant/charts/{id}`, `GET dataanalysisapi/usage-insights/{query}/export`.

## Design note
The panel uses plain `axios` with `getAuthHeaders()` (the helper the AI module uses), not the shared axios of `interceptors.js`. That interceptor opens a modal and signs the user out on 401/403, and shows a blank modal on 429 (quota). The panel shows every error inline in the chat instead.

## Screenshots
Produced with a test harness that runs the real panel, API layer and contract mapper with the network faked (desktop es, expanded chart, en, mobile). Not attached here.

## Test results
- Jest 23.6.0 (the version pinned in the repo): 7 suites, 102 tests pass (panel, expanded chart, chart types, config, API layer, contract mapper, dashboard mount).
- Lint (CRA `react-app` config; the repo has no lint config): 0 errors, 53 warnings, identical to `main`. No new warnings.
- `react-scripts build` (`.env.msj-dev`): compiled successfully.

### How the tests were run (the repo's `npm test` was already broken before this ticket)
`npm test` runs `node scripts/test.js`, but neither `scripts/test.js` nor the `config/jest/*` transforms referenced by the `jest` block of `package.json` are in the repo; `react-scripts test` rejects that stale `jest` block; and `yarn.lock` cannot be parsed (the pipeline uses `npm install --force`). This PR does not change any of that. The suites were run with Jest 23.6.0 and a local config kept outside the repo (Babel 7 with `babel-preset-react-app` and `babel-plugin-jest-hoist`, CRA's CSS/file transforms, jsdom). A follow-up ticket should version `scripts/test.js` and `config/jest` (or move to `react-scripts test`) so `npm test` works.

## Review fixes
- M4: `config.test.js` uses `jest.resetModules()` instead of `jest.isolateModules()` (not available in Jest 23).
- C4: `Language` is always sent in the ask body (the panel's current language, `es` if none). The backend uses it first and falls back to `Accept-Language`.
- C1/C2/C3: the front reads `References[].HasMoreRows`, `Summary` (`Ties`) and `Note`, and the error codes `ai_disabled` and `quota_exceeded` (with the `http_429` fallback). Both codes are covered by tests.

## Enable the flag per environment
In `.env.<environment>` set `REACT_APP_DATA_ASSISTANT_AVAILABLE = true` and rebuild. Every environment ships with `false`. Enable it only after the backend routes are deployed in that environment. Only the exact value `true` enables it.

## Branch note
`main` is not an ancestor of `uat`: it is 3 commits ahead (two merge commits and `719bf6cc`), but the trees are identical, so a PR to `uat` has no content differences, only extra history.

## Open items
- Contract: the fields the plan did not list (chart row key `Category`, `Grouping`, `Extra`, `Order`, `IncludeZeros`, `All`, `Highlighted`, `Period.A/B`, `Filters.School`, `Filters.TestAccounts`) were checked against the backend in review and match. The backend will return `HasMoreRows`, `Summary` and `Note` in `References`, and `ErrorCode` `ai_disabled` / `quota_exceeded`, once its review fixes land; the front already reads those names.
- Subscription chart colours match English status names only (server texts are localized); a stable key per category would fix this.
- No end-to-end run against the monolith yet.
