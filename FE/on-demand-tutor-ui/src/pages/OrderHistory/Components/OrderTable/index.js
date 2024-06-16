import Table from 'react-bootstrap/Table';
import { useEffect, useState } from 'react';
import orderHistoryAPI from '../../../../api/orderHistoryAPI';
function OrderHistoryList({ user }) {
  const [listOrder, setListOrder] = useState([]);
  console.log("userID>>>>>>>>>>>>>>>>>", user.id)

  useEffect(() => {
    getOrderHistory();
  }, [])
  const getOrderHistory = async () => {
    let res = await orderHistoryAPI.getOrderHistoryById(user.id);
    if (res && res.data) {
      setListOrder(res.data);
    }
  }


  return (
    <div className='Container'>
      <Table striped bordered hover>
        <thead>
          <tr>
            <th>Order ID</th>
            <th>Order Date</th>
            <th>Order Status</th>
            <th>Total</th>
            <th>Curriculum ID</th>
          </tr>
        </thead>
        <tbody>
          {listOrder && listOrder.length > 0 &&

            listOrder.map((item, index) => {
              return (
                <tr key={`order-${index}`}>
                  <th>{item.OrderId}</th>
                  <th>{item.OrderDate}</th>
                  <th>{item.OrderStatus}</th>
                  <th>{item.Total}</th>
                  <th>{item.CurriculumId}</th>
                </tr>
              )
            })
          }
        </tbody>
      </Table>
    </div>
  );
}

export default OrderHistoryList;