import React, { useState, useEffect } from 'react';

function NotificationCom() {
  const [user, setUser] = useState({});
  
//   useEffect(() => {
//     const fetchUser = async () => {
//       try {
//         const email = localStorage.getItem('email');
//         const res = await userAPI.getUserByEmail(email);
//         setUser(res.data);
//         console.log("user>>>>>>>>>>>>>>>>>>>>>>>>>>", user)
//       } catch (error) {
//         console.error("Error fetching user:", error);
//       }
//     };
//     fetchUser();
//   }, [])
  return (
    <div className='Container' style={{ marginTop: '50px' }}>
        
      
    </div>
  );
}

export default NotificationCom;