import React from 'react'
import './style.scss'
import { Link } from 'react-router-dom'
function SignUpStudent() {
    return (
        <>
            <div className='login-container col-3'>
                <div className='tittle'>Sign up as a student</div>
                <div className='haveacc'>Already have an account? <Link className="forgot-password" as={Link} to={"/login"}> Log in</Link></div>
                <button className='ex-button'><i class="fa-brands fa-google"></i>  Continue with Google</button>
                <button className='ex-button'><i class="fa-brands fa-facebook"></i>  Continue with Facebook</button>
                <div className='text'>or</div>

                <div className='text'>Name</div>
                <input type='text' placeholder='Your name' className='form-control' />
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

export default SignUpStudent