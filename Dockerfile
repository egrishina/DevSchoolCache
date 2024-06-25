# Use the ASP.NET Core runtime as a base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Use the .NET SDK to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["WebApplication/WebApplication.csproj", "WebApplication/"]
COPY ["DevSchoolCache/DevSchoolCache.csproj", "DevSchoolCache/"]
RUN dotnet restore "WebApplication/WebApplication.csproj"
COPY . .
WORKDIR "/src/WebApplication"
RUN dotnet build "WebApplication.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "WebApplication.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the .NET SDK to run migrations
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS ef-migration
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /src /src
WORKDIR "/app"
RUN dotnet ef database update --no-build --project /src/DevSchoolCache --startup-project .

# Final stage to run the application
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "WebApplication.dll"]
