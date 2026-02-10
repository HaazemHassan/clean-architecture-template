FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Template.API/Template.API.csproj", "Template.API/"]
COPY ["src/Template.Core/Template.Core.csproj", "Template.Core/"]
COPY ["src/Template.Infrastructure/Template.Infrastructure.csproj", "Template.Infrastructure/"]
RUN dotnet restore "./Template.API/Template.API.csproj"
COPY src/ .
WORKDIR "/src/Template.API"
RUN dotnet build "./Template.API.csproj" -c $BUILD_CONFIGURATION -o /app/build



FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Template.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false



FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Template.API.dll"]