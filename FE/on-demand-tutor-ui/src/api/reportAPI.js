import apiClient from './apiClient';

const reportAPI = {
    sendReport(content, token) {
        const url = 'https://localhost:7010/api/Report/add-new-report';
        return apiClient.get(url, content, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
    },
    getFavourite(id) {
        const url = '/LearnerFavourites/learner/' + id;
        return apiClient.get(url);
    }
}

export default reportAPI;