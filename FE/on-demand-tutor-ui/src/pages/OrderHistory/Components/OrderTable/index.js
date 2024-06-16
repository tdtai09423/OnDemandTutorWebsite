import Table from 'react-bootstrap/Table';
import { useEffect, useState } from 'react';
import orderHistoryAPI from '../../../../api/orderHistoryAPI';
import CancelOrder from '../../CancelOrder';
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
              // Calculate time difference
              const timeDiff = new Date() - new Date(item.OrderDate);
              const isCancellable = item.OrderStatus === "Not start" && timeDiff > 48 * 60 * 60 * 1000; // 48 hours in milliseconds

              return (
                <tr key={`order-${index}`}>
                  <th>{item.OrderId}</th>
                  <th>{item.OrderDate}</th>
                  <th>{item.OrderStatus}</th>
                  <th>{item.Total}</th>
                  <th>{item.CurriculumId}</th>
                  {isCancellable && (
                    <th>
                      <CancelOrder
                        order={item}
                      />
                    </th>
                  )}
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