import apiClient from './apiClient';

const filterAPI = (major, nationality, available, native, also) => {
    return apiClient.post("/api/login", { major, nationality, available, native, also });
}

export default filterAPI