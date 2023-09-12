var dataTable;

$(document).ready(function () {
  loadDataTable();
});

function loadDataTable() {
  dataTable = $("#tblData").DataTable({
    ajax: { url: "/admin/company/getall" },
    columns: [
      { data: "name", width: "15%" },
      { data: "streetAddress", width: "25%" },
      { data: "city", width: "10%" },
      { data: "state", width: "5%" },
      { data: "phoneNumber", width: "15%" },
      {
        data: "id",
        render: function (data) {
          return `
              <div class="w-75 btn-group" role="group">
                <a href="/admin/company/upsert?id=${data}" class="btn btn-primary mx-2">
                  <i class="bi bi-pencil-square"></i> Edit
                </a>
                
                
                <a onclick=DeleteItem(${data}) class="btn btn-danger mx-2">
                  <i class="bi bi-trash-fill"></i> Delete
                </a>
              </div>
            `;
        },
        width: "30%",
      },
    ],
  });
}

function DeleteItem(id) {
  const isDelete = confirm("Are you sure you want to delete?");
  if (isDelete) {
    fetch(`/admin/company/delete/${id}`)
      .then(() => {
        dataTable.ajax.reload();
        toastr.success("Company deleted successfully");
      })
      .catch((error) => console.error("Unable to delete company.", error));
  }
}
