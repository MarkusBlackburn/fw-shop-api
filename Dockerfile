FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5027

ENV ASPNETCORE_URLS=http://+:5027

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["fw-shop-api.csproj", "./"]
RUN dotnet restore "fw-shop-api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "fw-shop-api.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "fw-shop-api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "fw-shop-api.dll"]
