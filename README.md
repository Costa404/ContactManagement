# ContactManagement

.NET 6 web app for managing contacts (CRUD, soft delete, and validations).

## Requirements
- .NET 6 SDK, Git, and `dotnet ef`
- Database configured in `appsettings.json` (`ConnectionStrings:DefaultConnection`)

## Run Locally
    git clone https://github.com/Costa404/ContactManagement.git
    cd ContactManagement
    dotnet restore
    dotnet ef database update
    dotnet run

## Install / Deploy
    git pull
    dotnet publish ContactManagement.csproj -c Release -o /home/costanuno-dot/build
    dotnet ef database update
    touch /home/costanuno-dot/build/restart

## Links
- App: https://costanuno-dot.recruitment.alfasoft.pt
- Repo: https://github.com/Costa404/ContactManagement
