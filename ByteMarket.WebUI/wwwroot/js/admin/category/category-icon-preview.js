const CategoryIconPreview = {
    init: function (inputId, previewId) {
        document.addEventListener('DOMContentLoaded', () => {
            const iconInput = document.getElementById(inputId);
            const iconPreview = document.getElementById(previewId);

            if (iconInput && iconPreview) {
                const updateIcon = () => {
                    const val = iconInput.value.trim();
                    iconPreview.className = val ? `bi ${val} text-ty-orange` : 'bi bi-tag text-ty-orange';
                };
                iconInput.addEventListener('input', updateIcon);
                updateIcon();
            }
        });
    }
};

CategoryIconPreview.init('categoryIconInput', 'iconPreview');