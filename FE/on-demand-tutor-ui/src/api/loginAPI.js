import apiClient from './apiClient';

const loginAPI = (email, password) => {
    return apiClient.post("/api/login", { email, password });
}

export default loginAPI