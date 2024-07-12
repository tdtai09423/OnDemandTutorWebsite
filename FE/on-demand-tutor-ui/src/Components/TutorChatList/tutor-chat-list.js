import { Card } from "@mui/material";
import learnerAPI from "../../api/learnerAPI";
import tutorAPI from "../../api/tutorAPI";
import '../ChatListItem/chat-list-item.scss'
import { useEffect, useState } from 'react';
import userAPI from "../../api/userAPI";
import chatAPI from "../../api/chatAPI";
import TutorChatListItem from "./tutor-chat-list-item";


function TutorChatList() {


    const [learners, setLearners] = useState([]);

    useEffect(() => {
        const fetchUser = async () => {
            try {
                const email = localStorage.getItem('email');
                const user = await userAPI.getUserByEmail(email);
                const learnersRes = await chatAPI.loadChatBoxByOneId(user.data.id);
                console.log(learnersRes.data.$values)
                setLearners(learnersRes.data.$values)
                // console.log(tutorRes.data)
                // setAvatar(tutorRes.data.tutorPicture)
                // setName(tutorRes.data.tutorNavigation.firstName + '' + tutorRes.data.tutorNavigation.lastName)
            } catch (error) {
                console.error("Error fetching user:", error);
            }
        };

        fetchUser();
    }, [])

    return (
        <>
            {learners.map((item) => {
                return (
                    <TutorChatListItem
                        chatBoxId={item.id}
                        tutorId={item.tutorId}
                        learnerId={item.learnerId}
                        lastMessageId={item.lastMessageId}
                        sendDate={item.sendDate}
                    />
                )
            })}
        </>
    );
}

export default TutorChatList;