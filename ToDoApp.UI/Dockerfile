FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ToDoApp.UI/ToDoApp.UI.csproj", "ToDoApp.UI/ToDoApp.UI.csproj"]
RUN dotnet restore "./ToDoApp.UI/ToDoApp.UI.csproj"
COPY . .
WORKDIR "/src/ToDoApp.UI"
RUN dotnet build "./ToDoApp.UI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ToDoApp.UI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 5225


# Tambahkan perintah untuk menjalankan migrasi sebelum menjalankan aplikasi
ENTRYPOINT ["dotnet", "ToDoApp.UI.dll"]

