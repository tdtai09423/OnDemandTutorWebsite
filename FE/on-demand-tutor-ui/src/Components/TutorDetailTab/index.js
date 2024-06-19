import React from 'react';
import { Button, Card, Row, Col, Image, Container } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css'
import './TutorDetailTab.scss'
import { Link } from 'react-router-dom'
import { Globe, PersonFill, ChatSquareDotsFill, StarFill, ArrowClockwise, Clock, Star } from 'react-bootstrap-icons'
import Tab from 'react-bootstrap/Tab';
import Tabs from 'react-bootstrap/Tabs';
import { useState } from 'react';
import ScheduleTab from '../../pages/TutorDetail/components/ScheduleTab';
import { useEffect } from 'react';
import tutorAPI from '../../api/tutorAPI';
import majorAPI from '../../api/majorAPI';
import sectionAPI from '../../api/sectionAPI';
import reviewRatingAPI from '../../api/ReviewRatingAPI';
import LearnerReview from './component/learnerReview/learnerReview';


function TutorDetailTab({ tutorId }) {

    const [key, setKey] = useState('about');
    const [tutor, setTutor] = useState({});
    const [major, setMajor] = useState('');
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [price, setPrice] = useState();
    const [rating, setRating] = useState();
    const [certi, setCerti] = useState([]);
    const [reviews, setReviews] = useState([]);

    useEffect(() => {
        const fetchTutors = async () => {
            try {
                const tutor = await tutorAPI.get(tutorId);
                //const price = await sectionAPI.get(tutorId);
                const rating = await reviewRatingAPI.getRating(tutorId);
                const reviews = await reviewRatingAPI.getReview(tutorId);
                const certi = await tutorAPI.getCerti(tutorId);
                setTutor(tutor.data);
                //setPrice(price.data);
                setRating(rating.data);
                setCerti(certi.data.$values);
                setReviews(reviews.data.$values);
                const majorID = tutor.data.majorId;
                const major = await majorAPI.get(majorID);
                setMajor(major.data);
                setFirstName(tutor.data.tutorNavigation.firstName);
                setLastName(tutor.data.tutorNavigation.lastName);

            } catch (error) {
                console.error("Error fetching tutors:", error);
            }
        };
        fetchTutors();


    }, []);


    return (
        <Container className="profile-card-detail">

            <Row>
                <Col md={7}>
                    <Row className="profile-footer-detail">
                        <Col md={3}>
                            <Image src={tutor.tutorPicture} className="profile-pic-detail" style={{ height: '100px', width: 'auto' }} />
                        </Col>
                    </Row>
                    <Row className="profile-footer-detail">
                        <Container>
                            <Row>
                                <Col md={6}>
                                    <Card.Body>
                                        <Card.Title>
                                            {firstName} {lastName}
                                            <span className="flag" style={{ fontSize: '10px', marginLeft: '20px' }}>{tutor.nationality}</span>
                                        </Card.Title>
                                        <Card.Text>
                                            <p className="language">
                                                <Globe className="icon" /> {major.majorName}
                                            </p>
                                            <p className="students">
                                                <PersonFill className="icon" /> 2 active students · 2 lessons
                                            </p>
                                            <p className="speaks">
                                                <ChatSquareDotsFill className="icon" /> {tutor.tutorEmail}
                                            </p>
                                        </Card.Text>

                                    </Card.Body>
                                </Col>

                            </Row>
                        </Container>
                    </Row>
                </Col>
                <Col className="text-right right-option" md={5}>
                    <Row>
                        <Col md={4}>
                            <div className='d-flex'>
                                <h2 style={{ marginRight: '15px' }}>{rating}</h2><span><StarFill style={{ fontSize: '40px' }}></StarFill></span>
                            </div>
                        </Col>
                        <Col md={8}>
                            <div className="price align-item-center">
                                <span className="amount">₫{price}</span>
                                <span className="duration">50-min lesson</span>
                            </div>
                        </Col>

                    </Row>
                    <Row>
                        <Container className="button-container">
                            <Button variant="primary" className="book-btn" as={Link} to={"/tutor-detail"}>Book trial lesson</Button>
                            <Button variant="outline-secondary" className="message-btn">Send message</Button>
                        </Container>
                    </Row>
                    <Row>
                        <Row className='more-info-tutor'>
                            <div className="more-info-tutor-detail align-item-left float-left">
                                <ArrowClockwise style={{ marginRight: '7px' }}></ArrowClockwise>
                                <span className="duration">3 lessons booked in the last 48 hours</span>
                            </div>
                            <div className="more-info-tutor-detail align-item-left">
                                <Star style={{ marginRight: '7px' }}></Star>
                                <span className="duration">Super popular: 15 students contacted this tutor in the last 48 hours</span>
                            </div>
                            <div className="more-info-tutor-detail align-item-left">
                                <Clock style={{ marginRight: '7px' }}></Clock>
                                <span className="duration">Usually responds in 2 hrs</span>
                            </div>
                        </Row>
                    </Row>
                </Col>
            </Row>
            <Tabs
                id="controlled-tab-example"
                activeKey={key}
                onSelect={(k) => setKey(k)}
                className="mb-3 tab tab-container"
                variant="underline"
            >
                <Tab eventKey="about" title={<span className="information-tab-text">About</span>} className="information-tab">
                    {tutor.tutorDescription}
                    <hr />
                </Tab>
                <Tab eventKey="schedule" title={<span className="information-tab-text">Schedule</span>} className="information-tab">
                    <ScheduleTab
                        tutorId={tutorId}
                    />
                </Tab>
                <Tab eventKey="review" title={<span className="information-tab-text">Review</span>} className="information-tab">
                    {reviews.map((review) => (
                        <div key={review.$id} className='review-block'>
                            <LearnerReview id={review.learnerId} /><p>{review.review}</p>
                        </div>
                    ))}
                </Tab>
                <Tab eventKey="resume" title={<span className="information-tab-text">Resume</span>} className="information-tab">
                    <ul>
                        {certi.map((certi) => (
                            <li key={certi.$id}>{certi.tutorCertificate}</li>
                        ))}
                    </ul>
                </Tab>
            </Tabs>
        </Container>
    );
}

export default TutorDetailTab;