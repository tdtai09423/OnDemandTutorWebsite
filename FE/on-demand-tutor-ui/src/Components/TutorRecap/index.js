import { Button, Card, Row, Col, Image, Container } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css'
import './TutorRecap.scss'
import { Link } from 'react-router-dom'
import { Globe, PersonFill, ChatSquareDotsFill, StarFill } from 'react-bootstrap-icons'
import { useState, useEffect } from 'react';
import sectionAPI from '../../api/sectionAPI';
import reviewRatingAPI from '../../api/ReviewRatingAPI';
import majorAPI from '../../api/majorAPI';

function TutorRecap({ tutor }) {

    const firstName = tutor.tutorNavigation.firstName;
    const lastName = tutor.tutorNavigation.lastName;

    const [price, setPrice] = useState();
    const [rating, setRating] = useState();
    const [major, setMajor] = useState('');

    useEffect(() => {
        const fetchTutors = async () => {
            try {
                const price = await sectionAPI.get(tutor.tutorId);
                const rating = await reviewRatingAPI.getRating(tutor.tutorId);
                setPrice(price.data);
                setRating(rating.data);
                const majorID = tutor.majorId;
                const major = await majorAPI.get(majorID);
                setMajor(major.data);
            } catch (error) {
                console.error("Error fetching tutors:", error);
            }
        };
        fetchTutors();
    }, []);

    return (
        <Card className="profile-card">
            <Row noGutters>
                <Col md={3}>
                    <Link className="read-more" as={Link} to={"/tutor-detail?tutorId=" + tutor.tutorId + ""}>
                        <Image src={tutor.tutorPicture} className="profile-pic" />
                    </Link>
                </Col>
                <Col md={9}>
                    <Card.Body>
                        <Row>
                            <Col md={12}>
                                <Card.Title className="tutor-name" >
                                    <Link className="" as={Link} to={"/tutor-detail?tutorId=" + tutor.tutorId + ""}>
                                        {firstName} {lastName}
                                    </Link>
                                    <span className="flag" style={{ fontSize: '10px', marginLeft: '20px' }}>{tutor.nationality}</span>
                                </Card.Title>
                                <Card.Text>
                                    <p className="language">
                                        <Globe className="icon" /> {major.majorName}
                                    </p>
                                    <p className="students">
                                        <PersonFill className="icon" /> {tutor.activeStudents}2 active students · {tutor.lessons}2 lessons
                                    </p>
                                    <p className="speaks">
                                        <ChatSquareDotsFill className="icon" /> {tutor.tutorEmail}
                                    </p>
                                </Card.Text>
                                <Card.Text>
                                    <p className="tutor-description">
                                        {tutor.tutorDescription}
                                    </p>
                                    <Link className="read-more" as={Link} to={"/tutor-detail?tutorId=" + tutor.tutorId + ""}>More information</Link>
                                </Card.Text>
                            </Col>
                        </Row>

                        <Row className="profile-footer">
                            <Col md={3}>
                                <div className='d-flex'>
                                    <h2 style={{ marginRight: '15px' }}>{rating}</h2><span><StarFill style={{ fontSize: '40px' }}></StarFill></span>
                                </div>
                            </Col>
                            <Col md={3}>
                                <div className='d-block'>
                                    <h3><strong>₫</strong>{price}</h3><div>50-min lesson</div>
                                </div>

                            </Col>
                            <Col className="text-right" md={6}>
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
