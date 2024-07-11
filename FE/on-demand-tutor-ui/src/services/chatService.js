// src/services/signalRService.js
import * as signalR from '@microsoft/signalr';

class SignalRService {
    constructor() {
        this.connection = null;
    }

    startConnection(chatBoxId) {
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(`https://localhost:7010/chatHub?chatBoxId=${chatBoxId}`, {
                withCredentials: true
            })
            .withAutomaticReconnect()
            .build();

        return this.connection.start()
            .catch(err => console.error('SignalR Connection Error: ', err));
    }

    stopConnection() {
        if (this.connection) {
            return this.connection.stop();
        }
    }

    onReceiveMessage(callback) {
        if (this.connection) {
            this.connection.on('ReceiveMessage', callback);
        }
    }

    sendMessage(chatBoxId, sender, message) {
        if (this.connection) {
            return this.connection.invoke('SendMessage', chatBoxId, sender, message)
                .catch(err => console.error('Send Message Error: ', err));
        }
    }
}

export default new SignalRService();
