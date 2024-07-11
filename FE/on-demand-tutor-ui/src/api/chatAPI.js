import apiClient from './apiClient';

const chatAPI = {

    loadChat(chatboxId) {
        const url = 'Chat/chatMessages/' + chatboxId;
        return apiClient.get(url);
    }
}

export default chatAPI;