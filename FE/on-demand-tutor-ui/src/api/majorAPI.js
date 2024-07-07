import apiClient from './apiClient';

const majorAPI = {
    getAll(params) {
        const url = '/Major';
        return apiClient.get(url, { params });
    },

    get(id) {
        const url = '/Major/get-major/' + id;
        return apiClient.get(url);
    }
}

export default majorAPI;