import apiClient from './apiClient';

const reviewRatingAPI = {

    get(id) {
        const url = '/ReviewRating/getAverageRating/' + id;
        return apiClient.get(url);
    }
}

export default reviewRatingAPI;