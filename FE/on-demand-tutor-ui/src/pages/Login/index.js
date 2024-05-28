import React, { useState } from 'react';
import './style.scss';
import { Link } from 'react-router-dom';
import loginAPI from '../../api/loginAPI';
import { toast } from 'react-toastify';
function Login() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    const handleLogIn = async () => {
        if (!email || !password) {
            toast.error("Missing email or password");
            return;
        }
        let res = await loginAPI(email, password);
        if (res && res.token) {
            localStorage.setItem("token", res.token);
            toast.success("Login okay!!!");
        }

    }

    return (
        <>
            <div className='login-container col-3'>
                <div className='tittle'>Log in</div>
                <div className='text'><Link className="forgot-password" as={Link} to={"/sign-up-student"}>Sign up as a student</Link>
                    or <Link className="forgot-password" as={Link} to={"/sign-up-tutor"}> Sign up as a tutor</Link>
                </div>
                <button className='ex-button'><i class="fa-brands fa-google"></i>  Continue with Google</button>
                <button className='ex-button'><i class="fa-brands fa-facebook"></i>  Continue with Facebook</button>
                <button className='ex-button'><i class="fa-brands fa-apple"></i> Continue with Apple</button>
                <div className='text'>or</div>

                <div className='text'>Email</div>
                <input type='email' placeholder='Your email' className='form-control' value={email} onChange={(event) => setEmail(event.target.value)} />

                <div className='text'>Password</div>
                <input type='password' placeholder='Your password' className='form-control' value={password} onChange={(event) => setPassword(event.target.value)} />
                <a className='forgot-password' href="forgot_password.html">Forgot Password?</a>
                <div className='checkBox'>
                    <input type="checkbox" id="rememberMe" name="rememberMe" defaultChecked />
                    <label for="rememberMe" >Remember me</label>
                </div>
                <button className={email && password ? "active" : ""}
                    onClick={() => handleLogIn()}
                >Log in</button>
            </div>
        </>
    )
}

export default Login