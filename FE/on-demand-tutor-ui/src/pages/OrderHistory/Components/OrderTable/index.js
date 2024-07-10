import Table from 'react-bootstrap/Table';
import { useEffect, useState } from 'react';
import orderHistoryAPI from '../../../../api/orderHistoryAPI';
import CancelOrder from '../../CancelOrder';
import ChangeTime from '../../ChangeTime';
import { Margin } from '@mui/icons-material';
import Invoice from '../../Invoice/Invoice';
import Feedback from '../../Feedback/Feedback';

const OrderHistoryList = ({ learnerId }) => {

  const [listOrder, setListOrder] = useState([]);

  useEffect(() => {
    const getOrderHistory = async () => {
      try {
        let res = await orderHistoryAPI.getOrderHistoryById(learnerId);
        setListOrder(res.data.$values);

      } catch (error) {
        console.log("try order history failed", error)
      }

    }
    getOrderHistory();
  }, [learnerId])

  const formatDate = (date) => {
    const day = date.getDate();
    const month = date.getMonth() + 1;
    const year = date.getFullYear();
    return `${year}-0${month}-${day}`;
  };

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
              const formattedOrderDate = formatDate(new Date(item.orderDate));
              const timeDiff = new Date() - new Date(item.orderDate);
              const avail = item.orderStatus === "Pending" || item.orderStatus === "Paid"
              const isCancellable = avail && timeDiff < 48 * 60 * 60 * 1000;
              const isCompleted = (item.orderStatus === "Paid" || item.orderStatus === "Accepted" || item.orderStatus === "Pending")
              const isAbleToFeedback = (item.orderStatus === "Accepted")
              return (
                <tr key={`order-${index}`}>
                  <td>{item.orderId}</td>
                  <td>{formattedOrderDate}</td>
                  <td>{item.orderStatus}</td>
                  <td>₫{item.total}</td>
                  <td>{item.curriculumId}</td>
                  <td>
                    {isCancellable ? (
                      <td>
                        <ChangeTime order={item} />
                        <CancelOrder order={item} learnerId={learnerId} />
                      </td>
                    ) : (
                      <td></td>
                    )}

                    {isCompleted ? (
                      <td>
                        <Invoice order={item} />
                      </td>
                    ) : (
                      <td></td>
                    )}

                    {isAbleToFeedback ? (
                      <td>
                        <Feedback order={item} learnerId={learnerId} />
                      </td>
                    ) : (
                      <td></td>
                    )}

                  </td>


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