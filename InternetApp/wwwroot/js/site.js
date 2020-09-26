// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(window).on('load', onWindowLoaded);

function onWindowLoaded() {
    $(".post_delete").on('click', deletePost);
}

function deletePost() {
    if (!confirm("Вы действительно хотите удалить эту запись?")) {
        event.preventDefault();
    }
}