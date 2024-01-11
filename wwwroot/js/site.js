$(function () {
    $('#exportBtn').click(function () {
        var studentId = $('#studentId').val(); 
        $.ajax({
            url: '/Studenti?studentId=' + studentId,
            type: 'GET',
            success: function (data) {
                var blob = new Blob([data], { type: 'application/json' });

                var link = document.createElement('a');
                link.href = URL.createObjectURL(blob);
                link.download = 'student.json';

                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);
            },
            error: function () {
                alert('Failed to export student data as JSON.');
            }
        });
    });
});


function authenticate() {
    var authenticated = false;
    while (!authenticated) {
        var username = prompt("Introduceți numele de utilizator:");
        var password = prompt("Introduceți parola:");

        if (username === "admin" && password === "pass") {
            authenticated = true;
        } else {
            alert("Autentificarea a eșuat. Vă rugăm să introduceți numele de utilizator și parola corecte.");
        }
    }
    return true;
}
