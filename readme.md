# Galytix Web API

## Description
This is a simple Web API project that provides endpoint which calculates average GrossWrittenPremium
It is built using ASP.NET Core and follows the Vertical Slice Architecture principles. The project is designed to be modular, maintainable, and testable.
Application loads all data from csv file, calculates average GrossWrittenPremium and stores it in a database.
So, no need to calculate and cache it in memory, because all values already stored in database.

### Api documentation
The API documentation is available at the following URL:
```
https://localhost:9091/swagger/index.html
```
Documentation is generated using Swagger and provides detailed information about the available endpoints, request/response formats.
Unfortunately, there is no detailed documentation for the endpoints, but you can use the Swagger UI to explore the API and see the available endpoints.

## How to run project 
### Prerequisites
- .NET 9.0 SDK or later
```bash
dotnet run --project Galytix.WebApi
```

## How to run tests
This project includes unit tests for main loginc. The tests are located in the `Galytix.UnitTests` project.
```bash
dotnet test --project Galytix.WebApi.Tests
```

## Postman API tests

This project includes a Postman collection for testing the API endpoints. The collection is located in the root folder and can be imported into Postman for testing.


### Prerequisites
- Install Newman (Postman CLI)

```bash
 newman run GalytixAPI.postman_collection.json 
```
