import apiClient from './apiClient';

const logoutAPI = () => {
    return apiClient.post("/Account/logout");
}

export default logoutAPI;