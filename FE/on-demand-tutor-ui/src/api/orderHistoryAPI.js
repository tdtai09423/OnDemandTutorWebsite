import apiClient from './apiClient';

const orderHistoryAPI = {

    getOrderHistoryById(learnerId) {
        const url = '/Orders/history/' + learnerId;
        return apiClient.get(url);
    }
}

export default orderHistoryAPI;