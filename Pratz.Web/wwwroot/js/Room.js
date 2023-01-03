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

connection.on("StartNewVote", (vote) => {
    $(".voting").show();
});

connection.start().then(function () {
    //Do things
}).catch(function (err) {
    return console.error(err.toString());
});

$("#submit-vote").click(e => {
    var value = $("#vote-value").val();
    if (value.length > 0) {
        connection.invoke("Vote", value).catch(function (err) {
            return console.error(err.toString());
        }).then(() => {
            $("#vote-value").val('');
            $(".voting").hide();
        });
    }
    e.preventDefault();
});

$("#start-vote").click(e => {
    connection.invoke("StartNewVote", 'teste').catch(function (err) {
        return console.error(err.toString());
    }).then(() => {
        $(".vote-responses").empty();
    });
    e.preventDefault();
});

$("#show-votes").click(e => {
    $(".vote-value").show();
    e.preventDefault();
});

connection.on("VoteSubmitted", function (value, userName, userId) {
    console.log('VoteSubmitted', value, userName, userId);

    var responsesEl = $(".vote-responses");
    var responseEL = jQuery('<div>', {
        'data-id': userId,
        class: 'vote-response'
    });
    var userNameEL = jQuery('<span>');
    userNameEL.text(userName);
    var voteValueEL = jQuery('<span>', {
        class: 'vote-value badge badge-secondary',
        style: 'display: none'
    });
    voteValueEL.text(value);
    responsesEl.append(responseEL);
    responseEL.append(userNameEL);
    responseEL.append(voteValueEL);
});
//document.getElementById("sendButton").addEventListener("click", function (event) {
//    var user = document.getElementById("userInput").value;
//    var message = document.getElementById("messageInput").value;
//    connection.invoke("SendMessage", user, message).catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});