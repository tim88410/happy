//*****詳細發文*****
$(document).ready(function () {
    loadpost();
    function loadpost() {
        $.ajax({
            url: "/Blog/loadpost",
            type: 'get',
            success:
                function (data) {

                    var poststr = JSON.parse(data);
                    for (var i = 0; i < poststr.length; i++) {
                        $("#first_article").after(poststr[i]);
                    }
                }
        });
    }
});
//當input接收檔案，檔案就是this傳到readURL方法中
$("#detail_imgPost").change(function () {
    readdetailURL(this);
});
//將input的檔案逐一開啟，並加到網頁上
function readdetailURL(input) {
    if (input.files && input.files[0]) {
        $(input.files).each(function () {
            if ((this).type.match(/image.*/)) {
                let reader = new FileReader();
                reader.readAsDataURL(this);
                reader.onload = function (e) {
                    let str = `<img class="w-75" src="${e.target.result}"/>`;
                    $("#postContent").append(str);
                };
            }
            else if ((this).type.match(/video.*/)) {
                let reader = new FileReader();
                reader.readAsDataURL(this);
                reader.onload = function (e) {
                    let str = `<div class="description"><video src="${e.target.result}"></video></div>`;
                    $("#postContent").append(str);
                };
            }
        });
    }
    //如果傳過來的是<img>
    else if (input.src.length !== 0) {
        let str = '<div class="alert d-inline-block " role="alert">' +
            `<img src="${input.src}" style="width:50px" />` +
            '<button type="button" class="close" data-dismiss="alert" aria-label="Close">' +
            '<span aria-hidden="true">&times;</span></button></div>';
        $("#pre_postArea").append(str);
    }
}





//*****簡單發文*****
//當input接收檔案，檔案就是this傳到readURL方法中
$("#pre_imgPost").change(function () {
    readURL(this);
});
//將input的檔案逐一開啟，並加到網頁上
function readURL(input) {
    if (input.files && input.files[0]) {
        $(input.files).each(function () {
            if ((this).type.match(/image.*/)) {
                let reader = new FileReader();
                reader.readAsDataURL(this);
                reader.onload = function (e) {
                    let str = '<div class="alert d-inline-block " role="alert">' +
                        `<img src="${e.target.result}" style="width:50px"/>` +
                        '<button type="button" class="close" data-dismiss="alert" aria-label="Close">' +
                        '<span aria-hidden="true">&times;</span></button></div>';
                    $("#pre_postArea").append(str);
                };
            }
            else if ((this).type.match(/video.*/)) {
                let reader = new FileReader();
                reader.readAsDataURL(this);
                reader.onload = function (e) {
                    let str = '<div class="alert d-inline-block " role="alert">' +
                        `<video src="${e.target.result}" style="width:50px"></video>` +
                        '<button type="button" class="close" data-dismiss="alert" aria-label="Close">' +
                        '<span aria-hidden="true">&times;</span></button></div>';
                    $("#pre_postArea").append(str);
                };
            }
        });
    }
    //如果傳過來的是<img>
    else if (input.src.length !== 0) {
        let str = '<div class="alert d-inline-block " role="alert">' +
            `<img src="${input.src}" style="width:50px" />` +
            '<button type="button" class="close" data-dismiss="alert" aria-label="Close">' +
            '<span aria-hidden="true">&times;</span></button></div>';
        $("#pre_postArea").append(str);
    }
}
//使用鏡頭功能
function TakePicture() {
    //$("#screenshotModal").modal('show');
    if (
        !"mediaDevices" in navigator ||
        !"getUserMedia" in navigator.mediaDevices
    ) {
        alert("Camera API is not available in your browser");
        return;
    }

    // get page elements
    const video = document.querySelector("#video");
    const canvas = document.querySelector("#canvas");

    // video constraints
    const constraints = {
        video: {
            width: {
                min: 1280,
                ideal: 1920,
                max: 2560,
            },
            height: {
                min: 720,
                ideal: 1080,
                max: 1440,
            },
        },
    };

    // use front face camera
    let useFrontCamera = true;

    // current video stream
    let videoStream;

    // handle events
    // play
    $("#btnPlay").one("click", function () {
        video.play();
    });

    // cancel
    $("#btnCancel").one("click", function () {
        stopVideoStream();
        $("#btnScreenshot").off();
        $("#btnPlay").off();
        $("#btnChangeCamera").off();
        //$("#screenshotModal").modal('hide');
    });

    // take screenshot
    $("#btnScreenshot").one("click", function () {
        let img = document.createElement("img");
        canvas.width = video.videoWidth;
        canvas.height = video.videoHeight;
        canvas.getContext("2d").drawImage(video, 0, 0);
        img.src = canvas.toDataURL("image/png");
        readURL(img);
        //stopVideoStream();
        $("#btnCancel").trigger('click');
    });

    // switch camera
    $("#btnChangeCamera").click(function () {
        useFrontCamera = !useFrontCamera;
        initializeCamera();
    });

    // stop video stream
    function stopVideoStream() {
        if (videoStream) {
            videoStream.getTracks().forEach((track) => {
                track.stop();
            });
        }
    }

    // initialize
    async function initializeCamera() {
        stopVideoStream();
        constraints.video.facingMode = useFrontCamera ? "user" : "environment";

        try {
            videoStream = await navigator.mediaDevices.getUserMedia(constraints);
            video.srcObject = videoStream;
        } catch (err) {
            alert("Could not access the camera");
        }
    }

    initializeCamera();
}

