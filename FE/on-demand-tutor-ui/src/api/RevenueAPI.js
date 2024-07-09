import apiClient from './apiClient';

const RevenueAPI = {
    getRevenueByMonth(tutorId, month, year, token) {
        const url = '/Analyst/get-monthly-bookings-summary/'+tutorId+'?month='+month+'&year='+year;
        return apiClient.get(url, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
    },
    
}

export default RevenueAPI;