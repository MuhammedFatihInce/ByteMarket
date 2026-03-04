
async function changeQuantity(itemId, amount) {

    var $input = $('#qty-' + itemId);
    var currentVal = parseInt($input.val());

    var newVal = currentVal + amount;

    if (newVal >= 1) {

        var basketData = {
            BasketItemId: itemId,
            quantity: newVal
        };

        try {

            const response = await CustomAjax.post('/Basket/UpdateQuantity', basketData);

            if (response && response.success) {
                    $input.val(newVal);
                    location.reload()
            }

        } catch (error) {
            console.error("Sepete ekleme hatası:", error);
            Alert.toast({ title: "Bir hata oluştu", icon: 'error' });
        }
    }
}
    

    
$(document).ready(function () {

    $(document).on("click", ".delete-basket-item", async function (e) {
        e.preventDefault();

        const $btn = $(this);
        var basketItemId = $(this).attr("data-basketItemId");

        console.log(basketItemId)

        const $row = $btn.closest('tr');

        try {
            const response = await CustomAjax.delete(`/Basket/Delete/${basketItemId}`);

            if (response && response.success) {
                Alert.toast({ title: response.message, icon: 'success' });

                $row.fadeOut(300, function () {
                    $(this).remove();
                    location.reload();
                });
            } else {
                Alert.toast({ title: response.message || "Silinemedi", icon: 'error' });
            }
        } catch (error) {
            console.error("Sepet ekleme hatası:", error);
            Alert.toast({ title: "Bir hata oluştu", icon: 'error' });
        }

    });
});
    

   
$(document).ready(function () {
            
    $('#cardNumber').on('input', function (e) {
        let target = e.target, position = target.selectionEnd;
        let length = target.value.length;
        target.value = target.value.replace(/[^\d]/g, '').replace(/(.{4})/g, '$1 ').trim();
        target.selectionEnd = position + (target.value.length !== length ? 1 : 0);
    });

    $('#paymentForm').on('submit', async function (e) {
        e.preventDefault();

        const expiryParts = $('#cardExpiry').val().split('/');
        const month = expiryParts[0];
        const year = expiryParts.length > 1 ? "20" + expiryParts[1] : "";

        var basketId = $(this).find('button[type="submit"]').attr("data-basketId");
                

        const requestData = {

            orderModel: {
                Address: $("#adress").val(),
                Description: $("#description").val(),
                BasketId: basketId
            },

            paymentModel: {
                CardNumber: $('#cardNumber').val().replace(/\s/g, ''),
                CardHolderName: $('#cardName').val(),
                ExpirationMonth: month,
                ExpirationYear: year,
                Cvv: $('#cardCvc').val(),
                TotalAmount: parseFloat("@Model.BasketItem.Sum(x => x.Total)".replace(',', '.'))
            }
        };


        try {

            Alert.toast({ title: "İşleminiz yapılıyor...", icon: 'info' });

            const response = await CustomAjax.post(`/Order/Create`, requestData);

            if (response && response.success) {
                Alert.toast({ title: response.message, icon: 'success' });
                setTimeout(() => { window.location.href = "/Order/Success"; }, 2000);
                        
            } else {
                Alert.toast({ title: response.message || "İşlem başarısız", icon: 'error' });
            }
        } catch (error) {
            console.error("Hata:", error);
            Alert.toast({ title: "Bir hata oluştu", icon: 'error' });
        }
               
        Alert.toast({ title: "Ödemeniz alınıyor, lütfen bekleyin...", icon: 'info' });
    });
});
  