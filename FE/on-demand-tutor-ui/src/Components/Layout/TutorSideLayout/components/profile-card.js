import './style-tutor-page.scss'
import { Card, Row, Col } from 'react-bootstrap';
import { PersonLinesFill } from 'react-bootstrap-icons'

function ProfileCard() {
    return (
        <Card className="side-card">
            <Row className='card-item'>
                <Col md={2}>

                </Col>
                <Col md={2}>
                    <PersonLinesFill />
                </Col>
                <Col md={8}>
                    <p className='card-text'>Your Profile</p>
                </Col>
            </Row>
        </Card>
    );
}

export default ProfileCard;