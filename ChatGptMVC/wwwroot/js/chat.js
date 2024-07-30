$(document).ready(function () {
    $("#send-button").click(function () {
        sendMessage();
    });

    $("#user-input").keypress(function (e) {
        if (e.which == 13) {
            sendMessage();
        }
    });

    function sendMessage() {
        var userInput = $("#user-input").val();
        if (userInput.trim() === "") return;

        appendMessage("user", userInput);
        $("#user-input").val("");

        $.ajax({
            url: chatUrl,
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ userInput: userInput }),
            success: function (response) {
                console.log("Respuesta del servidor:", response);
                if (response && response.response) {
                    appendMessage("bot", response.response);
                } else {
                    appendMessage("bot", "Error al procesar la respuesta.");
                }
            },
            error: function (xhr, status, error) {
                console.log("Error en la solicitud:", status, error);
                appendMessage("bot", "Error al obtener respuesta. Por favor, inténtalo de nuevo.");
            }
        });
    }

    function appendMessage(sender, message) {
        var messageClass = sender === "user" ? "chat-message user" : "chat-message bot";
        var messageHtml = '<div class="' + messageClass + '">' + message.replace(/\n/g, '<br>') + '</div>';
        $("#chat-box").append(messageHtml);
        $("#chat-box").scrollTop($("#chat-box")[0].scrollHeight);
    }
});
