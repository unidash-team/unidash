#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Foodies.Foody.Chat.Application/Foodies.Foody.Chat.Application.csproj", "Foodies.Foody.Chat.Application/"]
RUN dotnet restore "Foodies.Foody.Chat.Application/Foodies.Foody.Chat.Application.csproj"
COPY . .
WORKDIR "/src/Foodies.Foody.Chat.Application"
RUN dotnet build "Foodies.Foody.Chat.Application.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Foodies.Foody.Chat.Application.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Foodies.Foody.Chat.Application.dll"]