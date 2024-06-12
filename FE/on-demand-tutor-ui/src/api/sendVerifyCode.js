import apiClient from './apiClient';

const sendVerifyCode = (email) => {
    console.log("goi api send verification code");
    return apiClient.post("/Account/send-verification-code", { email});
}

export default sendVerifyCode;