var dataTable;

$(document).ready(function () {
  loadDataTable();
});

function loadDataTable() {
  dataTable = $("#tblData").DataTable({
    ajax: { url: "/admin/company/getall" },
    columns: [
      { data: "name", width: "25%" },
      { data: "streetAddress", width: "10%" },
      { data: "city", width: "15%" },
      { data: "state", width: "10%" },
      { data: "phoneNumber", width: "10%" },
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
  fetch(`/admin/company/delete/${id}`, {
    // method: "DELETE",
  })
    .then(() => dataTable.ajax.reload())
    .catch((error) => console.error("Unable to delete company.", error));
}
