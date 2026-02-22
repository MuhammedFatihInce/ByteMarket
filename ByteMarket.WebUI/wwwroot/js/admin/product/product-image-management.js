
const dataTransfer = new DataTransfer();

const fileInput = document.getElementById('image-input');
const container = document.getElementById('image-preview-container');

const MAX_FILE_COUNT = 5;
const MAX_FILE_SIZE = 5 * 1024 * 1024;


fileInput.addEventListener('change', function (e) {

    const newFiles = e.target.files;
    let rejectedFiles = [];

    for (let i = 0; i < newFiles.length; i++) {
        const file = newFiles[i];

        const existingCount = document.querySelectorAll('#existing-images-container .position-relative').length;
        const newlyAddedCount = dataTransfer.items.length;

        if (!file.type.startsWith('image/')) continue;

        if (existingCount + newlyAddedCount >= MAX_FILE_COUNT) {
            Alert.fire({
                title: 'Limit Doldu!',
                text: `Toplam resim sınırı ${MAX_FILE_COUNT} adettir.`,
                icon: 'info',
                showCancelButton: false,
                confirmButtonText: 'Anladım',
                showSuccessAlert: false
            });
            break;
        }

        if (file.size > MAX_FILE_SIZE) {
            rejectedFiles.push(file.name);
            continue;
        }

        dataTransfer.items.add(file);
    }

    if (rejectedFiles.length > 0) {
        Alert.fire({
            title: 'Büyük Dosya!',
            text: `Bazı dosyalar 5MB'dan büyük olduğu için eklenmedi: ${rejectedFiles.join(", ")}`,
            icon: 'error',
            showCancelButton: false,
            confirmButtonText: 'Tamam',
            showSuccessAlert: false
        });
    }

    fileInput.files = dataTransfer.files
    renderPreviews();
});

function renderPreviews() {

    container.innerHTML = '';

    const files = dataTransfer.files;

    for (let i = 0; i < files.length; i++) {
        const file = files[i];
        const reader = new FileReader();

        reader.onload = function (event) {
            const wrapperDiv = document.createElement('div');
            wrapperDiv.className = 'col-3 col-md-2 position-relative mt-3';

            wrapperDiv.innerHTML = `
                <div class="ratio ratio-1x1 border rounded overflow-hidden shadow-sm bg-white">
                    <img src="${event.target.result}" class="img-fluid object-fit-cover" style="object-fit: cover;">
                </div>
                <div class="btn-delete-image" data-index="${i}">
                    <i class="bi bi-x"></i>
                </div>
            `;

            container.appendChild(wrapperDiv);

            wrapperDiv.querySelector('.btn-delete-image').addEventListener('click', function () {
                const indexToRemove = parseInt(this.getAttribute('data-index'));
                removeFile(indexToRemove);
            });
        };

        reader.readAsDataURL(file);
    }
}

function removeFile(index) {

    dataTransfer.items.remove(index);

    fileInput.files = dataTransfer.files;

    renderPreviews();
}
