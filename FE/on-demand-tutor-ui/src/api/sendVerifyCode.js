
import apiClient from './apiClient';



const sendVerifyCode = {
    sendCodeToEmail(email) {
        const url = '/Account/send-verification-code?toEmail=' + email;
        return apiClient.post(url);
    },
    verifyCode(email, code) {
        const url = '/Account/verify-code?email=' + email + '&code=' + code;
        return apiClient.post(url);
    }
}

export default sendVerifyCode;