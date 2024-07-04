import apiClient from './apiClient';

const userAPI = {

    getUserByEmail(email) {
        const url = '/Account/get-account-by-email?email=' + email;
        return apiClient.post(url);
    },
    getAll(token) {
        const url = 'Account/all-accounts';
        return apiClient.get(url, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
    },
    getBalance(id) {
        const url = '/Wallet/get-wallet?accountId=' + id;
        return apiClient.get(url);
    }
}

export default userAPI;