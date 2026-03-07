const CustomAjax = (function () {

    const baseUrl = "/";

    function getCsrfToken() {
        return document.querySelector('input[name="__RequestVerificationToken"]')?.value;
    }

    async function ajaxRequest(endpoint, method, data = null) {
        const url = `${baseUrl}${endpoint.startsWith('/') ? endpoint.substring(1) : endpoint}`;

        console.log(url);

        return new Promise((resolve, reject) => {
            $.ajax({
                url: url,

                type: method,
                contentType: 'application/json',
                data: data ? JSON.stringify(data) : null,
                headers: {
                    'RequestVerificationToken': getCsrfToken()
                },
                success: function (response) {
                    resolve(response);
                },
                error: function (xhr) {
                    console.error(`AJAX Hatası [${url}]:`, xhr);

                    const errorMsg = xhr.responseJSON?.message || "İşlem sırasında bir hata oluştu.";

                    Alert.toast({ title: errorMsg, icon: 'error' });

                    reject(xhr);
                }
            });
        });
    }

    return {
        get: (url) => ajaxRequest(url, 'GET'),
        post: (url, data) => ajaxRequest(url, 'POST', data),
        put: (url, data) => ajaxRequest(url, 'PUT', data),
        delete: (url) => ajaxRequest(url, 'DELETE') 
    };
})();