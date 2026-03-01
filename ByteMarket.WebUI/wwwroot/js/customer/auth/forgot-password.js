$(document).ready(function () {
    $('#forgotPasswordForm').on('submit', async function (e) {
        e.preventDefault();

        const email = $('#Email').val();
        if (!email) {
            Alert.toast({ title: "Lütfen e-posta adresinizi giriniz.", icon: 'warning' });
            return;
        }

        try {
            const response = await CustomAjax.post('/Account/ForgotPassword', email);

            if (response && response.success) {
                Alert.fire({
                    title: 'E-Posta Gönderildi!',
                    text: response.message || "Lütfen e-posta kutunuzu kontrol ediniz.",
                    icon: 'success',
                    confirmButtonText: 'Tamam'
                });
                $('#forgotPasswordForm')[0].reset();
            }
        } catch (error) {
            console.error("Şifre sıfırlama hatası:", error);
        }
    });
});