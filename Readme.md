# PersonalWebsite
This is a full source code of my multilingual website implemented in ASP.NET Core

Hosting: Azure, App Service, West US 2

# See it in action
https://vladimir-kiselev.me

# Dependencies
- [Docker](https://www.docker.com)
- [.NET Core sdk](https://www.microsoft.com/net/download)

# Running it locally

- Bash
```sh
$ docker-compose -f development/docker-compose.yaml up --build
$ cd src/PersonalWebsite
$ npm ci
$ npm run build
$ ASPNETCORE_ENVIRONMENT=Development dotnet ef database update --context DataDbContext
$ ASPNETCORE_ENVIRONMENT=Development dotnet ef database update --context AuthDbContext
$ ASPNETCORE_ENVIRONMENT=Development dotnet run 
```

- PowerShell on Windows
```powershell
PS docker-compose -f development/docker-compose.yaml up --build
PS cd src/PersonalWebsite
PS npm ci 
PS npm run build
PS $Env:ASPNETCORE_ENVIRONMENT = "Development"; dotnet ef database update --context DataDbContext
PS $Env:ASPNETCORE_ENVIRONMENT = "Development"; dotnet ef database update --context AuthDbContext
PS $Env:ASPNETCORE_ENVIRONMENT = "Development"; dotnet run 
```

# PersonalWebsite/Tests
XUnit tests
```
$ dotnet test
```

# License
License - [MIT](https://github.com/nettsundere/PersonalWebsite/blob/develop/License.md)

