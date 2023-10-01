﻿$(document).ready(function () {
    loadDataTable();
}); 

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/category/getall' },
        "columns": [   
            {
                data: 'imageUrl',
                "width": "15%",
                "render": function (data) {
                    return `<div>
                     <img class="p-3" src="${data}" style="width:100%; border-radius:5px; border:1px solid #ffffff" />
                    </div>`;
                }
            },
            { data: 'id', "width": "20%"},
            { data: 'name', "width": "20%" },
            { data: 'description', "width": "20%" },
            { data: 'displayOrder', "width": "25%" },
            {
                data: 'id',
                "width": "20%",
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                     <a href="/category/edit?id=${data}" class="btn btn-sm btn-warning mx-2 rounded"> <i class="bi bi-pencil-square"></i></a>               
                     <a onClick=Delete('/category/delete/${data}') class="btn btn-sm btn-danger mx-2 rounded"> <i class="bi bi-trash-fill"></i></a>
                    </div>`
                }
            },
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    })
}