// src/services/chatService.js
import * as signalR from '@microsoft/signalr';

class SignalRService {
    constructor() {
        this.connection = null;
        this.isConnected = false;
    }

    startConnection(chatBoxId) {
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(`http://localhost:7010/chatHub?chatBoxId=${chatBoxId}`, {
                withCredentials: true
            })
            .withAutomaticReconnect()
            .build();

        this.connection.onclose(() => {
            this.isConnected = false;
        });

        return this.connection.start()
            .then(() => {
                this.isConnected = true;
                console.log('SignalR Connected');
            })
            .catch(err => {
                this.isConnected = false;
                console.error('SignalR Connection Error: ', err);
            });
    }

    stopConnection() {
        if (this.connection) {
            return this.connection.stop()
                .then(() => {
                    this.isConnected = false;
                    console.log('SignalR Disconnected');
                });
        }
    }

    onReceiveMessage(callback) {
        if (this.connection) {
            this.connection.on('ReceiveMessage', callback);
        }
    }

    sendMessage(chatBoxId, sender, message) {
        if (this.isConnected && this.connection) {
            return this.connection.invoke('SendMessage', chatBoxId, sender, message)
                .catch(err => console.error('Send Message Error: ', err));
        } else {
            console.error('Cannot send message: SignalR connection is not established.');
            return Promise.reject('SignalR connection is not established.');
        }
    }
}

export default new SignalRService();
