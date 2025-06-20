
import { Button, Card, Row, Col, Image, Container } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css'
import { Link } from 'react-router-dom'
import { Globe, PersonFill, ChatSquareDotsFill, StarFill } from 'react-bootstrap-icons'
import { useState, useEffect } from 'react';
import learnerAPI from '../../api/learnerAPI';
import userAPI from '../../api/userAPI';
import tutorAPI from '../../api/tutorAPI';

function FavoriteTutor() {

    const [tutors, setTutors] = useState([]);

    useEffect(() => {
        const fetchTutors = async () => {
            try {
                let tutorArr = [];
                const email = localStorage.getItem('email');
                const user = await userAPI.getUserByEmail(email);

                const tutorList = await learnerAPI.getFavourite(user.data.id);
                console.log(tutorList.data.$values)
                for (let i = 0; i < tutorList.data.$values.length; i++) {
                    const tutor = await tutorAPI.get(tutorList.data.$values[i].tutorId);
                    console.log('>>>', tutor)
                    tutorArr[i] = tutor.data;
                }
                setTutors(tutorArr);
                console.log(tutorArr);
            } catch (error) {
                console.error("Error fetching tutors:", error);
            }
        };
        fetchTutors();

    }, []);

    return (
        <div style={{ marginTop: '20px' }}>
            <strong>Number of tutor: {tutors.length} Tutor</strong>
            {tutors.map((tutor) => {
                const truncatedDescription = tutor.tutorDescription.length > 150 ? tutor.tutorDescription.substring(0, 150) + '...' : tutor.tutorDescription;
                return (
                    <Card className="profile-card">
                        <Row noGutters>
                            <Col md={3}>
                                <Link className="read-more" as={Link} to={"/tutor-detail?tutorId=" + tutor.tutorId + ""}>
                                    <Image src={tutor.tutorPicture} className="profile-pic" style={{ height: '12em', width: 'auto' }} />
                                </Link>
                            </Col>
                            <Col md={9}>
                                <Card.Body>
                                    <Row>
                                        <Col md={12}>
                                            <Card.Text>
                                                <p className="language">
                                                    <Globe className="icon" /> {tutor.majorId}
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
                                                    {truncatedDescription}
                                                </p>
                                                <Link className="read-more" as={Link} to={"/tutor-detail?tutorId=" + tutor.tutorId + ""}>Read more</Link>
                                            </Card.Text>
                                        </Col>
                                    </Row>
                                </Card.Body>
                            </Col>
                        </Row>
                    </Card>
                )
            })}
        </div>

    );
}

export default FavoriteTutor;
