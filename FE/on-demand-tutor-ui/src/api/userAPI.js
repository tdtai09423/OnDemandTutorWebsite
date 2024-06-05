import apiClient from './apiClient';

const userAPI = {

    getUserByEmail(email) {
        const url = '/Account/get-account-by-email?email=' + email;
        return apiClient.post(url);
    }
}

export default userAPI;