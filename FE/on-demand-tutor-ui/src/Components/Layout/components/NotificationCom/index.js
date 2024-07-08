import Card from 'react-bootstrap/Card';
import Button from 'react-bootstrap/Button';
import { useEffect, useState } from 'react';
import './notiCom.scss';
import { toast } from 'react-toastify';
import NotificationAPI from '../../../../api/notificationAPI';
const NotificationCom = ({ notificationInfo, userId }) => {
    const token = localStorage.getItem("token")
    console.log(userId)
    const formatDate = (date) => {
        const day = date.getDate();
        const month = date.getMonth() + 1;
        const year = date.getFullYear();
        return `${year}-0${month}-${day}`;
    };
    const markAsRead = async () => {
        try {
            console.log("API USERID", userId);
            console.log("API TOKEN", token);

            let res = await NotificationAPI.putMarkAsRead(userId, token);
            console.log(res)
            toast.success(res.message)
        } catch (error) {
            console.log(error)
        }
    }
    console.log("NOTI in COM", notificationInfo)

    return (
        <>
            <Button className='mark-as-read' onClick={markAsRead}>Mark as read</Button>{' '}
            {notificationInfo.map((item, index) => {
                const formattedOrderDate = formatDate(new Date(item.notificateDay));
                const isRead = item.notiStatus === "NEW";
                const key = index;
                return (
                    <Card key={item.notificationId} className='containerCard'>
                        <Card.Body>
                            {isRead ? (
                                <Card.Text className='card-new'>
                                    NEW
                                </Card.Text>)
                                : (<></>)
                            }
                            <Card.Text className='card-text'>
                                <Megaphone />
                                <div>{item.content}</div>
                            </Card.Text>
                            <Card.Text className='card-text'>
                                {formattedOrderDate}
                            </Card.Text>

                        </Card.Body>
                    </Card>
                )
            })}

        </>

    );
}

export default NotificationCom;