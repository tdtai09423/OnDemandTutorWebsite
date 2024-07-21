import Table from 'react-bootstrap/Table';
import { useEffect, useState } from 'react';
import orderHistoryAPI from '../../../../api/orderHistoryAPI';
import CancelOrder from '../../CancelOrder';
import ChangeTime from '../../ChangeTime';
import { Margin } from '@mui/icons-material';
import Invoice from '../../Invoice/Invoice';
import Feedback from '../../Feedback/Feedback';
import './style.scss'
import {
  MDBCard,
  MDBCardBody,
  MDBCol,
  MDBRow,
} from "mdb-react-ui-kit";
const OrderHistoryList = ({ learnerId }) => {

  // const [listOrder, setListOrder] = useState([{ orderId: 1, orderDate: '2024-06-18 19:13:15.880', orderStatus: 'Accepted', total: 100, curriculumId: 1, isCompleted: false },
  // { orderId: 10, orderDate: '2024-06-18 19:41:58.240', orderStatus: 'Accepted', total: 100, curriculumId: 1, isCompleted: true },
  // { orderId: 2, orderDate: '2024-06-19 13:21:39.747', orderStatus: 'Paid', total: 100, curriculumId: 1, isCompleted: false },
  // { orderId: 3, orderDate: '2024-06-20 23:29:24.410', orderStatus: 'Rejected', total: 100, curriculumId: 1, isCompleted: false },
  // { orderId: 4, orderDate: '2024-06-24 00:56:21.053', orderStatus: 'Accepted', total: 100, curriculumId: 1, isCompleted: true },
  // { orderId: 5, orderDate: '2024-06-24 01:32:56.913', orderStatus: 'Rejected', total: 100, curriculumId: 1, isCompleted: false },
  // { orderId: 6, orderDate: '2024-06-24 18:08:58.743', orderStatus: 'Paid', total: 100, curriculumId: 1, isCompleted: true },
  // { orderId: 7, orderDate: '2024-06-24 18:10:56.460', orderStatus: 'Accepted', total: 100, curriculumId: 1, isCompleted: false },
  // { orderId: 11, orderDate: '2024-07-19 18:10:56.460', orderStatus: 'Paid', total: 100, curriculumId: 1, isCompleted: false },
  // ]);
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
      {listOrder && listOrder.length > 0 ? (
        listOrder.map((item, index) => {
          const formattedOrderDate = formatDate(new Date(item.orderDate));
          const timeDiff = new Date() - new Date(item.orderDate);
          const avail = item.orderStatus === "Pending" || item.orderStatus === "Paid"
          const isCancellable = avail && timeDiff < 48 * 60 * 60 * 1000;
          const isCompletedCheckOut = (item.orderStatus === "Paid" || item.orderStatus === "Accepted" || item.orderStatus === "Pending")
          const isAbleToFeedback = (item.orderStatus === "Accepted")
          return (
            <MDBCard className="shadow-0 border mb-4">
              <MDBCardBody>
                <MDBRow>
                  <MDBCol
                    md="2"
                    className="text-center d-flex justify-content-center align-items-center"
                  >
                    <p className="text-muted mb-0">#{item.orderId}</p>
                  </MDBCol>
                  <MDBCol
                    md="2"
                    className="text-center d-flex justify-content-center align-items-center"

                  >
                    <p className="text-muted mb-0">{item.orderId}</p>
                  </MDBCol>
                  <MDBCol
                    md="2"
                    className="text-center d-flex justify-content-center align-items-center"
                  >
                    {item.isCompleted ? <p className="orderComplete">Completed</p> : <p className="orderUnComplete">Not Completed</p>}
                  </MDBCol>
                  <MDBCol
                    md="2"
                    className="text-center d-flex justify-content-center align-items-center"
                  >
                    <p className="text-muted mb-0 small">{formattedOrderDate}</p>
                  </MDBCol>
                  <MDBCol
                    md="2"
                    className="text-center d-flex justify-content-center align-items-center"
                  >
                    <strong className="text-muted mb-0 small">{item.orderStatus}</strong>
                  </MDBCol>
                  <MDBCol
                    md="2"
                    className="text-center d-flex justify-content-center align-items-center"
                  >
                    <p className="text-muted mb-0 small">${item.total}</p>
                  </MDBCol>


                  <hr style={{ backgroundColor: "#e0e0e0", opacity: 1, marginBottom: '5px' }} />
                  <MDBRow className="align-items-center" style={{ marginTop: '10px' }}>
                    <MDBCol md="4"></MDBCol>
                    <MDBCol md="4"></MDBCol>
                    <MDBCol md="4" className=' parent-container'>

                      <div className='button-container'>
                        {isCancellable ? (
                          <>
                            <ChangeTime

                              order={item} />
                            <CancelOrder

                              order={item} learnerId={learnerId} />
                          </>
                        ) : null}

                        {isCompletedCheckOut ? (
                          <>

                            <Invoice

                              order={item} />

                          </>
                        ) : (
                          <></>
                        )}

                        {isAbleToFeedback ? (
                          <>
                            <Feedback

                              order={item} learnerId={learnerId} />
                          </>
                        ) : (
                          <></>
                        )}
                      </div>

                    </MDBCol>
                  </MDBRow>
                </MDBRow>
              </MDBCardBody>
            </MDBCard>
          );
        })
      ) : (
        <>
          <p>You have no order</p>
        </>
      )}


    </div>
  );
}

export default OrderHistoryList;