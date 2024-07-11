import React from 'react';
import { Button, Card, Row, Col, Image, Container, Dropdown } from 'react-bootstrap';
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
import curriculumAPI from '../../api/curriculumAPI';


function TutorDetailTab({ tutorId, roleUser }) {

    const [key, setKey] = useState('about');
    const [tutor, setTutor] = useState({});
    const [major, setMajor] = useState('');
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [price, setPrice] = useState();
    const [rating, setRating] = useState();
    const [certi, setCerti] = useState([]);
    const [reviews, setReviews] = useState([]);
    const [subject, setSubject] = useState('Choose...');
    const [subjects, setSubjects] = useState([]);
    const [available, setAvailable] = useState('morning');



    const handleSelectSubject = (Subject) => {
        setSubject(Subject); // update the major state when an option is selected
    };
    const handleSelectAvailable = (Available) => {
        setAvailable(Available); // update the major state when an option is selected
    };

    useEffect(() => {
        const fetchTutors = async () => {
            try {
                const tutor = await tutorAPI.get(tutorId);
                const price = await sectionAPI.get(tutorId);
                const rating = await reviewRatingAPI.getRating(tutorId);
                const reviews = await reviewRatingAPI.getReview(tutorId);
                const certi = await tutorAPI.getCerti(tutorId);
                const subject = await curriculumAPI.getCurriculumByTutorId(tutorId);
                setTutor(tutor.data);
                setPrice(price.data);
                setRating(rating.data);
                setCerti(certi.data.$values);
                setReviews(reviews.data.response.items.$values);
                const majorID = tutor.data.majorId;
                const major = await majorAPI.get(majorID);
                setMajor(major.data.major);
                setFirstName(tutor.data.tutorNavigation.firstName);
                setLastName(tutor.data.tutorNavigation.lastName);
                setSubjects(subject.data.$values);
                setSubject(subject.data.$values[0].curriculumDescription)


            } catch (error) {
                console.error("Error fetching tutors:", error);
            }
        };
        fetchTutors();


    }, [tutorId]);


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
                                <span className="amount">${price}</span>
                                <span className="duration">50-min lesson</span>
                            </div>
                        </Col>

                    </Row>
                    <Row>
                        <Container className="button-container">
                            {/* <Button variant="primary" className="book-btn" as={Link} to={"/tutor-detail"}>Book trial lesson</Button> */}
                            {(roleUser === 'TUTOR') ? (<></>) : (
                                <Button variant="outline-secondary" className="book-btn">Send message</Button>
                            )}
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
                    <Row>
                        <Col md={6}>
                            <Dropdown align="end" className="" style={{ width: '80%' }}>
                                <div style={{ paddingLeft: '10px', marginBottom: '5px' }}>What do you want to learn</div>
                                <Dropdown.Toggle variant="outline-secondary" id="dropdown-learn-language" className="w-100 text-left dropdown-toggle-multi-line rounded-2">
                                    <div className="dropdown-text">
                                        <span className="dropdown-tittle">{subject}</span>
                                    </div>
                                </Dropdown.Toggle>
                                <Dropdown.Menu className=" dropdown-menu">
                                    {subjects.map((item) => {
                                        return (
                                            <Dropdown.Item key={item.$id} onClick={() => handleSelectSubject(item.curriculumDescription)}>
                                                {item.curriculumDescription}
                                            </Dropdown.Item>
                                        );
                                    })}
                                </Dropdown.Menu>
                            </Dropdown>
                        </Col>
                        <Col md={6}>
                            <Dropdown align="end" className="" style={{ width: '80%' }}>
                                <div style={{ paddingLeft: '10px', marginBottom: '5px' }}>Available on:</div>
                                <Dropdown.Toggle variant="outline-secondary" id="dropdown-learn-language" className="w-100 text-left dropdown-toggle-multi-line rounded-2">
                                    <div className="dropdown-text">
                                        <span className="dropdown-tittle">{available}</span>
                                    </div>
                                </Dropdown.Toggle>
                                <Dropdown.Menu className=" dropdown-menu">
                                    <Dropdown.Item onClick={() => handleSelectAvailable('morning')}>
                                        Morning
                                    </Dropdown.Item>
                                    <Dropdown.Item onClick={() => handleSelectAvailable('afternoon')}>
                                        Afternoon
                                    </Dropdown.Item>
                                    <Dropdown.Item onClick={() => handleSelectAvailable('night')}>
                                        Night
                                    </Dropdown.Item>

                                </Dropdown.Menu>
                            </Dropdown>
                        </Col>
                    </Row>
                    <ScheduleTab
                        tutorId={tutorId}
                        subject={subject}
                        available={available}
                        roleUser={roleUser}
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
                        {certi.map((item) => (
                            <li key={item.$id}>{item.tutorCertificate}</li>
                        ))}
                    </ul>
                    <hr />
                </Tab>
            </Tabs>
        </Container>
    );
}

export default TutorDetailTab;