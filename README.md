# StockChatRoom
This is a simple chatroom which allows users to register/login and chat with other users. 
It also allows users to post commands with the following format /stock=stock_code where stock_code is any valid stock identifying code (such as AAPL.US). The command will
be processed via a decoupled bot and will return a message which consists of a stock quote, such as: “APPL.US quote is $93.42 per share”.

# Set up guide
This application was built using .NET 6 targeting .NET6.0 framework so you will need to install the runtime on your computer. You can find the download resources here: https://dotnet.microsoft.com/en-us/download/dotnet/6.0

The bot which processes the commands uses RabbitMQ as a message broker. It will initiate a server on the localhost, for this you'll need to install RabbitMQ, you can read their installation guides here: https://www.rabbitmq.com/download.html

# Technologies

-ASP.NET Core Razor Pages
-SignalR
-RabbitMQ
-Vanilla JS
-Bootstrap

# Recommended testing steps

Run the application either using **dotnet run** on a command line open on the folder where you cloned the repo, or opening the solution on your preferred IDE and running it. The console window will output he port on which the application is listening for requests:
![image](https://user-images.githubusercontent.com/109869725/186070638-7ce682e1-823e-468f-be0d-c23f066f4ba3.png)

In the above case: 5157. You can change this port on the launchSettings.json file if you need to. 

Open a browser window and paste this localhost address. It should take you to the login page, create a new user or use one of our test users available:

User name: Freims
Password: P@ssw0rd

User name: Freims2
Password: P@ssw0rd

Login with your user. The application should now take you to the Chat page. Open another browser window and repeat the steps. NOTE: You will probably need to open the second browser window with a different browser or the same browser on incognito mode. 

Now that you have two browser windows with different users logged in. You can start testing the application by sending messages and command on the chatroom. You should see them appear on realtime.



