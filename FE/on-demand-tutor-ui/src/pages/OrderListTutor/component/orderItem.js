import {
    MDBCard,
    MDBCardBody,
    MDBCol,
    MDBRow,
} from "mdb-react-ui-kit";
import { useState, useEffect } from "react";
import learnerAPI from "../../../api/learnerAPI";
import './orderItem.scss';
import { Button, Form, InputGroup } from 'react-bootstrap';
import orderHistoryAPI from "../../../api/orderHistoryAPI";

function OrderItem({ orderItem, id }) {

    const [learner, setLearner] = useState({});
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [inputUrl, setInputUrl] = useState('');

    useEffect(() => {
        const fetchUser = async () => {
            try {
                const learner = await learnerAPI.get(orderItem.learnerId);
                setLearner(learner.data);
                setFirstName(learner.data.learnerNavigation.firstName);
                setLastName(learner.data.learnerNavigation.lastName);
            } catch (error) {
                console.error("Error fetching user:", error);
            }
        };
        fetchUser();
    }, [orderItem.learnerId]);

    const formatDate = (date) => {
        const day = date.getDate();
        const month = date.getMonth() + 1;
        const year = date.getFullYear();
        return `${year}-${month}-${day}`;
    };

    const formattedOrderDate = formatDate(new Date(orderItem.orderDate));

    const handleAccept = async () => {
        const token = localStorage.getItem('token');
        const res = await orderHistoryAPI.postAcceptOrder(id, orderItem.orderId, token);
        console.log(res);
        window.location.reload();
    }

    const handleReject = async () => {
        const token = localStorage.getItem('token');
        const res = await orderHistoryAPI.postRejectOrder(id, orderItem.orderId, token);
        console.log(res);
        window.location.reload();
    }

    const handleUrlChange = (e) => {
        setInputUrl(e.target.value);
    }

    return (
        <>
            <MDBCard className="shadow-0 border mb-4">
                <MDBCardBody>
                    <MDBRow>
                        <MDBCol
                            md="2"
                            className="text-center d-flex justify-content-center align-items-center"
                        >
                            <p className="text-muted mb-0">#{orderItem.orderId}</p>
                        </MDBCol>
                        <MDBCol
                            md="2"
                            className="text-center d-flex justify-content-center align-items-center"
                        >
                            <p className="text-muted mb-0">{firstName} {lastName}</p>
                        </MDBCol>
                        <MDBCol
                            md="2"
                            className="text-center d-flex justify-content-center align-items-center"
                        >
                            {orderItem.isComplete ? <p className="orderComplete">Completed</p> : <p className="orderUnComplete">Not Completed</p>}
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
                            <strong className="text-muted mb-0 small">{orderItem.orderStatus}</strong>
                        </MDBCol>
                        <MDBCol
                            md="2"
                            className="text-center d-flex justify-content-center align-items-center"
                        >
                            <p className="text-muted mb-0 small">${orderItem.total}</p>
                        </MDBCol>
                    </MDBRow>
                    <hr style={{ backgroundColor: "#e0e0e0", opacity: 1, marginBottom: '5px' }} />
                    <MDBRow className="align-items-center" style={{ marginTop: '10px' }}>
                        <MDBCol md="4">
                            {orderItem.orderStatus === 'Paid' && (
                                <InputGroup>
                                    <Form.Control
                                        placeholder="Input classroom URL"
                                        className="me-2 search-bar"
                                        aria-label="Classroom URL"
                                        value={inputUrl}
                                        onChange={handleUrlChange}
                                    />
                                </InputGroup>
                            )}
                        </MDBCol>
                        <MDBCol md="4"></MDBCol>
                        <MDBCol md="4">
                            {orderItem.isComplete || orderItem.orderStatus !== 'Paid' ? null : (
                                <div className="d-flex justify-content-around mb-1">
                                    <Button
                                        className="text-muted mt-1 mb-0 small ms-xl-5 custom-button-text-order-item"
                                        onClick={handleAccept}
                                        variant="primary"
                                        style={{ color: 'white' }}
                                        disabled={!inputUrl}
                                    >
                                        Accept
                                    </Button>
                                    <Button
                                        className="text-muted mt-1 mb-0 small ms-xl-5 custom-button-text-order-item"
                                        onClick={handleReject}
                                        variant="danger"
                                        style={{ color: 'white' }}
                                        disabled={!inputUrl}
                                    >
                                        Reject
                                    </Button>
                                </div>
                            )}
                        </MDBCol>
                    </MDBRow>
                </MDBCardBody>
            </MDBCard>
        </>
    );
}

export default OrderItem;
