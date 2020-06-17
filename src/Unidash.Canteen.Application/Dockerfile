#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Unidash.Canteen.Application/Unidash.Canteen.Application.csproj", "Unidash.Canteen.Application/"]
COPY ["Unidash.Canteen.Core/Unidash.Canteen.Core.csproj", "Unidash.Canteen.Core/"]
RUN dotnet restore "Unidash.Canteen.Application/Unidash.Canteen.Application.csproj"
COPY . .
WORKDIR "/src/Unidash.Canteen.Application"
RUN dotnet build "Unidash.Canteen.Application.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Unidash.Canteen.Application.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Unidash.Canteen.Application.dll"]