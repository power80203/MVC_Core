FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

##########
## docker build -t aspnetcore-in-docker -f Dockerfile .
##########

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
# out folder to app folder
COPY --from=build-env /app/out .
# .env to app folder
COPY --from=build-env /app/.env .
ENTRYPOINT ["dotnet", "MVC_Core.dll"]