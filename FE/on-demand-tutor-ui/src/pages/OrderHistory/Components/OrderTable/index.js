import Table from 'react-bootstrap/Table';
import { useEffect, useState } from 'react';
import orderHistoryAPI from '../../../../api/orderHistoryAPI';
import CancelOrder from '../../CancelOrder';
import ChangeTime from '../../ChangeTime';
import { Margin } from '@mui/icons-material';
const OrderHistoryList = ({ learnerId }) => {

  const [listOrder, setListOrder] = useState([]);

  useEffect(() => {
    const getOrderHistory = async () => {
      try {
        console.log("try order history")
        // let userId = learnerId;
        let res = await orderHistoryAPI.getOrderHistoryById(learnerId);
        console.log("res>>>>>>>>>>>>>>>>", res.data.$values)
        setListOrder(res.data.$values);
        console.log("try order history finish")

      } catch (error) {
        console.log("try order history failed", error)

      }

    }
    getOrderHistory();
  }, [learnerId])



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
        <tbody>
          {listOrder && listOrder.length > 0 ? (
            listOrder.map((item, index) => {
              const timeDiff = new Date() - new Date(item.orderDate);
              const avail = item.orderStatus === "Pending" || item.orderStatus === "Paid"
              const isCancellable = avail && timeDiff < 48 * 60 * 60 * 1000;
              const isCompleted = item.orderStatus === "Completed"
              return (
                <tr key={`order-${index}`}>
                  <td>{item.orderId}</td>
                  <td>{item.orderDate}</td>
                  <td>{item.orderStatus}</td>
                  <td>{item.total}</td>
                  <td>{item.curriculumId}</td>
                  {isCancellable ? (
                    <td>
                      <ChangeTime order={item} />
                      <CancelOrder order={item} learnerId={learnerId} />
                    </td>
                  ) : (
                    <td></td>
                  )}
                  {/* {isCompleted ? (
                    <td>
                      <Feedback order={item} /> 
                    </td>
                  ) : (
                    <td></td>
                  )} */}

                </tr>
              );
            })
          ) : (
            <tr>
              <td colSpan="6">No orders available</td>
            </tr>
          )}
        </tbody>
      </Table>
    </div>
  );
}

export default OrderHistoryList;