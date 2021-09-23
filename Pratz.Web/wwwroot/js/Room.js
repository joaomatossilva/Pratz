//"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/votingHub?roomId=" + roomId).build();

connection.on("ReceiveMessage", function (user, message) {
});

connection.on("UserJoined", function (userName) {
    console.log('UserJoined', userName);
});

connection.start().then(function () {
    //Do things
}).catch(function (err) {
    return console.error(err.toString());
});

//document.getElementById("sendButton").addEventListener("click", function (event) {
//    var user = document.getElementById("userInput").value;
//    var message = document.getElementById("messageInput").value;
//    connection.invoke("SendMessage", user, message).catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});