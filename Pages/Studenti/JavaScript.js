/*$(function () {
    // When the export button is clicked
    $('#exportBtn').click(function () {
        // Get the student data from the server using AJAX
        $.ajax({
            url: '/Studenti?handler=ExportJson',
            type: 'GET',
            success: function (data) {
                // Create a blob from the JSON data
                var blob = new Blob([data], { type: 'application/json' });

                // Create a temporary link element to download the file
                var link = document.createElement('a');
                link.href = URL.createObjectURL(blob);
                link.download = 'student.json';

                // Click the link to start the download
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
*/