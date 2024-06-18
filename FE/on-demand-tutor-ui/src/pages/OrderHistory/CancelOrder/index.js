import { useEffect, useState } from 'react';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
// import { deleteUpdateCourse } from '../api/CourseService';

function CancelOrder({ order }) {



    const [show, setShow] = useState(false);
    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);

    const handleCancelOrder = async () => {
        try {
            const confirmed = window.confirm('Are you sure to cancel this order?');
            // if (confirmed) {
            //     let res = await deleteUpdateCourse(order.OrderId);
            //     handleClose();
                
            //     console.log("check  res", res);
            // }
        } catch (error) {
            console.log("error when cancel order", error);
        }



    }

    return (
        <>
            <Button variant="danger" size="sm" onClick={handleShow}>
                Cancel Order
            </Button>

            <Modal
                show={show}
                onHide={handleClose}
                backdrop="static"
                keyboard={false}
            >
                <Modal.Header closeButton>
                    <Modal.Title>Cancel Order</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <div className='body-add-new'>
                        {/* <div className='text'>Description</div>
                        <input type='text' placeholder='Course description' className='form-control' value={desc} onChange={(event) => setDesc(event.target.value)} />
                         */}
                    </div>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>
                        Close
                    </Button>
                    <Button variant="primary" onClick={() => handleCancelOrder()}>Cancel Order</Button>
                </Modal.Footer>
            </Modal>
        </>
    );
}

export default CancelOrder;
