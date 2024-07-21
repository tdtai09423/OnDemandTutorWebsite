import { Token } from '@mui/icons-material';
import apiClient from './apiClient';

const ToggleStatusAPI = (email, status, token) => {

    //return apiClient.put("/Account/toggle-status", { email, status, token });
    return apiClient.put("/Account/toggle-status", {
        email: email,
        status: status
    }, {
        headers: {
            Authorization: `Bearer ${token}`
        }
    });
}

export default ToggleStatusAPI;


