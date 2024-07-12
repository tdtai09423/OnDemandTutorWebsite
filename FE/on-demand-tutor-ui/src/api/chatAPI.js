import apiClient from './apiClient';

const chatAPI = {

    loadChat(chatboxId) {
        const url = 'Chat/chatMessages/' + chatboxId;
        return apiClient.get(url);
    },
    loadChatBoxByOneId(userId) {
        const url = 'Chat/chatboxes/' + userId;
        return apiClient.get(url);
    },
    loadChatBoxByTwoId(learnerId, tutorId) {
        const url = 'Chat/chatbox/' + learnerId + '/' + tutorId;
        return apiClient.get(url);
    },
}

export default chatAPI;

