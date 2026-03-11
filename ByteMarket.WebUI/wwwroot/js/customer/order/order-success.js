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
});