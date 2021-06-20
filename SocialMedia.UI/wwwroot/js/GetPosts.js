$(document).ready(() => {

    LoadData("GetJsonPosts");


});

function LoadData(url) {
    $("#search_button").prop("disabled", true)
    $.ajax({
        url: url,
        type: 'GET',
        dataType: 'json',

        success: function (json) {
            var content = "";
            var obj = json.data.data;
            var meta = json.data.meta;
            var obj0 = json.result;
            console.log(obj)
            if (obj0 == "Ok") {
                for (var i = 0; i < obj.length; i++) {
                    content += '<div class="card mb-3" style="max-width: 90%;">' +
                        '<div class="row g-0"> <div class="col-md-2"><img height="150" src="' + obj[i].profilePhoto + '" alt="..."></div> <div class="col-md-10">' +
                        '<div class="card-body pt-1"><p id="post_Text"><h5 id="username_Post" class="card-title">' + obj[i].name + '</h5></p>' +
                        '<div class="col-12 mb-3 ml-1 text-justify">' + obj[i].description + "</div>";
                    var comments = obj[i].comments;
                    if (comments != null)
                        for (var j = 0; j < comments.length; j++) {
                            content += '<div class="card ml-3 " style="width: 97%;"><div class="card-body p-1"><div class="col-md-2"></div><div class="col-md-10">' +
                                '<h5 id="username_Comment" class="card-title">' + comments[j].smUserName + ' - ' + new Date(comments[j].date).toLocaleDateString() + '</h5>' +
                                '<p id="comment_Text" class="card-text">' + comments[j].description + '</p >' +
                                '</div></div></div></div>';

                        }





                    content += ' <input id="postid" type="hidden" value="' + obj[i].postId + '" /><button type="button" class="btn btn-primary m-2" data-toggle="modal" data-target="#exampleModal">' +
                        'Leave a comment</button> </div></div></div>';

                }

                $("#load_data").html(content);


            }
            else {
                $("#load_data").html('<div class="alert alert-danger">Sorry no data found.</div>');
                $("#paging_data").html('');
            }
            $("#search_button").prop("disabled", false)
        },

        error: function (jqXHR, status, error) {

        },
    });






}