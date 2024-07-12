import { NavDropdown, Navbar, Nav, Button, Container, Dropdown } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css'
import './Header.scss'
import { Link, useNavigate } from 'react-router-dom'
import { BoxArrowInRight, PersonCircle, WalletFill, PlusCircle, StarFill, Gem, ChatDots } from 'react-bootstrap-icons'
import images from '../../../../assets/images';
import logoutAPI from '../../../../api/logoutAPI';
import { useEffect, useState } from 'react';
import userAPI from '../../../../api/userAPI';
import learnerAPI from '../../../../api/learnerAPI';
import tutorAPI from '../../../../api/tutorAPI';
import ChatListItem from '../../../ChatListItem/chat-list-item.';

import NotificationAPI from '../../../../api/notificationAPI';
import Offcanvas from 'react-bootstrap/Offcanvas';
import NotificationCom from '../NotificationCom';
import Modal from 'react-bootstrap/Modal';
import walletAPI from '../../../../api/walletAPI';
import chatAPI from '../../../../api/chatAPI';
import { Card } from "@mui/material";
import ChatWindow from '../../../ChatWindow/chat-window';

function Header() {
    const [userRole, setUserRole] = useState();
    const [userId, setUserId] = useState();
    const [notifications, setNotifications] = useState([]);
    const [chats, setChats] = useState([]);
    const [showChatBox, setShowChatBox] = useState(false);

    const [chatboxId, setChatboxId] = useState();



    const [membership, setMembership] = useState('None')
    const newNotifications = notifications.filter((notification) => notification.notiStatus === "NEW");
    const newNotificationsCount = newNotifications.length;
    //Offcanvas
    const [show, setShow] = useState(false);
    const handleClose = () => setShow(false);
    const handleShow = () => {
        setShow(true);
        HandleNotification();
    };

    const [showTopUp, setShowTopUp] = useState(false);
    const handleTopUpClose = () => setShowTopUp(false);
    const handleTopUpShow = () => setShowTopUp(true);

    const [balance, setBalance] = useState();

    const Jtoken = localStorage.getItem('token');
    const email = localStorage.getItem('email');
    const navigate = useNavigate();

    const HandleLogOut = async () => {
        localStorage.removeItem('token');
        localStorage.removeItem('email');
        let res = await logoutAPI();
        console.log(res);
        navigate("/");
    }
    const HandleNotification = async () => {
        try {
            let res = await NotificationAPI.getNotification(userId);
            setNotifications(res);
            console.log("NOTIFICATION", res.data.notificationList.$values)
            setNotifications(res.data.notificationList.$values)
        } catch (error) {
            console.log(error);
        }

    }

    useEffect(() => {
        const fetchUser = async () => {
            try {
                const user = await userAPI.getUserByEmail(email);
                setUserRole(user.data.roleId)
                if (user.data.roleId === 'LEARNER') {
                    const learner = await learnerAPI.get(user.data.id);
                    setMembership(learner.data.membershipId)
                }
                const balance = await userAPI.getBalance(user.data.id);
                setUserId(user.data.id)
                setBalance(balance.data.wallet.balance)
                const chatBoxsRes = await chatAPI.loadChatBoxByOneId(user.data.id);
                console.log(chatBoxsRes.data.$values);
                setChats(chatBoxsRes.data.$values);

            } catch (error) {
                console.error("Error fetching user:", error);
            }
        };

        fetchUser();
    }, [])

    const [amount, setAmount] = useState('');

    const handleClickWallet = async () => {
        try {
            let user = await userAPI.getUserByEmail(email);
            let res = await walletAPI.topUp(amount, user.data.id);
            console.log('>>>', res.data.url);
            window.location.href = res.data.url;
        } catch (e) {
            console.log(e);
        }
    }

    const [learnerChat, setLearnerChat] = useState();
    const [tutorChat, setTutorChat] = useState();

    const handleChatClick = async (chat) => {
        try {
            let chatboxRes = await chatAPI.loadChatBoxByTwoId(chat.learnerId, chat.tutorId);
            let tutorChatRes = await learnerAPI.get(chat.learnerId);
            let learnerChatRes = await tutorAPI.get(chat.tutorId);
            setTutorChat(learnerChatRes.data)
            setLearnerChat(tutorChatRes.data)
            setChatboxId(chatboxRes.data.$values[0].id);
        } catch (e) {
            console.log(e);
        }
        setShowChatBox(true);
    }

    const handleCloseChatBox = () => {
        setShowChatBox(false);
    };

    return (
        <Navbar expand="lg" className="bg-body-tertiary">
            <Container fluid>
                <Navbar.Brand as={Link} to={"/"}>
                    <img src={images.logo} alt="OnDemandTutorLogo" />
                </Navbar.Brand>
                <Navbar.Toggle aria-controls="navbarScroll" />
                <Navbar.Collapse id="navbarScroll">
                    <Nav
                        className="me-auto my-2 my-lg-0"
                        style={{ maxHeight: '100px' }}
                        navbarScroll
                    >

                        <Nav.Link as={Link} to={"/"}><span className="navBarContent">Find tutor</span></Nav.Link>
                        <Nav.Link as={Link} to={"/sign-up-tutor"}><span className="navBarContent">Become a tutor</span></Nav.Link>
                        <NavDropdown title={<span className="navBarContent">Contact us</span>} id="navbarScrollingDropdown">
                            <NavDropdown.Item><Link as={Link} to={"/send-report"} style={{ textDecoration: 'none', color: 'black' }}>Send report</Link></NavDropdown.Item>
                            <NavDropdown.Item href="#action4">
                                Membership
                            </NavDropdown.Item>
                            <NavDropdown.Divider />
                            <NavDropdown.Item>
                                <Link as={Link} to={"/policy"} style={{ textDecoration: 'none', color: 'black' }}>Policy</Link>
                            </NavDropdown.Item>
                        </NavDropdown>
                        {
                            Jtoken ? (
                                <>
                                    <NavDropdown
                                        title={<ChatDots style={{ fontSize: '25px' }} />}
                                        style={{ padding: '0', display: 'flex', marginLeft: '15px', width: '25em' }} // Adjust the width as needed
                                    >
                                        {chats.map((chat, index) => {
                                            return (
                                                <NavDropdown.Item key={index} style={{ padding: '0' }}>
                                                    <li className="p-2 border-bottom" style={{ width: '100%' }} onClick={() => handleChatClick(chat)}>
                                                        <ChatListItem
                                                            tutorId={chat.tutorId}
                                                            learnerId={chat.tutorId}
                                                            lastMessageId={chat.lastMessageId}
                                                            sendDate={chat.sendDate}
                                                        />
                                                    </li>
                                                </NavDropdown.Item>
                                            );
                                        })}
                                    </NavDropdown>
                                    {showChatBox && (
                                        <ChatWindow
                                            chatBoxId={chatboxId}
                                            userRole={userRole}
                                            onClose={handleCloseChatBox}
                                            me={learnerChat}
                                            they={tutorChat}
                                        />
                                    )}
                                </>
                            ) : (
                                <></>
                            )
                        }


                    </Nav>
                    <Modal
                        style={{ marginTop: '90px' }}
                        show={showTopUp}
                        onHide={handleTopUpClose}
                        backdrop="static"
                        keyboard={false}
                    >
                        <Modal.Header closeButton>
                            <Modal.Title>Topup</Modal.Title>
                        </Modal.Header>
                        <Modal.Body>
                            <div className='body-add-new'>
                                <div className='text'>Amount</div>
                                <input type='text' placeholder='$...' className='form-control' value={amount} onChange={(event) => setAmount(event.target.value)} />
                            </div>
                        </Modal.Body>
                        <Modal.Footer>
                            <Button variant="primary" onClick={handleClickWallet}>Confirm</Button>
                        </Modal.Footer>
                    </Modal>
                    {
                        Jtoken ? (
                            <>
                                <div className='balance-box'>
                                    <WalletFill className='balance-icon'></WalletFill> : ${balance}
                                    <PlusCircle className='top-up-icon' onClick={handleTopUpShow}>
                                    </PlusCircle>
                                </div>
                                <div>

                                    {
                                        newNotificationsCount > 0 ?
                                            (
                                                <Button variant="btn btn-outline-danger" onClick={handleShow}>
                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-bell-fill" viewBox="0 0 16 16">
                                                        <path d="M8 16a2 2 0 0 0 2-2H6a2 2 0 0 0 2 2m.995-14.901a1 1 0 1 0-1.99 0A5 5 0 0 0 3 6c0 1.098-.5 6-2 7h14c-1.5-1-2-5.902-2-7 0-2.42-1.72-4.44-4.005-4.901" />
                                                    </svg>
                                                </Button>
                                            )
                                            : (
                                                <Button variant="" onClick={handleShow}>
                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-bell" viewBox="0 0 16 16">
                                                        <path d="M8 16a2 2 0 0 0 2-2H6a2 2 0 0 0 2 2M8 1.918l-.797.161A4 4 0 0 0 4 6c0 .628-.134 2.197-.459 3.742-.16.767-.376 1.566-.663 2.258h10.244c-.287-.692-.502-1.49-.663-2.258C12.134 8.197 12 6.628 12 6a4 4 0 0 0-3.203-3.92zM14.22 12c.223.447.481.801.78 1H1c.299-.199.557-.553.78-1C2.68 10.2 3 6.88 3 6c0-2.42 1.72-4.44 4.005-4.901a1 1 0 1 1 1.99 0A5 5 0 0 1 13 6c0 .88.32 4.2 1.22 6" />
                                                    </svg>
                                                </Button>

                                            )
                                    }

                                    <Offcanvas show={show} onHide={handleClose}>
                                        <Offcanvas.Header closeButton>
                                            <Offcanvas.Title
                                                style={{
                                                    fontSize: '30px',
                                                    fontWeight: 700,
                                                    fontFamily: "'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif",
                                                }}

                                            >Notification</Offcanvas.Title>
                                        </Offcanvas.Header>
                                        <Offcanvas.Body>
                                            {notifications && notifications.length > 0 ? (
                                                <NotificationCom notificationInfo={notifications} userId={userId} />
                                            ) : (
                                                <div>
                                                    you have no notification
                                                </div>
                                            )}
                                        </Offcanvas.Body>
                                    </Offcanvas>

                                </div>
                                <Dropdown align="" className='log-out-button'>
                                    <Dropdown.Toggle variant="" id="dropdown-sort-by" className="rounded-2" drop="start">
                                        <PersonCircle style={{ fontSize: '2em' }}></PersonCircle>
                                    </Dropdown.Toggle>
                                    <Dropdown.Menu className="dropdown-menu" align="end">
                                        <Dropdown.Item>
                                            <div style={{ display: 'flex', textDecoration: 'none', color: 'black', fontSize: '0.7em' }}>MemberShip: {(membership === 'M001') ? (<><p style={{ marginLeft: '10px' }}> Silver</p><StarFill /></>) : (membership === 'M002') ? (<><p style={{ marginLeft: '10px' }}> Premium</p><Gem /></>) : (<><p style={{ marginLeft: '10px' }}> None</p></>)}

                                            </div>
                                        </Dropdown.Item>
                                        <Dropdown.Item><Link as={Link} to={"/user-profile"} style={{ textDecoration: 'none', color: 'black' }}>User profile</Link></Dropdown.Item>
                                        <Dropdown.Item>
                                            <Link as={Link} to={"/order-history"} style={{ textDecoration: 'none', color: 'black' }}>View history</Link>
                                        </Dropdown.Item>
                                        <Dropdown.Item>
                                            <Link as={Link} to={"/favorite-tutor"} style={{ textDecoration: 'none', color: 'black' }}>Favorite tutor</Link>
                                        </Dropdown.Item>
                                        <Dropdown.Item>
                                            <Link as={Link} to={"/personal-schedule"} style={{ textDecoration: 'none', color: 'black' }}>Schedule</Link>
                                        </Dropdown.Item>
                                        <Dropdown.Divider />
                                        <Button className="loginButton text-black border border-2 border-dark" variant="" onClick={HandleLogOut} style={{ width: '60%', position: 'relative', float: 'inline-end', marginRight: '10px' }}>
                                            <span className="loginContent">Log Out</span>
                                        </Button>
                                    </Dropdown.Menu>
                                </Dropdown>
                            </>

                        ) : (
                            <Button className="loginButton text-black border border-2 border-dark" variant="" as={Link} to={"/login"}>
                                <BoxArrowInRight className="loginIcon"></BoxArrowInRight>
                                <span className="loginContent">Log In</span>
                            </Button>
                        )
                    }


                </Navbar.Collapse>
            </Container>
        </Navbar >
    );
}

export default Header;
