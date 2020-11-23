//import { parseJSON } from "jquery";

$(function () {
    //getUserId From Navbar
    let memberId = $('#NavbarMemberId').text();
    // Reference the auto-generated proxy for the hub.
    var blog = $.connection.blogHub;
    // dummy call to force the [OnConnected()] method on signalR hub
    blog.client.foo = function () { };
    // Start the connection.
    //$.connection.hub.server.connected = function () { };
    $.connection.hub.start().done(function () {
        blog.server.putUserInfo(memberId);
        putFriendStatus();
        //$('#sendmessage').click(function () {
            
        //});
    });

    //當遇到url為bloggerIndex 更新朋友清單
    $(window).on('beforeunload', function () {
        putFriendStatus();
    });
    function putFriendStatus() {
        let url = this.location.href;
        let ary1 = url.split(':');
        let ary2 = ary1[2].split('/');
        if (ary2[2] === 'bloggerIndex') {
            blog.server.getFriendStatusByfMemberId(memberId);
        }
    }

    

    // Create a function that the hub can call back to change User Login status.
    blog.client.putFriendStatusToClient =function (friendIdList) {
        let json = JSON.parse(friendIdList);

        $.each(json, function (a, value1) {
            let ary = value1.split(',');
            if (ary[2].split(':')[1] !== '0}') {
                if ($(`#friendId_${ary[0].split(':')[1]}`) !== null) {
                    $(`#friendId_${ary[0].split(':')[1]} span`).removeClass("status f-off").addClass("status f-online");
                }
            }
            else {
                if ($(`#friendId_${ary[0].split(':')[1]}`) !== null) {
                    $(`#friendId_${ary[0].split(':')[1]} span`).removeClass("status f-online").addClass("status f-off");
                }
            }
        });
    };

    //blog.client.changeLogInStatus = function (userId) {

    //};
});



//TODO Send Message
// Add the message to the page.
//$('#discussion').append('<li><strong>' + htmlEncode(name)
//    + '</strong>: ' + htmlEncode(message) + '</li>');
//// This optional function html-encodes messages for display in the page.
//function htmlEncode(value) {
//    var encodedValue = $('<div />').text(value).html();
//    return encodedValue;
//}