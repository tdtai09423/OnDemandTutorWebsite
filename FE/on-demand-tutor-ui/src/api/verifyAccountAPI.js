
import apiClient from './apiClient';



const VerifyAccount = {
    postSendCodeToEmail(email) {
        const url = '/Account/forgot-password?email=' + email;
        return apiClient.post(url);
    },
    getVerifyCode(email, code) {
        const url = '/Account/reset-password?email=' + email + '&code=' + code;
        return apiClient.get(url);
    },
    postResetPassword(newPassword, confirmPassword) {
        const url = `/Account/reset-password?newPassword=${newPassword}&confirmPassword=${confirmPassword}`;
        return apiClient.post(url);
    }
}

export default VerifyAccount;