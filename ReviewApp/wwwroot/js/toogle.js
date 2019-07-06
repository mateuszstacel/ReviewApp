function myFunction(element, element2) {
    var x = document.getElementById(element);
    if (x.style.display === "none") {
        $(x).show(1000);
     

    } else {
        $(x).hide(1000);
       
    }
}