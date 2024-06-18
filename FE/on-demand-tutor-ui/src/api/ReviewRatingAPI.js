import apiClient from './apiClient';

const reviewRatingAPI = {

    getRating(id) {
        const url = '/ReviewRating/getAverageRating/' + id;
        return apiClient.get(url);
    },
    getReview(id) {
        const url = '/ReviewRating/getReviews/' + id;
        return apiClient.get(url);
    }
}

export default reviewRatingAPI;