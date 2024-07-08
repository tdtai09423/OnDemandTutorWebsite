import apiClient from './apiClient';

const learnerAPI = {
    get(id) {
        const url = '/Learner/' + id;
        return apiClient.get(url);
    },
    getFavourite(id) {
        const url = '/LearnerFavourites/learner/' + id;
        return apiClient.get(url);
    },
    getAll(token) {
        const url = '/Learner/get-all-learners?page=1&pageSize=10';
        return apiClient.get(url, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
    }
}

export default learnerAPI;