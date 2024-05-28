import React from 'react';
import { Button, Card, Row, Col, Image, Container } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css'
import './TutorDetail.scss'
import { Link } from 'react-router-dom'
import { Globe, PersonFill, ChatSquareDotsFill } from 'react-bootstrap-icons'
import { useState } from 'react';
import Tab from 'react-bootstrap/Tab';
import Tabs from 'react-bootstrap/Tabs'; // Import Bootstrap components

function TutorDetail() {
    const [key, setKey] = useState('about');

    return (

        <Container className="profile-card-td">
            <Row className="profile-footer">
                <Col md={3}>
                    <Image src="path/to/profile.jpg" roundedCircle className="profile-tutor" />
                </Col>
            </Row>
            <Row className="profile-footer">
                <Container>
                    <Row>
                        <Col md={6}>
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

                            </Card.Body>
                        </Col>
                        <Col className="text-right" md={4}>
                            <Row>
                                <div className="price">
                                    <span className="amount">₫509,424</span>
                                    <span className="duration">50-min lesson</span>
                                </div>
                            </Row>
                            <Row>
                                <Container className="button-container">
                                    <Button variant="primary" className="book-btn" as={Link} to={"/tutor-detail"}>Book trial lesson</Button>
                                    <Button variant="outline-secondary" className="message-btn">Send message</Button>
                                    <Button variant="outline-secondary" className="message-btn my-list-btn">Add to my list</Button>
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
                    Tab content for about
                </Tab>
                <Tab eventKey="schedule" title={<span className="information-tab-text">Schedule</span>} className="information-tab">
                    Tab content for schedule
                </Tab>
                <Tab eventKey="resume" title={<span className="information-tab-text">Resume</span>} className="information-tab">
                    Tab content for resume
                </Tab>
            </Tabs>
        </Container>
    );
};

export default TutorDetail;