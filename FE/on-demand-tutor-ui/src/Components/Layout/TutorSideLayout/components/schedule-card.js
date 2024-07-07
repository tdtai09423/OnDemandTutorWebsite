import './style-tutor-page.scss'
import { Card, Row, Col } from 'react-bootstrap';
import { Calendar3 } from 'react-bootstrap-icons'

function Schedule() {
    return (
        <Card className="side-card">
            <Row className='card-item'>
                <Col md={2}>

                </Col>
                <Col md={2}>
                    <Calendar3 />
                </Col>
                <Col md={8}>
                    <p className='card-text'>Your Schedule</p>
                </Col>
            </Row>
        </Card>
    );
}

export default Schedule;