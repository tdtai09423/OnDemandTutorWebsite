import apiClient from './apiClient';

const orderHistoryAPI = {

    getOrderHistoryById(learnerId) {
        const url = '/Orders/history/' + learnerId;
        return apiClient.get(url);
    },
    getOrderListByTutorId(tutorId, token) {
        const url = '/LearnerOrder/get-orders-list/' + tutorId + '?page=1&pageSize=100';
        return apiClient.get(url, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
    }
}

export default orderHistoryAPI;