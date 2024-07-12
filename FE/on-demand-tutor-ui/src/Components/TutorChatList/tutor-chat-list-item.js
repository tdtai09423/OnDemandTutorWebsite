import { Card } from "@mui/material";
import learnerAPI from "../../api/learnerAPI";
import tutorAPI from "../../api/tutorAPI";
import '../ChatListItem/chat-list-item.scss'
import { useEffect, useState } from 'react';
import userAPI from "../../api/userAPI";
import chatAPI from "../../api/chatAPI";
import TutorChatWindow from "./tutor-chat-window";


function TutorChatListItem({ chatBoxId, tutorId, learnerId, lastMessageId, sendDate }) {

    console.log(chatBoxId, tutorId, learnerId, lastMessageId, sendDate)

    const [learner, setLearner] = useState();
    const [tutor, setTutor] = useState();
    const [name, setName] = useState('');
    const [avatar, setAvatar] = useState('');
    const [showChatBox, setShowChatBox] = useState(false);

    const formatDate = (date) => {
        const day = String(date.getDate()).padStart(2, '0');
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const year = date.getFullYear();
        const hours = String(date.getUTCHours()).padStart(2, '0');
        const minutes = String(date.getUTCMinutes()).padStart(2, '0');
        const seconds = String(date.getUTCSeconds()).padStart(2, '0');
        return `${year}-${month}-${day} ${hours}:${minutes}`;
    };

    useEffect(() => {
        const fetchUser = async () => {
            try {
                const learner = await learnerAPI.get(learnerId);
                setLearner(learner.data);
                setName(learner.data.learnerNavigation.firstName + ' ' + learner.data.learnerNavigation.lastName)
                const tutor = await tutorAPI.get(tutorId);
                setTutor(tutor.data)
                setAvatar(learner.data.learnerPicture)
                // setName(tutorRes.data.tutorNavigation.firstName + '' + tutorRes.data.tutorNavigation.lastName)
            } catch (error) {
                console.error("Error fetching user:", error);
            }
        };

        fetchUser();
    }, [])

    const handleChatClick = () => {
        setShowChatBox(true)
    }

    const handleCloseChatBox = () => {
        setShowChatBox(false);
    };


    return (
        <>
            <div className="chat-item d-flex align-item-center justify-content-between" onClick={handleChatClick}>
                <div className="d-flex flex-row">
                    <img
                        src={avatar}
                        alt="avatar"
                        className="rounded-circle d-flex align-self-center me-3 shadow-1-strong"
                        width="60"
                    />
                    <div className="pt-1">
                        <p className="fw-bold mb-0">{name}</p>
                        <p className="small text-muted">
                            {lastMessageId}
                        </p>
                    </div>
                </div>
                <div className="pt-1">
                    <p className="small text-muted mb-1">{formatDate(new Date(sendDate))}</p>
                </div>
            </div>
            {showChatBox && (
                <TutorChatWindow
                    chatBoxId={chatBoxId}
                    userRole='TUTOR'
                    onClose={handleCloseChatBox}
                    me={tutor}
                    they={learner}
                />
            )}
        </>
    );
}

export default TutorChatListItem;