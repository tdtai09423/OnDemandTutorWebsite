import React from "react";
import { useEffect } from 'react';
import { Link, useNavigate } from "react-router-dom";
import '../style.scss';
import { useState } from 'react';
import tutorAPI from "../../../api/tutorAPI";
import reviewRatingAPI from "../../../api/ReviewRatingAPI";
import majorAPI from "../../../api/majorAPI";
import { Button, Card, Row, Col, Image, Container } from 'react-bootstrap';
import { Globe, PersonFill, ChatSquareDotsFill, StarFill, ArrowClockwise, Clock, Star } from 'react-bootstrap-icons'

function OrderInfo({ tutorId }) {

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
        <div className="form" style={{ marginTop: '20px' }}>
            <main>
                <Col md={7}>
                    <Row className="profile-footer-detail">
                        <Col md={3}>
                            <Image src={tutor.tutorPicture} className="profile-pic-detail" />
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
            </main>
        </div>
    );
}

export default OrderInfo;