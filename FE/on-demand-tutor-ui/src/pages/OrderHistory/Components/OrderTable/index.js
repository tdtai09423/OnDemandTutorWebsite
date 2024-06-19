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
    try {
      console.log("try order history")
      let res = await orderHistoryAPI.getOrderHistoryById(user.id);
      console.log("res>>>>>>>>>>>>>>>>", res.data.$values)
      setListOrder(res.data.$values);
      console.log("try order history finish")

    } catch (error) {
      console.log("try order history failed", error)

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
            <th>More</th>
          </tr>
        </thead>
        {/* <tbody>
          {listOrder && listOrder.length > 0 ? (
            listOrder.map((item, index) => {
              const timeDiff = new Date() - new Date(item.orderDate);
              const isCancellable = item.orderStatus === "pending" && timeDiff < 48 * 60 * 60 * 1000;
              const isCompleted = item.orderStatus === "completed"
              return (
                <tr key={`order-${index}`}>
                  <td>{item.orderId}</td>
                  <td>{item.orderDate}</td>
                  <td>{item.orderStatus}</td>
                  <td>{item.total}</td>
                  <td>{item.curriculumId}</td>
                  {isCancellable ? (
                    <td>
                      <CancelOrder order={item} />
                    </td>
                  ) : (
                    <td></td>
                  )}{isCompleted ? (
                    <td>
                      <Feedback order={item} /> 
                    </td>
                  ) : (
                    <td></td>
                  )}

                </tr>
              );
            })
          ) : (
            <tr>
              <td colSpan="6">No orders available</td>
            </tr>
          )}
        </tbody> */}
        <tbody>
          <tr>
            <td>11</td>
            <td>2024-06-12 08:39:07.633</td>
            <td>Pending</td>
            <td>232</td>
            <td>4</td>
            <td></td>
          </tr>
          <tr>
            <td>12</td>
            <td>2024-06-11 08:39:07.633</td>
            <td>Completed</td>
            <td>321</td>
            <td>5</td>
            <td></td>
          </tr>
          <tr>
            <td>13</td>
            <td>2024-06-18 08:39:07.633</td>
            <td>Pending</td>
            <td>321</td>
            <td>5</td>
            <td><CancelOrder /></td>
          </tr>
          <tr>
            <td>15</td>
            <td>2024-06-19 08:39:07.633</td>
            <td>Pending</td>
            <td>221</td>
            <td>5</td>
            <td><CancelOrder /></td>
          </tr>

        </tbody>
      </Table>
    </div>
  );
}

export default OrderHistoryList;