//動態註冊btnpostcomment click事件
$(document).on("click", "button[id='post_comment']", function () {
    let postid = $(this).attr('postid');
    let area = $(this).siblings('textarea');
    let baseul = $(this).parents("li")/*.children("li[class='post - comment']")*/;
    let comment = area.val();
    let data = { 'postid': postid, 'comment': comment};

    $.ajax({
        url: "/Blog/postcomment",
        type: 'post',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(data),
        success: function (data) {
            alert('回覆成功');
            var jsondata = JSON.parse(data);
            let str = ` <li class='parrentcomment'>
                                            <div class="comet-avatar">
                     <img src="/images/IdPhoto/${jsondata.photo}" alt="" style='border-radius:50%;width:52px'>
                                            </div>
                                            <div class="we-comment">
                        <div class="coment-head">
                            <h5>
                                <a href="time-line.html" title="">${jsondata.Name}</a>
                            </h5>
                            <span>${jsondata.time}</span>
                            <button id="firstclassreply" class="we-reply" title="Reply" commentid='${jsondata.commentid}'><i class="fa fa-reply"></i></button>
                        </div>
                        <p>${jsondata.comment}</p>
                    </div></li>`;

            baseul.before(str);
        }
    });

});

//動態註冊btnreply click事件
$(document).on("click", "button[id='firstclassreply']", function () {
    let commentid = $(this).attr('commentid');
    let photo = $(this).closest("ul[class='we-comet']").children("li[class='post-comment']").find('img').attr('src');
    console.log(photo);
    let parentcomment = $(this).closest('li');
    let str = `<ul id="childarea"><li class="post-comment"><div class="comet-avatar"><img src=${photo} alt=""></div><div class="post-comt-box"><textarea placeholder="回覆"></textarea><button id="childpost_comment" class="btn btn-danger float-right" type="button" postid=${commentid}>送出留言</button></div></li></ul>`;
    parentcomment.append(str);

    $(document).one("click", "button[id='childpost_comment']", function () {
        let postid = $(this).attr('postid');
        let area = $(this).siblings('textarea');
        let baseul = $(this).parents("ul[id='childarea']");
        let comment = area.val();
        let data = { 'postid': postid, 'comment': comment };
        $(this).parents("li[class='post-comment']").remove();
        $.ajax({
            url: "/Blog/postchildcomment",
            type: 'post',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(data),
            success: function (data) {
                alert('回覆成功');
                var jsondata = JSON.parse(data);
                let str = ` <li class='childcomment'>
                                            <div class="comet-avatar">
                     <img src="/images/IdPhoto/${jsondata.photo}" alt="" style='border-radius:50%;width:52px'>
                                            </div>
                                            <div class="we-comment">
                        <div class="coment-head">
                            <h5>
                                <a href="time-line.html" title="">${jsondata.Name}</a>
                            </h5>
                            <span>${jsondata.time}</span>
                            <button id="secondclassreply" class="we-reply" title="Reply" commentid='${jsondata.commentid}'><i class="fa fa-reply"></i></button>
                        </div>
                        <p>${jsondata.comment}</p>
                    </div></li>`;

                baseul.prepend(str);
            }
        });       
    });
    parentcomment.removeClass("li[class='post-comment']");//移除子評論位置
});





