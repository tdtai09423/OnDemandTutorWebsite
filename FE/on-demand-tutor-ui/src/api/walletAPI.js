import apiClient from './apiClient';

const walletAPI = {

    topUp(amount, learnerId) {
        const url = `LearnerOrder/top-up-wallet?amount=${amount}&learnerId=${learnerId}`;
        return apiClient.post(url);
    }
}

export default walletAPI;