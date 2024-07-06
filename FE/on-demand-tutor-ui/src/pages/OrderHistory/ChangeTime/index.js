import { useEffect, useState } from 'react';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import orderHistoryAPI from '../../../api/orderHistoryAPI';
// import { deleteUpdateCourse } from '../api/CourseService';
import ScheduleTab from '../../TutorDetail/components/ScheduleTab/index.js';

function ChangeTime({ order }) {


    const token = localStorage.getItem('token');
    const [show, setShow] = useState(false);
    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);
    const [selectDate, setSelectDate] = useState('');
    const [selectedHour, setSelectHour] = useState('');

    const handleInputChange = (event) => {
        setSelectDate(event.target.value);
    };

    const hours = [
        { name: '7', value: 7 },
        { name: '8', value: 8 },
        { name: '9', value: 9 },
        { name: '10', value: 10 },
        { name: '11', value: 11 },
        { name: '12', value: 12 },
        { name: '13', value: 13 },
        { name: '14', value: 14 },
        { name: '15', value: 15 },
        { name: '16', value: 16 },
        { name: '17', value: 17 },
        { name: '18', value: 18 },
        { name: '19', value: 19 },
        { name: '20', value: 20 },
        { name: '21', value: 21 },
        { name: '22', value: 22 },
        { name: '23', value: 23 },
    ];

    const handleUpdateOrder = async () => {
        try {
            const date = new Date(selectDate)
            const day = String(date.getDate()).padStart(2, '0');
            const month = String(date.getMonth() + 1).padStart(2, '0');
            const year = date.getFullYear();
            const hours = selectedHour.padStart(2, '0');
            const time = `${year}-${month}-${day}T${hours}:00:00.00Z`;
            console.log(">>>>", time)
            let res = orderHistoryAPI.putTimeOrder(order.orderId, time, token);
            handleClose();
            console.log("check  res", res);
            ///window.location.reload();

        } catch (error) {
            console.log("error when cancel order", error);
        }
    }

    return (
        <>
            <Button variant="warning" size="sm" onClick={handleShow} style={{ marginRight: '10px' }}>
                Change time
            </Button>

            <Modal
                show={show}
                onHide={handleClose}
                backdrop="static"
                keyboard={false}
            >
                <Modal.Header closeButton>
                    <Modal.Title>Change Time</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <div className='body-add-new'>
                        <h4>Input your expect time</h4>
                        {/* <div className='text'>Description</div>
                        <input type='text' placeholder='Course description' className='form-control' value={desc} onChange={(event) => setDesc(event.target.value)} />
                         */}
                    </div>
                    {/* <ScheduleTab
                        tutorId={tutorId}
                        subject={subject}
                    /> */}
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>
                        Close
                    </Button>
                    <Button variant="primary" onClick={() => handleUpdateOrder()}>Update Order</Button>
                </Modal.Footer>
            </Modal>
        </>
    );
}

export default ChangeTime;
