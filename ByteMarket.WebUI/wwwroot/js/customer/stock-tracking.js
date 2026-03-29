document.addEventListener("DOMContentLoaded", function () {

	const apiBaseUrlMeta = document.querySelector('meta[name="api-base-url"]');
	if (!apiBaseUrlMeta) return;

	const apiBaseUrl = apiBaseUrlMeta.content;
	const url = new URL(apiBaseUrl);
	const originUrl = url.origin;
	const apiHubUrl = `${originUrl}/stockHub`;

	const connection = new signalR.HubConnectionBuilder()
		.withUrl(apiHubUrl)
		.withAutomaticReconnect()
		.build();

	connection.start()
		.then(() => {
			console.log("SignalR: API Hub'ına başarıyla bağlanıldı. Canlı stok dinleniyor...");
		})
		.catch(err => {
			console.error("SignalR Bağlantı Hatası:", err.toString());
		});

	connection.on("ReceiveStockUpdate", (productId, newStock) => {

		const safeProductId = productId.toLowerCase();

		const stockElement = document.getElementById(`stock-${safeProductId}`);
		if (stockElement) {
			stockElement.innerText = `Stok: ${newStock}`;
		}

		const detailElement = document.getElementById(`stock-detail-${safeProductId}`);
		if (newStock > 0) {
			detailElement.innerText = `Şu an stokta ${newStock} adet var!`;
			detailElement.classList.remove("text-danger");
			detailElement.classList.add("text-success");
		} else {
			detailElement.innerText = "Tükendi";
			detailElement.classList.remove("text-success");
			detailElement.classList.add("text-danger");
		}


	});
});