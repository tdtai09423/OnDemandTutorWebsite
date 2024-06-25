
import React, { useState, useEffect } from 'react';
import './style.scss';

import {useNavigate } from 'react-router-dom';

import { toast } from 'react-toastify';
import VerifyAccount from '../../api/verifyAccountAPI';
function ForgotPassword() {
    const navigate = useNavigate();
    //preview and upload image
    const [email, setEmail] = useState('');


    const handleSendVerifyCode = async () => {
        try {
            let sendCode = await VerifyAccount.postSendCodeToEmail(email);
            toast.success("Sent verification code to your email!");
            navigate("/verify-account", { state: { email: email } });
        } catch (error) {
            toast.error(error.message);
            console.error("Send code error:", error);
        }
    }


    return (
        <>
        <div className='signup-container col-3 sign-up-form'>
            <div className='text'>Email</div>
            <input type='email' placeholder='Your email' className='form-control' value={email} onChange={(event) => setEmail(event.target.value)} />

            <button className={email  ? "active" : ""}
                onClick={() => handleSendVerifyCode()}
            >Send Code To Email</button>
        </div>
    </>
    )
}

export default ForgotPassword