Identity Project - How to run the project

This repo contains three .NET projects that run together:

IdentityProvider - Duende IdentityServer + ASP.NET Identity (auth server)

WebApi - ASP.NET Core Web API (profiles, access grants, etc.)

IdentityProjectUI - Razor/MVC UI used by end-users

Prerequisites

.NET 8 SDK (LTS)
Verify: dotnet --version (should start with 8.)

Optional: Docker (to run PostgreSQL and RabbitMQ)
Alternatively, run local instances of PostgreSQL and RabbitMQ.

Optional: JetBrains Rider or Visual Studio (for multi-project run)

If you have .NET 9 installed as well, make sure the projects target net8.0 and you're using the .NET 8 SDK for commands.

1. Trust Dev certificates
   - open a terminal within the project and run
     dotnet dev-certs https --trust
2. Start infrastructure
   If using docker:
   RabbitMQ

   - docker run -d --name rabbitmq \
     -p 5672:5672 -p 15672:15672 \
     -e RABBITMQ_DEFAULT_USER=guest \
     -e RABBITMQ_DEFAULT_PASS=guest \
     rabbitmq:3-management

   PostGreSQL

   - docker run -d --name postgres \
     -p 5432:5432 \
     -e POSTGRES_USER=postgres \
     -e POSTGRES_PASSWORD=postgres \
     -e POSTGRES_DB=appdb \
     -v pgdata:/var/lib/postgresql/data \
     postgres:16

   NOTE: If you run your instances at different ports, go to appsettings.Development.json of the IdentityProvider and WebApi projects and adjust the connection strings.

3. Run migrations (from the terminal):

   WebApi migrations:

   - dotnet ef database update --project Infrastructure --startup-project WebApi

   Identity provider migrations:

   - dotnet ef database update --context ConfigurationDbContext --project IdentityProvider
   - dotnet ef database update --context PersistedGrantDbContext --project IdentityProvider

4. Run the three projects - WebApi, IdentityProvider and IdentityProjectUI.

5. Visit localhost:5000 from your favourite browser and test.

   NOTE: Since this is a test project, and won't be running on any production environment, no secrets need to be configured as they are hardcoded in the code, but this is obviously not following best practices and shouldn't be done for production. Nevertheless, they are useless to hackers so it is OK.
