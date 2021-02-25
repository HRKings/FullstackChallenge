# Fullstack Challenge

This project is a challenge that I've done that took more or less 48h with the goal to make a fullstack project (backend with REST API and SPA frontend). I've got a little carried away and made some additions. Bellow is a breakdown of what is included in this repository.

## Architecture

The backend project is divided into layers (Onion Architecture) and follow the principles of SOLID and uses dependency injections, thus making it independent and modular.
## Backend

The backend is written in C#, using .NET Core 3.1 on the ASP.NET Core platform. It has two main projects: Authentication and Challenge.

### Authentication

The authentication compromise of an Identity Server 4 implementation with login screen and token API request support.

### REST API

The Challenge project is divided in two: EF and Dapper, both accomplishes the same goal: A full feature REST API with authentication via OpenID and Swagger documentation, but each one uses a different ORM, one being Entity Framework and the other being Dapper. Only the EF one has tests, as Dapper doesn't support in-memory databases. You can login via OpenID using the username: `test` with the password `1234`. If you need to test the API endpoints inside swagger, there is a debug endpoint that will generate a token for you to use.

### Database

 The database used is PostgreSQL and you can find the diagram I modeled in the `.resources` folder on the repository root. The EF project includes migrations, so you can run `dotnet ef update` and it will generate all the tables and relations used for the project.

## Frontend

The frontend was written in TypeScript, using Angular, it is an access to the API, featuring OpenID authentication using the backend server. The project is separate from the other projects to be more portable in the event that multiple team members of different sector working together, making possible to in a certain event, put the frontend in its own repository.

## Docker Files

The entire backend has docker files and two docker-compose ones that you can build. Currently the frontend won't work if you spin the backend from docker because of SSL Certificates, IdentityServer is very strict with them. The frontend don't have docker support because I find really easy to use the Angular CLI to serve the SPA.
