import React, { useState } from 'react';
import './style.scss';
import { Link, useNavigate } from 'react-router-dom';
import loginAPI from '../../api/loginAPI';
import { toast } from 'react-toastify';
import loginGoogleAPI from '../../api/loginGoogleAPI';
import { NavDropdown, Navbar, Nav, Button, Container } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css'
import '../../Components/Layout/components/Header/Header.scss'
import { BoxArrowInRight } from 'react-bootstrap-icons'
import images from '../../assets/images';

function Login() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [rememberMe, setRememberMe] = useState(false);
    const navigate = useNavigate();
    const handleRememberMeChange = (event) => {
        setRememberMe(event.target.checked);
    };
    const handleLogIn = async () => {
        console.log("handleLogin");
        console.log(email, password, rememberMe);
        try {
            let res = await loginAPI(email, password, rememberMe);
            if (res && res.data.token) {
                localStorage.setItem("token", res.data.token);
                localStorage.setItem("email", email);
                console.log(localStorage);
                toast.success("Login successful!");
                navigate("/");
            }
        } catch (error) {
            // Handle errors from the API call here
            toast.error("An error occurred during login.");
            console.error("Login error:", error);
        }
    }

    const handleLoginGoogle = async () => {
        try {
            let res = await loginGoogleAPI();
            console.log("res>>>>>>", res)
            // if (res && res.data.token) {
            //     localStorage.setItem("token", res.data.token);
            //     localStorage.setItem("email", email);
            //     console.log(localStorage);
            //     toast.success("Login successful!");
            //     navigate("/");
            // }
        } catch (error) {

        }
    }

    return (
        <>
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
            </Navbar>
            <div className='login-container col-3'>
                <div className='tittle'>Log in</div>
                <div className='text'><Link className="forgot-password" as={Link} to={"/sign-up-student"}>Sign up as a student </Link>
                    or <Link className="forgot-password" as={Link} to={"/sign-up-tutor"}> Sign up as a tutor</Link>
                </div>
                <button className='ex-button' onClick={handleLoginGoogle}><i className="fa-brands fa-google"></i>  Continue with Google</button>
                <div className='text'>or</div>

                <div className='text'>Email</div>
                <input type='email' placeholder='Your email' className='form-control' value={email} onChange={(event) => setEmail(event.target.value)} />

                <div className='text'>Password</div>
                <input type='password' placeholder='Your password' className='form-control' value={password} onChange={(event) => setPassword(event.target.value)} />
                <Link className="forgot-password" as={Link} to={"/forgot-password"}> Forgot your password</Link>
                <div className='checkBox' style={{ marginTop: '10px' }}>
                    <input type="checkbox" id="rememberMe" name="rememberMe" checked={rememberMe} onChange={handleRememberMeChange} />
                    <label for="rememberMe" style={{ marginLeft: '5px' }}>Remember me</label>
                </div>
                <button className={email && password ? "active" : ""}
                    onClick={() => handleLogIn()}
                >Log in</button>
            </div>
        </>
    )
}

export default Login