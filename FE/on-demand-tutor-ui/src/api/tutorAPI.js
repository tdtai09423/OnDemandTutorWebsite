import apiClient from './apiClient';

const tutorAPI = {
    getApproved(params) {
        const url = '/Tutor/approved';
        return apiClient.get(url, { params });
    },
    getPending(token, page, pageSize) {
        const url = '/Tutor/pending?page=' + page + '&pageSize=' + pageSize;
        return apiClient.get(url, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
    },
    getRejected(token, page, pageSize) {
        const url = '/Tutor/rejected?page=' + page + '&pageSize=' + pageSize;
        return apiClient.get(url, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
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