import apiClient from './apiClient';

const curriculumAPI = {

    getCurriculumByOrderId(orderId) {
        const url = '/Curriculum/curriculum-by-order/' + orderId;
        return apiClient.get(url);
    }
}

export default curriculumAPI;