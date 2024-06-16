import React, { useState, useEffect } from 'react';
import OrderHistoryList from './Components/OrderTable';
import userAPI from '../../api/userAPI';

import './orderHistory.scss'


function OrderHistory() {
  const [user, setUser] = useState({});

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const email = localStorage.getItem('email');
        const user = await userAPI.getUserByEmail(email);
        setUser(user.data);
        console.log("user>>>>>>>>>>>>>>>>>>>>>>>>>>", user)
      } catch (error) {
        console.error("Error fetching user:", error);
      }
    };
    fetchUser();
  }, [])
  return (
    <div className='Container'>
      <OrderHistoryList
        user={user}
      />
    </div>
  );
}

export default OrderHistory;