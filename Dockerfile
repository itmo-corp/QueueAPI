FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY QueueAPI.csproj QueueAPI.csproj
RUN dotnet restore QueueAPI.csproj
COPY . .
WORKDIR /src
RUN dotnet build QueueAPI.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish QueueAPI.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "QueueAPI.dll"]