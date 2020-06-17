# Configuration
All configuration entries can be configured via environment variables or appsettings.json.

Every service can be equipped with Azure Application Insights. For this, set `APPINSIGHTS_INSTRUMENTATIONKEY` with the instrumentation key from your AI instance. 

## Auth Service
| Key | Description |
| --- | --- |
| `ConnectionStrings:AuthDbContext` | The connection string to the MSSQL database |
| `Unidash:AuthSecurityKey` | The signing key that's being used for JWT tokens |

## Canteen Service

## Chat Service

## Time Table Service
| Key | Description |
| --- | --- |
| `TimeTable:UpstreamICalUrl` | The URL to the upstream iCal file |
| `ConnectionStrings:TimeTableDbContext` | The connection string to the MSSQL database |