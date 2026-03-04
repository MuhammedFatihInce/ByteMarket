$(document).ready(function () {

    $(document).on("click", ".basket-btn-group", async function (e) {
        e.preventDefault();

        var productId = $(this).attr("data-productId");
        var $btn = $(this);

        var basketData = {
            ProductId: productId,
            quantity: 1
        };

        try {

            const response = await CustomAjax.post('/Basket/Add', basketData);

            if (response && response.success) {
                Alert.toast({ title: response.message, icon: 'success' });
            }

        } catch (error) {
            console.error("Sepete ekleme hatası:", error);
            Alert.toast({ title: "Bir hata oluştu", icon: 'error' });
        }

    });
});