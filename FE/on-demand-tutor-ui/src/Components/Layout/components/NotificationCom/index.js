import Card from 'react-bootstrap/Card';
import { useEffect, useState } from 'react';
import './notiCom.scss';
const NotificationCom = ({ notificationInfo }) => {


    return (
        <>
            {notificationInfo.map((item, index) => {
                return (
                    <Card className='containerCard'>
                        <Card.Body>
                            <Card.Text className='card-text'>
                                {item.content}
                            </Card.Text>
                            <Card.Subtitle>{item.notificationDay}</Card.Subtitle>
                        </Card.Body>
                    </Card>
                )
            })}

        </>

    );
}

export default NotificationCom;