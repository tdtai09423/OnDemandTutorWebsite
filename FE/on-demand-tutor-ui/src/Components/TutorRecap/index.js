import { Button, Card, Row, Col, Image, Container } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css'
import './TutorRecap.scss'
import { Link } from 'react-router-dom'
import { Globe, PersonFill, ChatSquareDotsFill, StarFill, SuitHeart, SuitHeartFill } from 'react-bootstrap-icons'
import { useState, useEffect } from 'react';
import sectionAPI from '../../api/sectionAPI';
import reviewRatingAPI from '../../api/ReviewRatingAPI';
import majorAPI from '../../api/majorAPI';
import userAPI from '../../api/userAPI';
import learnerAPI from '../../api/learnerAPI';
import ChatWindow from '../ChatWindow/chat-window.js'
import chatAPI from '../../api/chatAPI.js';


function TutorRecap({ tutor, tutorFavouriteList }) {

    const firstName = tutor.tutorNavigation.firstName;
    const lastName = tutor.tutorNavigation.lastName;
    const Jtoken = localStorage.getItem('token');

    const [price, setPrice] = useState();
    const [rating, setRating] = useState();
    const [major, setMajor] = useState('');
    const [showChatBox, setShowChatBox] = useState(false);
    //const [tutorFavourites, setTutorFavourites] = useState([]);
    //const [isFavourite, setIsFavourite] = useState();
    const [show, setShow] = useState();


    useEffect(() => {
        const tmp = tutorFavouriteList.map(item => item.tutorId);
        //setIsFavourite(tmp.includes(tutor.tutorId));
        tmp.includes(tutor.tutorId) ? setShow(<SuitHeartFill style={{ fontSize: '1.5em' }} onClick={handleHeartFillIconClick} />) : setShow(<SuitHeart style={{ fontSize: '1.5em' }} onClick={handleHeartIconClick} />);

        const fetchTutors = async () => {
            try {
                const price = await sectionAPI.get(tutor.tutorId);
                const rating = await reviewRatingAPI.getRating(tutor.tutorId);
                setPrice(price.data);
                setRating(rating.data);
                const majorID = tutor.majorId;
                const major = await majorAPI.get(majorID);
                setMajor(major.data.major.majorName);
            } catch (error) {
                console.error("Error fetching tutors:", error);
            }
        };
        fetchTutors();
    }, [tutorFavouriteList]);

    const handleHeartIconClick = async () => {
        setShow(<SuitHeartFill style={{ fontSize: '1.5em' }} onClick={handleHeartFillIconClick} />)
        //setIsFavourite(true);
        const token = localStorage.getItem('token');
        const email = localStorage.getItem('email');
        const user = await userAPI.getUserByEmail(email);
        console.log(user.data.id)
        const addFavour = await learnerAPI.addFavourite(tutor.tutorId, user.data.id, token)
    }

    const handleHeartFillIconClick = async () => {
        setShow(<SuitHeart style={{ fontSize: '1.5em' }} onClick={handleHeartIconClick} />);
        //setIsFavourite(false);
        const email = localStorage.getItem('email');
        const user = await userAPI.getUserByEmail(email);
        console.log(user.data.id)
        const removeFavour = await learnerAPI.removeFavourite(tutor.tutorId, user.data.id)
    }

    const [chatboxId, setChatboxId] = useState();
    const [userRole, setUserRole] = useState();
    const [learnerChat, setLearnerChat] = useState();

    const handleSendMess = async () => {
        try {
            const email = localStorage.getItem('email');
            const user = await userAPI.getUserByEmail(email);
            let res = await chatAPI.createChatBox(user.data.id, tutor.tutorId, Jtoken);
            let learnerRes = await learnerAPI.get(user.data.id);
            console.log(res.data.box.id)
            setChatboxId(res.data.box.id)
            setUserRole(user.data.roleId)
            setLearnerChat(learnerRes.data)
        } catch (e) {
            console.log(e)

        }
        setShowChatBox(true);
    }

    const handleCloseChatBox = () => {
        setShowChatBox(false);
    };

    // const renderHeartIcon = () => {
    //     if (isFavourite) {
    //         return <SuitHeartFill style={{ fontSize: '1.5em' }} onClick={handleHeartFillIconClick} />
    //     } else {
    //         return <SuitHeart style={{ fontSize: '1.5em' }} onClick={handleHeartIconClick} />
    //     }
    // }

    const truncatedDescription = tutor.tutorDescription.length > 150 ? tutor.tutorDescription.substring(0, 150) + '...' : tutor.tutorDescription;

    return (
        <Card className="profile-card">
            <Row noGutters>
                <Col md={3}>
                    <Link className="read-more" as={Link} to={"/tutor-detail?tutorId=" + tutor.tutorId + ""}>
                        <Image src={tutor.tutorPicture} className="profile-pic" />
                    </Link>
                </Col>
                <Col md={9}>
                    <Card.Body>
                        <Row>
                            <Col md={7}>
                                <Row>
                                    <Col md={8}>
                                        <Card.Title className="tutor-name" >
                                            <Link className="" as={Link} style={{ textDecoration: 'none', color: 'black' }} to={"/tutor-detail?tutorId=" + tutor.tutorId + ""}>
                                                {firstName} {lastName}
                                            </Link>
                                            <span className="flag" style={{ fontSize: '10px', marginLeft: '20px' }}>{tutor.nationality}</span>
                                        </Card.Title>
                                    </Col>
                                    <Col md={2}>
                                    </Col>
                                    <Col md={2}
                                        style={{
                                            textAlign: 'center',
                                            padding: '1em 0',
                                        }}
                                    >
                                        {
                                            Jtoken ? (
                                                <>
                                                    {show}
                                                </>
                                            ) : (
                                                <></>
                                            )
                                        }

                                    </Col>
                                </Row>
                                <Card.Text>
                                    <p className="language">
                                        <Globe className="icon" /> {major}
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


                            <Col md={5} className="profile-button">
                                <Row>
                                    <Col md={6}>
                                        <div className='d-flex'>
                                            <h2 style={{ marginRight: '15px' }}>{rating}</h2><span><StarFill style={{ fontSize: '40px' }}></StarFill></span>
                                        </div>
                                    </Col>
                                    <Col md={6}>
                                        <div className='d-block'>
                                            <h3><strong>$</strong>{price}</h3><div>50-min lesson</div>
                                        </div>

                                    </Col>
                                </Row>
                                <Row>
                                    <Col className="text-right" md={12}>
                                        <Container className="button-container">
                                            <Button variant="primary" className="book-btn" as={Link} to={"/tutor-detail?tutorId=" + tutor.tutorId + ""}>Book trial lesson</Button>
                                            <Button variant="outline-secondary" className="message-btn" onClick={handleSendMess}>Send message</Button>
                                        </Container>
                                    </Col>
                                </Row>
                            </Col>
                        </Row>
                    </Card.Body>
                </Col>
            </Row>
            {showChatBox && (
                <ChatWindow
                    chatBoxId={chatboxId}
                    userRole={userRole}
                    onClose={handleCloseChatBox}
                    me={learnerChat}
                    they={tutor}
                />
            )}
        </Card>
    );
}

export default TutorRecap;
