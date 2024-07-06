import './style-tutor-page.scss'
import { Card, Row, Col } from 'react-bootstrap';
import { Clipboard2Check } from 'react-bootstrap-icons'

function OrderListCard() {
    return (
        <Card className="side-card">
            <Row className='card-item'>
                <Col md={2}>

                </Col>
                <Col md={2}>
                    <Clipboard2Check />
                </Col>
                <Col md={8}>
                    <p className='card-text'>Order List</p>
                </Col>
            </Row>
        </Card>
    );
}

export default OrderListCard;