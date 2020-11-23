let UserInfoHtml3 = '<div id="aaa" class="feature-photo">' +
    //fCoverPhoto
    '<figure><img src="/images/resources/timeline-1.jpg" alt=""></figure>' +
    '</div>' +
    '<div class="row">' +
    '<div class="ml-3">' +
    //fIdPhoto
    '<figure>' +
    '<img src="/images/resources/friend-avatar10.jpg" style="border-radius:50%"' +
    '</figure>' +
    '</div>' +
    //UserName
    '<div class="d-flex align-items-center"><strong>Janice Griffith</strong></div>' +
    '</div > ' +
    '<div class="text-right">' +
    //button to add and follow.
    '<button class="m-1 btn btn-secondary btn-sm ">加入好友</button>' +
    '<button class="m-1 btn btn-secondary btn-sm ">加入追蹤</button></div>';

//$('[data-toggle="popover"]').popover({
//    title: UserInfoHtml3,
//    html: true,
//    delay: { "show": 100, "hide": 100 },
//});   

$(document).on("mouseover", "[data-toggle='popover']", function () {

    $(this).popover({
        title: $(this).attr('title'),
        html: true,
        delay: { "show": 100, "hide": 100 }

    });
});
//新增好友
$(document).on("click", "button[id='friend_hover']", function () {
    
    let member = $(this).parent().attr('creatorid');
    let mem = { 'memberid': member };
    let btn = $(`img[creatorid=${member}]`);
    console.log(btn);
    $.ajax({
        url: "/Blog/addFriend",
        type: 'post',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(mem),
        success: function () {  
            alert('送出申請');
            console.log(btn);
            let newstr = btn.attr('data-original-title');
            
            newstr = newstr.replace('加入好友', '等待確認'); 
            btn.each(function () {
                console.log(this);
                console.log($(this).attr("data-original-title"));
                $(this).attr("data-original-title", newstr);
                $(this).attr("title", newstr);
            });
           
        }
    });

});


$(document).on("click", "button[id='follower_hover']", function () {
    //console.log(this);

});