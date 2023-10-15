$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    console.log('loadDataTable function called');
    dataTable = $('#tblUsers').DataTable({
        "order": [],
        "ajax": { url: '/superadmin/getallusers' },
        "columns": [
            {
                data: 'name',
                "width": "20%",
                render: function (data, type, row) {
                    if (row.roles.includes('SuperAdmin')) {
                        return `<div class="w-100" role="group">
                                <span class="btn btn-success disabled">${data}</span>
                                </div>`
                    } else {
                        return data;
                    }
                }
            },
            { data: 'email', "width": "25%" },
            { data: 'address', "width": "%35" },
            {
                data: 'roles',
                "width": "10%",
                render: function (data) {
                    var badges = data.map(function (role) {
                        return '<span class="badge bg-info rounded">' + role + '</span>';
                    });
                    return badges.join(' ');
                }
            },
            {
                data: 'id',
                width: '20%',
                render: function (data, type, row) {
                    if (row.roles.includes('SuperAdmin')) { return null; }
                    if (row.isSuspended) {
                        return `<div class="w-100 btn-group" role="group">

                                <a href="/superadmin/RoleAssign?userId=${data}" class="w-50 btn btn-sm btn-warning mx-2 rounded">
                                    <i class="bi bi-pencil-square"></i> Role</a>

                                <a onClick="BlockOrUnblock('/superadmin/changesuspendstatus?userId=${data}&isSuspended=false')" 
                                    class="w-50 btn btn-sm btn-success rounded"><i class="bi bi-unlock-fill"></i> Unblock</a>

                                </div>`
                    }
                    return `<div class="w-100 btn-group" role="group">

                            <a href="/superadmin/RoleAssign?userId=${data}" class="w-50 btn btn-sm btn-warning mx-2 rounded">
                            <i class="bi bi-pencil-square"></i> Role</a>

                            <a onClick="BlockOrUnblock('/superadmin/changesuspendstatus?userId=${data}&isSuspended=true')" 
                                class="w-50 btn btn-sm btn-danger rounded"><i class="bi bi-lock-fill"></i> Block</a>

                            </div>` 
                }
            }
        ]
    });
}

function BlockOrUnblock(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, I am sure!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'POST',
                success: function (data) {
                    if (data && data.result) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    } else if (data && !data.result) {
                        toastr.error(data.message);
                    } else {
                        toastr.error('An unexpected error occurred.');
                    }
                }
            });
        }
    });
}
