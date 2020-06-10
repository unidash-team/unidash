# Unidash
TODO

## Configuration
All configuration entries can be configured via environment variables or appsettings.json.

Every service can be equipped with Azure Application Insights. For this, set `APPINSIGHTS_INSTRUMENTATIONKEY` with the instrumentation key from your AI instance. 

### Auth Service
### Canteen Service
### Chat Service
### Time Table Service
| Key | Description |
| --- | --- |
| `TimeTable:UpstreamICalUrl` | The URL to the upstream iCal file |
| `ConnectionStrings:TimeTableDbContext` | The connection string to the MSSQL database |

## Deployment
```sh
docker stack deploy unidash -c docker-compose.yml -c docker-compose.dev.yml -c docker-compose.traefik.yml 
```
