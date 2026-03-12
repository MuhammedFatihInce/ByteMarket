const CustomAjax = (function () {

    const baseUrl = "/";

    function getCsrfToken() {
        return document.querySelector('input[name="__RequestVerificationToken"]')?.value;
    }

    async function ajaxRequest(endpoint, method, data = null, isFile = false) {
        const url = `${baseUrl}${endpoint.startsWith('/') ? endpoint.substring(1) : endpoint}`;

        console.log(url);

        return new Promise((resolve, reject) => {
            let ajaxSettings = {
                url: url,
                type: method,
                headers: {
                    'RequestVerificationToken': getCsrfToken()
                },
                success: (response) => resolve(response),
                error: (xhr) => {
                    console.error(`AJAX Hatası [${url}]:`, xhr);
                    const errorMsg = xhr.responseJSON?.message || "İşlem sırasında bir hata oluştu.";
                    if (typeof Alert !== 'undefined') {
                        Alert.toast({ title: errorMsg, icon: 'error' });
                    }
                    reject(xhr);
                }
            };

            if (isFile) {
                ajaxSettings.contentType = false;
                ajaxSettings.processData = false;
                ajaxSettings.data = data; 
            } else {
                ajaxSettings.contentType = 'application/json';
                ajaxSettings.data = data ? JSON.stringify(data) : null;
            }

            $.ajax(ajaxSettings);
        });
    }

    return {
        get: (url) => ajaxRequest(url, 'GET'),
        post: (url, data) => ajaxRequest(url, 'POST', data),
        postFile: (url, data) => ajaxRequest(url, 'POST', data, true),
        put: (url, data) => ajaxRequest(url, 'PUT', data),
        delete: (url) => ajaxRequest(url, 'DELETE') 
    };
})();