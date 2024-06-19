// import React from "react";
// import { useEffect } from 'react';
// import { Link, useNavigate } from "react-router-dom";
// import '../style.scss';
// import { useState } from 'react';
// import tutorAPI from "../../../api/tutorAPI";
// import reviewRatingAPI from "../../../api/ReviewRatingAPI";
// import majorAPI from "../../../api/majorAPI";
// import { Card, Button, ListGroup, ListGroupItem, Badge, Row, Col } from 'react-bootstrap';
// import { Globe, PersonFill, ChatSquareDotsFill, StarFill, ArrowClockwise, Clock, Star } from 'react-bootstrap-icons'

// function OrderInfo({ tutorId }) {

//     const [key, setKey] = useState('about');
//     const [tutor, setTutor] = useState({});
//     const [major, setMajor] = useState('');
//     const [firstName, setFirstName] = useState('');
//     const [lastName, setLastName] = useState('');
//     const [price, setPrice] = useState();
//     const [rating, setRating] = useState();
//     const [certi, setCerti] = useState([]);
//     const [reviews, setReviews] = useState([]);

//     useEffect(() => {
//         const fetchTutors = async () => {
//             try {
//                 const tutor = await tutorAPI.get(tutorId);
//                 //const price = await sectionAPI.get(tutorId);
//                 const rating = await reviewRatingAPI.getRating(tutorId);
//                 const reviews = await reviewRatingAPI.getReview(tutorId);
//                 const certi = await tutorAPI.getCerti(tutorId);
//                 setTutor(tutor.data);
//                 //setPrice(price.data);
//                 setRating(rating.data);
//                 setCerti(certi.data.$values);
//                 setReviews(reviews.data.$values);
//                 const majorID = tutor.data.majorId;
//                 const major = await majorAPI.get(majorID);
//                 setMajor(major.data);
//                 setFirstName(tutor.data.tutorNavigation.firstName);
//                 setLastName(tutor.data.tutorNavigation.lastName);

//             } catch (error) {
//                 console.error("Error fetching tutors:", error);
//             }
//         };
//         fetchTutors();


//     }, []);

//     return (
//         <div className="form" style={{ marginTop: '20px' }}>
//             <Card style={{ width: '20rem' }}>
//                 <Card.Header>
//                     <Row>
//                         <Col xs={2} className="d-flex justify-content-center">
//                             <img src="https://via.placeholder.com/50" alt="Tutor" />
//                         </Col>
//                         <Col xs={10}>
//                             <Card.Title>Emiliia</Card.Title>
//                             <Card.Subtitle className="text-muted">
//                                 English
//                             </Card.Subtitle>
//                         </Col>
//                     </Row>
//                 </Card.Header>
//                 <Card.Body>
//                     <Row className="mt-2">
//                         <Col>
//                             <Button variant="outline-secondary">
//                                 50 mins - $90
//                             </Button>
//                         </Col>
//                     </Row>
//                 </Card.Body>
//                 <ListGroup className="list-group-flush">
//                     <ListGroupItem>
//                         Friday, June 21 at 23:00
//                         <br />
//                         Time is based on your location
//                     </ListGroupItem>
//                 </ListGroup>
//                 <Card.Footer>
//                     <ListGroup className="list-group-flush">
//                         <ListGroupItem>
//                             <h5>Your order</h5>
//                         </ListGroupItem>
//                         <ListGroupItem>
//                             25-min lesson <span className="float-end">$45.00</span>
//                         </ListGroupItem>
//                         <ListGroupItem>
//                             Processing fee{' '}
//                             <span className="float-end">
//                                 $0.30
//                                 <i className="bi bi-question-circle-fill"></i>
//                             </span>
//                         </ListGroupItem>
//                         <ListGroupItem>
//                             <strong>Total</strong> <span className="float-end">$45.30</span>
//                         </ListGroupItem>
//                     </ListGroup>
//                     <Button variant="link" className="mb-2">
//                         Have a promo code?
//                     </Button>
//                     <Card.Text className="text-center">
//                         <Badge bg="success" className="p-2">
//                             <i className="bi bi-check-circle-fill"></i>
//                             Free replacement or refund
//                         </Badge>
//                         <br />
//                         Try another tutor for free or get a refund
//                     </Card.Text>
//                 </Card.Footer>
//             </Card>
//         </div>
//     );
// }

// export default OrderInfo;