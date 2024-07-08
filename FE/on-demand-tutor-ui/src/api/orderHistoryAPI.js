import apiClient from './apiClient';

const orderHistoryAPI = {

    getOrderHistoryById(learnerId) {
        const url = '/LearnerOrder/Learner/' + learnerId;
        return apiClient.get(url);
    },
    getOrderListByTutorId(tutorId, token) {
        const url = '/LearnerOrder/get-orders-list/' + tutorId + '?page=1&pageSize=100';
        return apiClient.get(url, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
    },
    postAcceptOrder(tutorId, orderId, token) {
        const url = '/LearnerOrder/accept-booking';
        return apiClient.post(url, {
            tutorId: tutorId,
            orderId: orderId
        }, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
    },
    postRejectOrder(tutorId, orderId, token) {
        const url = '/LearnerOrder/reject-booking';
        return apiClient.post(url, {
            tutorId: tutorId,
            orderId: orderId
        }, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
    },
    postCancelBooking(learnerId, orderId, token) {
        const url = `/LearnerOrder/cancel-booking?orderId=${orderId}&learnerId=${learnerId}`;
        return apiClient.post(url, {
            learnerId: learnerId,
            orderId: orderId
        }, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
    },
    putTimeOrder(orderId, time, token) {
        const formData = new FormData();
        formData.append('startTime', time);
        const url = '/LearnerOrder/update-booking/' + orderId;
        return apiClient.put(url, formData, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
    },
    postConfirmCompleteSection(sectionId, orderId, token) {
        const url = '/LearnerOrder/confirm-section-completion?orderId=' + orderId + '&sectionId=' + sectionId;
        return apiClient.post(url, null, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
    }
}

export default orderHistoryAPI;