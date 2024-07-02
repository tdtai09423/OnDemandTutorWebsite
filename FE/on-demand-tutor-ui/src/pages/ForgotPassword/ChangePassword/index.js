
import React, { useState, useEffect } from 'react';
import './style.scss';

import { Link, useNavigate, useLocation } from 'react-router-dom';

import { toast } from 'react-toastify';
import VerifyAccount from '../../../api/verifyAccountAPI';
function ChangePassword() {
    const location = useLocation();


    const navigate = useNavigate();
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    //preview and upload image


    const handleResetPassword = async () => {
        try {

            let verify = await VerifyAccount.postResetPassword(password, confirmPassword);
            toast.success("Change password succesfully!");
            navigate("/login")
        } catch (error) {
            toast.error(error);
            console.error("Verify error:", error);
        }


    }


    return (
        <>
            <div className='signup-container col-3 sign-up-form'>
                <div className='tittle'><b>Verifycation Email</b></div>


                <div className='text'>New Password</div>
                <input type='password' placeholder='Your New Password' className='form-control' value={password} onChange={(event) => setPassword(event.target.value)} />
                <div className='text'>Confirm Password</div>
                <input type='password' placeholder='Re-enter Your New Password' className='form-control' value={confirmPassword} onChange={(event) => setConfirmPassword(event.target.value)} />
                <button onClick={() => handleResetPassword()}>Reset Password</button>
            </div>
        </>
    )
}

export default ChangePassword