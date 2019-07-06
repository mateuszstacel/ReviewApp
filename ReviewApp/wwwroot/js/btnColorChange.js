$(document).ready(function () {



var currentButton = "#ButtonId";

function disableButton(btn) {
    $(btn).attr("disabled", true);
}

function enableButton(btn) {
    $(btn).attr("disabled", false);
}

function myFunction(btn) {
    $(btn).addClass("btn btn-success");
}

function setDefaultCss(btn) {
    $(btn).removeClass("btn btn-success");
    $(btn).addClass("btn nav-btn");
}


$("#navbtn1").click(function test() {


    setDefaultCss(currentButton)

    currentButton = "#navbtn1"

    myFunction("#navbtn1")



});

$("#navbtn2").click(function test() {

  
    setDefaultCss(currentButton)

    currentButton = "#navbtn2"

    myFunction("#navbtn2")



});

$("#navbtn3").click(function test() {

 

    setDefaultCss(currentButton)

    currentButton = "#navbtn3"

    myFunction("#navbtn3")

    

});

$("#navbtn4").click(function test() {



    setDefaultCss(currentButton)

    currentButton = "#navbtn4"

    myFunction("#navbtn4")

 

});

$("#navbtn5").click(function test() {



    setDefaultCss(currentButton)

    currentButton = "#navbtn5"

    myFunction("#navbtn5")

   

});

$("#navbtn6").click(function test() {



    setDefaultCss(currentButton)

    currentButton = "#navbtn6"

    myFunction("#navbtn6")



});

$("#navbtn7").click(function test() {


    setDefaultCss(currentButton)

    currentButton = "#navbtn7"

    myFunction("#navbtn7")



});

$("#navbtn8").click(function test() {



    setDefaultCss(currentButton)

    currentButton = "#navbtn8"

    myFunction("#navbtn8")



});

$("#navbtn9").click(function test() {


    setDefaultCss(currentButton)

    currentButton = "#navbtn9"

    myFunction("#navbtn9")


});

$("#navbtn10").click(function test() {

  

    setDefaultCss(currentButton)

    currentButton = "#navbtn10"

    myFunction("#navbtn10")

 

});

$("#homebtn").click(function test() {

    setDefaultCss(currentButton)


});

});
