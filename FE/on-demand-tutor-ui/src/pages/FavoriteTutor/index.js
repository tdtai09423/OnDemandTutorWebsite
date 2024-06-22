
import { Button, Card, Row, Col, Image, Container } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css'
import { Link } from 'react-router-dom'
import { Globe, PersonFill, ChatSquareDotsFill, StarFill } from 'react-bootstrap-icons'
// import { useState, useEffect } from 'react';
// import sectionAPI from '../../api/sectionAPI';
// import reviewRatingAPI from '../../api/ReviewRatingAPI';
// import majorAPI from '../../api/majorAPI';

function FavoriteTutor() {

    // const firstName = tutor.tutorNavigation.firstName;
    // const lastName = tutor.tutorNavigation.lastName;

    // const [price, setPrice] = useState();
    // const [rating, setRating] = useState();
    // const [major, setMajor] = useState('');

    // useEffect(() => {
    //     const fetchTutors = async () => {
    //         try {
    //             const price = await sectionAPI.get(tutor.tutorId);
    //             const rating = await reviewRatingAPI.getRating(tutor.tutorId);
    //             setPrice(price.data);
    //             setRating(rating.data);
    //             const majorID = tutor.majorId;
    //             const major = await majorAPI.get(majorID);
    //             setMajor(major.data);
    //         } catch (error) {
    //             console.error("Error fetching tutors:", error);
    //         }
    //     };
    //     fetchTutors();
    // }, []);

    // const truncatedDescription = tutor.tutorDescription.length > 150 ? tutor.tutorDescription.substring(0, 150) + '...' : tutor.tutorDescription;

    return (
        <>

            <div className='container'><h3 className="tutor-name">2 Favorite Tutors</h3></div>
            <div className='container'>
                <Card className="profile-card">
                    <Row noGutters>
                        <Col md={3}>
                            <Link className="read-more" as={Link} >
                                <Image src="https://avatars.preply.com/i/logos/i/logos/avatar_31bmb93znbu.jpg" className="profile-pic" />
                            </Link>
                        </Col>
                        <Col md={9}>
                            <Card.Body>
                                <Row>
                                    <Col md={7}>
                                        <Card.Title className="tutor-name" >
                                            <Link className="" as={Link} >
                                                John Doe
                                            </Link>
                                            <span className="flag" style={{ fontSize: '10px', marginLeft: '20px' }}>American</span>
                                        </Card.Title>
                                        <Card.Text>
                                            <p className="language">
                                                <Globe className="icon" />
                                            </p>
                                            <p className="students">
                                                <PersonFill className="icon" /> 2 active students · 2 lessons
                                            </p>
                                            <p className="speaks">
                                                <ChatSquareDotsFill className="icon" /> john.doe@example.com
                                            </p>
                                        </Card.Text>
                                        <Card.Text>
                                            <p className="tutor-description">
                                                Experienced language tutor
                                            </p>
                                            <Link className="read-more" as={Link} >Read more</Link>
                                        </Card.Text>
                                    </Col>


                                    <Col md={5} className="profile-button">
                                        <Row>
                                            <Col md={6}>
                                                <div className='d-flex'>
                                                    <h2 style={{ marginRight: '15px' }}></h2><span><StarFill style={{ fontSize: '40px' }}></StarFill></span>
                                                </div>
                                            </Col>
                                            <Col md={6}>
                                                <div className='d-block'>
                                                    <h3><strong>₫</strong>500000</h3><div>50-min lesson</div>
                                                </div>

                                            </Col>
                                        </Row>
                                        <Row>
                                            <Col className="text-right" md={12}>
                                                <Container className="button-container">
                                                    <Button variant="primary" className="book-btn" as={Link} to={"/tutor-detail"}>Book trial lesson</Button>
                                                    <Button variant="outline-secondary" className="message-btn">Send message</Button>
                                                </Container>
                                            </Col>
                                        </Row>
                                    </Col>
                                </Row>
                            </Card.Body>
                        </Col>
                    </Row>
                </Card>
            </div>
            <div className='container'>
                <Card className="profile-card">
                    <Row noGutters>
                        <Col md={3}>
                            <Link className="read-more" as={Link} >
                                <Image src="https://avatars.preply.com/i/logos/i/logos/avatar_lzrc9w28ogb.jpg" className="profile-pic" />
                            </Link>
                        </Col>
                        <Col md={9}>
                            <Card.Body>
                                <Row>
                                    <Col md={7}>
                                        <Card.Title className="tutor-name" >
                                            <Link className="" as={Link} >
                                                Jane Smith
                                            </Link>
                                            <span className="flag" style={{ fontSize: '10px', marginLeft: '20px' }}>British</span>
                                        </Card.Title>
                                        <Card.Text>
                                            <p className="language">
                                                <Globe className="icon" />
                                            </p>
                                            <p className="students">
                                                <PersonFill className="icon" /> 2 active students · 2 lessons
                                            </p>
                                            <p className="speaks">
                                                <ChatSquareDotsFill className="icon" /> jane.smith@example.com
                                            </p>
                                        </Card.Text>
                                        <Card.Text>
                                            <p className="tutor-description">
                                                Specializes in English language
                                            </p>
                                            <Link className="read-more" as={Link} >Read more</Link>
                                        </Card.Text>
                                    </Col>


                                    <Col md={5} className="profile-button">
                                        <Row>
                                            <Col md={6}>
                                                <div className='d-flex'>
                                                    <h2 style={{ marginRight: '15px' }}></h2><span><StarFill style={{ fontSize: '40px' }}></StarFill></span>
                                                </div>
                                            </Col>
                                            <Col md={6}>
                                                <div className='d-block'>
                                                    <h3><strong>₫</strong>200000</h3><div>50-min lesson</div>
                                                </div>

                                            </Col>
                                        </Row>
                                        <Row>
                                            <Col className="text-right" md={12}>
                                                <Container className="button-container">
                                                    <Button variant="primary" className="book-btn" as={Link} to={"/tutor-detail"}>Book trial lesson</Button>
                                                    <Button variant="outline-secondary" className="message-btn">Send message</Button>
                                                </Container>
                                            </Col>
                                        </Row>
                                    </Col>
                                </Row>
                            </Card.Body>
                        </Col>
                    </Row>
                </Card>
            </div>




        </>

    );
}

export default FavoriteTutor;
