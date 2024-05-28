import { Button, Card, Row, Col, Image, Container } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css'
import './TutorRecap.scss'
import { Link } from 'react-router-dom'
import { Globe, PersonFill, ChatSquareDotsFill } from 'react-bootstrap-icons'

function TutorRecap({ tutor }) {

    return (
        <Card className="profile-card">
            <Row noGutters>
                <Col md={3}>
                    <Image src={tutor.image} className="profile-pic" />
                </Col>
                <Col md={9}>
                    <Card.Body>
                        <Card.Title className="tutor-name" >
                            {tutor.title} <span className="flag">{tutor.nationality}</span>
                        </Card.Title>
                        <Card.Text>
                            <p className="language">
                                <Globe className="icon" /> {tutor.majorId}
                            </p>
                            <p className="students">
                                <PersonFill className="icon" /> {tutor.activeStudents} active students · {tutor.lessons} lessons
                            </p>
                            <p className="speaks">
                                <ChatSquareDotsFill className="icon" /> {tutor.speaks}
                            </p>
                        </Card.Text>
                        <Card.Text>
                            <p className="tutor-description">
                                {tutor.tutorDescription}
                            </p>
                            <Link className="read-more" as={Link} to={"/tutor-detail"}>Read more</Link>
                        </Card.Text>
                        <Row className="profile-footer">
                            <Col>
                                <div className="price">
                                    <span className="amount">{tutor.price}</span>
                                    <span className="duration">{tutor.duration}</span>
                                </div>
                            </Col>
                            <Col className="text-right">
                                <Container className="button-container">
                                    <Button variant="primary" className="book-btn" as={Link} to={"/tutor-detail?tutorId=" + tutor.tutorId}>Book trial lesson</Button>
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
