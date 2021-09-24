"use strict";

//TODO: send this urls to config or sent by page
var connection = new signalR.HubConnectionBuilder().withUrl("/votingHub?roomId=" + roomId).build();

connection.on("ReceiveMessage", function (user, message) {
});

connection.on("UserJoined", function (room, userName, userId) {
    console.log('UserJoined', room, userName, userId);

    var membersEl = $(".members");
    room.members.map(member => {
        console.log("processing member", member.userName, member.userId);
        var userEl = membersEl.find(".user").filter(el => {
            return el.data('id') === userId;
        });
        if (userEl.length === 0) {
            userEl = jQuery('<div>', {
                'data-id': member.userId,
                class: 'user'
            });
            membersEl.append(userEl);
            userEl.text(member.userName);
        }
    });
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