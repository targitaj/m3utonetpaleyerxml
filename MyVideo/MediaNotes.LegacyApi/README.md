# MediaNotes Legacy API

Совместимый серверный адаптер для существующего ASP.NET Framework 4.8 сайта
MyVideo на `https://a.mosalski.de/`.

API загружается из `bin` без отдельного IIS Application:

- `/api/health`
- `/api/auth/register`, `/api/auth/login`
- `/api/sync`
- `/api/history`, `/api/history/{id}`, `/api/history/today`

SQLite находится в корневом `App_Data/medianotes.db`. Для работы нужны
`MediaNotes.LegacyApi.dll`, `System.Data.SQLite.dll` и нативные
`x86/x64/SQLite.Interop.dll`.

После первого обращения API поддерживает ежедневные снимки фоновым таймером
раз в шесть часов. Синхронизация также создаёт снимок текущего UTC-дня.
