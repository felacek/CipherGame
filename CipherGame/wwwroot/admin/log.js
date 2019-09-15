const url = "https://ptrk.azurewebsites.net/api/auditlogs";

function list() {
  $("#list").html("");

  $.get(url, data => {
    for (let i = 0; i < data.length; i++) {
      $("#list").append("<p>" + data[i].created + data[i].log + "</p><br />");
    }
  }).fail(function(jqXHR, textStatus, errorThrown) {
    if (jqXHR.status == "401") {
      window.location.href = "./index.html";
    }
    alert(textStatus);
  });
}

function remove() {
  $.ajax({
    type: "DELETE",
    url: url,
    success: () => {
      list();
    }
  }).fail(function(jqXHR, textStatus, errorThrown) {
    if (jqXHR.status == "401") {
      window.location.href = "./index.html";
    }
    alert(textStatus);
  });
}

$(document).ready(() => {
  $.ajaxSetup({ xhrFields: { withCredentials: true }, crossDomain: true });
});
