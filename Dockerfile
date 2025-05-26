# Use SDK image to build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy everything from the build context (solution root)
COPY . .

# Restore all projects in the solution
RUN dotnet restore "AdithyaBank.sln"

# Build and publish only the API project
RUN dotnet publish "AdithyaBank.Api/AdithyaBank.Api.csproj" -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

# Copy published files from build
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "AdithyaBank.Api.dll"]
