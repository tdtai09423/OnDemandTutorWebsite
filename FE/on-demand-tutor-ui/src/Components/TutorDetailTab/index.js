import React from 'react';
import { Button, Card, Row, Col, Image, Container } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css'
import './TutorDetailTab.scss'
import { Link } from 'react-router-dom'
import { Globe, PersonFill, ChatSquareDotsFill, StarFill } from 'react-bootstrap-icons'
import Tab from 'react-bootstrap/Tab';
import Tabs from 'react-bootstrap/Tabs';
import { useState } from 'react';
import ScheduleTab from '../../pages/TutorDetail/components/ScheduleTab';
import { useEffect } from 'react';
import tutorAPI from '../../api/tutorAPI';
import majorAPI from '../../api/majorAPI';
import sectionAPI from '../../api/sectionAPI';
import reviewRatingAPI from '../../api/ReviewRatingAPI';


function TutorDetailTab({ tutorId }) {

    const [key, setKey] = useState('about');
    const [tutor, setTutor] = useState({});
    const [major, setMajor] = useState('');
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [price, setPrice] = useState();
    const [rating, setRating] = useState();

    useEffect(() => {
        console.log(tutorId);
        const fetchTutors = async () => {
            try {
                const tutor = await tutorAPI.get(tutorId);
                const price = await sectionAPI.get(tutorId);
                const rating = await reviewRatingAPI.get(tutorId);
                //const certi = await tutorAPI.getCerti(tutorId);
                setTutor(tutor.data);
                setPrice(price.data);
                setRating(rating.data);
                const majorID = tutor.data.majorId;
                const major = await majorAPI.get(majorID);
                setMajor(major.data);
                console.log(tutor.data);
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
            <Row className="profile-footer">
                <Col md={3}>
                    <Image src="path/to/profile.jpg" roundedCircle className="profile-pic-detail" />
                </Col>
            </Row>
            <Row className="profile-footer">
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
                        <Col className="text-right" md={4}>
                            <Row>
                                <Col md={4}>
                                    <div className='d-flex'>
                                        <h2 style={{ marginRight: '15px' }}>{rating}</h2><span><StarFill style={{ fontSize: '40px' }}></StarFill></span>
                                    </div>
                                </Col>
                                <Col md={8}>
                                    <div className="price">
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

                        </Col>
                    </Row>
                </Container>
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
                </Tab>
                <Tab eventKey="schedule" title={<span className="information-tab-text">Schedule</span>} className="information-tab">
                    <ScheduleTab />
                </Tab>
                <Tab eventKey="resume" title={<span className="information-tab-text">Resume</span>} className="information-tab">
                    {tutor.resume}
                </Tab>
            </Tabs>
        </Container>
    );
}

export default TutorDetailTab;