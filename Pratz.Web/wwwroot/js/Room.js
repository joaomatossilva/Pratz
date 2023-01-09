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
            toastr.success(member.userName + ' has joined the room', 'User Joined!');
        }
    });
});

connection.on("StartNewVote", (vote) => {
    //$(".voting").show();
    toastr.info('A new vote has started', 'New Vote!');
    
    var template = $("#user-vote-template").html();
    var templateFn = doT.template(template);
    var templateResult = templateFn(vote);
    $("#voting").append($(templateResult));
});

connection.start().then(function () {
    //Do things
}).catch(function (err) {
    return console.error(err.toString());
});

const submitVote = (id) => {
    var el = $(`.vote-card[data-id=${id}]`);
    var form = $("form", el);
    var value = $("input#vote-value", form).val();
    if (value.length > 0) {
        connection.invoke("Vote", id, value).catch(function (err) {
            return console.error(err.toString());
        }).then(() => {
            form.html('Vote Submitted');
            toastr.success('Thank you for your vote', 'Success!');
        });
    }
}
/*
$("#submit-vote").click(e => {
    
    e.preventDefault();
});
*/

$("#start-vote").click(e => {
    var voteName = $("#new-vote-name").val();
    if(voteName.length > 0) {
        $("#new-vote-name").val('');
        connection.invoke("StartNewVote", voteName).catch(function (err) {
            return console.error(err.toString());
        }).then((vote) => {
            //$(".vote-responses").empty();
            toastr.success('New Vote Started', 'Success!');
            console.log(vote);
            var template = $("#vote-template").html();
            var templateFn = doT.template(template);
            var templateResult = templateFn(vote);
            $("#votes-cards").append($(templateResult));
        });
    }
    e.preventDefault();
});

const showVotes = (id) => {
    var card = $(`.vote-card[data-id=${id}]`);
    $(".vote-value", card).show();
};
/*
$("#show-votes").click(e => {
    $(".vote-value").show();
    e.preventDefault();
});
*/


connection.on("VoteSubmitted", function (id, value, userName, userId) {
    console.log('VoteSubmitted', value, userName, userId);

    var responsesEl = $(`.vote-card[data-id=${id}]`);
    var template = $("#user-vote-response-template").html();
    var templateFn = doT.template(template);
    var templateResult = templateFn({ userName: userName, userId: userId, value: value });
    $(".vote-responses", responsesEl).append($(templateResult));
});
//document.getElementById("sendButton").addEventListener("click", function (event) {
//    var user = document.getElementById("userInput").value;
//    var message = document.getElementById("messageInput").value;
//    connection.invoke("SendMessage", user, message).catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});