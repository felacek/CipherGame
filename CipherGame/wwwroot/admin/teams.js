const url = "https://ptrk.azurewebsites.net/api/Teams";

function list() {
  $("#list").html("");

  $.get(url, data => {
    for (let i = 0; i < data.length; i++) {
      $("#list").append(
        "<p>Code: " +
          data[i].code +
          " Name: " +
          data[i].name +
          "</p><button onclick='remove(`" +
          data[i].code +
          "`)'>Delete Team</button><br />"
      );
    }
  }).fail(function(jqXHR, textStatus, errorThrown) {
    if (jqXHR.status == "401") {
      window.location.href = "./index.html";
    }
    alert(textStatus);
  });
}

function add() {
  let obj = {};
  obj.code = $("#addcode").val();
  $("#addcode").val("");
  obj.name = $("#addname").val();
  $("#addname").val("");

  $.ajax({
    type: "POST",
    url: url,
    data: JSON.stringify(obj),
    success: data => {
      alert("added user with code " + data.code + " and name " + data.name);
      list();
    },
    contentType: "application/json",
    dataType: "json"
  }).fail(function(jqXHR, textStatus, errorThrown) {
    if (jqXHR.status == "401") {
      window.location.href = "./index.html";
    }
    alert(textStatus);
  });
}

function remove(code) {
  $.ajax({
    type: "DELETE",
    url: url.concat("/" + code),
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
  $.ajaxSetup({ xhrFields: { withCredentials: true } });
});