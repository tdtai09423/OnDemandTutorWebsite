import axios from 'axios';

const loginAPI = (email, password) => {
    return axios.post("/api/login", { email, password });
}

export default loginAPI