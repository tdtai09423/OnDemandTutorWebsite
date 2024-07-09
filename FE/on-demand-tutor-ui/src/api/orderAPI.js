import apiClient from './apiClient';

const orderAPI = {
    getAll() {
        const url = '/LearnerOrder';
        return apiClient.get(url);
    },

}

export default orderAPI;