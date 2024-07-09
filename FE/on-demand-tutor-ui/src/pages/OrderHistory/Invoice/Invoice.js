import { useEffect, useState } from 'react';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import './invoice.scss'
// import { deleteUpdateCourse } from '../api/CourseService';

function Invoice({ order }) {


    const token = localStorage.getItem('token');
    const [show, setShow] = useState(false);
    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);
    console.log("ORDER", order)

    const formatDate = (date) => {
        const day = date.getDate();
        const monp = date.getMonth() + 1;
        const year = date.getFullYear();
        return `${year}-0${monp}-${day}`;
    };

    //const newDate = new Date(originalDate);
    const formattedOrderDate = formatDate(new Date(order.orderDate));



    return (
        <>
            <Button variant="secondary" size="sm" onClick={handleShow}>
                View Invoice
            </Button>

            <Modal
                show={show}
                onHide={handleClose}
                backdrop="static"
                keyboard={false}
            >
                <Modal.Header closeButton>
                    <Modal.Title>Your Invoice</Modal.Title>
                </Modal.Header>
                <Modal.Body className='invoice-container'>
                    <div>
                        <p className='p1'>Order ID:</p><p className='p2'>{order.orderId}</p>
                    </div>
                    <div>
                        <p className='p1'>Order Date:</p><p className='p2'>{formattedOrderDate}</p>
                    </div>
                    <div>
                        <p className='p1'>Order Status:</p><p className='p2'>{order.orderStatus}</p>
                    </div>
                    <div>
                        <p className='p1'>Total:</p><p className='p2'>₫{order.total}</p>
                    </div>
                    <div>
                        <p className='p1'>Curriculum ID:</p><p className='p2'>{order.curriculumId}</p>
                    </div>

                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>
                        Close
                    </Button>
                </Modal.Footer>
            </Modal>
        </>
    );
}

export default Invoice;
