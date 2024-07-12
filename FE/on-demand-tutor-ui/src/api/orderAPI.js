import apiClient from './apiClient';

const orderAPI = {
    getAll() {
        const url = '/LearnerOrder';
        return apiClient.get(url);
    },
    getOrdersByTimePeriod(year) {
        const url = `/LearnerOrder/get-orders-by-time-period?startTime=${year}-01-01&endTime=${year}-12-31&page=1&pageSize=10000`;
        return apiClient.get(url);
    },

}

export default orderAPI;