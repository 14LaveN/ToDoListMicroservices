FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release

# Install Node.js
RUN curl -fsSL https://deb.nodesource.com/setup_14.x | bash - \
    && apt-get install -y \
        nodejs \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /src
COPY ["ToDoListMicroservices.Web/ToDoListMicroservices.Web.csproj", "ToDoListMicroservices.Web/"]
RUN dotnet restore "ToDoListMicroservices.Web/ToDoListMicroservices.Web.csproj"
COPY . .
WORKDIR "/src/ToDoListMicroservices.Web"
RUN dotnet build "ToDoListMicroservices.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ToDoListMicroservices.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ToDoListMicroservices.Web.dll"]
