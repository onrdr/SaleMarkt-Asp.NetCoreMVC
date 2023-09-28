$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/product/getall' },
        "columns": [
            { data: 'title', "width": "25%" },
            {
                data: 'description',
                "width": "20%",
                "render": function (data) {
                    if (data.length > 20) {
                        return data.substr(0, 20) + '...';
                    } else {
                        return data;
                    }
                }
            },
            { data: 'isbn', "width": "15%" },
            { data: 'author', "width": "20%" },
            { data: 'listPrice', "width": "10%" },
            { data: 'category.name', "width": "15%" },
            {
                data: 'id', 
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                     <a href="/product/edit?id=${data}" class="btn btn-sm btn-warning mx-2"> <i class="bi bi-pencil-square"></i></a>               
                     <a onClick=Delete('/product/delete/${data}') class="btn btn-sm btn-danger mx-2"> <i class="bi bi-trash-fill"></i></a>
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
