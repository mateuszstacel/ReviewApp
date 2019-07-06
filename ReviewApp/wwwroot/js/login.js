$("#registerSign").click(function () {
    $("#registerSign").hide(1000);
    $("#backToLogin").show(1000);
    $("#form").hide(1000);
    $("#registrationForm").show(1000);
});

$("#backToLogin").click(function () {
    $("#backToLogin").hide(1000);
    $("#registerSign").show(1000);
    $("#form").show(1000);
    $("#registrationForm").hide(1000);
});

$(document).ready(function () {
    if ($("#errorContainer").is(':visible')) {
        $("#errorContainer").delay(5000).hide(1000);
    }
});



