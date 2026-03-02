$(document).ready(function () {

    $(document).on("click", ".wishlist-btn-group", async function (e) {
		e.preventDefault();

        var productId = $(this).attr("data-productId");
        var isInWishList = $(this).attr("data-isInWishList");
        var $btn = $(this);

     

        try {
            if (isInWishList === 'True') {
                const response = await CustomAjax.delete(`/WishList/Delete/${productId}`);

                if (response && response.success) {
                    Alert.toast({ title: response.message, icon: 'success' });

                    $btn.find("i").removeClass("bi-heart-fill text-ty-orange").addClass("bi-heart");

                    $btn.attr("data-isInWishList", "False");
                }
            } else {
                const response = await CustomAjax.post('/WishList/Add', productId);

                if (response && response.success) {
                    Alert.toast({ title: response.message, icon: 'success' });

                    $btn.find("i").removeClass("bi-heart").addClass("bi-heart-fill text-ty-orange");

                    $btn.attr("data-isInWishList", "True");
                }
            }
           
        } catch (error) {
            console.error("İstek listesine ekleme hatası:", error);
            Alert.toast({ title: "Bir hata oluştu", icon: 'error' });
        }

	});
});