import apiClient from './apiClient';

const learnerAPI = {
    get(id) {
        const url = '/Learner/' + id;
        return apiClient.get(url);
    },
    getFavourite(id) {
        const url = '/LearnerFavourites/learner/' + id;
        return apiClient.get(url);
    }
}

export default learnerAPI;