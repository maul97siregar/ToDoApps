FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ToDoApp.Api/ToDoApp.Api.csproj", "ToDoApp.Api/ToDoApp.Api.csproj"]
RUN dotnet restore "./ToDoApp.Api/ToDoApp.Api.csproj"
COPY . .
WORKDIR "/src/ToDoApp.Api"
RUN dotnet build "./ToDoApp.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Install dotnet-ef tool globally with specified project file
RUN dotnet tool install --global dotnet-ef

# Set the PATH for the tool
ENV PATH="$PATH:/root/.dotnet/tools"

# Run migrations using the correct project file
RUN dotnet ef database update --project "./ToDoApp.Api.csproj"

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ToDoApp.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
COPY --from=build /src/ToDoApp.Api/todo.db .

ENTRYPOINT ["dotnet", "ToDoApp.Api.dll"]
