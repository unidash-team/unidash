FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Foodies.Foody.Auth.Application/Foodies.Foody.Auth.Application.csproj", "Foodies.Foody.Auth.Application/"]
RUN dotnet restore "Foodies.Foody.Auth.Application/Foodies.Foody.Auth.Application.csproj"
COPY . .
WORKDIR "/src/Foodies.Foody.Auth.Application"
RUN dotnet build "Foodies.Foody.Auth.Application.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Foodies.Foody.Auth.Application.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Foodies.Foody.Auth.Application.dll"]