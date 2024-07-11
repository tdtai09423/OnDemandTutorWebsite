import React, { useEffect, useState } from 'react';
import SignalRService from '../../services/chatService';
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
import chatAPI from '../../api/chatAPI';

const ChatWindow = () => {
    const chatboxId = 1;
    const userId = 11;
    const [messages, setMessages] = useState([]);
    const [newMessage, setNewMessage] = useState('');

    useEffect(() => {
        const fetchChat = async () => {
            try {
                const response = await chatAPI.loadChat(chatboxId);
                setMessages(response.data.$values);
                console.log(response.data.$values)
            } catch (err) {
                console.error('Fetch Chat Error: ', err);
            }
        }
        fetchChat();

        SignalRService.startConnection(chatboxId)
            .then(() => {
                console.log('Connected to SignalR Hub');
                SignalRService.onReceiveMessage((sender, message) => {
                    setMessages(prevMessages => [...prevMessages, { sender, message }]);
                });
            })
            .catch(err => console.error('SignalR Connection Error: ', err));

        return () => {
            SignalRService.stopConnection();
        };
    }, []);

    const handleSendMessage = () => {
        SignalRService.sendMessage(chatboxId, 'LEARNER', newMessage)
            .then(() => setNewMessage(''))
            .catch(err => console.error('Send Message Error: ', err));
    };

    return (
        <>
            <MDBContainer fluid className="py-5" style={{ backgroundColor: "#eee" }}>
                <MDBRow className="d-flex justify-content-center">
                    <MDBCol md="8" lg="6" xl="4">
                        <MDBCard>
                            <MDBCardHeader
                                className="d-flex justify-content-between align-items-center p-3"
                                style={{ borderTop: "4px solid #ffa900" }}
                            >
                                <h5 className="mb-0">Chat messages</h5>
                                <div className="d-flex flex-row align-items-center">
                                    <span className="badge bg-warning me-3">10</span>
                                    <MDBIcon
                                        fas
                                        icon="minus"
                                        size="xs"
                                        className="me-3 text-muted"
                                    />
                                    <MDBIcon
                                        fas
                                        icon="comments"
                                        size="xs"
                                        className="me-3 text-muted"
                                    />
                                    <MDBIcon
                                        fas
                                        icon="times"
                                        size="xs"
                                        className="me-3 text-muted"
                                    />
                                </div>
                            </MDBCardHeader>
                            <div>
                                <MDBCardBody>
                                    <div>
                                        {messages.map((items) => {
                                            console.log('>>>', items)
                                            return (
                                                <div
                                                    key={items.id}
                                                    className='my-message'
                                                >
                                                    {items.content}
                                                </div>
                                            )
                                        })}
                                    </div>
                                </MDBCardBody>
                            </div>
                            <MDBCardFooter className="text-muted d-flex justify-content-start align-items-center p-3">
                                <MDBInputGroup className="mb-0">
                                    <input
                                        className="form-control"
                                        type="text"
                                        value={newMessage}
                                        onChange={e => setNewMessage(e.target.value)}
                                        placeholder="Type a message..."
                                    />
                                    <MDBBtn color="warning" style={{ paddingTop: ".55rem" }} onClick={handleSendMessage}>
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
