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


$(document).ready(function () {


    $(".btn-edit-review").on("click", function (e) {
        e.preventDefault();

        var $btn = $(this);

        const reviewId = $(this).data("id");
        const comment = $(this).data("comment");
        const rating = $(this).data("rating");

        $("#editReviewId").val(reviewId);
        $("#editComment").val(comment);

        $(`input[name="editRating"][value="${rating}"]`).prop("checked", true);

        const editModal = new bootstrap.Modal(document.getElementById('editReviewModal'));
        editModal.show();

    });

    $("#btnUpdateReview").on("click", async function () {

        const reviewId = $("#editReviewId").val();
        const comment = $("#editComment").val();
        const rating = $("input[name='editRating']:checked").val();

        if (!rating) {
            $("#editModelResponseMessage").html('<div class="alert alert-danger">Lütfen bir puan seçiniz!</div>');
            return;
        }
        $("#responseMessage").addClass("d-none");


        const data = {
            Id: reviewId,
            Comment: comment,
            Rating: rating
        };

        try {
            const response = await CustomAjax.put('Product/UpdateReview', data);

            if (response && response.success) {
                Alert.toast({ title: response.message, icon: 'success' });
                setTimeout(() => location.reload(), 1500);
            }
        } catch (error) {
            console.error("Yorum hatası:", error);
        }
    });


    $(".btn-delete-review").on("click", async function () {

        var reviewId = $(this).data("id");
        var reviewCard = $("#review-" + reviewId);

        var reviewsCount = parseInt($('#comment-count').text(), 10);

        var ratingText = $('#average-comment-rating').text().trim().replace(',', '.');
        var averageCommentRating = parseFloat(ratingText);

        var reviewRating =  parseInt($(this).data("rating"), 10);

        Alert.fire({
            title: 'Yorum Silinecek!',
            text: `Bu yorumu silmek istediğinize emin misiniz?`,
            icon: 'warning',
            confirmButtonText: 'Evet, Sil',
            onConfirm: async () => {
                try {
                    const response = await CustomAjax.delete(`/Product/DeleteReview/${reviewId}`);

                    if (response && response.success) {
                        Alert.toast({ title: response.message, icon: 'success' });
                        reviewCard.fadeOut(500, function () { $(this).remove(); });

                        if (!isNaN(reviewsCount) && !isNaN(averageCommentRating) && !isNaN(reviewRating) && reviewsCount > 0 ) {

                            var totalRating = averageCommentRating * reviewsCount;
                            var newTotalRating = totalRating - reviewRating;
                            var newReviewsCount = reviewsCount - 1;
                            var newAverageRating = (newTotalRating / newReviewsCount).toFixed(1);

                            $('#comment-count').text(newReviewsCount);
                            $('#average-comment-rating').text(newAverageRating);
                        }

                    } else {
                        Alert.error({ title: "Hata", text: response.message });
                    }
                } catch (error) {
                    Alert.error({ title: "Hata", text: "Bağlantı kurulamadı!" });
                } 
            },
            successMessage: 'Yorum başarıyla silindi!'
        });
        
    });
});