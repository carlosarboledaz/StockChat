# Stock Chat

## Tech stack

The solution was developed using .NET 6, SignalR for the real time communication, that provides the basic chat functionality, with RabbitMQ as the message broker and with Blazor for the frontend part. It was used a localDB SQL server database to store the user and the messages information, and it was used .NET identity to handle the authentication.

It was used the CSV Helper nuget package to handle the CSV parsing and it was used MSTest for the tests.

## Code structure

It was used the Blazor Server App template provided by Visual Studio and it was followed this Microsoft guide to create the initial setup for the basic chat functionality with SignalR: [Use ASP.NET Core SignalR with Blazor](https://learn.microsoft.com/en-us/aspnet/core/blazor/tutorials/signalr-blazor?view=aspnetcore-7.0&tabs=visual-studio&pivots=server). Then, the solution and folder structure was adjusted according to the requirements.

These are some of the main projects and folder to consider in the solution:

* The StockChat project contains all the main code of the application, both the frontend and the backend and the StockChat.Test folder contains the tests.

* The Data folder contains the Entity Framework context with the corresponding migrations (since it was followed the EF Code First approach) and with the entities that map to the database tables to store the user information and the chat messages.

* The Events folder contains the RabbitMQ producer and consumer. The consumer is a background task that keeps listening for changes added into the queue in RabbitMQ and whenever a new message is added, it reads it and sends it back to the chat room.

* The Hubs folder contains the code that handles messages to be sent to the chat room and the logic to either call or not call the repository layer to save changes on the database.

* The Services folder contains a service to perform the HttpClient call to the API that retrieves the stock information.




## Steps to run the application

1. Make sure to install Docker in your machine.

2. Run this command to run the container for RabbitMQ:

`docker run -d --hostname my-rabbitmq-server --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management`

3. Once the container is running, go to http://localhost:15672/ to open the RabbitMQ Management UI, and login with "guest" as the username and password. This UI allows you to check the queues and messages added to RabbitMQ.

4. Make sure to install the .NET 6 SDK: https://dotnet.microsoft.com/en-us/download/dotnet/6.0. Choose the operating system that better works for you and
use the latest stable version. At the moment of writing this documentation, the latest version is 6.0.405. Once the installation is finished, run the command `dotnet` in the terminal. If the installation was correct, it would output the corresponding version.

5. Create the database by running this Entity Framework commmand:

`update-database`

If you are using Visual Studio, it should be possible to see the created database in the SQL Server Object Explorer, since that will create a localBD database. If not, it should be possible to explore and query the database using any other SQL integrated environment, such as SQL Server Management Studio.

6. Open the terminal tool of your preference and navigate to the path where the `Program.cs` file is located. The relative path would be *\StockChat\StockChat*.
You can also use an IDE that allows you to open a terminal, such as, for example, Visual Studio Code.

7. Once there, run the application with the command `dotnet run`

// TODO: Complete the documentation


## Steps to run the unit tests 

// TODO







