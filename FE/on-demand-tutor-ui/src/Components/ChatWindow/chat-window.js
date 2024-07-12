import React, { useEffect, useState } from 'react';
import SignalRService from '../../services/chatService';
import * as signalR from '@microsoft/signalr';
import chatAPI from '../../api/chatAPI';
import {
    MDBContainer,
    MDBRow,
    MDBCol,
    MDBCard,
    MDBCardHeader,
    MDBCardBody,
    MDBIcon,
    MDBBtn,
    MDBCardFooter,
    MDBInputGroup,
} from "mdb-react-ui-kit";
import './chat-window.scss'

const ChatWindow = ({ chatBoxId, userRole, onClose, me, they }) => {

    const [connection, setConnection] = useState(null);
    const [messages, setMessages] = useState([]);
    const [message, setMessage] = useState('');
    const [isConnected, setIsConnected] = useState(false);

    useEffect(() => {
        const newConnection = new signalR.HubConnectionBuilder()
            .withUrl(`https://localhost:7010/chatHub?chatBoxId=${chatBoxId}`, { withCredentials: true })
            .withAutomaticReconnect()
            .build();

        setConnection(newConnection);

        const fetchMessages = async () => {
            try {
                const response = await chatAPI.loadChat(chatBoxId);
                setMessages(response.data.$values);
                console.log(response.data.$values)
            } catch (error) {
                console.error('Error fetching messages:', error);
            }
        };

        fetchMessages();
    }, []);

    useEffect(() => {
        if (connection) {
            connection.start()
                .then(result => {
                    console.log('Connected!');
                    setIsConnected(true);


                })
                .catch(e => {
                    console.log('Connection failed: ', e);
                    setIsConnected(false);
                    console.log(isConnected);
                });

            connection.on('ReceiveMessage', message => {
                setMessages(messages => [...messages, message]);
                console.log('>>>RECEIVE')
            });

            connection.onclose(e => {
                console.log('Connection closed: ', e);
                setIsConnected(false);
                console.log(isConnected);
            });

            connection.onreconnecting(error => {
                console.log('Reconnecting: ', error);
                setIsConnected(false);
                console.log(isConnected);
            });

            connection.onreconnected(connectionId => {
                console.log('Reconnected: ', connectionId);
                setIsConnected(true);
                console.log(isConnected);
            });
        }
    }, [connection]);

    const sendMessage = async () => {
        if (isConnected) {
            const messageObj = {
                sender: userRole,
                content: message,
                sendDate: new Date().toISOString(),
            };
            try {
                // Update local state immediately
                // setMessages((prevMessages) => [...prevMessages, messageObj]);
                setMessage("");
                await connection.send('SendMessage', chatBoxId, userRole, message);
            } catch (err) {
                console.error('Sending message failed: ', err);
            }
        } else {
            console.log('No connection to server yet.');
        }
    };


    return (
        <>
            <MDBContainer className="py-5 chat-box">
                <MDBRow className="d-flex">
                    <MDBCol md="8" lg="6" xl="4" className='w-100'>
                        <MDBCard>
                            <MDBCardHeader
                                className="d-flex justify-content-between align-items-center p-3"
                                style={{ borderTop: "4px solid #ff7aac" }}
                            >
                                <h5 className="mb-0">Chat</h5>
                                <div className="d-flex flex-row align-items-center">
                                    <MDBIcon
                                        fas
                                        icon="times"
                                        size="xs"
                                        className="me-3 text-muted"
                                        onClick={onClose}
                                    />
                                </div>
                            </MDBCardHeader>
                            <MDBCardBody className='chat-content'>
                                {messages.map((m, index) => {
                                    if (m.sender !== userRole) {
                                        return (
                                            <div key={index} className={(m.sender !== userRole) ? 'my-message' : 'other-message'}>
                                                <div className="d-flex justify-content-between">
                                                    <p className="small mb-1">{they.tutorNavigation.firstName}</p>
                                                    <p className="small mb-1 text-muted">{m.SendDate}</p>
                                                </div>
                                                <div className="d-flex flex-row justify-content-start">
                                                    <img
                                                        src={(they.tutorPicture) ? they.tutorPicture : "https://villagesonmacarthur.com/wp-content/uploads/2020/12/Blank-Avatar.png"}
                                                        alt="avatar 1"
                                                        style={{ width: "45px", height: "100%" }}
                                                    />
                                                    <div>
                                                        <p
                                                            className="small p-2 ms-3 mb-3 rounded-3"
                                                            style={{ backgroundColor: "#f5f6f7" }}
                                                        >
                                                            {m.content}
                                                        </p>
                                                    </div>
                                                </div>
                                            </div>
                                        );
                                    } else {
                                        return (
                                            <div key={index} className={(m.sender === userRole) ? 'my-message' : 'other-message'}>
                                                <div className="d-flex justify-content-between">
                                                    <p className="small mb-1 text-muted">23 Jan 2:05 pm</p>
                                                    <p className="small mb-1">{me.learnerNavigation.firstName}</p>
                                                </div>
                                                <div className="d-flex flex-row justify-content-end mb-4 pt-1">
                                                    <div>
                                                        <p className="small p-2 me-3 mb-3 text-black rounded-3" style={{ backgroundColor: "#ff7aac" }}>
                                                            {m.content}
                                                        </p>
                                                    </div>
                                                    <img
                                                        src={(me.learnerPicture) ? me.learnerPicture : "https://villagesonmacarthur.com/wp-content/uploads/2020/12/Blank-Avatar.png"}
                                                        alt="avatar 1"
                                                        style={{ width: "45px", height: "100%" }}
                                                    />
                                                </div>
                                            </div>
                                        );
                                    }
                                })}
                            </MDBCardBody>
                            <MDBCardFooter className="text-muted d-flex justify-content-start align-items-center p-3" style={{ height: '4em' }}>
                                <MDBInputGroup className="mb-0" style={{ height: '3em' }}>
                                    <input
                                        className="form-control"
                                        type="text"
                                        value={message}
                                        onChange={e => setMessage(e.target.value)}
                                        style={{ height: '3em' }}
                                    />
                                    <MDBBtn color='dark' style={{ paddingTop: ".55rem", backgroundColor: "#ff7aac", color: 'black', height: '3em', width: '5em' }} onClick={sendMessage}>
                                        Send
                                    </MDBBtn>
                                </MDBInputGroup>
                            </MDBCardFooter>
                        </MDBCard>
                    </MDBCol>
                </MDBRow>
            </MDBContainer>
        </>
    );
};

export default ChatWindow;
