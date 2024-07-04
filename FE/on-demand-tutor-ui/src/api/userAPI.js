import apiClient from './apiClient';

const userAPI = {

    getUserByEmail(email) {
        const url = '/Account/get-account-by-email?email=' + email;
        return apiClient.post(url);
    },
    getAll() {
        const url = 'Account/all-accounts';
        return apiClient.get(url);
    },
    getBalance(id) {
        const url = '/Wallet/get-wallet?accountId=' + id;
        return apiClient.get(url);
    }
}

export default userAPI;