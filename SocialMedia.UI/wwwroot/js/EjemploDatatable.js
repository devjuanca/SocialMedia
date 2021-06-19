//dataTable = $("#users_table").DataTable({
    //    //processing: true,
    //    //serverSide: true,
    //    //searching: true,
    //    filtering:false,
    //    paging: true,
    //    pageLength: 50,
    //    responsive: true,
    //    lengthMenu: [[5, 10, 20, 50, 100, -1], [5, 10, 20, 50, 100, "Todos"]],

    //    ajax: {
    //        url: "/Users/GetUsersJson",
    //        type: "GET",
    //        datatype: "json"
    //    },
    //    columns: [
    //        { data: "id", name: "Id", visible: false, searchable: false, orderable: false },
    //        { data: "photoName", width: "20%" },
    //        { data: "name", "width": "20%" },
    //        { data: "lastname", "width": "20%" },
    //        { data: "birthDate", "width": "20%" },
    //        { data: "email", "width": "20%" },
    //        { data: "phone", "width": "20%" }
    //    ],
    //    columnDefs: [
    //        {
    //            targets: 1,
    //            render: function (data) {
    //                return '<img src="' + data + '" width="200px" height="200px" />';
    //            }
    //        },
    //        {
    //            targets: 4,
    //            render: function (data) {
    //                let date = new Date(data);
    //                return date.toDateString();
    //            }
    //        }
    //    ],

    //    order: [[1, "desc"]],
    //    select: {
    //        style: "single",
    //        info: false
    //    },
    //    createdRow: function (row, data, index) {
    //        $("td", row).addClass("align-middle");

    //    }
    //});

    //$("input[type=search]").hide(); // ocultar input de busqueda del datatable.