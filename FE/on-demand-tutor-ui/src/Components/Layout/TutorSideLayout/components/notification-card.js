import './style-tutor-page.scss'
import { Card, Row, Col } from 'react-bootstrap';
import { Bell } from 'react-bootstrap-icons'

function NotificationCard() {
    return (
        <Card className="side-card">
            <Row className='card-item'>
                <Col md={2}>

                </Col>
                <Col md={2}>
                    <Bell />
                </Col>
                <Col md={8}>
                    <p className='card-text'>Notification</p>
                </Col>
            </Row>
        </Card>
    );
}

export default NotificationCard;