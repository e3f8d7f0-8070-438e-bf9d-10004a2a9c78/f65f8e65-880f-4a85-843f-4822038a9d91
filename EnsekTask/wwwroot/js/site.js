(function ($) {
    $('#results').hide();

    function makeAjaxCall(url, fileContents, callbackSuccess, callbackFailure) {
        $.ajax({
            url: url,
            type: "POST",
            contentType: "application/octet-stream",
            processData: false,
            data: fileContents,
            dataType: "json"
        })
            .done(function(data, msg) {
                if (msg === 'success') {
                    return callbackSuccess(data);
                }
            }
            .bind(this))
            .fail(function(jqXHR, status) {
                const error = {
                    "message": "There was a server error with your request. Please try again."
                };
                return callbackFailure(error);
            });
    }

    $('#btnSubmit').on('click', (ev) => {
        const fileControl = $('#selectedFile')[0];
        if (fileControl.files.length != 1) {
            return;
        }
        const fileItem = fileControl.files[0];

        const reader = new FileReader();
        reader.onload = function() {
            const arrayBuffer = this.result;
            const arrayBytes = new Uint8Array(arrayBuffer);

            makeAjaxCall("/meter-reading-uploads", arrayBytes,
                function (data) {
                    $('#successCount').text(data.successCount);
                    $('#failureCount').text(data.failureCount);
                    $('#results').show();
                },
                function (data) {
                    console.error("Server request failed");
                    alert(data.message)
                }
            );
        }
        reader.readAsArrayBuffer(fileItem);
    });

})(jQuery);
