﻿let dataTable;

$(document).ready(function () {
    loadDataTable(); 
});

function loadDataTable() {
    dataTable = $('#tblOrder').DataTable({
        "ajax": { url: '/order/getall' },
        "columns": [
            { data: 'id', "width": "20%" },
            { data: 'name', "width": "20%" },
            { data: 'phoneNumber', "width": "15%" },
            { data: 'appUser.email', "width": "15%" },
            { data: 'orderStatus', "width": "10%" }, 
            { data: 'paymentStatus', "width": "10%" }, 
            { data: 'orderTotal', "width": "5%" },
            {
                data: 'id',
                "width": "5%",
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                     <a href="/order/details?orderHeaderId=${data}" class="btn btn-sm btn-warning mx-2 rounded"> <i class="bi bi-pencil-square"></i></a>          
                    </div>`
                }
            },
        ]
    });
} 

function loadData(buttonId, textToSearch) {
    $("#pending, #completed, #approved, #all").removeClass("btn-info text-white");
    $("#" + buttonId).addClass("btn-info text-white");
    dataTable.search(textToSearch).draw();
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
                success: function (result) { 
                    dataTable.ajax.reload();
                    window.location.href = result.redirectTo;                    
                }
            })
        }
    })
}