let state = {
  teamCode: "",
  teamName: "",
  message: "",
  cipherPlace: "",
  result: "",
  isPlaceFound: false
};

const url = "https://ptrk.azurewebsites.net/api/game";

function signIn() {
  //get team code
  state.teamCode = $("#teamcode")
    .val()
    .toUpperCase();

  //get team data
  $.ajax({
    url: url,
    type: "POST",
    data: { teamCode: state.teamCode },
    success: data => {
      state.teamName = data.teamName;
      state.message = data.message;
      state.isPlaceFound = data.isPlaceFound;

      //write message
      sendMessage();

      //if successful
      if (state.teamName !== "") {
        //set team name
        $("#teamcode").html("");
        $(".teamname").html("Přihlášen tým " + state.teamName);

        //make cookie for easier signin next time
        document.cookie = "teamCode="+state.teamCode+";max-age=7200";

        //go to different webpage
        if (state.isPlaceFound) {
          goToResult();
        } else {
          goToPlace();
        }
      }
    }
  });
}

function checkPlace() {
  //get input
  state.cipherPlace = $("#placecode")
    .val()
    .toUpperCase();

  //get team data
  $.ajax({
    url: url.concat("/SetPlaceCode"),
    type: "POST",
    data: { teamCode: state.teamCode, placeCode: state.cipherPlace },
    success: data => {
      state.message = data.message;
      state.isPlaceFound = data.isPlaceFound;
      //write message
      sendMessage();

      //if successful
      if (state.isPlaceFound) {
        goToResult();
      }
    }
  });
}

function checkResult() {
  //get input
  state.result = $("#cipher")
    .val()
    .toUpperCase();

  //get team data
  $.ajax({
    url: url.concat("/SetCipherResult"),
    type: "POST",
    data: { teamCode: state.teamCode, result: state.result },
    success: data => {
      state.message = data.message;
      state.isPlaceFound = data.isPlaceFound;
      //write message
      sendMessage();

      //if successful
      if (!state.isPlaceFound) {
        goToPlace();
      }
    }
  });
}

function goToPlace() {
  $("#login").hide();
  $("#result").hide();
  clearText();
  $("#place").slideDown();
}

function goToResult() {
  $("#login").hide();
  $("#place").hide();
  clearText();
  $("#result").slideDown();
}

function sendMessage() {
  $(".msg").prepend("<p class='text-info'>" + state.message + "</p>");
}

function clearText() {
  $("#teamcode").val("");
  $("#placecode").val("");
  $("#cipher").val("");
}

$(document).ready(() => {
  $("#login").show();
  $("#place").hide();
  $("#result").hide();
});