//btnPublish click
$("#btnPublish").click(function () {
    let textArea = $("#postTextarea").val();
    let pre_postArea = $("#pre_postArea").html();
    let data = { "text": textArea, "img": pre_postArea };
    $.ajax({
        url: "/Blog/simplePost",
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        type: "POST",
        success: function (data) {
            var jsonDoc = JSON.parse(data);
            console.log(data);
            alert(jsonDoc);
            let str = ` <!--動態-->
                                <div class="central-meta item">
                                    <div class="user-post">
                                        <div class="friend-info">
                                            <figure>
                                                <img src="/images/IdPhoto/${jsonDoc.fIdPhoto}" alt="">
                                            </figure>
                                            <div class="friend-name">
                                                <ins><a href="time-line.html" title="">${jsonDoc.fName}</a></ins>
                                                <span>發布時間: ${jsonDoc.fPosttime}</span>
                                            </div>
                                            <div class="post-meta">
                                                <div class="description">
                                                    <p>${jsonDoc.text}</p>
                                                    ${jsonDoc.img}
                                                </div>
                                                <div class="we-video-info">
                                                    <ul>
                                                        <li>
                                                            <span class="like" data-toggle="tooltip" title="讚">
                                                                <i class="fas fa-heart"></i>
                                                                <ins>2.2k</ins>
                                                            </span>
                                                        </li>
                                                        <li>
                                                            <a href="#a">
                                                                <span class="comment" data-toggle="tooltip" title="留言">
                                                                    <i class="far fa-comment"></i>
                                                                    <ins>52</ins>
                                                                </span>
                                                            </a>
                                                        </li>
                                                        <li>
                                                            <span class="" data-toggle="tooltip" title="分享">
                                                                <i class="fa fa-share-square"></i>
                                                                <ins>1.2k</ins>
                                                            </span>
                                                        </li>
                                                        <li>
                                                            <span class="" data-toggle="tooltip" title="檢舉">
                                                                <i class="fas fa-flag"></i>
                                                            </span>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                                <!--動態-->`;
            $("#first_article").after(str);
            alert("sucees");
            addUser_post(data);
            $("#postTextarea").val("");
            $("#pre_postArea").html("");
        },
        error: function () {
            //test
            alert('ajax error');
            $("#postTextarea").val();
            $("#pre_postArea").html("");
        }
    });

}); $("#detailpublish").click(function () {
    //let textArea = $("#postTextarea").val();
    let pre_postArea = $("#postContent").html();
    let data = { "detailtext": pre_postArea };
    $.ajax({
        url: "/Blog/detailpost",
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        type: "POST",
        success: function (data) {
            var jsonDoc = JSON.parse(data);
            console.log(data);
            alert(jsonDoc);
            let str = ` <!--動態-->
                                <div class="central-meta item">
                                    <div class="user-post">
                                        <div class="friend-info">
                                            <figure>
                                                <img src="/images/IdPhoto/${jsonDoc.fIdPhoto}" alt="">
                                            </figure>
                                            <div class="friend-name">
                                                <ins><a href="time-line.html" title="">${jsonDoc.fName}</a></ins>
                                                <span>發布時間: ${jsonDoc.fPosttime}</span>
                                            </div>
                                            <div class="post-meta">
                                                <div class="description">
                                                    ${jsonDoc.text}
                                                    
                                                </div>
                                                <div class="we-video-info">
                                                    <ul>
                                                        <li>
                                                            <span class="like" data-toggle="tooltip" title="讚">
                                                                <i class="fas fa-heart"></i>
                                                                <ins>2.2k</ins>
                                                            </span>
                                                        </li>
                                                        <li>
                                                            <a href="#a">
                                                                <span class="comment" data-toggle="tooltip" title="留言">
                                                                    <i class="far fa-comment"></i>
                                                                    <ins>52</ins>
                                                                </span>
                                                            </a>
                                                        </li>
                                                        <li>
                                                            <span class="" data-toggle="tooltip" title="分享">
                                                                <i class="fa fa-share-square"></i>
                                                                <ins>1.2k</ins>
                                                            </span>
                                                        </li>
                                                        <li>
                                                            <span class="" data-toggle="tooltip" title="檢舉">
                                                                <i class="fas fa-flag"></i>
                                                            </span>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                                <!--動態-->`;
            $("#first_article").after(str);
            alert("sucees");
            //addUser_post(data);
            $("#postTextarea").val("");
            $("#pre_postArea").html("");

        },
        error: function () {
            //test
            alert('ajax error');
            $("#postTextarea").val();
            $("#pre_postArea").html("");
        }
    });

});
function addUser_post(data) {
    let postContent = "";
    postContent += "" +
        '<div class="user-post">' +
        '<div class="friend-info">' +
        `<figure><img src="${data.img}" alt=""></figure>` +
        '< div class="friend-name">' +
        `<ins><a href="time-line.html" title="">${data.name}</a></ins>` +
        `<span>published: ${data.date}</span>` +
        '</div >';
    return postContent;
}

