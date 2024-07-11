import './style-tutor-page.scss'
import { Card, Row, Col } from 'react-bootstrap';
import { FilePerson } from 'react-bootstrap-icons'

function TutorProfilePageCard() {
    return (
        <Card className="side-card">
            <Row className='card-item'>
                <Col md={2}>

                </Col>
                <Col md={2}>
                    <FilePerson />
                </Col>
                <Col md={8}>
                    <p className='card-text'>See your page</p>
                </Col>
            </Row>
        </Card>
    );
}

export default TutorProfilePageCard;