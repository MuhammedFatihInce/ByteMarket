$(document).ready(function () {

	$('#header-search-input').select2({
        theme: 'bootstrap-5',
        width: 'resolve',
        minimumInputLength: 2,
        ajax: {
            url: '/Search/GetAllProductsForSelect',
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    q: params.term
                };
            },
            processResults: function (data) {
                return {
                    results: data
                };
            }
        },
        placeholder: "Aradığınız ürünün adını yazınız...",
        language: {
            inputTooShort: function (args) {
                var remaining = args.minimum - args.input.length;
                return "En az " + remaining + " karakter daha girmelisiniz";
            },
            noResults: function () {
                return "Sonuç bulunamadı";
            },
            searching: function () {
                return "Aranıyor...";
            },
            errorLoading: function () {
                return "Sonuçlar yüklenirken bir hata oluştu";
            }
        },
        allowClear: true
	});

    $('#header-search-input').on('select2:select', function (e) {
        var selectedData = e.params.data;
        var productId = selectedData.id;

        $(this).val(null).trigger('change');

        $(this).blur();

        if (productId) {
            window.location.href = '/Product/Detail/' + productId;
        }
        
    });
});