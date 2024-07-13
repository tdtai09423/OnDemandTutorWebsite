import apiClient from './apiClient';

const userAPI = {

    getUserByEmail(email) {
        const url = '/Account/get-account-by-email?email=' + email;
        return apiClient.post(url);
    },
    getAll(token, page, rowsPerPage) {
        const url = 'Account/all-accounts?page=' + page + '&pageSize=' + rowsPerPage;
        return apiClient.get(url, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
    },
    getBalance(id) {
        const url = '/Wallet/get-wallet?accountId=' + id;
        return apiClient.get(url);
    },
    upgradeMembership(id, token) {
        const url = 'Membership/upgrade-membership?membershipLevel=SILVER';
        return apiClient.post(url, {
            learnerId: id
        }, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
    }
}

export default userAPI;