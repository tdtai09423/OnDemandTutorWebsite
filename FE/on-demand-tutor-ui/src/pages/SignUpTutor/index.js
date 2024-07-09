import React, { useState, useEffect } from 'react';
import './style.scss'
import { Link, useNavigate } from 'react-router-dom'
// import SignUpTutorAPI from '../../api/signUpTutor';
import axios from "axios";
import { toast } from 'react-toastify';
import { NavDropdown, Navbar, Nav, Button, Container } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css'
import '../../Components/Layout/components/Header/Header.scss'
import { BoxArrowInRight } from 'react-bootstrap-icons'
import images from '../../assets/images';



function SignUpTutor() {
    const [firstName, setFirstName] = useState("");
    const [lastName, setLastName] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [tutorAge, setTutorAge] = useState("");
    const [nationality, setNationality] = useState("");
    const [tutorDescription, setTutorDescription] = useState("");
    const [tutorImage, settutorImage] = useState("");
    const [majorId, setMajorId] = useState("");
    const [certificateLink, setCertificateLink] = useState("");

    const navigate = useNavigate();

    //preview and upload image
    useEffect(() => {
        return () => {
            tutorImage && URL.revokeObjectURL(tutorImage.preview)
        }
    }, [tutorImage])

    const handlePreviewAvartar = (e) => {
        const file = e.target.files[0];

        file.preview = URL.createObjectURL(file);
        settutorImage(file)
    }

    const handleMajorChange = (event) => {
        const value = event.target.value;
        // Set the selected option only if it's one of the allowed choices
        if (['CHN', 'ENG', 'JPN'].includes(value)) {
            setMajorId(value);
        };
    }


    const handleSignUp = async () => {
        try {
            const formData = new FormData();
            formData.append('tutorAge', tutorAge);
            formData.append('tutorImage', tutorImage);
            formData.append('nationality', nationality);
            formData.append('tutorDescription', tutorDescription);
            formData.append('majorId', majorId);
            formData.append('certificateLink', certificateLink);
            formData.append('firstName', firstName);
            formData.append('lastName', lastName);
            formData.append('email', email);
            formData.append('password', password);
            formData.append('confirmPassword', confirmPassword);
            // Validate tutor picture
            if (!tutorImage) {
                // Handle missing tutor picture
                toast.error("Please upload your photo when registering as a tutor.");
                return;
            }

            // Validate file type (e.g., allow only image files)
            const allowedImageTypes = ['image/jpeg', 'image/png'];
            if (!allowedImageTypes.includes(tutorImage.type)) {
                toast.error("Invalid image format. Please upload a valid image file (JPEG or PNG).");
                return;
            }

            // Set a maximum file size (adjust as needed)
            const maxFileSizeBytes = 5 * 1024 * 1024; // 5 MB
            if (tutorImage.size > maxFileSizeBytes) {
                toast.error("File size exceeds the maximum allowed limit (5 MB).");
                return;
            }

            console.log(formData)
            let res = await axios.post("https://localhost:7010/api/Account/tutor-register", formData, {
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
            const formData = new FormData();
            formData.append('toEmail', email);
            let res1 = await axios.post("https://localhost:7010/api/Account/send-verification-code", formData, {
                headers: {
                    'Content-Type': 'multipart/form-data',
                },
            });
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
                <div className='tittle'>Sign up as a tutor</div>
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
                <input type='text' placeholder='Your age' className='form-control' value={tutorAge} onChange={(event) => setTutorAge(event.target.value)} />
                <div className='text'>Nationality</div>
                <input type='text' placeholder='Your nation?' className='form-control' value={nationality} onChange={(event) => setNationality(event.target.value)} />
                <div className='text'>Description</div>
                <input type='text' placeholder='Your Description' className='form-control' value={tutorDescription} onChange={(event) => setTutorDescription(event.target.value)} />


                <div className='text'>Picture</div>
                <div>
                    <input
                        type='file' onChange={handlePreviewAvartar}
                    />
                    {tutorImage && (
                        <img src={tutorImage.preview} alt='' width="50%" />
                    )}
                </div>


                <div className='text'>Major</div>
                <select className='select-major' value={majorId} onChange={handleMajorChange}>
                    <option value="">--Select your major--</option>
                    <option value="CHN">Chinese</option>
                    <option value="ENG">English</option>
                    <option value="JPN">Japanese</option>
                </select>


                <div className='text'>Certificate</div>
                <input type='text' placeholder='Your certificate' className='form-control' value={certificateLink} onChange={(event) => setCertificateLink(event.target.value)} />


                <button className={email && password ? "active" : ""}
                    onClick={() => handleSignUp()}
                >Sign up</button>
            </div>
        </>
    )
}

export default SignUpTutor