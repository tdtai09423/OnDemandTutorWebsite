import React from 'react';
import { Card, Row, Col, Button, Form, InputGroup, FormControl, Container, ListGroup, ListGroupItem, Badge, } from 'react-bootstrap';
import { FaUserCircle, FaCheckCircle, FaStar, FaInfoCircle, FaAngleLeft, FaAngleRight } from 'react-icons/fa';
import { useSearchParams } from 'react-router-dom';
import { useState, useEffect } from 'react';
import tutorAPI from '../../api/tutorAPI';
import majorAPI from '../../api/majorAPI';
import sectionAPI from '../../api/sectionAPI';
import reviewRatingAPI from '../../api/ReviewRatingAPI';
import { Image } from 'react-bootstrap';
import './Payment.scss'
import axios from 'axios';
import { useNavigate } from 'react-router-dom'


function Payment() {

    const [searchParam] = useSearchParams();
    const tutorId = searchParam.get('tutorId');
    const curriculumnDescription = searchParam.get('course');
    const startTime = searchParam.get('time');
    console.log(startTime)

    const [tutor, setTutor] = useState({});
    const [price, setPrice] = useState();
    const [rating, setRating] = useState();
    const [reviews, setReviews] = useState([]);
    const navigate = useNavigate();

    const handleCheckout = async () => {
        const data = {
            "tutorId": tutorId,
            "learnerId": 11,
            "curriculumDescription": curriculumnDescription,
            "startTime": startTime,
            "duration": 50
        };

        try {
            const response = await axios.post('https://localhost:7010/api/LearnerOrder/short-term-booking', data, {
                headers: {
                    'Content-Type': 'application/json'
                }
            });
            let orderId = response.order.orderId
            const formData = new FormData();
            formData.append('orderId', orderId);
            const checkout = await axios.post('https://localhost:7010/api/LearnerOrder/checking-out', formData, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                }
            });
            console.log(checkout);
            navigate("/payment-success");
        } catch (error) {
            console.error('Error:', error);
        }

    }

    useEffect(() => {
        const fetchTutors = async () => {
            try {
                const tutor = await tutorAPI.get(tutorId);
                const price = await sectionAPI.get(tutorId);
                const rating = await reviewRatingAPI.getRating(tutorId);
                const reviews = await reviewRatingAPI.getReview(tutorId);
                setTutor(tutor.data);
                setPrice(price.data);
                setRating(rating.data);
                setReviews(reviews.data.response.items.$values);
            } catch (error) {
                console.error("Error fetching tutors:", error);
            }
        };
        fetchTutors();


    }, []);




    return (
        <Container className="mt-5">
            <Row className="justify-content-center">
                <Col md={6}>
                    <Card>
                        <Card.Header>
                            <div className="d-flex justify-content-between align-items-center">
                                <div>
                                    <Image src={tutor.tutorPicture} className="profile-pic-detail" style={{ height: '100px', width: 'auto' }} />
                                </div>
                                <div>
                                    <FaStar className="text-warning" />
                                    <span style={{ marginLeft: '5px' }}>{rating}</span>
                                    <span className="ml-1">({reviews.length} reviews)</span>
                                </div>
                            </div>
                        </Card.Header>
                        <Card.Body>
                            <Row>
                                <Col md={8}>
                                    <Button variant="outline-secondary" block>
                                        50 mins - $90
                                    </Button>
                                </Col>
                            </Row>
                            <ListGroup className="mt-3">
                                <ListGroupItem>
                                    <div className="d-flex justify-content-between">
                                        <span>Friday, June 21 at 23:00</span>
                                        <span className="text-muted">Time is based on your location</span>
                                    </div>
                                </ListGroupItem>
                                <ListGroupItem>
                                    <div className="d-flex justify-content-between align-items-center">
                                        <span>Your order</span>
                                        <Badge variant="light">
                                            <strong>₫</strong>{price}
                                        </Badge>
                                    </div>
                                </ListGroupItem>
                                <ListGroupItem>
                                    <div className="d-flex justify-content-between">
                                        <span>50-min lesson</span>
                                        <span><strong>₫</strong>{price}</span>
                                    </div>
                                </ListGroupItem>
                                <ListGroupItem>
                                    <div className="d-flex justify-content-between">
                                        <span>Total</span>
                                        <span><strong>₫</strong>{price}</span>
                                    </div>
                                </ListGroupItem>
                            </ListGroup>
                            <p className="mt-3"><strong>Have a promo code?</strong></p>
                            <Card className="mt-3">
                                <Card.Body className='under-promote'>
                                    <div className="d-flex align-items-center">
                                        <FaCheckCircle className="text-success mr-2" style={{ color: 'black' }} />
                                        <span style={{ marginLeft: '5px' }}>Free replacement or refund</span>
                                    </div>
                                    <p className="mt-2">Try another tutor for free or get a refund</p>
                                </Card.Body>
                            </Card>
                        </Card.Body>
                    </Card>
                </Col>
                <Col md={6}>
                    <Card>
                        <Card.Header>
                            <h3>Checkout confirmation</h3>
                        </Card.Header>
                        <Card.Body>
                            <Button variant="primary" block className="mt-3" type="submit" onClick={handleCheckout}>
                                Confirm payment - <span><strong>₫</strong>{price}</span>
                            </Button>
                            <p className="mt-3 text-muted">
                                By pressing the "Confirm payment" button, you agree to
                                <a href="#" className="text-muted"> our website's Refund and Payment Policy.</a>
                            </p>
                            <p className="mt-1 text-muted">
                                It's safe to pay on Preply. All transactions are protected by SSL encryption.
                            </p>
                        </Card.Body>
                    </Card>
                </Col>
            </Row>
        </Container>
    );
};

export default Payment;
