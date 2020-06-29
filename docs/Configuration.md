# Configuration
All configuration entries can be configured via environment variables or appsettings.json.

Every service can be equipped with Azure Application Insights. For this, set `APPINSIGHTS_INSTRUMENTATIONKEY` with the instrumentation key from your AI instance. 

## All Services
| Key | Description | Global | Remarks |
| --- | --- | --- | --- |
| `APPINSIGHTS_INSTRUMENTATIONKEY` | The instrumentation key for Azure Application Insights | No | Optional |
| `Auth:SecurityKey` | The signing key that's being used for issueing and validating JWT tokens | Yes | |

## Gateway
| Key | Description |
| --- | --- |
| `Gateway:Cors:WithOrigins` | An array of allowed origins for the CORS policy |

## Auth Service
| Key | Description |
| --- | --- |
| `ConnectionStrings:AuthDbContext` | The connection string to the MSSQL database |

## Canteen Service

## Chat Service

## Time Table Service
| Key | Description |
| --- | --- |
| `TimeTable:UpstreamICalUrl` | The URL to the upstream iCal file |
| `ConnectionStrings:TimeTableDbContext` | The connection string to the MSSQL database |