import apiClient from './apiClient';

const orderAPI = {
    postShortTermBooking() {
        const url = '/Tutor/approved';
        return apiClient.get(url);
    },
}

export default orderAPI;