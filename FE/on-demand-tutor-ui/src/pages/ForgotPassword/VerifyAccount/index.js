
import React, { useState, useEffect } from 'react';
import './style.scss';
import { Link, useNavigate, useLocation } from 'react-router-dom';

import { toast } from 'react-toastify';
import VerifyAccountAPI from '../../../api/verifyAccountAPI';
function VerifyAccount() {
    const location = useLocation();
    const { email } = location.state || {};
    const [verifyCode, setVerifyCode] = useState("");
    const navigate = useNavigate();
    //preview and upload image


    const handleSendVerifyCode = async () => {
        try {

            let verify = await VerifyAccountAPI.getVerifyCode(email, verifyCode);
            toast.success("Verify succesfully!");
            navigate("/change-password")
        } catch (error) {
            toast.error(error);
            console.error("Verify error:", error);
        }


    }


    return (
        <>
            <div className='signup-container col-3 sign-up-form'>
                <div className='tittle'><b>Verifycation Email</b></div>
                <div className='text'>Verifycation code</div>
                <input type='text' placeholder='Verify code from your email...' className='form-control' value={verifyCode} onChange={(event) => setVerifyCode(event.target.value)} />
                <button onClick={() => handleSendVerifyCode()}>Verify email</button>
            </div>
        </>
    )
}

export default VerifyAccount