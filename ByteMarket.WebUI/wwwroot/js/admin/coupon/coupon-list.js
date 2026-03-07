$(document).ready(function () {

    let allProducts = [];

    async function fetchAllProducts() {
        try {
            const response = await CustomAjax.get('/Admin/Coupon/GetAllProductsForSelect');
            allProducts = response;
        } catch (error) {
            console.error("Ürünler yüklenemedi:", error);
        }
    }

    function initializeProductSelect() {
        $('.select2-products').select2({
            theme: 'bootstrap-5',
            dropdownParent: $('#createCouponModal'),
            data: allProducts,
            closeOnSelect: false, 
            allowClear: true,
            placeholder: "Ürün ismine göre arayın...",
            templateResult: formatProduct,
            templateSelection: formatProductSelection,
            language: {
                noResults: function () { return "Eşleşen ürün bulunamadı"; },
                searching: function () { return "Aranıyor..."; }
            }
        });
    }


    function formatProduct(product) {
        if (!product.id) return product.text;

        var $product = $(
            '<div class="product-item-wrapper">' +
            '<img src="' + (product.image || '/img/no-image.png') + '" class="select2-product-img" />' +
            '<div class="product-info">' +
            '<div class="product-info-name">' + product.text + '</div>' +
            '</div>' +
            '</div>'
        );
        return $product;
    }

    function formatProductSelection(product) {
        if (!product.id) return product.text;
        return $('<span>' + product.text + '</span>');
    }

    fetchAllProducts();

    $('#Target').change(function () {
        var selectedTarget = $(this).val();
        if (selectedTarget == "2") {
            $('#productSelectionArea').removeClass('d-none');

            if (!$('.select2-products').hasClass("select2-hidden-accessible")) {
                initializeProductSelect();
            }
        } else {
            $('#productSelectionArea').addClass('d-none');
            $('#ProductIds').val(null).trigger('change');
        }
    });


    $('#createCouponForm').submit(async function (e) {
        e.preventDefault();

        var formData = {
            Code: $('#Code').val(),
            Target: parseInt($('#Target').val()),
            DiscountValue: parseFloat($('#DiscountValue').val()),
            IsPercentage: $('#IsPercentage').is(':checked'),
            ProductIds: $('#ProductIds').val()
        };

        try {

            const response = await CustomAjax.post('/Admin/Coupon/Create', formData);

            if (response && response.success) {
                Alert.toast({ title: response.message, icon: 'success' });
                location.reload();
            }
            else {
                Alert.toast({ title: response.message, icon: 'error' });
            }

        } catch (error) {
            console.error("Kupon ekleme hatası:", error);
            Alert.toast({ title: "Bir hata oluştu", icon: 'error' });
        }
    });


    function initializeUpdateProductSelect(dataList) {
        $('.select2-products-update').select2({
            theme: 'bootstrap-5',
            dropdownParent: $('#updateCouponModal'),
            data: dataList,
            templateResult: formatProduct,
            templateSelection: formatProductSelection
        });
    }

    $(document).on('click', '.btn-edit-coupon', async function (e) {
        e.preventDefault();

        const id = $(this).data('id');
        const $btn = $(this);

        console.log("tetiklendi");

        $btn.prop('disabled', true);

        try {
            if (allProducts.length === 0) {
                await fetchAllProducts();
            }

            const response = await CustomAjax.get(`/Admin/Coupon/GetCouponForUpdate/${id}`);

            if (response) {

                $('#UpdateId').val(response.id);
                $('#UpdateCode').val(response.code);
                $('#UpdateDiscountValue').val(response.discountValue);
                $('#UpdateIsPercentage').prop('checked', response.isPercentage);
                $('#UpdateTarget').val(response.target);

                const productsList = response.products

                if (response.target == 2 && productsList) {
                    $('#updateProductSelectionArea').removeClass('d-none');

                    const selectedIds = productsList.map(p => String(p.id || p.Id).toUpperCase());


                    if ($('.select2-products-update').hasClass("select2-hidden-accessible")) {
                        $('.select2-products-update').select2('destroy').empty();
                    }

                    initializeUpdateProductSelect(allProducts);

                    $('#UpdateProductIds').val(null).trigger('change');
                    

                    $('#UpdateProductIds').val(selectedIds).trigger('change');

                } else {
                    $('#updateProductSelectionArea').addClass('d-none');
                    $('#UpdateProductIds').val(null).trigger('change');
                }

                var myModal = new bootstrap.Modal(document.getElementById('updateCouponModal'));
                myModal.show();
            }
        } catch (error) {
            Alert.toast({ title: "Veriler alınırken hata oluştu", icon: 'error' });
        } finally {
        $btn.prop('disabled', false); 
    }
    });


    $('#updateCouponForm').submit(async function (e) {
        e.preventDefault();

        var formData = {
            Id: $('#UpdateId').val(),
            Code: $('#UpdateCode').val(),
            Target: parseInt($('#UpdateTarget').val()),
            DiscountValue: parseFloat($('#UpdateDiscountValue').val()),
            IsPercentage: $('#UpdateIsPercentage').is(':checked'),
            ProductIds: $('#UpdateProductIds').val()
        };

        console.log(formData);

        try {
            const response = await CustomAjax.put('/Admin/Coupon/Update', formData);
            if (response.success) {
                Alert.toast({ title: response.message, icon: 'success' });
                setTimeout(() => location.reload(), 1000);
            } else {
                Alert.toast({ title: response.message, icon: 'error' });
            }
        } catch (error) {
            Alert.toast({ title: "Güncelleme yapılamadı", icon: 'error' });
        }
    });

    $('#UpdateTarget').change(function () {
        if ($(this).val() == "2") {
            $('#updateProductSelectionArea').removeClass('d-none');
            if (!$('.select2-products-update').hasClass("select2-hidden-accessible")) {
                initializeUpdateProductSelect();
            }
        } else {
            $('#updateProductSelectionArea').addClass('d-none');
            $('#UpdateProductIds').val(null).trigger('change');
        }
    });




    $(document).on('click', '.btn-delete-coupon', function () {
        const id = $(this).data('id');

        Alert.fire({
            title: 'Kuponu Sil?',
            text: ` Kupon silinecek. Bu işlem geri alınamaz!`,
            confirmButtonText: 'Evet, Sil!',
            onConfirm: async () => {
                const response = await CustomAjax.delete(`Admin/Coupon/Delete/${id}`)

                if (response && response.success) {
                    const row = document.getElementById(`coupon-row-${id}`);
                    if (row) {
                        row.classList.add('fade-out');
                        setTimeout(() => row.remove(), 300);
                    }
                }


            },
            successMessage: 'Kupon başarıyla silindi.'
        });
    });




});