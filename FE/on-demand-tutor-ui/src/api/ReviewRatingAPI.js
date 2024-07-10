import apiClient from './apiClient';

const reviewRatingAPI = {

    getRating(id) {
        const url = '/ReviewRating/getAverageRating/' + id;
        return apiClient.get(url);
    },
    getReview(id) {
        const url = '/ReviewRating/getReviews/tutor/' + id + '?page=1&pageSize=10'
        return apiClient.get(url);
    },
    postReview(orderId, tutorId, learnerId, formData, token) {
        const url = '/ReviewRating/review-tutor?orderId=' + orderId + '&tutorId=' + tutorId + '&learnerId=' + learnerId;
        return apiClient.post(url, formData, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
    },
}

export default reviewRatingAPI;