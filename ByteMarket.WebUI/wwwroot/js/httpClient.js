
const HttpClient = (function () {
	const baseUrl = document.querySelector('meta[name="api-base-url"]').getAttribute('content');

	async function request(endpoint, options = {}) {
		const url = `${baseUrl}${endpoint.startsWith('/') ? endpoint.substring(1) : endpoint}`;

		const defaultOptions = {
			headers: {
				'Accept': 'application/json'
			}
		}

		try {
			const response = await fetch(url, { ...defaultOptions, ...options });

			if (!response.ok) {
				const errorData = await response.json().catch(() => ({}));
				throw { status: response.status, data: errorData }
			}

			if (response.status === 204) return null;

			return await response.json();

		} catch (error) {
			console.error(`API Hatası [${url}]:`, error);
			throw error;
		}
	}

	return {
		get: (url) => request(url, { method: 'GET' }),
		post: (url, data) => request(url, {
			method: 'POST',
			headers: { 'Content-Type': 'application/json' },
			body: JSON.stringify(data)
		}),
		put: (url, data) => request(url, {
			method: 'PUT',
			headers: { 'Content-Type': 'application/json' },
			body: JSON.stringify(data)
		}),
		delete: (url) => request(url, { method: 'DELETE' }),
	};
})();