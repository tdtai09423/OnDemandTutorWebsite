import apiClient from './apiClient';

const reviewRatingAPI = {

    getRating(id) {
        const url = '/ReviewRating/getAverageRating/' + id;
        return apiClient.get(url);
    },
    getReview(id) {
        const url = '/ReviewRating/getReviews/tutor/' + id + '?page=1&pageSize=5'
        return apiClient.get(url);
    }
}

export default reviewRatingAPI;