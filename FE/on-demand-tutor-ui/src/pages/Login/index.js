import React, { useState } from 'react';
import './style.scss';
import { Link, useNavigate } from 'react-router-dom';
import loginAPI from '../../api/loginAPI';
import { toast } from 'react-toastify';
import loginGoogleAPI from '../../api/loginGoogleAPI';
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
                <a className='forgot-password' href="forgot_password.html">Forgot Password?</a>
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