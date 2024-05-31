import axiosClient from './axiosClient';

const tutorAPI = {
    getAll(params) {
        const url = '/hot';
        return axiosClient.get(url, { params });
    },
    get(id) {
        const url = '/tutors/' + id;
        return axiosClient.get(url);
    },
};

export default tutorAPI;