$(document).ready(() => {


    var user_id = getParameterByName("userId")

    if (user_id == null)
        LoadData("GetJsonPosts");
    else {
      
        LoadData("/SocialPost/GetJsonPosts" + "?userId=" + user_id);
    }



    $("#comment_Button").click(function () {
       
        var text = $("#commentText").val();
        var post_id = $("#PostValue").val();
       
        if (text != '') {
          
            $.ajax({
                url: "/Comment/AddComment",
                type: 'POST',
                dataType: 'json',
                data: {
                    PostId: post_id,
                    Description: text
                },
                success: function (json) {
                    $('#exampleModal').modal('toggle');
                    LoadData("GetJsonPosts");
                }
            });
        }
        else {
            console.log(text)
            $("#validation_div").removeClass("d-none")
            $("#validation_div").addClass("d-block")
        }

    });

    $("#search_button").click(function () {

        var description = $('#description_input').val();
        var date = $('#date_input').val();


        var queryString = user_id != null ? "userId=" + user_id + "&" : null;

        queryString += description != null ? "descriptionSearch=" + description + "&" : null;

        queryString += date != null ? "date=" + date + "&" : null;

        queryString = queryString.substr(0, queryString.length - 1);


        LoadData("/SocialPost/GetJsonPosts" + "?" + queryString);


    });
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
          
            if (obj0 == "Ok") {
                for (var i = 0; i < obj.length; i++) {
                    content += '<div class="card mb-3" style="max-width: 90%;">' +
                        '<div class="row g-0"> <div class="col-md-2"><img height="150" src="' + obj[i].profilePhoto + '" alt="..."></div> <div class="col-md-10">' +
                        '<div class="card-body pt-1"><p id="post_Text"><h5 id="username_Post" class="card-title">' + obj[i].name + ' - ' +
                        new Date(obj[i].date).toDateString() + '</h5></p>' +
                        '<div class="col-12 mb-3 ml-1 text-justify">' + obj[i].description + "</div>";
                    var comments = obj[i].comments;
                    if (comments != null)
                        for (var j = 0; j < comments.length; j++) {
                            content += '<div class="card ml-3 mb-3 " style="width: 97%;"><div class="card-body p-1"><div class="col-md-2"></div><div class="col-md-10">' +
                                '<h5 id="username_Comment" class="card-title">' + comments[j].smUserName + ' - ' + new Date(comments[j].date).toLocaleDateString() + '</h5>' +
                                '<p id="comment_Text" class="card-text">' + comments[j].description + '</p >' +
                                '</div></div></div>';

                        }

                    content += ' <input id="postid" type="hidden" value="' + obj[i].postId + '" />' +
                        '<button type="button" class="btn btn-primary m-2" data-toggle="modal" data-target="#exampleModal"' +
                        ' onclick = ' + 'ModalAndPostCod(' + obj[i].postId+')>'+
                        'Leave a comment</button> </div></div></div></div>';

                }

                var state_previous = (meta.hasPrevious != true) ? 'disabled' : '';
                var state_next = (meta.hasNext != true) ? 'disabled' : '';

                var next_function = 'LoadData("/SocialPost/GetJsonPosts' + meta.nextPageUrl + '");';
                var previous_function = 'LoadData("/SocialPost/GetJsonPosts' + meta.previousPageUrl + '");';

                var paging_content = '<p class="mb-1"> Page ' + meta.currentPage + ' of ' + meta.totalPages + ' | Showing ' + obj.length + ' of ' + meta.itemsCount + ' users.</p>';
                paging_content += '<br/>' +
                    '<nav aria-label="Page navigation example"><ul class="pagination">' +
                    '<button id="previous_button" type="button" onclick=' + previous_function + ' class="btn btn-md btn-primary" ' + state_previous + '>Previous</button>' +
                    '<button id="next_button" type="button" onclick=' + next_function + ' class="btn btn-md btn-primary ml-1" ' + state_next + '>Next</button>'
                '</ul ></nav > '


                $("#paging_data").html(paging_content);


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

function ModalAndPostCod(cod) {
    $("#PostValue").val(cod);
}

function getParameterByName(name, url = window.location.href) {
    name = name.replace(/[\[\]]/g, '\\$&');
    var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, ' '));
}
