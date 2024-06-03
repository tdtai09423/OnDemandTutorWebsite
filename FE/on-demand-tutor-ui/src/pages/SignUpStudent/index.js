
import React, { useState, useEffect } from 'react';
import './style.scss'
import { Link, useNavigate } from 'react-router-dom'
import SignUpStudentAPI from '../../api/signUpStudent';
import { toast } from 'react-toastify';
function SignUpStudent() {
    const [firstName, setFirstName]= useState("");
    const [lastName, setLastName]= useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [learnerAge, setLearnerAge]= useState("");
    const [learnerPicture, setLearnerPicture]= useState("");


    const navigate = useNavigate();
    //preview and upload image
    useEffect(() => {
        return () => {
            learnerPicture && URL.revokeObjectURL(learnerPicture.preview)
        }
    }, [learnerPicture])

    const handlePreviewAvartar = (e) =>{
        const file = e.target.files[0];

        file.preview = URL.createObjectURL(file);
        setLearnerPicture(file)
    }
   
    const handleSignUp = async () => {
        console.log("handleSignUp");
        console.log(firstName,lastName,email, password,confirmPassword,learnerAge,learnerPicture.name);
        try {
            let res = await SignUpStudentAPI(firstName,lastName,email, password,confirmPassword,learnerAge,learnerPicture);
            if (res && res.Ok) {
                toast.success(res.Ok.message);
                navigate("/login");
            } 
        } catch (error) {
            // Handle errors from the API call here
            toast.error("An error occurred during login.");
            console.error("Sign up error:", error);
        }
    }
    return (
        <>
            <div className='login-container col-3'>
                <div className='tittle'>Sign up as a student</div>
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
                <input type='number' placeholder='Your age' className='form-control' value={learnerAge} onChange={(event) => setLearnerAge(event.target.value)} />


                {/* input and preview avatar */}
                <div className='text'>Upload your picture</div>
                {/* <input type='text' placeholder='Upload your picture' className='form-control' value={learnerPicture} onChange={(event) => setLearnerPicture(event.target.value)} /> */}
                <div>
                    <input
                        type='file' onChange={handlePreviewAvartar}
                    />
                    {learnerPicture && (
                        <img src={learnerPicture.preview} alt='' width="50%"/>
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