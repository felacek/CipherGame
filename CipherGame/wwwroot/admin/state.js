const url = "https://ptrk.azurewebsites.net/api/gameStates";

function list() {
  $("#list").html("");

  $.get(url, data => {
    for (let i = 0; i < data.length; i++) {
      $("#list").append(
        "<p>TeamCode: " +
          data[i].teamCode +
          " CipherCode: " +
          data[i].cipherCode +
          " IsPlaceFound: " +
          data[i].isPlaceFound +
          " IsAnswerFound: " +
          data[i].isAnswerFound +
          "</p><br />"
      );
    }
  }).fail(function(jqXHR, textStatus, errorThrown) {
    if (jqXHR.status == "401") {
      window.location.href = "./index.html";
    }
    alert(textStatus);
  });
}

function gen() {
  $.post(url, () => {
    list();
  }).fail(function(jqXHR, textStatus, errorThrown) {
    if (jqXHR.status == "401") {
      window.location.href = "./index.html";
    }
    alert(textStatus);
  });
}

function remove() {
  if (confirm("Are you sure?")) {
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
}

$(document).ready(() => {
  $.ajaxSetup({ xhrFields: { withCredentials: true } });
});
