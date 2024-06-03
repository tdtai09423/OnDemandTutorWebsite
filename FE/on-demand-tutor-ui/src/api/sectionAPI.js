import apiClient from './apiClient';

const sectionAPI = {

    get(id) {
        const url = '/Section/tutor-max-price/' + id;
        return apiClient.get(url);
    }
}

export default sectionAPI;