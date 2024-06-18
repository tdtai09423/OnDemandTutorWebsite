import apiClient from './apiClient';

const tutorAPI = {
    getApproved(params) {
        const url = '/Tutor/approved';
        return apiClient.get(url, { params });
    },
    getPending(params) {
        const url = '/Tutor/pending';
        return apiClient.get(url, { params });
    },
    getRejected(params) {
        const url = '/Tutor/rejected';
        return apiClient.get(url, { params });
    },
    get(id) {
        const url = '/Tutor/' + id;
        return apiClient.get(url);
    },
    getCerti(id) {
        const url = '/TutorCerti/' + id;
        return apiClient.get(url);
    },
    putCertificateStatus(id, statusType) {
        const url = '/TutorCerti/' + statusType + '/' + id;
        return apiClient.post(url);
    }
}

export default tutorAPI;