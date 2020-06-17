#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Unidash.Auth.Application/Unidash.Auth.Application.csproj", "Unidash.Auth.Application/"]
COPY ["Unidash.Core/Unidash.Core.csproj", "Unidash.Core/"]
RUN dotnet restore "Unidash.Auth.Application/Unidash.Auth.Application.csproj"
COPY . .
WORKDIR "/src/Unidash.Auth.Application"
RUN dotnet build "Unidash.Auth.Application.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Unidash.Auth.Application.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Unidash.Auth.Application.dll"]