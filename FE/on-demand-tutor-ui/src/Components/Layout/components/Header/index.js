import { NavDropdown, Navbar, Nav, Button, Container, Dropdown } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css'
import './Header.scss'
import { Link, useNavigate } from 'react-router-dom'
import { BoxArrowInRight, PersonCircle } from 'react-bootstrap-icons'
import images from '../../../../assets/images';
import logoutAPI from '../../../../api/logoutAPI';
import { useEffect, useState } from 'react';
import userAPI from '../../../../api/userAPI';
import NotificationAPI from '../../../../api/notificationAPI';
import Offcanvas from 'react-bootstrap/Offcanvas';

function Header() {
    const [showNotification, setNotification] = useState("")
    const [userRole, setUserRole] = useState();
    const [userId, setUserId] = useState();
    const [notifications, setNotifications] = useState([]);
    
    
    //Offcanvas
    const [show, setShow] = useState(false);
    const handleClose = () => setShow(false);
    const handleShow = () => {
        setShow(true);
        HandleNotification();
    };


    const Jtoken = localStorage.getItem('token');
    const email = localStorage.getItem('email');
    console.log(Jtoken);
    const navigate = useNavigate();

    const HandleLogOut = async () => {
        localStorage.removeItem('token');
        localStorage.removeItem('email');
        let res = await logoutAPI();
        window.location.reload();
        console.log(res);
        navigate("/");

    }
    const HandleNotification = async () => {
        try {
            // let res = NotificationAPI.getNotification(userId);
            // setNotifications(res);
            console.log("NOTIFICATION", res)
        } catch (error) {
            console.log(error);
        }
    }

    useEffect(() => {
        const fetchUser = async () => {
            try {
                const user = await userAPI.getUserByEmail(email);
                setUserRole(user.data.roleId)
                setUserId(user.data.id);
            } catch (error) {
                console.error("Error fetching user:", error);
            }
        };
        fetchUser();

    }, [])

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
                            <NavDropdown.Item><Link as={Link} to={"/send-report"}>Send report</Link></NavDropdown.Item>
                            <NavDropdown.Item href="#action4">
                                Another action
                            </NavDropdown.Item>
                            <NavDropdown.Divider />
                            <NavDropdown.Item>
                                <Link as={Link} to={"/policy"}>Policy</Link>
                            </NavDropdown.Item>
                        </NavDropdown>
                    </Nav>
                    {
                        Jtoken ? (
                            <>
                                
                                <Button variant="" onClick={handleShow}>
                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-bell" viewBox="0 0 16 16">
                                        <path d="M8 16a2 2 0 0 0 2-2H6a2 2 0 0 0 2 2M8 1.918l-.797.161A4 4 0 0 0 4 6c0 .628-.134 2.197-.459 3.742-.16.767-.376 1.566-.663 2.258h10.244c-.287-.692-.502-1.49-.663-2.258C12.134 8.197 12 6.628 12 6a4 4 0 0 0-3.203-3.92zM14.22 12c.223.447.481.801.78 1H1c.299-.199.557-.553.78-1C2.68 10.2 3 6.88 3 6c0-2.42 1.72-4.44 4.005-4.901a1 1 0 1 1 1.99 0A5 5 0 0 1 13 6c0 .88.32 4.2 1.22 6"/>
                                    </svg>
                                </Button>

                                <Offcanvas show={show} onHide={handleClose}>
                                    <Offcanvas.Header closeButton>
                                        <Offcanvas.Title>Notification</Offcanvas.Title>
                                    </Offcanvas.Header>
                                    <Offcanvas.Body>
                                        Some text as placeholder. In real life you can have the elements you
                                        have chosen. Like, text, images, lists, etc.
                                    </Offcanvas.Body>
                                </Offcanvas>
                                
                                
                       
                            <Dropdown align="" className='log-out-button'>
                                <Dropdown.Toggle variant="" id="dropdown-sort-by" className="rounded-2" drop="start">
                                    <PersonCircle style={{ fontSize: '2em' }}></PersonCircle>
                                </Dropdown.Toggle>
                                <Dropdown.Menu className="dropdown-menu" align="end">
                                    <Dropdown.Item><Link as={Link} to={"/user-profile"}>User profile</Link></Dropdown.Item>
                                    <Dropdown.Item>
                                        {(userRole === 'LEARNER') ?
                                            <Link as={Link} to={"/order-history"}>View history</Link>
                                            :
                                            <Link as={Link} to={"/order-list"}>Order list</Link>
                                        }

                                    </Dropdown.Item>
                                    <Dropdown.Item>
                                        {(userRole === 'LEARNER') ?
                                            <Link as={Link} to={"/favorite-tutor"}>Favorite tutor</Link>
                                            :
                                            <Link as={Link} to={"/personal-schedule"}>Schedule</Link>
                                        }
                                    </Dropdown.Item>
                                    <Dropdown.Item>
                                        {(userRole === 'LEARNER') ?
                                            <Link as={Link} to={"/personal-schedule"}>Schedule</Link>
                                            :
                                            <div></div>
                                        }
                                    </Dropdown.Item>
                                    <Dropdown.Divider />
                                    <Button className="loginButton text-black border border-2 border-dark" variant="" as={Link} to={"/"} onClick={HandleLogOut} style={{ width: '60%', position: 'relative', float: 'inline-end', marginRight: '10px' }}>
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
