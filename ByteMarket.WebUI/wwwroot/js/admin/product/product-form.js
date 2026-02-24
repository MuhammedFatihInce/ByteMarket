$(document).ready(function () {
    if ($('.select2-multiple').length > 0) {
        $('.select2-multiple').select2({
            theme: "default",
            allowClear: true,
            closeOnSelect: false,
            width: '100%',
            language: {
                noResults: function () {
                    return "Kategori bulunamadı";
                }
            }
        });
    }
});