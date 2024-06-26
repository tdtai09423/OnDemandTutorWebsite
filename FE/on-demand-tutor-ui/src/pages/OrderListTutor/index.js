import {
    MDBCard,
    MDBCardBody,
    MDBCardFooter,
    MDBCardHeader,
    MDBCol,
    MDBContainer,
    MDBRow,
    MDBTypography,
} from "mdb-react-ui-kit";
import React from "react";
import { Link } from 'react-router-dom'
import { useState, useEffect } from "react";
import userAPI from '../../api/userAPI';
import orderHistoryAPI from '../../api/orderHistoryAPI'
import OrderItem from "./component/orderItem";

function OrderListTutor() {

    const [user, setUser] = useState({});
    const [orders, setOrders] = useState([]);
    const [tutorId, setTutorId] = useState('')

    useEffect(() => {
        const fetchUser = async () => {
            try {
                const token = localStorage.getItem('token');
                const email = localStorage.getItem('email');
                const user = await userAPI.getUserByEmail(email);
                setUser(user.data);
                const orderList = await orderHistoryAPI.getOrderListByTutorId(user.data.id, token);
                console.log(orderList.data.response.items.$values);
                setOrders(orderList.data.response.items.$values);
                setTutorId(user.data.id);
            } catch (error) {
                console.error("Error fetching user:", error);
            }
        };
        fetchUser();

    }, [])

    return (
        <>
            <section
                className="h-100 gradient-custom"
                style={{ backgroundColor: "white" }}
            >
                <MDBContainer className="py-10 h-100">
                    <MDBRow className="justify-content-center align-items-center h-100">
                        <MDBCol lg="12" xl="12">
                            <MDBCard style={{ borderRadius: "10px" }}>
                                <MDBCardHeader className="px-4 py-5">
                                    <MDBTypography tag="h5" className="text-muted mb-0">
                                        <span style={{ color: "#a8729a" }}><Link as={Link} to={"/user-profile"}>{user.firstName} {user.lastName}</Link></span>
                                    </MDBTypography>
                                </MDBCardHeader>
                                <MDBCardBody className="p-4">
                                    <div className="d-flex justify-content-between align-items-center mb-4">
                                        <p
                                            className="lead fw-normal mb-0"
                                            style={{ color: "#a8729a" }}
                                        >
                                            All of your order
                                        </p>
                                        <p className="small text-muted mb-0">
                                            quantity : {orders.length}
                                        </p>
                                    </div>

                                    <MDBCard className="shadow-0 border mb-4">
                                        <MDBCardBody>
                                            <MDBRow>
                                                <MDBCol
                                                    md="2"
                                                    className="text-center d-flex justify-content-center align-items-center"
                                                >
                                                    <p className="text-muted mb-0">#id</p>
                                                </MDBCol>
                                                <MDBCol
                                                    md="2"
                                                    className="text-center d-flex justify-content-center align-items-center"
                                                >
                                                    <p className="text-muted mb-0">Full name</p>
                                                </MDBCol>
                                                <MDBCol
                                                    md="2"
                                                    className="text-center d-flex justify-content-center align-items-center"
                                                >
                                                    <p className="text-muted mb-0">Status</p>
                                                </MDBCol>
                                                <MDBCol
                                                    md="2"
                                                    className="text-center d-flex justify-content-center align-items-center"
                                                >
                                                    <p className="text-muted mb-0 small">
                                                        Date
                                                    </p>
                                                </MDBCol>
                                                <MDBCol
                                                    md="2"
                                                    className="text-center d-flex justify-content-center align-items-center"
                                                >
                                                    <strong className="text-muted mb-0 small">Order status</strong>
                                                </MDBCol>
                                                <MDBCol
                                                    md="2"
                                                    className="text-center d-flex justify-content-center align-items-center"
                                                >
                                                    <p className="text-muted mb-0 small">Total amount</p>
                                                </MDBCol>
                                            </MDBRow>
                                        </MDBCardBody>
                                    </MDBCard>

                                    {orders.map((orderItem) => {

                                        return <OrderItem
                                            orderItem={orderItem}
                                            id={tutorId}
                                            key={orderItem.$id}
                                        />
                                    })}

                                    <div className="d-flex justify-content-between pt-2">
                                        <p className="fw-bold mb-0">Order Details</p>
                                        <p className="text-muted mb-0">
                                            <span className="fw-bold me-4">Total</span> $898.00
                                        </p>
                                    </div>

                                    <div className="d-flex justify-content-between pt-2">
                                        <p className="text-muted mb-0">Invoice Number : 788152</p>
                                        <p className="text-muted mb-0">
                                            <span className="fw-bold me-4">Discount</span> $19.00
                                        </p>
                                    </div>

                                    <div className="d-flex justify-content-between">
                                        <p className="text-muted mb-0">
                                            Invoice Date : 22 Dec,2019
                                        </p>
                                        <p className="text-muted mb-0">
                                            <span className="fw-bold me-4">GST 18%</span> 123
                                        </p>
                                    </div>

                                    <div className="d-flex justify-content-between mb-5">
                                        <p className="text-muted mb-0">
                                            Recepits Voucher : 18KU-62IIK
                                        </p>
                                        <p className="text-muted mb-0">
                                            <span className="fw-bold me-4">Delivery Charges</span>{" "}
                                            Free
                                        </p>
                                    </div>
                                </MDBCardBody>
                                <MDBCardFooter
                                    className="border-0 px-4 py-5"
                                    style={{
                                        backgroundColor: "#a8729a",
                                        borderBottomLeftRadius: "10px",
                                        borderBottomRightRadius: "10px",
                                    }}
                                >
                                    <MDBTypography
                                        tag="h5"
                                        className="d-flex align-items-center justify-content-end text-white text-uppercase mb-0"
                                    >
                                        Total paid: <span className="h2 mb-0 ms-2">$1040</span>
                                    </MDBTypography>
                                </MDBCardFooter>
                            </MDBCard>
                        </MDBCol>
                    </MDBRow>
                </MDBContainer>
            </section>
        </>
    );
}

export default OrderListTutor;