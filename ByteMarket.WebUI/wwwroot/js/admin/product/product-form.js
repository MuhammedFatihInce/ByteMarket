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


    $('#summernote').summernote({
        placeholder: 'İçeriğinizi buraya yazın...',
        tabsize: 2,
        height: 300, 
        toolbar: [
            ['style', ['style']],
            ['font', ['bold', 'underline', 'clear']],
            ['color', ['color']],
            ['para', ['ul', 'ol', 'paragraph']],
            ['table', ['table']],
            ['insert', ['link', 'picture', 'video']],
            ['view', ['fullscreen', 'codeview', 'help']]
        ],
        callbacks: {
            onImageUpload: function (files) {
                uploadEditorImage(files[0]);
            },
            onMediaDelete: function (target) {
                deleteEditorImage(target[0].src);
            }
        }
    });

    $('.note-image-input').attr('name', 'summernoteDummyFiles');

});

async function uploadEditorImage(file) {
    const formData = new FormData();
    formData.append("file", file);

    try {
        const response = await CustomAjax.postFile('/Admin/Product/UploadEditorImage', formData);

        if (response.success) {
            $('#summernote').summernote('insertImage', response.url);
        }
    } catch (error) {
        console.error("Yükleme hatası:", error);
    }
}

async function deleteEditorImage(url) {
    try {
        const endpoint = `/Admin/Product/DeleteEditorImage?url=${encodeURIComponent(url)}`;

        const response = await CustomAjax.delete(endpoint);

    } catch (error) {
        console.error("Silme hatası:", error);
    }
}