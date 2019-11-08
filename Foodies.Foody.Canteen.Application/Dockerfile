FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Foodies.Foody.Canteen.Application/Foodies.Foody.Canteen.Application.csproj", "Foodies.Foody.Canteen.Application/"]
COPY ["Foodies.Foody.Canteen.Core/Foodies.Foody.Canteen.Core.csproj", "Foodies.Foody.Canteen.Core/"]
RUN dotnet restore "Foodies.Foody.Canteen.Application/Foodies.Foody.Canteen.Application.csproj"
COPY . .
WORKDIR "/src/Foodies.Foody.Canteen.Application"
RUN dotnet build "Foodies.Foody.Canteen.Application.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Foodies.Foody.Canteen.Application.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Foodies.Foody.Canteen.Application.dll"]
