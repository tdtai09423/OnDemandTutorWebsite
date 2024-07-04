import apiClient from './apiClient';

const NotificationAPI = {
    getNotification(userId) {
        const url = '/Notification/get-notifications-by-account/' + userId;
        return apiClient.get(url);
    }
    

}

export default NotificationAPI;