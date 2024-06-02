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
                        <Nav.Link as={Link} to={"/"}><span className="navBarContent">Become a tutor</span></Nav.Link>
                        <NavDropdown title={<span className="navBarContent">Do Something Here</span>} id="navbarScrollingDropdown">
                            <NavDropdown.Item href="#action3">Action</NavDropdown.Item>
                            <NavDropdown.Item href="#action4">
                                Another action
                            </NavDropdown.Item>
                            <NavDropdown.Divider />
                            <NavDropdown.Item href="#action5">
                                Something else here
                            </NavDropdown.Item>
                        </NavDropdown>
                        <Nav.Link as={Link} to={"/"}>
                            <span className="navBarContent">Contact us</span>
                        </Nav.Link>
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
