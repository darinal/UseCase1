# .NET Web API Application

This repository contains a .NET Web API application that provides an endpoint to retrieve a list of countries with optional filtering, sorting, and pagination.

## How to Run Locally

To run the .NET Web API application locally, follow these steps:

1. **Prerequisites**: Make sure you have the following installed on your machine:

   - [.NET SDK](https://dotnet.microsoft.com/download/dotnet) (version 6.0)

2. **Clone the Repository**: Clone this repository to your local machine.

3. **Navigate to the Project Directory**: Open a terminal and navigate to the directory containing the `Readme.md` file.

4. **Build and Run the Application**: Run the following commands:
   dotnet build
   dotnet run --project Countries.API

5. **Access the API Endpoint**: Once the application is running, you can access the API endpoint using a web browser or a tool like [curl](https://curl.se/) or [Postman](https://www.postman.com/). The endpoint URL will be `https://localhost:7052/countries`.

## API Endpoint Example

### Retrieve Countries

- **Endpoint**: `/countries`
- **Method**: GET

#### Query Parameters

- `name` (optional): Filter countries by name
- `population` (optional): Filter countries by population. If the value is 0 or less, it will be ignored
- `sort` (optional): Specify sorting order (`ascend` or `descend`). If the value is different or absent, `ascend` value will be used
- `page` (optional): Page number (default: 1). If the value is 0 or less, default value will be used
- `pageSize` (optional): Number of items per page (default: 10). If the value is 0 or less, default value will be used

**Example Request**:

```http
GET https://localhost:7052/countries?name=United&population=10&sort=Descend&page=2&pageSize=5
GET https://localhost:7052/countries
GET https://localhost:7052/countries?name=Germany&page=3&pageSize=8
GET https://localhost:7052/countries?population=60&sort=Descend
GET https://localhost:7052/countries?sort=InvalidSortValue&page=-2&pageSize=0
GET https://localhost:7052/countries?name=pol&sort=InvalidSortValue&page=2&pageSize=0
GET https://localhost:7052/countries?name=u&sort=asc&page=2&pageSize=large
GET https://localhost:7052/countries?name=Ukraine&population=-1&pageSize=8
GET https://localhost:7052/countries?population=-100&pageSize=0
GET https://localhost:7052/countries?page=20&pageSize=30

GET https://localhost:7052/swagger/index.html
```
