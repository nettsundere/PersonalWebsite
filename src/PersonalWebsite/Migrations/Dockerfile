# Migrate the database
FROM microsoft/dotnet:2.1-sdk
WORKDIR /app
COPY ./ ./migrate
WORKDIR /app/migrate/PersonalWebsite
RUN dotnet restore

ENTRYPOINT dotnet ef database update --context AuthDbContext && dotnet ef database update --context DataDbContext
