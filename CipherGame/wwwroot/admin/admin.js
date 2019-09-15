const url = "https://ptrk.azurewebsites.net/api/auth";

function login() {
  const password = $("#word").val();
  $("#word").val("");

  $.ajax({
    type: "POST",
    url: url,
    data: { password: password },
    success: () => {
        $("#msg").html("Přihlášení proběhlo úspěšně");
    }
  }).fail((jqXHR, textStatus, errorThrown) => {
    if (jqXHR.status == "400") {
      $("#msg").html("Špatné heslo");
    }
  });
}
