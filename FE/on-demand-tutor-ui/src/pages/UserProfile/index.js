import React, { useState, useEffect } from 'react';
import userAPI from '../../api/userAPI';
import UserProfileTab from '../../Components/UserProfileTab';

function UserProfile() {

    const [user, setUser] = useState({});

    useEffect(() => {
        const fetchUser = async () => {
            try {
                const email = localStorage.getItem('email');
                const user = await userAPI.getUserByEmail(email);
                setUser(user.data);
            } catch (error) {
                console.error("Error fetching user:", error);
            }
        };
        fetchUser();




    }, [])



    return (
        <>
            <UserProfileTab
                user={user}
            />
        </>
    )
}

export default UserProfile