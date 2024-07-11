import apiClient from './apiClient';

const curriculumAPI = {

    getCurriculumByOrderId(orderId) {
        const url = '/Curriculum/curriculum-by-order/' + orderId;
        return apiClient.get(url);
    },
    getCurriculumByTutorId(tutorId) {
        const url = '/Curriculum/get-all-curricula/' + tutorId;
        return apiClient.get(url);
    },
    addNewCurriculumn(data, token) {
        const url = '/Curriculum/add-new-curriculum';
        return apiClient.post(url, data, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
    },
    deleteCurriculumn(id, token) {
        const url = '/Curriculum/reject-curriculum/' + id;
        return apiClient.put(url, null, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
    }
}

export default curriculumAPI;