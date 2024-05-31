import { Button, Card, Row, Col, Image, Container } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css'
import './TutorRecap.scss'
import { Link } from 'react-router-dom'
import { Globe, PersonFill, ChatSquareDotsFill } from 'react-bootstrap-icons'

function TutorRecap({ tutorInfo }) {
    return (
        <Card className="profile-card">
            <Row noGutters style={{ margin: '10px' }}>
                <Col md={3}>
                    <Image src={tutorInfo.image} className="profile-pic" />
                </Col>
                <Col md={9}>
                    <Card.Body>
                        <Card.Title>
                            {tutorInfo.name}
                        </Card.Title>
                        <Card.Text>
                            <p className="language">
                                <Globe className="icon" />{tutorInfo.language}
                            </p>
                            <p className="students">
                                <PersonFill className="icon" /> {tutorInfo.activeStudents} active students · {tutorInfo.lessons} lessons
                            </p>
                            <p className="speaks">
                                <ChatSquareDotsFill className="icon" /> {tutorInfo.speaks}
                            </p>
                        </Card.Text>
                        <Card.Text>
                            <p className="tutor-description">
                                {tutorInfo.description}
                            </p>
                            <Link className="read-more" as={Link} to={"/tutor-detail"}>Read more</Link>
                        </Card.Text>
                        <Row className="profile-footer">
                            <Col>
                                <div className="price">
                                    <span className="amount">{tutorInfo.price}</span>
                                    <span className="duration">{tutorInfo.duration}</span>
                                </div>
                            </Col>
                            <Col className="text-right">
                                <Container className="button-container">
                                    <Button variant="primary" className="book-btn" as={Link} to={"/tutor-detail"}>Book trial lesson</Button>
                                    <Button variant="outline-secondary" className="message-btn">Send message</Button>
                                </Container>
                            </Col>
                        </Row>
                    </Card.Body>
                </Col>
            </Row>
        </Card>

    );
}

export default TutorRecap;
