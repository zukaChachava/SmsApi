FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Sms.Api/Sms.Api.csproj", "Sms.Api/"]
COPY ["Sms.Infrastructure/Sms.Infrastructure.csproj", "Sms.Infrastructure/"]
COPY ["Sms.Application/Sms.Application.csproj", "Sms.Application/"]
COPY ["Sms.Domain/Sms.Domain.csproj", "Sms.Domain/"]
RUN dotnet restore "Sms.Api/Sms.Api.csproj"
COPY . .
WORKDIR "/src/Sms.Api"
RUN dotnet build "Sms.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Sms.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sms.Api.dll"]
