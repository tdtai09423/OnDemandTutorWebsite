import apiClient from './apiClient';

const learnerAPI = {
    get(id) {
        const url = '/Learner/' + id;
        return apiClient.get(url);
    },
    getFavourite(id) {
        const url = '/LearnerFavourite/' + id;
        return apiClient.get(url);
    },
    getAll(token) {
        const url = '/Learner/get-all-learners?page=1&pageSize=10';
        return apiClient.get(url, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
    },
    addFavourite(tutorId, learnerId, token) {
        const url = 'LearnerFavourite/add-favorite?learnerId=' + learnerId + '&tutorId=' + tutorId;
        return apiClient.post(url, null, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
    },
    removeFavourite(tutorId, learnerId) {
        const url = 'LearnerFavourite/' + learnerId + '/' + tutorId;
        return apiClient.delete(url);
    },
}

export default learnerAPI;