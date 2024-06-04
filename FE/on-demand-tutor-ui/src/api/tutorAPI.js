import apiClient from './apiClient';

const tutorAPI = {
    getAll(params) {
        const url = '/Tutor/approved';
        return apiClient.get(url, { params });
    },

    get(id) {
        const url = '/Tutor/' + id;
        return apiClient.get(url);
    },
    getCerti(id){
        const url = '/TutorCerti/' + id;
        return apiClient.get(url);
    }
}

export default tutorAPI;