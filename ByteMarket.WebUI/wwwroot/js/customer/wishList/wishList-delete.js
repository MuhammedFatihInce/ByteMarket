$(document).ready(function () {

    $(document).on("click", ".remove-btn", async function (e) {
        e.preventDefault();

        const $btn = $(this);
        var productId = $(this).attr("data-productId");

        console.log(productId)

        const $productCard = $btn.closest('.col');

        try {
            const response = await CustomAjax.delete(`/WishList/Delete/${productId}`);

            if (response && response.success) {
                Alert.toast({ title: response.message, icon: 'success' });

                $productCard.fadeOut(300, function () {
                    $(this).remove();
                    if($('.product-card').length === 0){
                        location.reload();
                    }
                });
            } else {
                Alert.toast({ title: response.message || "Silinemedi", icon: 'error' });
            }
        } catch (error) {
            console.error("İstek listesine ekleme hatası:", error);
            Alert.toast({ title: "Bir hata oluştu", icon: 'error' });
        }

    });
});