# PersonalWebsite
This is a full source code of my multilingual website implemented in ASP.NET Core

![Test the app](https://github.com/nettsundere/PersonalWebsite/workflows/Test%20the%20app/badge.svg?branch=develop)

Hosting: Azure, App Service, West US 2

# See it in action
https://vladimir-kiselev.me

# Dependencies
- [Node.js](https://nodejs.org/en/)
- [.NET Core sdk](https://www.microsoft.com/net/download)

# Running it locally
```sh
$ cd src/PersonalWebsite
$ npm install
$ npm run build
$ ASPNETCORE_ENVIRONMENT=Development dotnet run 
```
Then navigate to http://localhost:8080

---
NB: Migrations will run automatically during the startup. 

# PersonalWebsite/Tests
XUnit tests
```
$ dotnet test
```

# License
License - [MIT](https://github.com/nettsundere/PersonalWebsite/blob/develop/License.md)

