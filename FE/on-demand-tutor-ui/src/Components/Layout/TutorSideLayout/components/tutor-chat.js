import './style-tutor-page.scss'
import { Card, Row, Col } from 'react-bootstrap';
import { ChatDots } from 'react-bootstrap-icons'

function TutorChatListCard() {
    return (
        <Card className="side-card">
            <Row className='card-item'>
                <Col md={2}>

                </Col>
                <Col md={2}>
                    <ChatDots />
                </Col>
                <Col md={8}>
                    <p className='card-text'>Chat</p>
                </Col>
            </Row>
        </Card>
    );
}

export default TutorChatListCard;