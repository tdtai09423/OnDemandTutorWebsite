import apiClient from './apiClient';

const sectionAPI = {

    get(id) {
        const url = '/Section/tutor-max-price/' + id;
        return apiClient.get(url);
    },
    getTutorSection(id, start, end) {
        const url = '/Section/weekly-schedule-tutor?tutorId=' + id + '&startTime=' + start + '&endTime=' + end
        return apiClient.get(url);
    },
    getLearnerSection(learnerId, formattedStartDate, formattedEndDate, token) {
        const url = '/Section/weekly-schedule-learner?learnerId=' + learnerId + '&startTime=' + formattedStartDate + '&endTime=' + formattedEndDate;
        return apiClient.get(url, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
    }
}

export default sectionAPI;