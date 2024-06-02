import { NavDropdown, Navbar, Nav, Button, Container } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css'
import './Header.scss'
import { Link } from 'react-router-dom'
import { BoxArrowInRight } from 'react-bootstrap-icons'
import images from '../../../../assets/images';


function Header() {
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
                    <Button className="loginButton text-black border border-2 border-dark" variant="" as={Link} to={"/login"}>
                        <BoxArrowInRight className="loginIcon"></BoxArrowInRight>
                        <span className="loginContent">Log In</span>
                    </Button>

                </Navbar.Collapse>
            </Container>
        </Navbar >
    );
}

export default Header;
