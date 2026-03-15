
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
        var basketItemId = $btn.attr("data-basketItemId");

        $btn.prop('disabled', true);

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
        var totalBasePrice = $(this).find('button[type="submit"]').attr("data-totalBasePrice");
        var discountAmount = $(this).find('button[type="submit"]').attr("data-discountAmount");
        var finalTotalPrice = $(this).find('button[type="submit"]').attr("data-finalTotalPrice");
                

        const requestData = {

            orderModel: {
                Address: $("#adress").val(),
                Description: $("#description").val(),
                BasketId: basketId,
                TotalBasePrice: parseFloat(totalBasePrice.replace(',', '.')),
                DiscountAmount: parseFloat(discountAmount.replace(',', '.')),
                FinalTotalPrice: parseFloat(finalTotalPrice.replace(',', '.'))
            },

            paymentModel: {
                CardNumber: $('#cardNumber').val().replace(/\s/g, ''),
                CardHolderName: $('#cardName').val(),
                ExpirationMonth: month,
                ExpirationYear: year,
                Cvv: $('#cardCvc').val(),
                TotalAmount: parseFloat(finalTotalPrice.replace(',', '.'))
            }
        };


        try {

            Alert.toast({ title: "İşleminiz yapılıyor...", icon: 'info' });

            const response = await CustomAjax.post(`/Order/Create`, requestData);

            if (response && response.success) {
                Alert.toast({ title: "Ödeme başarılı! Yönlendiriliyorsunuz...", icon: 'success' });
                //setTimeout(() => { window.location.href = `/Order/Success/${basketId}`; }, 2000);
                setTimeout(() => { window.location.href = response.redirectUrl; }, 2000);
                        
            } else {
                console.log("Hata Mesajı:", response.message);
                Alert.toast({
                    title: response.message || "Ödeme onaylanmadı, bilgilerinizi kontrol edin.",
                    icon: 'error'
                });
            }
        } catch (error) {
            console.error("Hata:", error);
            Alert.toast({ title: "Bir hata oluştu", icon: 'error' });
        }
               
       
    });
});


$(document).ready(function () {

    $(document).on("click", ".coupon-apply-button", async function () {

        const $btn = $(this);

        $btn.prop('disabled', true);

        let inputValue = $btn.closest('.flex-column').find('#couponCode').val();

        try {
            const response = await CustomAjax.post('/Basket/ApplyCouponToBasket', inputValue);

            if (response && response.success) {
                Alert.toast({ title: response.message, icon: 'success' });
                location.reload();
            } else {
                Alert.toast({ title: response.message || "Kupon uygulanmadı.", icon: 'error' });
            }
        } catch (error) {
            console.error("Kupon hatası:", error);
            Alert.toast({ title: "Bir hata oluştu", icon: 'error' });
        } finally {
            $btn.prop('disabled', false);
        }

        

    });
});


$(document).ready(function () {

    $(document).on("click", ".remove-coupon-btn", async function (e) {
        console.log("Butona tıklandı!");
        e.preventDefault();

        const $btn = $(this);
        var couponId = $btn.attr("data-id");


        $btn.prop('disabled', true);

        const $couponElement = $btn.closest('.coupon-group');

        try {
            const response = await CustomAjax.delete(`/Basket/RemoveCouponFromBasket/${couponId}`);

            if (response && response.success) {
                Alert.toast({ title: response.message, icon: 'success' });

                $couponElement.fadeOut(300, function () {
                    $(this).remove();
                    location.reload();
                });
            } else {
                Alert.toast({ title: response.message || "Silinemedi", icon: 'error' });
            }
        } catch (error) {
            console.error("Kupon silme hatası:", error);
            Alert.toast({ title: "Bir hata oluştu", icon: 'error' });
        }
        finally {
            $btn.prop('disabled', false);
        }

    });
});




let usdRate = 1;

async function getUsdRate() {
    try {

        const response = await CustomAjax.get(`/Basket/GetCurrency/USD`);

        usdRate = response.data.sellingRate;
        return usdRate;
    } catch (error) {
        console.error("Kur çekilirken hata oluştu:", error);
        Alert.toast({ title: "Bir hata oluştu", icon: 'error' });
    }
}

function updatePrices(currency) {
    
    $('.basket-item-price, .summary-price, .discount-price').each(function () {
        let $el = $(this);

        let originalPrice = parseFloat($el.attr('data-price').toString().replace(',', '.'));

        if (currency === 'USD') {
            let convertedPrice = (originalPrice / usdRate).toFixed(2);
            $el.text('$' + convertedPrice);
        } else {
            
            let formattedPrice = new Intl.NumberFormat('tr-TR', {
                style: 'currency',
                currency: 'TRY'
            }).format(originalPrice);

            $el.text(formattedPrice);
        }
    });
}

$(document).ready(function () {

    
    $('#btnUSD').on('click', async function () {

        if (usdRate === 1) {
            await getUsdRate();
        }

        
        $(this).addClass('active btn-ty-orange text-white').removeClass('btn-outline-secondary');
        $('#btnTRY').removeClass('active btn-ty-orange text-white').addClass('btn-outline-secondary');

        updatePrices('USD');
    });

    $('#btnTRY').on('click', function () {
       
        $(this).addClass('active btn-ty-orange text-white').removeClass('btn-outline-secondary');
        $('#btnUSD').removeClass('active btn-ty-orange text-white').addClass('btn-outline-secondary');

        updatePrices('TRY');
    });
});