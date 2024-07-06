import './style-tutor-page.scss'
import { Card, Row, Col } from 'react-bootstrap';
import { GraphUpArrow } from 'react-bootstrap-icons'

function TutorRevenueCard() {
    return (
        <Card className="side-card">
            <Row className='card-item'>
                <Col md={2}>

                </Col>
                <Col md={2}>
                    <GraphUpArrow />
                </Col>
                <Col md={8}>
                    <p className='card-text'>Revenue</p>
                </Col>
            </Row>
        </Card>
    );
}

export default TutorRevenueCard;