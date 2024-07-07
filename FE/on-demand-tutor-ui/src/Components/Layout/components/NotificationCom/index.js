import Card from 'react-bootstrap/Card';
import { useEffect, useState } from 'react';
import './notiCom.scss';
const NotificationCom = ({ notificationInfo }) => {
    const formatDate = (date) => {
        const day = date.getDate();
        const month = date.getMonth() + 1;
        const year = date.getFullYear();
        return `${year}-0${month}-${day}`;
    };
    console.log("NOTI in COM", notificationInfo)

    return (
        <>
            {notificationInfo.map((item, index) => {
                const formattedOrderDate = formatDate(new Date(item.notificateDay));
                const key = index;
                return (
                    <Card className='containerCard'>
                        <Card.Body>
                            <Card.Text className='card-text'>
                                {item.content}
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