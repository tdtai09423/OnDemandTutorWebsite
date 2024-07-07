import { useEffect, useState } from 'react';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import orderHistoryAPI from '../../../api/orderHistoryAPI';
// import { deleteUpdateCourse } from '../api/CourseService';
import {
    format,
    subMonths,
    addMonths,
    startOfWeek,
    addDays,
    isSameDay,
    lastDayOfWeek,
    getWeek,
    addWeeks,
    subWeeks
} from "date-fns";
import { Row, Col } from "react-bootstrap";
import { ArrowLeftShort, ArrowRightShort } from 'react-bootstrap-icons'
import curriculumAPI from '../../../api/curriculumAPI.js';
import ScheduleCellChangeTime from './component/ScheduleCellChangeTime.js';
import './change-time.scss'


function ChangeTime({ order }) {

    const [currentMonth, setCurrentMonth] = useState(new Date());
    const [currentWeek, setCurrentWeek] = useState(getWeek(currentMonth));
    const [selectedDate, setSelectedDate] = useState(new Date());
    const [curriculum, setCurriculum] = useState()

    const token = localStorage.getItem('token');
    const [show, setShow] = useState(false);
    const handleClose = () => setShow(false);
    const handleShow = () => {
        setShow(true);
    };
    const [selectDate, setSelectDate] = useState('');
    const [selectedHour, setSelectHour] = useState('');

    const renderHeader = () => {
        const dateFormat = "MMM yyyy";
        return (
            <div className="header row flex-middle" style={{ marginBottom: '10px' }}>
                <div className="col col-start">
                </div>
                <div className="col col-center text-center">
                    <span>{format(currentMonth, dateFormat)}</span>
                </div>
                <div className="col col-end">
                    {/* <div className="icon" onClick={() => changeMonthHandle("next")}>next month</div> */}
                </div>
            </div>
        );
    };
    const renderDays = () => {
        const dateFormat = "EEE";
        const days = [];
        let startDate = startOfWeek(currentMonth, { weekStartsOn: 1 });
        for (let i = 0; i < 7; i++) {
            days.push(
                <div className="col col-center text-center" key={i}>
                    {format(addDays(startDate, i), dateFormat)}
                </div>
            );
        }
        return <div className="days row">{days}</div>;
    };
    const renderFooter = () => {
        return (
            <Row className="footer row">
                <Col md={3} className="icon-wrapper prev">
                    <button className="icon px-3" onClick={() => changeWeekHandle("prev")}>
                        <ArrowLeftShort style={{ fontSize: '30px' }}></ArrowLeftShort>prev week
                    </button>
                </Col>
                <Col md={6} className="current text-center">Current week: {currentWeek}</Col>
                <Col md={3} className="icon-wrapper next d-flex justify-content-end" onClick={() => changeWeekHandle("next")}>
                    <button className="icon px-3">next week<ArrowRightShort style={{ fontSize: '30px' }}></ArrowRightShort></button>
                </Col>
            </Row>
        );
    };
    const renderCells = () => {
        const startDate = startOfWeek(currentMonth, { weekStartsOn: 1 });
        const endDate = lastDayOfWeek(currentMonth, { weekStartsOn: 1 });
        // const tutorId = curriculum.tutorId;
        // const subject = curriculum.curriculumDescription;
        const dateFormat = "d";
        const rows = [];
        let days = [];
        let day = startDate;
        let formattedDate = "";
        while (day <= endDate) {
            for (let i = 0; i < 7; i++) {

                formattedDate = format(day, dateFormat);
                days.push(
                    <ScheduleCellChangeTime
                        curriculum={curriculum}
                        day={day}
                        selectedDate={selectedDate}
                        formattedDate={formattedDate}
                        orderId={order.orderId}
                    />
                );
                day = addDays(day, 1);
            }

            rows.push(
                <div className="row text-center" key={day}>
                    {days}
                </div>
            );
            days = [];
        }
        return <div className="body">{rows}</div>;
    };

    const changeWeekHandle = (btnType) => {
        if (btnType === "prev") {
            setCurrentMonth(subWeeks(currentMonth, 1));
            setCurrentWeek(getWeek(subWeeks(currentMonth, 1)));
        }
        if (btnType === "next") {
            setCurrentMonth(addWeeks(currentMonth, 1));
            setCurrentWeek(getWeek(addWeeks(currentMonth, 1)));
        }
    };


    const handleUpdateOrder = async () => {
        try {

            //let res = orderHistoryAPI.putTimeOrder(order.orderId, time, token);
            handleClose();
            //console.log("check  res", res);
            ///window.location.reload();

        } catch (error) {
            console.log("error when cancel order", error);
        }
    }

    useEffect(() => {
        const fetchCurriculum = async () => {
            try {
                const curriculumRes = await curriculumAPI.getCurriculumByOrderId(order.orderId);
                console.log('>>>>', curriculumRes.data.curriculum);
                setCurriculum(curriculumRes.data.curriculum);
            } catch (error) {
                console.log("error when get curriculum", error);
            }
        }
        fetchCurriculum();
    }, []);

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
                className='custome-modal'
            >
                <Modal.Header closeButton>
                    <Modal.Title>Change Time</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <div className='body-add-new'>
                        <h4>Choose your expect time</h4>
                        {/* <div className='text'>Description</div>
                        <input type='text' placeholder='Course description' className='form-control' value={desc} onChange={(event) => setDesc(event.target.value)} />
                         */}
                    </div>
                    {renderHeader()}
                    {renderDays()}
                    {renderCells()}
                    {renderFooter()}
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

export default ChangeTime;