//Post modal click
//預覽貼文
function previewPost() {
    $("#postModal").modal('toggle');
    $("#previewModal").modal('toggle');
    let postTitle = $("#postTitle").val();
    let postContent = $("#postContent").html();
    $("#previewTitle").text(postTitle);
    $("#previewContent").html(postContent);
}
//標註朋友
function tagFriend() {
    $("#tagArea").removeClass('d-none');
    $("#tagText").text("與 : ");
    $("#txtTag").attr('type', 'text');
    $("#txtForPoint").attr('type', 'hidden');
    $('.collapse').collapse('hide');

    let availableTags;
    $.ajax({
        url: "/Blog/getAutoCompleteFriendList",
        type: "GET",
        success: function (data) {
            availableTags = data.split(",");
            $("#txtTag").autocomplete({
                source: availableTags,
                appendTo: "#postModal",
                scroll: true,
                select: function (event, ui) {
                    if (ui.item) {
                        let str = '<div class="alert d-sm-inline-block" contentEditable="false" role="alert">' +
                            //'<button type="button" class="close" data-dismiss="alert" aria-label="Close" contentEditable="false">' +
                            //`<span aria-hidden="true" contentEditable="false" >&times;</span></button>`+
                            `<a href="#" contentEditable="false" style="color:blue">@${ui.item.value}</a></div>`;
                        $('#postContent').append(str);
                        $("#txtTag").val(''); //TODO clear input ,no working
                        return false; //clear input works
                    }
                }
            });
        },
        error: function () {
            //test
            alert("aaa");
        }
    });
}
//標註商品
function tagProduct() {
    $("#tagArea").removeClass('d-none');
    $("#tagText").text("商品 : ");
    $("#txtTag").attr('type', 'text');
    $("#txtForPoint").attr('type', 'hidden');
    $('.collapse').collapse('hide');

    let availableTags;
    $.ajax({
        url: "/Blog/getAutoCompleteFriendList",
        type: "GET",
        success: function (data) {
            availableTags = data.split(",");
            $("#txtTag").autocomplete({
                source: availableTags,
                appendTo: "#postModal",
                scroll: true,
                select: function (event, ui) {
                    if (ui.item) {
                        let str = '<div class="alert d-sm-inline-block" contentEditable="false" role="alert">' +
                            //'<button type="button" class="close" data-dismiss="alert" aria-label="Close" contentEditable="false">' +
                            //`<span aria-hidden="true" contentEditable="false" >&times;</span></button>`+
                            `<a href="#" contentEditable="false" style="color:blue">@${ui.item.value}</a></div>`;
                        $('#postContent').append(str);
                        $("#txtTag").val('');
                        return false;
                    }
                }
            });
        },
        error: function () {
            //test
            alert("aaa");
        }
    });
}
//加入標籤
function addTag() {
    $("#tagArea").removeClass('d-none');
    $("#tagText").text("標籤 : ");
    $("#txtTag").attr('type', 'text');
    $("#txtForPoint").attr('type', 'hidden');
    $('.collapse').collapse('hide');

    let availableTags;
    $.ajax({
        url: "/Blog/getAutoCompleteFriendList",
        type: "GET",
        success: function (data) {
            availableTags = data.split(",");
            $("#txtTag").autocomplete({
                source: availableTags,
                appendTo: "#postModal",
                scroll: true,
                select: function (event, ui) {
                    if (ui.item) {
                        let str = `<a href="#" contentEditable="false" style="color:blue">#${ui.item.value}</a>`;
                        $('#postContent').append(str);
                        $("#txtTag").val('');
                        return false;
                    }
                }
            });
            $("#txtTag").keydown(function (e) {
                if (e.keyCode === 13) {
                    let hashtag = $("#txtTag").val();
                    let str = `<a href="#" contentEditable="false" style="color:blue">#${hashtag} </a>`;
                    $('#postContent').append(str);
                    $("#txtTag").val('');
                    return false;
                }
            });
        },
        error: function () {
            //test
            alert("aaa");
        }
    });
}
//標註地點
function tagLocation() {
    $('#tagArea').removeClass('d-none');
    $('#tagText').text('地點 : ');
    $("#txtTag").attr('type', 'hidden');
    $('.collapse').collapse('hide');
    $("#txtForPoint").attr('type', 'text');
}
let inputPoint = document.getElementById('txtForPoint');
let autoPoint = new google.maps.places.Autocomplete(inputPoint);

