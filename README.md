# Stock Chat

## Tech stack

The solution was developed using .NET 6 with SignalR for the real time communication, that provides the basic chat functionality, with RabbitMQ as the message broker, running locally from a Docker container, and with Blazor for the frontend part. It was used a localDB SQL Server database to store the user and the messages information, and it was used .NET identity to handle the authentication. 

It was used the CSV Helper nuget package to handle the CSV parsing and MSTest for the tests.

The IDE used was Visual Studio Professional 2022, version 17.2.6, in a Windows 10 operating system.

Note:

* This solution has been tested only on a Windows machine. It's pending to make sure all of the steps described here work on another operating system, but the general process should not vary too much.


## Code structure

It was used the Blazor Server App template provided by Visual Studio and it was followed this Microsoft guide to create the initial setup for the basic chat functionality with SignalR: [Use ASP.NET Core SignalR with Blazor](https://learn.microsoft.com/en-us/aspnet/core/blazor/tutorials/signalr-blazor?view=aspnetcore-7.0&tabs=visual-studio&pivots=server). Then, the solution and folder structure was adjusted according to the requirements.

These are some of the main projects and folder to consider in the solution:

* The StockChat project contains all the main code of the application, both the frontend and the backend and the StockChat.Tests projects contains the tests.

* The Data folder contains the Entity Framework context with the corresponding migrations (since it was followed the EF Code First approach), the repositories and the entities that map to the database tables to store the user information and the chat messages.

* The Events folder contains the RabbitMQ producer and consumer. The consumer is a background task that keeps listening for changes added into the queue in RabbitMQ and whenever a new message is added, it reads it and sends it back to the chat room.

* The Hubs folder contains the SignalR code that handles the messages to be sent to the chat room and the logic to either call or not call the repository layer to save changes on the database.

* The Pages folder contains the views of the application, with the index.blazor being the main one, where the chat messages are displayed.

* The Services folder contains a service to perform the HttpClient call to the API that retrieves the stock information.

## Steps to run the application

1. Make sure to install Docker in your machine.

2. Run this command to spin up the RabbitMQ server locally, using Docker:

`docker run -d --hostname my-rabbitmq-server --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management`

3. Once the server is running, go to http://localhost:15672/ to open the RabbitMQ Management UI, and login with "guest" as the username and password. This UI allows you to check the queues and messages added to RabbitMQ.

4. Make sure to install the .NET 6 SDK: https://dotnet.microsoft.com/en-us/download/dotnet/6.0. Choose the operating system that better works for you and
use the latest stable version. At the moment of writing this documentation, the latest version is 6.0.405. Once the installation is finished, run the command `dotnet` in the terminal. If the installation was correct, it would output the corresponding version.

5. Make sure to install [SQL Server Express LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-ver16). If you are using Visual Studio, it should already be installed.

6. Create the database by running this Entity Framework commmand in the Visual Studio Package Manager Console

`update-database`

If you are using Visual Studio, it should be possible to see the created database in the SQL Server Object Explorer, since that will create a localBD database. If not, it should be possible to explore and query the database using any other SQL integrated environment, such as SQL Server Management Studio.

7. Open the terminal tool of your preference and navigate to the path where the `Program.cs` file is located. The relative path would be *\StockChat\StockChat*.
You can also use an IDE that allows you to open a terminal, such as, for example, Visual Studio Code. 

8. Once there, run the application with the command `dotnet run`. If you are using Visual Studio, simply run the application from there.

9. After this, the chat application will be running and you will be able to access it from https://localhost:7149/

10. The login screen will be displayed. So, make sure to create a user providing an email and a password and then login with those credentials. Log in with as many users as you want, in order to test the chat functionality.


## Steps to run the tests 

1. Navigate to the path where the StockChat.Tests.csproj file is located. The relative path would be \StockChat\StockChat.Tests

2. To run all the tests, run the command `dotnet test`

3. To run tests individually, run the command dotnet test --filter Name=<TestName>. Here 'TestName' corresponds to the method name for the test. So, for example, to run the first test of the StockServiceTests.cs file, run the command `dotnet test --filter Name=GetStockCodeSuccesfully`

4. If you are using Visual Studio, simply run the test from the tests explorer.







