FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY firebase.json /app/firebase.json

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ToDoListMicroservices.Identity.Api/ToDoListMicroservices.Identity.Api.csproj", "ToDoListMicroservices.Identity.Api/"]
RUN dotnet restore "ToDoListMicroservices.Identity.Api/ToDoListMicroservices.Identity.Api.csproj"
COPY . .
WORKDIR "/src/ToDoListMicroservices.Identity.Api"
RUN dotnet build "ToDoListMicroservices.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ToDoListMicroservices.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ToDoListMicroservices.Identity.Api.dll"]
