$(document).ready(function () {

    const select2AjaxConfig = {
        url: '/Admin/Coupon/GetAllProductsForSelect',
        dataType: 'json',
        delay: 250,
        data: function (params) {
            return {
                q: params.term || '',
            };
        },
        processResults: function (data) {
            return {
                results: data
            };
        },
        cache: true
    };

    
    function initializeProductSelect() {
        $('.select2-products').select2({
            theme: 'bootstrap-5',
            dropdownParent: $('#createCouponModal'),
            width: '100%',
            closeOnSelect: false,
            allowClear: true,
            placeholder: "Ürün ismine göre arayın...",
            templateResult: formatProduct,
            templateSelection: formatProductSelection,
            language: {
                noResults: function () { return "Eşleşen ürün bulunamadı"; },
                searching: function () { return "Aranıyor..."; }
            },
            ajax: select2AjaxConfig, 
            minimumInputLength: 2, 
        });
    }

    function initializeUpdateProductSelect() {
        $('.select2-products-update').select2({
            theme: 'bootstrap-5',
            dropdownParent: $('#updateCouponModal'),
            width: '100%',
            closeOnSelect: false,
            allowClear: true,
            placeholder: "Ürün ismine göre arayın...",
            templateResult: formatProduct,
            templateSelection: formatProductSelection,
            language: {
                noResults: function () { return "Eşleşen ürün bulunamadı"; },
                searching: function () { return "Aranıyor..."; }
            },
            ajax: select2AjaxConfig,
            minimumInputLength: 2,
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


    $('#calenderPicker').flatpickr({
        locale: "tr",
        dateFormat: "Y-m-d",
        altInput: true,
        altFormat: "d.m.Y",
        allowInput: true
    });

    var updateCalenderPicker = $('#updateCalenderPicker').flatpickr({
        locale: "tr",
        dateFormat: "Y-m-d",
        altInput: true,
        altFormat: "d.m.Y",
        allowInput: true,
    });


    // --- EKLEME (CREATE) İŞLEMLERİ ---

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
            Name: $('#Name').val(),
            Code: $('#Code').val(),
            Target: parseInt($('#Target').val()),
            DiscountValue: parseFloat($('#DiscountValue').val()),
            IsPercentage: $('#IsPercentage').is(':checked'),
            ProductIds: $('#ProductIds').val(), 
            ExpireTime: $('#calenderPicker').val(),
            IsStackable: $('#IsStackable').is(':checked'),
            UsageLimitPerUser: parseInt($('#UsageLimitPerUser').val())
        };

        try {
            const response = await CustomAjax.post('/Admin/Coupon/Create', formData);
            if (response && response.success) {
                Alert.toast({ title: response.message, icon: 'success' });
                location.reload();
            } else {
                Alert.toast({ title: response.message, icon: 'error' });
            }
        } catch (error) {
            console.error("Kupon ekleme hatası:", error);
            Alert.toast({ title: "Bir hata oluştu", icon: 'error' });
        }
    });

    // --- GÜNCELLEME (UPDATE) İŞLEMLERİ ---

    $(document).on('click', '.btn-edit-coupon', async function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        const $btn = $(this);

        $btn.prop('disabled', true);

        try {
            const response = await CustomAjax.get(`/Admin/Coupon/GetCouponForUpdate/${id}`);

            console.log(response);

            if (response) {
                $('#UpdateId').val(response.id);
                $('#UpdateName').val(response.name);
                $('#UpdateCode').val(response.code);
                $('#UpdateDiscountValue').val(response.discountValue);
                $('#UpdateIsPercentage').prop('checked', response.isPercentage);
                $('#UpdateTarget').val(response.target);
                $('#updateIsStackable').prop('checked', response.isStackable);
                $('#updateUsageLimitPerUser').val(response.usageLimitPerUser);

                updateCalenderPicker.setDate(response.expireTime);

                const productsList = response.products;
                const $updateSelect = $('.select2-products-update');

                if (response.target == 2 && productsList && productsList.length > 0) {
                    $('#updateProductSelectionArea').removeClass('d-none');

                    if (!$updateSelect.hasClass("select2-hidden-accessible")) {
                        initializeUpdateProductSelect();
                    }

                    $updateSelect.empty();

                    const selectedIds = []

                    productsList.forEach(function (p) {
                        const productId = p.id || p.Id;
                        const productName = p.name || p.Title;
                        const productImage = p.image || p.Image;

                       
                        const option = new Option(productName, productId, true, true);

                        $(option).data('image', productImage);

                        $updateSelect.append(option);
                        selectedIds.push(productId);
                    });

                    $updateSelect.trigger('change');

                } else {
                    $('#updateProductSelectionArea').addClass('d-none');
                    $updateSelect.val(null).trigger('change');
                }

                var myModal = new bootstrap.Modal(document.getElementById('updateCouponModal'));
                myModal.show();
            }
        } catch (error) {
            console.error(error);
            Alert.toast({ title: "Veriler alınırken hata oluştu", icon: 'error' });
        } finally {
            $btn.prop('disabled', false);
        }
    });

    $('#updateCouponForm').submit(async function (e) {
        e.preventDefault();
        var formData = {
            Id: $('#UpdateId').val(),
            Name: $('#UpdateName').val(),
            Code: $('#UpdateCode').val(),
            Target: parseInt($('#UpdateTarget').val()),
            DiscountValue: parseFloat($('#UpdateDiscountValue').val()),
            IsPercentage: $('#UpdateIsPercentage').is(':checked'),
            ProductIds: $('#UpdateProductIds').val(),
            ExpireTime: $('#updateCalenderPicker').val(),
            IsStackable: $('#updateIsStackable').is(':checked'),
            UsageLimitPerUser: parseInt($('#updateUsageLimitPerUser').val())
        };

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
            $('.select2-products-update').val(null).trigger('change');
        }
    });

    // --- SİLME (DELETE) İŞLEMLERİ ---

    $(document).on('click', '.btn-delete-coupon', function () {
        const id = $(this).data('id');
        Alert.fire({
            title: 'Kuponu Sil?',
            text: `Kupon silinecek. Bu işlem geri alınamaz!`,
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