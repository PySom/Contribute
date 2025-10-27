const apiBase = 'https://competent-lamport.192-250-231-17.plesk.page';

const api = {
    get: async (endpoint) => {
        try {
            const response = await fetch(`${apiBase}/${endpoint}`);
            if (!response.ok) {
                throw new Error(`Error fetching ${endpoint}: ${response.statusText}`);
            }
            return await response.json();
        } catch (error) {
            console.error(error);
            return null;
        }
    },

    post: async (endpoint, data) => {
        try {
            const response = await fetch(`${apiBase}/${endpoint}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data)
            });
            if (!response.ok) {
                throw new Error(`Error posting to ${endpoint}: ${response.statusText}`);
            }
            return await response.json();
        } catch (error) {
            console.error(error);
            return null;
        } 
    }
}

export default api;