import apiClient from './apiClient';

const NotificationAPI = {
    getNotification(userId) {
        const url = '/Notification/get-notifications-by-account/' + userId;
        return apiClient.get(url);
    },
    putMarkAsRead(userId, token) {

        const url = '/Notification/mark-as-read';
        return apiClient.put(url, userId, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
    }
}

export default NotificationAPI;