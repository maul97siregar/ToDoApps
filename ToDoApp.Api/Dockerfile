FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ToDoApp.Api/ToDoApp.Api.csproj", "ToDoApp.Api/ToDoApp.Api.csproj"]
RUN dotnet restore "./ToDoApp.Api/ToDoApp.Api.csproj"
COPY . .
WORKDIR "/src/ToDoApp.Api"
RUN dotnet build "./ToDoApp.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build
# migrasi kesayangan <3
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"
RUN dotnet-ef database update --project ./

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ToDoApp.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM build as migrations

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
COPY --from=build /src/ToDoApp.Api/todo.db .
RUN chmod a+rw /app /app/todo.db


# Tambahkan perintah untuk menjalankan migrasi sebelum menjalankan aplikasi
ENTRYPOINT ["dotnet", "ToDoApp.Api.dll"]

