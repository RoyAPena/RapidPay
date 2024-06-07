## Default User Credentials

To access the application, you can use the following default user credentials:

- **Username:** Admin
- **Password:** Abc_12345

## Project Description

This API manages payments and saves transactions, providing you with the necessary methods and security to handle all your transactions. To use this API, you must have credentials. Features include creating cards and making payments. All payments incur a fee.

## Project Structure

The project is divided into four layers using Clean Architecture:

**RapidPay.Domain:** This layer is the core of your application, representing essential business concepts and rules. It defines entities, value objects, and domain logic that model your application's functionality and has no dependencies on external technologies or frameworks.

**RapidPay.Application:** This layer serves as an intermediary between the Domain and other layers, using domain objects and logic to implement application-specific use cases. It defines interfaces for interaction by the outer layers, relying on the Domain layer but independent of the Presentation or Infrastructure layers.

**RapidPay.Infrastructure:** This layer handles the technical details of how the application interacts with external systems and frameworks, implementing mechanisms for data access, external APIs, and other infrastructure needs. It relies on the Application layer and avoids direct references to the Domain or Presentation layers.

**RapidPay.Presentation:** Responsible for presenting information to users and receiving user input, this layer can consist of a web UI, mobile app, API, or any other form of interaction. It relies on the Application layer to access and manipulate domain logic.

Technology used:
- .NET 8
- Entity Framework

Patterns used:
- CQRS
- DDD (Domain-Driven Design)
- Repository Pattern
- Unit of Work Pattern

## Installation

This project requires an SQL Server database. Ensure SQL Server is installed on your machine; this project uses SQL Server 2019.

The connection string is located in the `appsettings.json` file; you can modify it to connect to your database, if you have localhost server with windows authentication enabled you don't need to change the URL.

The database is created automatically the first time you run the project, providing the following default user credentials:

## Testing

### Using the Postman Collection

You can test the project using the included Postman collection file, located in the repository at `./RapidPay.postman_collection-2.1`. Postman is a popular API client that allows you to send HTTP requests and test APIs.

Follow these steps to import and use the Postman collection:

1. **Download Postman:** If you haven't already, download and install [Postman](https://www.postman.com/downloads/).

2. **Import the Collection:**
   - Open Postman.
   - Click on the "Import" button in the top left corner.
   - Select the "RapidPay.postman_collection-2.1" file from the repository directory.
   - The collection will be imported into Postman.

3. **Set Environment Variables:**
   - After importing the collection, you'll need to set up environment variables for authentication and configuration.
   - Open the imported collection in Postman.
   - Click on the gear icon in the top right corner to open the "Manage Environments" window.
   - Add a new environment or select an existing one.
   - Set the following environment variables:
     - `bearerToken`: Enter the token received upon logging into the application.
     - `cardId`: Enter the card ID obtained when creating a card.
     - `url`: This is the base URL where the server is running, in the format "http(s)://host:port/api/vi".

4. **Run Requests:**
   - Once the environment variables are set, you can start running requests from the collection.
   - Click on the request you want to run, and ensure the correct environment is selected.
   - Click the "Send" button to execute the request.
   - View the response to verify the API's functionality.

For more detailed instructions on using Postman, refer to the [Postman Documentation](https://learning.postman.com/docs/getting-started/importing-and-exporting-data/).

### Running Unit Tests with NUnit

To execute the unit tests for this project, you can utilize NUnit, a widely-used unit testing framework for .NET applications. Follow these steps to run the unit tests:

- **Install NUnit Test Runner:**
   - If you have Visual Studio 2022, you can simply navigate to the Test menu and select "Run All Tests."
   - If you don't have Visual Studio 2022, proceed to the next steps.

- **Install .NET Core SDK:**
   - If you haven't already, install the .NET Core SDK on your machine. You can download it from the official Microsoft page: [https://dotnet.microsoft.com/en-us/download/dotnet/8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).

- **Download NUnit:**
   - Visit the official NUnit website at [https://nunit.org/](https://nunit.org/) and download the NUnit framework.

- **Locate Test Files:**
   - Once NUnit is installed, locate the project's test files. These files typically have a `.dll` extension and are located within the project's directory.

- **Run Tests with NUnit Test Runner:**
   - Open the NUnit Test Runner application.
   - Use the application to locate the project's test files.
   - Select the test assembly (`.dll` file) containing your unit tests.
   - Run all tests to verify the functionality of your code.

## Download and Run with Visual Studio Code

Follow these steps to download the repository and run the project using Visual Studio Code:

1. **Clone the Repository:**
   - Open your terminal or command prompt.
   - Navigate to the directory where you want to store the project.
   - Run the following command to clone the repository:

     git clone https://github.com/RoyAPena/RapidPay

2. **Open the Project in Visual Studio Code:**
   - Launch Visual Studio Code.
   - Use the "File" menu or the terminal to navigate to the project directory.
   - Open the project by selecting the folder containing the project files.

3. **Install Dependencies:**
   - If any dependencies are not already installed, you can install them using the integrated terminal in Visual Studio Code. Navigate to the project directory and run:

     dotnet restore

4. **Configure Database Connection:**
   - Open the `appsettings.json` file.
   - Modify the connection string to point to your SQL Server instance if necessary.

5. **Run the Application:**
   - Use the terminal in Visual Studio Code to navigate to the project directory.
   - Run the following command to build and run the application:

     dotnet run

6. **Access the Application:**
   - Once the application is running, you can access it through your web browser or API client using the specified URL.


## Download and Run with Visual Studio

### Prerequisites:
- [.NET Core SDK](https://dotnet.microsoft.com/download) installed on your machine.
- [Visual Studio](https://visualstudio.microsoft.com/) installed with the .NET workload.

Follow these steps to download the repository and run the project using Visual Studio:

1. **Clone the Repository:**
   - Open your terminal or command prompt.
   - Navigate to the directory where you want to store the project.
   - Run the following command to clone the repository:

     git clone https://github.com/RoyAPena/RapidPay

2. **Open the Project in Visual Studio:**
   - Open Visual Studio.
   - Select "Open a project or solution" from the start window.
   - Navigate to the directory where you cloned the repository and open the solution file (`*.sln`).

3. **Configure Database Connection:**
   - Open the `appsettings.json` file.
   - Modify the connection string to point to your SQL Server instance if necessary.

4. **Run the Application:**
   - Set the startup project by right-clicking on the project you want to run in the Solution Explorer and selecting "Set as Startup Project."
   - Press `F5` or select "Start Debugging" from the Debug menu to build and run the application.

5. **Access the Application:**
   - Once the application is running, you can access it through your web browser or API client using the specified URL: https://localhost:7197 or http://localhost:5235 if the project is running in IIS the following: http://localhost:49949
