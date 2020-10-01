window.addEventListener("keydown", function (event) {
    if (event.defaultPrevented) {
        return;
    }

    switch (event.key) {
        case "Down":
        case "ArrowDown":
            $.get("Home/HistoryDown", function (data) {
                $("#inputBar").val(data);
            });
            break;
        case "Up":
        case "ArrowUp":
            $.get("Home/HistoryUp", function (data) {
                $("#inputBar").val(data);
            });
            break;
        default:
            return;
    }
    event.preventDefault();
}, true);

$(document).ready(function () {
    updateStatus();
})

$(document).click(function () {
    $("#inputBar").focus();
})

function onComplete() {
    updateStatus();
    scrollDown();
}

function updateStatus() {
    $.get("Home/StatusInit", function (data) {
        $("#statusBar").html(data);
    });
}

function clearInput() {
    $("#inputBar").val("")
}

function scrollDown() {
    $("#textArea").scrollTop($("#textArea")[0].scrollHeight);
}