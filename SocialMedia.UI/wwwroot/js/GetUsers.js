
$(document).ready(() => {

    LoadData("/Users/GetUsersJson");
    LoadCountries();


    $("#search_button").click(function () {

        var name = $('#name_input').val();
        var lastName = $('#last_name_input').val();
        var email = $('#email_input').val();
        var phone = $('#phone_input').val();
        var country = $('#country_list').val();
        
        var queryString = name != null ? "name=" + name + "&" : null;
        queryString += lastName != null ? "lastName=" + lastName + "&" : null;
        queryString += email != null ? "email=" + email + "&" : null;
        queryString += phone != null ? "phone=" + phone + "&" : null;
        queryString += country != null ? "countryId=" + country  + "&" : null;

        queryString = queryString.substr(0, queryString.length - 1);

  

        LoadData("/Users/GetUsersJson" + "?" + queryString);
        
    });

});

function LoadData(url) {
    $("#search_button").prop("disabled", true)
    $.ajax({
        // la URL para la petición
        url: url,

        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        /* data: { id: 123 },*/

        // especifica si será una petición POST o GET
        type: 'GET',

        // el tipo de información que se espera de respuesta, jquery serializa a javascript object
        dataType: 'json',

        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (json) {
            var content = "";
           

            var obj0 = json.result;
            if (obj0 == "Ok") {
                var obj = json.data.data;
                var meta = json.data.meta;
                

                for (var i = 0; i < obj.length; i++) {
                    content += '<div class="card mb-3 mr-3" style="float:left; width: 14rem;">' +
                        '<input type="hidden" value=' + obj[i].id + '></input>'+
                        '<img src =' + '"' + obj[i].photoRoute + '"' + 'class="card-img-top" height="200px" alt = "..." >' +
                        '<div class="card-body">' +
                        '<h5 class="card-title">' + obj[i].name + ' ' + obj[i].lastname + '</h5>' +
                        '<p class="card-text">' + obj[i].email + '</p>' +
                        '<p class="card-text">' + obj[i].phone + '</p>' +
                        '<p class="card-text">' + new Date(obj[i].birthDate).toDateString() + '</p>' +
                        '<p class="card-text">' + obj[i].countryName + '</p>' +
                        '<a href="#" class="btn btn-primary mb-2">More details</a>' +
                        '<a href="#" class="btn btn-primary">Check my posts</a>' +
                        '</div></div>'
                }

                $("#load_data").html(content);

                var state_previous = (meta.hasPrevious != true) ? 'disabled' : '';
                var state_next = (meta.hasNext != true) ? 'disabled' : '';

                //var next_function = 'LoadData("/Users/GetUsersJson?' + 'PageNumber=' + parseInt(meta.currentPage + 1) + '");';
                //var previous_function = 'LoadData("/Users/GetUsersJson?' + 'PageNumber=' + parseInt(meta.currentPage - 1) + '");';
                var next_function = 'LoadData("/Users/GetUsersJson' + meta.nextPageUrl + '");';
                var previous_function = 'LoadData("/Users/GetUsersJson' + meta.previousPageUrl + '");';



                var paging_content = '<p class="mb-1"> Page ' + meta.currentPage + ' of ' + meta.totalPages + ' | Showing ' + obj.length + ' of ' + meta.itemsCount + ' users.</p>';
                paging_content += '<br/>' +
                    '<nav aria-label="Page navigation example"><ul class="pagination">' +
                    '<button id="previous_button" type="button" onclick=' + previous_function + ' class="btn btn-md btn-primary" ' + state_previous + '>Previous</button>' +
                    '<button id="next_button" type="button" onclick=' + next_function + ' class="btn btn-md btn-primary ml-1" ' + state_next + '>Next</button>'
                '</ul ></nav > '


                $("#paging_data").html(paging_content);
            }
            else {
                $("#load_data").html('<div class="alert alert-danger">Sorry no data found.</div>');
                $("#paging_data").html('');
            }
            $("#search_button").prop("disabled", false)
        },

        // código a ejecutar si la petición falla;
        // son pasados como argumentos a la función
        // el objeto jqXHR (extensión de XMLHttpRequest), un texto con el estatus
        // de la petición y un texto con la descripción del error que haya dado el servidor
        error: function (jqXHR, status, error) {

        },

        // código a ejecutar sin importar si la petición falló o no
        //complete: function (jqXHR, status) {
        //    alert('Petición realizada');
        //}
    });




}

function LoadCountries() {
    $.ajax({
        url: "/Country/GetCountries",
        type: "GET",
        dataType: 'json',
        success: function (json) {
            var countries = json.data;
            var array = countries.data;
            var options = "";
            for (var i = 0; i < array.length; i++) {
                options += '<option value="' + array[i].id + '" >' + array[i].name + '</option>';
             
            }
            $("#country_list").append(options);
        }
            
    });
}


