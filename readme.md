# Galytix Web API

## Description
This is a simple Web API project that provides endpoint which calculates average GrossWrittenPremium
It is built using ASP.NET Core and follows the Vertical Slice Architecture principles. The project is designed to be modular, maintainable, and testable.
Application loads all data from csv file, calculates average GrossWrittenPremium and stores it in a database.
So, no need to calculate and cache it in memory, because all values already stored in database.

## How to run project 

```
dotnet run --project Galytix.WebApi
```

## How to run tests

```
dotnet test --project Galytix.WebApi.Tests
```

## How to run Postman collection via cli
```bash
 newman run Galytix.postman_collection.json 
```
