FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ToDoListMicroservices.Notification.Api/ToDoListMicroservices.Notification.Api.csproj", "ToDoListMicroservices.Notification.Api/"]
RUN dotnet restore "ToDoListMicroservices.Notification.Api/ToDoListMicroservices.Notification.Api.csproj"
COPY . .
WORKDIR "/src/ToDoListMicroservices.Notification.Api"
RUN dotnet build "ToDoListMicroservices.Notification.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ToDoListMicroservices.Notification.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ToDoListMicroservices.Notification.Api.dll"]
