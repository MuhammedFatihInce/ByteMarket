document.getElementById('image-input').addEventListener('change', function (e) {
    const container = document.getElementById('image-preview-container');
    container.innerHTML = ''; // Önceki ön izlemeleri temizle

    const files = e.target.files;

    for (let i = 0; i < files.length; i++) {
        const file = files[i];
        if (!file.type.startsWith('image/')) continue; // Sadece resim dosyaları

        const reader = new FileReader();
        reader.onload = function (event) {
            // Her resim için küçük bir kart oluştur
            const div = document.createElement('div');
            div.className = 'col-3 col-md-2 position-relative';
            div.innerHTML = `
                        <div class="ratio ratio-1x1 border rounded overflow-hidden shadow-sm bg-white">
                            <img src="${event.target.result}" class="img-fluid object-fit-cover" style="object-fit: cover;">
                        </div>
                    `;
            container.appendChild(div);
        };
        reader.readAsDataURL(file);
    }
});