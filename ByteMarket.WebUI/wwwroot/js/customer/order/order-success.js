$(document).ready(function () {

    $(document).on('click', '#btnSendInvoice', async function (e) {
        e.preventDefault();

        const $btn = $(this);
        const id = $btn.data('orderid');

        console.log(id);

        $btn.prop('disabled', true);

        try {
            const response = await CustomAjax.post(`/Order/SendInvoice/${id}`);
            if (response && response.success) {
                Alert.toast({ title: response.message, icon: 'success' });
            } else {
                Alert.toast({ title: response.message, icon: 'error' });
                $btn.prop('disabled', false);
            }
        } catch (error) {
            Alert.toast({ title: "Bir hata oluştu", icon: 'error' });
            $btn.prop('disabled', false);
        }

    });


    $("#btnDownloadPdf").click(function () {

        const element = $('#invoiceContainer')[0];

        const opt = {
            margin: [10, 10, 10, 10],
            filename: 'Fatura_#' + document.querySelector('strong').innerText + '.pdf',
            image: { type: 'jpeg', quality: 0.98 },
            html2canvas: {
                scale: 2,
                useCORS: true,
                logging: true
            },
            jsPDF: { unit: 'mm', format: 'a4', orientation: 'portrait' }
        };

        html2pdf().set(opt).from(element).save();
    });
});