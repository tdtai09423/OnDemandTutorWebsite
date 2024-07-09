
import React, { useState, useEffect } from 'react';
import './style.scss';
import axios from "axios";
import { Link, useNavigate } from 'react-router-dom';
// import SignUpStudentAPI from '../../api/signUpStudent';
import { toast } from 'react-toastify';
import sendVerifyCode from '../../api/sendVerifyCode';
import './style.scss';
import { NavDropdown, Navbar, Nav, Button, Container } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css'
import '../../Components/Layout/components/Header/Header.scss'
import { BoxArrowInRight } from 'react-bootstrap-icons'
import images from '../../assets/images';


function SignUpStudent() {
    const [firstName, setFirstName] = useState("");
    const [lastName, setLastName] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [learnerAge, setLearnerAge] = useState("");
    const [learnerImage, setLearnerImage] = useState("");


    const navigate = useNavigate();
    //preview and upload image
    useEffect(() => {
        return () => {
            learnerImage && URL.revokeObjectURL(learnerImage.preview)
        }
    }, [learnerImage])

    const handlePreviewAvartar = (e) => {
        const file = e.target.files[0];

        file.preview = URL.createObjectURL(file);
        setLearnerImage(file)
        console.log(file);
    }

    const handleSignUp = async () => {
        try {
            const formData = new FormData();
            formData.append('LearnerImage', learnerImage);
            formData.append('FirstName', firstName);
            formData.append('LastName', lastName);
            formData.append('Email', email);
            formData.append('Password', password);
            formData.append('ConfirmPassword', confirmPassword);
            formData.append('LearnerAge', learnerAge);
            console.log(formData);
            let res = await axios.post("https://localhost:7010/api/Account/learner-register", formData, {
                headers: {
                    'Content-Type': 'multipart/form-data',
                },
            });
            toast.success(res.message);
            handleSendVerifyCode();
        } catch (error) {
            toast.error(error.message);
            console.error("Sign up error:", error);
        }
    }

    const handleSendVerifyCode = async () => {
        try {

            let sendCode = await sendVerifyCode.sendCodeToEmail(email);
            toast.success("Sent verification code to your email!");
            navigate("/verify-code", { state: { email: email } });
        } catch (error) {
            toast.error(error.message);
            console.error("Send code error:", error);
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
            <div className='signup-container col-3 sign-up-form'>
                <div className='tittle'>Sign up as a student</div>
                <div className='haveacc'>Already have an account? <Link className="forgot-password" as={Link} to={"/login"}> Log in</Link></div>

                <div className='text'>First name</div>
                <input type='text' placeholder='Your first name' className='form-control' value={firstName} onChange={(event) => setFirstName(event.target.value)} />

                <div className='text'>Last name</div>
                <input type='text' placeholder='Your last name' className='form-control' value={lastName} onChange={(event) => setLastName(event.target.value)} />

                <div className='text'>Email</div>
                <input type='email' placeholder='Your email' className='form-control' value={email} onChange={(event) => setEmail(event.target.value)} />

                <div className='text'>Password</div>
                <input type='password' placeholder='Your password' className='form-control' value={password} onChange={(event) => setPassword(event.target.value)} />

                <div className='text'>Confirm password</div>
                <input type='password' placeholder='Confirm your password' className='form-control' value={confirmPassword} onChange={(event) => setConfirmPassword(event.target.value)} />

                <div className='text'>Age</div>
                <input type='number' placeholder='Your age' className='form-control' value={learnerAge} onChange={(event) => setLearnerAge(event.target.value)} />


                {/* input and preview avatar */}
                <div className='text'>Upload your picture</div>
                {/* <input type='text' placeholder='Upload your picture' className='form-control' value={learnerImage} onChange={(event) => setlearnerImage(event.target.value)} /> */}
                <div>
                    <input
                        type='file' onChange={handlePreviewAvartar}
                    />
                    {learnerImage && (
                        <img src={learnerImage.preview} alt='' width="50%" />
                    )}
                </div>

                <button className={email && password ? "active" : ""}
                    onClick={() => handleSignUp()}
                >Sign up</button>
            </div>
        </>
    )
}

export default SignUpStudent