const url = "https://ptrk.azurewebsites.net/api/Ciphers";

function list() {
  $("#list").html("");

  $.get(url, data => {
    for (let i = 0; i < data.length; i++) {
      $("#list").append(
        "<p>Code: " +
          data[i].code +
          " Instruction: " +
          data[i].place +
          "</p><button onclick='remove(`" +
          data[i].code +
          "`)'>Delete Cipher</button><br />"
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
  obj.place = $("#addplace").val();
  $("#addplace").val("");
  obj.answer = $("#addanswer").val();
  $("#addanswer").val("");

  $.ajax({
    type: "POST",
    url: url,
    data: JSON.stringify(obj),
    success: data => {
      alert(
        "added cipher with code " + data.code + " and instruction " + data.place
      );
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
