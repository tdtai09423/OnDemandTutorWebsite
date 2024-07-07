import apiClient from './apiClient';

const transactionApi = {

    postReceiWage(tutorId, amount) {
        const url = '/Transaction/receive-wage?amount=' + amount;
        return apiClient.post(url);
    }
}

export default transactionApi;