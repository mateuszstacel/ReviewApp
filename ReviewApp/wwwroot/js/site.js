// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(".moreInfoBtnIndex").click(function () {
    // UpdateDBStartActivity
    var url = "/Home/Details";
    var id = $(".moreInfoBtnIndex").val();
    $.ajaxSetup({ async: false });
    $.post(url, { id: id });

});

