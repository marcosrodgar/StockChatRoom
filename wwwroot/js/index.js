

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (message) {
    var messageList = document.getElementById("messagesList");
    var listItems = messageList.getElementsByTagName("li");

    if (listItems.length >= 50)
    {
        messageToHide = document.getElementById(`message-${listItems.length - 50}`);
        messageToHide.style.display = "none";
    }

    var li = document.createElement("li");
    messageList.appendChild(li);
    var dateSentOn = new Date(message.sentOn);
    li.innerHTML = `${dateSentOn.toLocaleDateString('en-US')}, ${dateSentOn.toLocaleTimeString()} - <b>${message.userName}</b>: ${message.content}`;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});


window.onload = function loadRecentMessages() {
    document.getElementById("messageInput").disabled = true;


    fetch("/api/ChatMessages", {
        method: "GET",
        headers: {
            'Content-Type': 'application/json'
        }
    }).then(async (response) => {
        if (!response.ok) {
            console.log(response);
        }
        const messages = await response.json();
        console.log(messages);

        for (var i = 0; i < messages.length; i++)
        {
            console.log("hey for loop");
            var message = messages[i];
            var li = document.createElement("li");
            li.setAttribute('id', `message-${i}`)
            document.getElementById("messagesList").appendChild(li);
            var dateSentOn = new Date(message.sentOn);
            li.innerHTML = `${dateSentOn.toLocaleDateString('en-US')}, ${dateSentOn.toLocaleTimeString()} - <b>${message.userName}</b>: ${message.content}`;
        }

        document.getElementById("messageInput").disabled = false;
    });
    
}

document.getElementById("sendButton").addEventListener("click",  function (event) {
    var message = document.getElementById("messageInput").value;
    var data = { content: message };

    fetch("/api/ChatMessages", {
        method: "POST",
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    }).then(async (response) => {
        if (!response.ok) {
            console.log(response);    
        }
        const data = await response.json();
        const message = {
            content: data.content,
            sentOn: new Date(data.sentOn),
            userName: data.user.userName
        }
        connection.invoke("SendMessage", message).catch(function (err) {
            return console.error(err.toString());
        });
    });

    document.getElementById("messageInput").value = '';


    
    event.preventDefault();
});

