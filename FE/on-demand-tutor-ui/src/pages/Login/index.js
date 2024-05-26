import React from 'react'
import './style.scss'
import { Link } from 'react-router-dom'
function Login() {
    return (
        <>
            <div className='login-container col-3'>
                <div className='tittle'>Log in</div>
                <div className='text'><Link className="forgot-password" as={Link} to={"/tutor-detail"}>Sign up as a student</Link>
                    or <a className='quen' href="forgot_password.html">Sign up as a tutor</a></div>
                <button className='ex-button'><i class="fa-brands fa-google"></i>  Contunue with Google</button>
                <button className='ex-button'><i class="fa-brands fa-facebook"></i>  Contunue with Facebook</button>
                <button className='ex-button'><i class="fa-brands fa-apple"></i> Contunue with Apple</button>
                <div className='text'>or</div>

                <div className='text'>Email</div>
                <input type='email' placeholder='Your email' className='form-control' />

                <div className='text'>Password</div>
                <input type='password' placeholder='Your password' className='form-control' />
                <a className='forgot-password' href="forgot_password.html">Forgot Password?</a>

                <button className='login-button' >Log in</button>
            </div>
        </>
    )
}

export default Login