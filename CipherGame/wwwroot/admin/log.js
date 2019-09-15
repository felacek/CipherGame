const url = "https://ptrk.azurewebsites.net/api/auditlogs";

function list() {
  $("#list").html("");

  $.get(url, data => {
    for (let i = 0; i < data.length; i++) {
      $("#list").append("<p>" + data[i].created + data[i].log + "</p><br />");
    }
  });
}

function remove() {
    $.ajax({
      type: "DELETE",
      url: url,
      success: () => {
        list();
      }
    });
  }