import apiClient from './apiClient';

const learnerAPI = {
    get(id) {
        const url = '/Learner/' + id;
        return apiClient.get(url);
    }
}

export default learnerAPI;