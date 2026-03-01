$(document).ready(function () {
    $('#resetPasswordForm').on('submit', async function (e) {
        e.preventDefault();

        const password = $('#Password').val();
        const confirmPassword = $('#ConfirmPassword').val();

        if (password !== confirmPassword) {
            Alert.toast({ title: "Şifreler birbiriyle eşleşmiyor!", icon: 'error' });
            return;
        }

        const data = {
            Email: $('#Email').val(),
            Token: $('#Token').val(),
            Password: password,
            ConfirmPassword: confirmPassword
        };

        try {
            const response = await CustomAjax.post('Account/ResetPassword', data);

            if (response && response.success) {
                Alert.fire({
                    title: 'İşlem Başarılı',
                    text: "Şifreniz güncellendi. Yeni şifrenizle giriş yapabilirsiniz.",
                    icon: 'success',
                    onConfirm: () => {
                        window.location.href = "/Account/Login";
                    }
                });
            }
        } catch (error) {
            console.error("Şifre güncelleme hatası:", error);
        }
    });
});