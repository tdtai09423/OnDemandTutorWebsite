import Card from 'react-bootstrap/Card';
import { useEffect, useState } from 'react';
import './notiCom.scss';
import { Megaphone } from 'react-bootstrap-icons'

const NotificationCom = ({ notificationInfo }) => {


    return (
        <>
            {notificationInfo.map((item, index) => {
                return (
                    <Card className='containerCard'>
                        <Card.Body>
                            <Card.Text className='card-text'>
                                <Megaphone />
                                <div>{item.content}</div>
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