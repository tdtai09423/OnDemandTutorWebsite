import { Card } from "@mui/material";
import learnerAPI from "../../api/learnerAPI";
import tutorAPI from "../../api/tutorAPI";
import './chat-list-item.scss'
import { useEffect, useState } from 'react';




function ChatListItem({ tutorId, learnerId, lastMessageId, sendDate }) {

    const [tutor, setTutor] = useState();
    const [lastMessage, setLastMessage] = useState();
    const [showChatBox, setShowChatBox] = useState(false);
    const [avatar, setAvatar] = useState();
    const [name, setName] = useState('');

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
                const tutorRes = await tutorAPI.get(tutorId);
                setTutor(tutorRes.data)
                console.log(tutorRes.data)
                setAvatar(tutorRes.data.tutorPicture)
                setName(tutorRes.data.tutorNavigation.firstName + '' + tutorRes.data.tutorNavigation.lastName)
            } catch (error) {
                console.error("Error fetching user:", error);
            }
        };

        fetchUser();
    }, [tutorId])

    const handleChatClick = () => {
        setShowChatBox(true)
    }

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
                            {/* {lastMessageId} */}
                        </p>
                    </div>
                </div>
                <div className="pt-1">
                    <p className="small text-muted mb-1">{formatDate(new Date(sendDate))}</p>
                </div>
            </div>
        </>
    );
}

export default ChatListItem;