
import React, { useState, useEffect } from 'react';
import './style.scss';
import axios from "axios";
import { Link, useNavigate, useLocation } from 'react-router-dom';

import { toast } from 'react-toastify';
import sendVerifyCode from '../../api/sendVerifyCode';
function VerifyCode() {
    const location = useLocation();
    const { email } = location.state || {};
    const [verifyCode, setVerifyCode] = useState("");
    const navigate = useNavigate();
    //preview and upload image


    const handleSendVerifyCode = async () => {
        try {
            // const formData = new FormData();
            // formData.append('email', email);
            // formData.append('code', verifyCode);
            // let res = await axios.post("https://localhost:7010/api/Account/verify-code",formData, {
            //     headers: {
            //         'Content-Type': 'multipart/form-data',
            //     },
            // });
            let verify = await sendVerifyCode.verifyCode(email, verifyCode);
            toast.success("Verify succesfully!");
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


                <div className='text'>Verifycation code</div>
                <input type='text' placeholder='Verify code from your email...' className='form-control' value={verifyCode} onChange={(event) => setVerifyCode(event.target.value)} />
                <button onClick={() => handleSendVerifyCode()}>Verify email</button>
            </div>
        </>
    )
}

export default VerifyCode