// START of place_changed
autoPoint.addListener('place_changed', function () {
    let placePoint = autoPoint.getPlace(); //直接從autocomplete的結果取地點
    let placeNamePoint = placePoint.name;

    let srcstrPoint = "https://www.google.com/maps/embed/v1/place?key=AIzaSyD-jeraue0BQAE3-nHBELy0mrTXDPbPO7I&q=" + placeNamePoint;
    let strPoint = `<a href="#" contentEditable="false" style="color:blue">在  ${placeNamePoint} </a></br>` + `<iframe width="400" height="250" frameborder="0" style="border:0" src=${srcstrPoint} allowfullscreen ></iframe>`;
    $('#postContent').append(strPoint);
    $('#txtForPoint').val('');
});
//加入地圖
function addMap() {
    $('#tagArea').removeClass('d-none');
    $('#tagText').text('');
    $("#txtTag").attr('type', 'hidden');
    $("#txtForPoint").attr('type', 'hidden');
    $("#mapShow").empty();
    $("#mapShow").append('<iframe width="400" height="250" frameborder="0" style="border:0" src="https://www.google.com/maps/embed/v1/place?key=AIzaSyD-jeraue0BQAE3-nHBELy0mrTXDPbPO7I&q=資策會數位教育研究所數位人才培育中心" allowfullscreen ></iframe>');
}
let inputDraw = document.getElementById('txtForDraw');
autoDraw = new google.maps.places.Autocomplete(inputDraw);
let placeNameDraw = null;
// START of place_changed
autoDraw.addListener('place_changed', function () {
    $('#mapShow').empty();

    let placeDraw = autoDraw.getPlace(); //直接從autocomplete的結果取地點
    placeNameDraw = placeDraw.name;

    let srcstrDraw = "https://www.google.com/maps/embed/v1/place?key=AIzaSyD-jeraue0BQAE3-nHBELy0mrTXDPbPO7I&q=" + placeNameDraw;
    let strDraw = `<iframe width="400" height="250" frameborder="0" style="border:0" src=${srcstrDraw} allowfullscreen ></iframe>`;
    $('#mapShow').append(strDraw);
    $('#txtForDraw').val('');
});
// END of place_changed

let list = [];
$('#placeAdd').on('click', function () {
    if (placeNameDraw !== null) {
        let waypt = `<span>${placeNameDraw} <button class="btn btn-danger mt-1" place="${placeNameDraw}">移除</button><span>`;
        $('#waypoints').append(waypt);
        list.push(placeNameDraw);
    }
});

//移除標點 動態產生=>動態綁定
$('#waypoints').on('click', '.btn-danger', function () {
    let removep = $(this).attr('place');
    let index;
    for (let item of list) {
        if (item === removep) {
            index = list.indexOf(item);
            break;
        }
    }
    list.splice(index, 1);
    $(this).closest('span').remove();
});


//繪製
$('#drawing').on('click', function () {
    $('#mapShow').empty();

    //去除名稱中空白
    for (let i = 0; i < list.length; i++) {
        list[i] = list[i].replace(/ /ig, '');
    }

    //拆分起訖點和中途點
    let wayptstr = '';
    if (list.length > 2) {
        for (let i = 1; i < list.length - 1; i++) {
            wayptstr += list[i] + '|';
        }
        wayptstr = wayptstr.substring(0, wayptstr.length - 1);
    }

    let mode = $('input[name="mode"]:checked').val();

    let srcstr;
    if (wayptstr === '')
        srcstr = "https://www.google.com/maps/embed/v1/directions?key=AIzaSyD-jeraue0BQAE3-nHBELy0mrTXDPbPO7I&origin=" + list[0] + `&destination=${list[list.length - 1]}&mode=${mode}`;
    else
        srcstr = "https://www.google.com/maps/embed/v1/directions?key=AIzaSyD-jeraue0BQAE3-nHBELy0mrTXDPbPO7I&origin=" + list[0] + `&waypoints=${wayptstr}&destination=${list[list.length - 1]}&mode=${mode}`;


    let str = `<iframe width="400" height="250" frameborder="0" style="border:0" src=${srcstr} allowfullscreen ></iframe>`;
    $('#postContent').append(str);
    $('#waypoints').empty();
    list = [];
});