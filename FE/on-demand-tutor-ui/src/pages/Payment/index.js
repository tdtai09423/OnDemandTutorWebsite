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


function Payment() {

    const [searchParam] = useSearchParams();
    const tutorId = searchParam.get('tutorId');

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
                const price = await sectionAPI.get(tutorId);
                const rating = await reviewRatingAPI.getRating(tutorId);
                const reviews = await reviewRatingAPI.getReview(tutorId);
                const certi = await tutorAPI.getCerti(tutorId);
                setTutor(tutor.data);
                setPrice(price.data);
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
                            <h3>Pay with</h3>
                        </Card.Header>
                        <Card.Body>
                            <Form>
                                <InputGroup className="mb-3">
                                    <FormControl placeholder="1234 1234 1234 1234" />
                                </InputGroup>
                                <Row>
                                    <Col md={4}>
                                        <Form.Control type="text" placeholder="MM" />
                                    </Col>
                                    <Col md={4}>
                                        <Form.Control type="text" placeholder="YY" />
                                    </Col>
                                    <Col md={4}>
                                        <Form.Control type="text" placeholder="CVC" />
                                    </Col>
                                </Row>
                                <Form.Check
                                    type="checkbox"
                                    label="Save this card for future payments"
                                    className="mt-3"
                                />
                                <Button variant="primary" block className="mt-3" type="submit">
                                    Confirm payment - $45.30
                                </Button>
                                <p className="mt-3 text-muted">
                                    By pressing the "Confirm payment" button, you agree to
                                    <a href="#" className="text-muted"> our website's Refund and Payment Policy.</a>
                                </p>
                                <p className="mt-1 text-muted">
                                    It's safe to pay on Preply. All transactions are protected by SSL encryption.
                                </p>
                            </Form>
                        </Card.Body>
                    </Card>
                </Col>
            </Row>
        </Container>
    );
};

export default Payment;
