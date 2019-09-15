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
  });
}

function gen() {
  $.post(url, () => {
    list();
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
    });
  }
}
