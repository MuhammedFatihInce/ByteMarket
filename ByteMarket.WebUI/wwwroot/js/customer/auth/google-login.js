async function handleCredentialResponse(response) {
    const idToken = response.credential;

    try {
        const result = await CustomAjax.post('/Account/GoogleLogin', idToken);

        if (result && result.success) {
            Alert.toast({ title: "Google ile giriş başarılı!", icon: 'success' });
            window.location.href = "/Home/Index";
        } 
    } catch (error) {
        console.error("Google Login Error:", error);
    }
}