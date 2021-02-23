# Fullstack Challenge

This project is a challenge that I've done in 48h with the goal to make a fullstack project (backend with REST API and SPA frontend). I've got a little carried away and made some additions. Bellow is a breakdown of what is included in this repository.

## Backend

The backend is written in C#, using .NET Core 3.1 on the ASP.NET Core platform. It has two main projects: Authentication and Challenge. The authentication compromise of an Identity Server 4 implementation with login screen and token API request support. On the other way, the Challenge project is divided in two: EF and Dapper, both accomplishes the same goal: A full feature REST API with authentication via OpenID and Swagger documentation, but each one uses a different ORM, one being Entity Framework and the other being Dapper. Only the EF one has tests, as Dapper doesn't support in-memory databases. The database used is PostgreSQL. You can login via OpenID using the username: 'test' with the password '1234'.

## Frontend

The frontend was written in TypeScript, using Angular, it is an access to the API, featuring OpenID authentication using the backend server.

## Docker Files

The entire backend has docker files and two docker-compose ones that you can build, but you will have some problems with CORS if you don't pay attention. The frontend don't have docker support because I find really easy to use the Angular CLI to serve the SPA.
