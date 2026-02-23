document.addEventListener('DOMContentLoaded', function () {
    const radar = document.getElementById('temp-data-radar');
    if (!radar) return;

    const success = radar.getAttribute('data-success');
    const error = radar.getAttribute('data-error');
    const warning = radar.getAttribute('data-warning');

    if (success && success.trim() !== "") {
        Alert.toast({ title: success, icon: 'success' });
    }

    if (error && error.trim() !== "") {
        Alert.toast({ title: error, icon: 'error', iconColor: '#dc3545' });
    }

    if (warning && warning.trim() !== "") {
        Alert.toast({ title: warning, icon: 'warning', iconColor: '#ffc107' });
    }
});