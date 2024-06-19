import apiClient from './apiClient';

const loginGoogleAPI = () => {
    return apiClient.get("/Account/login-google");
}

export default loginGoogleAPI;