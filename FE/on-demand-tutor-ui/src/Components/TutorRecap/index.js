import { Button, Card, Row, Col, Image, Container } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css'
import './TutorRecap.scss'
import { Link } from 'react-router-dom'
import { Globe, PersonFill, ChatSquareDotsFill } from 'react-bootstrap-icons'

function TutorRecap() {
    return (
        <Card className="profile-card">
            <Row noGutters>
                <Col md={3}>
                    <Image src="path/to/profile.jpg" roundedCircle className="profile-pic" />
                </Col>
                <Col md={9}>
                    <Card.Body>
                        <Card.Title>
                            Yao Wowonyo D. <span className="flag">🇹🇬</span>
                        </Card.Title>
                        <Card.Text>
                            <p className="language">
                                <Globe className="icon" /> English
                            </p>
                            <p className="students">
                                <PersonFill className="icon" /> 2 active students · 2 lessons
                            </p>
                            <p className="speaks">
                                <ChatSquareDotsFill className="icon" /> Speaks English (Proficient), French (Proficient) +1
                            </p>
                        </Card.Text>
                        <Card.Text>
                            <p className="tutor-description">
                                <strong>
                                    Improve your English skills with flexibility and expertise
                                </strong> — Hello! I am Yao. I am Togolese, married and an experimental English Language Teacher for all the...
                            </p>
                            <Link className="read-more" as={Link} to={"/tutor-detail"}>Read more</Link>
                        </Card.Text>
                        <Row className="profile-footer">
                            <Col>
                                <div className="price">
                                    <span className="amount">₫509,424</span>
                                    <span className="duration">50-min lesson</span>
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
