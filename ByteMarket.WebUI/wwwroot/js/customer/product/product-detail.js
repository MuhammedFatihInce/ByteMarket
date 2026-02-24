function imageZoom(imgID, resultID, lensID) {
    const img = document.getElementById(imgID);
    const result = document.getElementById(resultID);
    const lens = document.getElementById(lensID);

    if (!img || !result || !lens) return;

    if (!img.complete) {
        img.onload = () => imageZoom(imgID, resultID, lensID);
        return;
    }

    const originalResultDisplay = result.style.display;
    const originalLensDisplay = lens.style.display;

    result.style.display = "block";
    lens.style.display = "block";

    const lWidth = lens.offsetWidth || parseFloat(window.getComputedStyle(lens).width);
    const lHeight = lens.offsetHeight || parseFloat(window.getComputedStyle(lens).height);

    const cx = result.offsetWidth / lWidth;
    const cy = result.offsetHeight / lHeight;

    result.style.display = originalResultDisplay;
    lens.style.display = originalLensDisplay;

    result.style.backgroundImage = `url('${img.src}')`;
    result.style.backgroundSize = `${img.offsetWidth * cx}px ${img.offsetHeight * cy}px`;

    const moveLens = (e) => {
        e.preventDefault();
        const pos = getCursorPos(e, img);
        let x = pos.x - (lens.offsetWidth / 2);
        let y = pos.y - (lens.offsetHeight / 2);

        if (x > img.width - lens.offsetWidth) x = img.width - lens.offsetWidth;
        if (x < 0) x = 0;
        if (y > img.height - lens.offsetHeight) y = img.height - lens.offsetHeight;
        if (y < 0) y = 0;

        lens.style.left = (x + img.offsetLeft) + "px";
        lens.style.top = (y + img.offsetTop) + "px";
        result.style.backgroundPosition = `-${x * cx}px -${y * cy}px`;
    };

    img.addEventListener("mousemove", moveLens);
    lens.addEventListener("mousemove", moveLens);

    const container = img.parentElement;
    container.addEventListener("mouseenter", () => {
        lens.style.display = "block";
        result.style.display = "block";
        result.style.backgroundSize = `${img.offsetWidth * cx}px ${img.offsetHeight * cy}px`;
    });

    container.addEventListener("mouseleave", () => {
        lens.style.display = "none";
        result.style.display = "none";
    });
}

function getCursorPos(e, img) {
    const a = img.getBoundingClientRect();
    return {
        x: e.pageX - a.left - window.scrollX,
        y: e.pageY - a.top - window.scrollY
    };
}

// Global Carousel Olayı
function initProductDetailCarousel(carouselID) {
    const myCarousel = document.getElementById(carouselID);
    if (!myCarousel) return;

    // Thumbnail Senkronizasyonu
    myCarousel.addEventListener('slide.bs.carousel', function (e) {
        document.querySelectorAll('.thumb-item').forEach(el => el.classList.remove('active'));
        document.querySelector(`.thumb-item[data-bs-slide-to="${e.to}"]`).classList.add('active');
    });

    // Zoom Senkronizasyonu
    myCarousel.addEventListener('slid.bs.carousel', function (e) {
        imageZoom(`image-${e.to}`, "zoom-result", `lens-${e.to}`);
    });
}