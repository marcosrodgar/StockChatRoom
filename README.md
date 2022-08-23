# StockChatRoom
This is a simple chatroom which allows users to register/login and chat with other users. 
It also allows users to post commands with the following format /stock=stock_code where stock_code is any valid stock identifying code (such as AAPL.US). The command will
be processed via a decoupled bot and will return a message which consists of a stock quote, such as: “APPL.US quote is $93.42 per share”.

# Set up guide
This application was built using .NET 6 targeting .NET6.0 framework so you will need to install the runtime on your computer. You can find the download resources here: https://dotnet.microsoft.com/en-us/download/dotnet/6.0

The bot which processes the commands uses RabbitMQ as a message broker. It will initiate a server on the localhost, for this you'll need to install RabbitMQ, you can read their installation guides here: https://www.rabbitmq.com/download.html


