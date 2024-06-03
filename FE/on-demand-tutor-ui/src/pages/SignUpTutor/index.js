import React, { useState, useEffect } from 'react';
import './style.scss'
import { Link, useNavigate } from 'react-router-dom'
import SignUpTutorAPI from '../../api/signUpTutor';
import { toast } from 'react-toastify';
function SignUpTutor() {
    const [firstName, setFirstName]= useState("");
    const [lastName, setLastName]= useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [tutorAge, setTutorAge] = useState("");
    const [nationality, setNationality] = useState("");
    const [tutorDescription, setTutorDescription] = useState("");
    const [tutorPicture, setTutorPicture] = useState("");
    const [majorId, setMajorId] = useState("");
    const [certificateLink, setCertificateLink] = useState("");

    const navigate = useNavigate();
    
    //preview and upload image
    useEffect(() => {
        return () => {
            tutorPicture && URL.revokeObjectURL(tutorPicture.preview)
        }
    }, [tutorPicture])

    const handlePreviewAvartar = (e) =>{
        const file = e.target.files[0];

        file.preview = URL.createObjectURL(file);
        setTutorPicture(file)
    }

    const handleMajorChange = (event) => {
        const value = event.target.value;
    // Set the selected option only if it's one of the allowed choices
        if (['CHN', 'ENG', 'JPN'].includes(value)) {
        setMajorId(value);
        };
    }


    const handleSignUp = async () => {
        console.log("handleSignUp");
        console.log(firstName,lastName,email, password,confirmPassword,tutorAge,nationality, tutorDescription, tutorPicture.name,majorId,certificateLink);
        try {
            let res = await SignUpTutorAPI(firstName,lastName,email, password,confirmPassword,tutorAge,nationality, tutorDescription, tutorPicture.name,majorId,certificateLink);
            if (res) {
                console.success(res)
                toast.success("sign up successful!");
                navigate("/login");
            } 
        } catch (error) {
            // Handle errors from the API call here
            toast.error("An error occurred during login.");
            console.error("sign up tutor error:", error);
        }
    }
    return (
        <>
            <div className='login-container col-3'>
                <div className='tittle'>Sign up as a tutor</div>
                <div className='haveacc'>Already have an account? <Link className="forgot-password" as={Link} to={"/login"}> Log in</Link></div>
                <button className='ex-button'><i className="fa-brands fa-google"></i>  Continue with Google</button>
                <button className='ex-button'><i className="fa-brands fa-facebook"></i>  Continue with Facebook</button>
                <div className='text'>or</div>

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
                    {tutorPicture && (
                        <img src={tutorPicture.preview} alt='' width="50%"/>
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