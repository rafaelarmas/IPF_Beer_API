## IPF WebAPI

An example of a REST API for the purpose of showing my skills for a Developer Test. The .NET 8 Web Api stores, updates and retrieves different types of beer, their associated breweries and bars that serve the beers.

Data is persisted using Sqlite and EfCore. There are unit tests that check key logic.

### Endpoints:

#### Beers

Method | Endpoint | Description
-------|----------|------------
POST | /beer | Insert a single beer
PUT | /beer/{id} | Update a beer by Id
GET | /beer?gtAlcoholByVolume=5.0&ltAlcoholByVolume=8.0 | Get all beers with optional filtering query parameters for alcohol content (gtAlcoholByVolume = greater than, ltAlcoholByVolume = less than)
GET | /beer/{id} | Get beer by Id

#### Breweries

Method | Endpoint | Description
-------|----------|------------
POST | /brewery | Insert a single brewery
PUT | /brewery/{id} | Update a brewery by Id
GET | /brewery | Get all breweries
GET | /brewery/{id} | Get brewery by Id
POST | /brewery/beer | Insert a single brewery beer link
GET | /brewery/{breweryId}/beer | Get a single brewery by Id with associated beers
GET | /brewery/beer | Get all breweries with associated beers

#### Bars

Method | Endpoint | Description
-------|----------|------------
POST | /bar | Insert a single bar
PUT | /bar/{id} | Update a bar by Id
GET | /bar | Get all bars
GET | /bar/{id} | Get bar by Id
POST | /bar/beer | Insert a single bar beer link
GET | /bar/{barId}/beer | Get a single bar with associated beers
GET | /bar/beer | Get all bars with associated beers

### Disclaimer

This code is by no means complete. There are many ways I can see this improved if time allowed it. Please email me if there are any queries.

rafaelarmas@hotmail.com

### Deployment

#### Using CLI commands

[Deploying an AWS Lambda Project with the .NET Core CLI](https://docs.aws.amazon.com/toolkit-for-visual-studio/latest/user-guide/lambda-cli-publish.html)
[Using the .NET Lambda Global CLI](https://docs.aws.amazon.com/lambda/latest/dg/csharp-package-cli.html)

In Visual Studio, open a command prompt window.

**dotnet new install Amazon.Lambda.Templates***
**dotnet tool install -g Amazon.Lambda.Tools**
**dotnet lambda deploy-function --name IPF_Beer_API_Lambda --profile default --region eu-west-2**

#### Using AWS Toolkit

[AWS Toolkit for Visual Studio](https://aws.amazon.com/visualstudio/)

In Visual Studio, right click on the IPF_Beer_API project in Solution Explorer and select:
**Publish to AWS...**