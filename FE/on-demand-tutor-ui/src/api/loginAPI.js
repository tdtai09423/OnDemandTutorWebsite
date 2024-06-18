import apiClient from './apiClient';

const loginAPI = (email, password, rememberMe) => {
    console.log("goi api ");
    return apiClient.post("/Account/login", { email, password, rememberMe });
}

export default loginAPI;