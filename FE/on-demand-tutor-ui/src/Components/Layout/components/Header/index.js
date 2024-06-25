import { NavDropdown, Navbar, Nav, Button, Container, Dropdown } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css'
import './Header.scss'
import { Link, useNavigate } from 'react-router-dom'
import { BoxArrowInRight, PersonCircle } from 'react-bootstrap-icons'
import images from '../../../../assets/images';
import logoutAPI from '../../../../api/logoutAPI';
import { useEffect, useState } from 'react';
import userAPI from '../../../../api/userAPI';


function Header() {

    const [userRole, setUserRole] = useState();

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

    useEffect(() => {
        const fetchUser = async () => {
            try {
                const user = await userAPI.getUserByEmail(email);
                setUserRole(user.data.roleId)
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
