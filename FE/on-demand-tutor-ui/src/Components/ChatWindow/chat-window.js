import React, { useState, useEffect } from 'react';
import * as signalR from '@microsoft/signalr';

const Chat = () => {
    const [connection, setConnection] = useState(null);
    const [messages, setMessages] = useState([]);
    const [message, setMessage] = useState("");
    const [toUserId, setToUserId] = useState(11); // ID của người nhận
    const [userId, setUserId] = useState(1); // ID của người gửi, có thể thay bằng giá trị thực

    useEffect(() => {
        const newConnection = new signalR.HubConnectionBuilder()
            .withUrl("/chathub")
            .withAutomaticReconnect()
            .build();

        setConnection(newConnection);
    }, []);

    useEffect(() => {
        if (connection) {
            connection.start()
                .then(result => {
                    console.log('Connected!');

                    connection.on('ReceiveMessage', (fromUserId, message) => {
                        const newMessage = `${fromUserId}: ${message}`;
                        setMessages(messages => [...messages, newMessage]);
                    });
                })
                .catch(e => console.log('Connection failed: ', e));
        }
    }, [connection]);

    const sendMessage = async () => {
        if (connection.connectionStarted) {
            try {
                await connection.invoke("SendMessageToUser", userId, toUserId, message);
                setMessage("");
            } catch (e) {
                console.error(e);
            }
        } else {
            alert('No connection to server yet.');
        }
    }

    return (
        <div>
            <div>
                <input type="text" value={message} onChange={e => setMessage(e.target.value)} />
                <button onClick={sendMessage}>Send</button>
            </div>
            <div>
                {messages.map((m, index) => <div key={index}>{m}</div>)}
            </div>
        </div>
    );
};

export default Chat;
