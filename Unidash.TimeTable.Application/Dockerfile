FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Unidash.TimeTable.Application/Unidash.TimeTable.Application.csproj", "Unidash.TimeTable.Application/"]
RUN dotnet restore "Unidash.TimeTable.Application/Unidash.TimeTable.Application.csproj"
COPY . .
WORKDIR "/src/Unidash.TimeTable.Application"
RUN dotnet build "Unidash.TimeTable.Application.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Unidash.TimeTable.Application.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Unidash.TimeTable.Application.dll"]