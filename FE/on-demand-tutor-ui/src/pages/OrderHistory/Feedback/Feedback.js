import { useEffect, useState } from 'react';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import orderHistoryAPI from '../../../api/orderHistoryAPI';
// import { deleteUpdateCourse } from '../api/CourseService';
import reviewRatingAPI from '../../../api/ReviewRatingAPI';
import curriculumAPI from '../../../api/curriculumAPI';
import Form from 'react-bootstrap/Form';

function Feedback({ order, learnerId }) {



    const [show, setShow] = useState(false);
    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);
    const [rating, setRating] = useState(5);
    const [review, setReview] = useState("");

    const handleFeedback = async () => {
        try {
            const token = localStorage.getItem('token');
            const tutor = await curriculumAPI.getCurriculumByOrderId(order.orderId)
            const formData = new FormData();
            formData.append('Rating', rating);
            formData.append('Review', review);
            formData.forEach((value, key) => {
                console.log(`${key}: ${value}`);
            });
            handleClose();
            const feedbackRes = await reviewRatingAPI.postReview(order.orderId, tutor.data.curriculum.tutorId, learnerId, formData, token);
        } catch (error) {
            console.log("error when cancel order", error);
        }



    }

    return (
        <>
            <Button variant="primary" size="sm" style={{ marginLeft: '10px' }} onClick={handleShow}>
                Feedback
            </Button>

            <Modal
                show={show}
                onHide={handleClose}
                backdrop="static"
                keyboard={false}
            >
                <Modal.Header closeButton>
                    <Modal.Title>Send your feedback</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <div className='body-add-new'>
                        <Form.Group>
                            <Form.Label>Rating</Form.Label>
                            <Form.Control
                                as="select"
                                value={rating}
                                onChange={(event) => setRating(event.target.value)}
                            >
                                {[...Array(5)].map((_, i) => (
                                    <option key={i} value={i + 1}>{i + 1}</option>
                                ))}
                            </Form.Control>
                        </Form.Group>
                        <Form.Group>
                            <Form.Label>Review</Form.Label>
                            <Form.Control
                                type='text'
                                placeholder='Your feedback'
                                value={review}
                                onChange={(event) => setReview(event.target.value)}
                            />
                        </Form.Group>
                    </div>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>
                        Close
                    </Button>
                    <Button variant="primary" onClick={handleFeedback}>Send feedback</Button>
                </Modal.Footer>
            </Modal>
        </>
    );
}

export default Feedback;
