import * as React from 'react';
import { ScheduleMeeting } from "react-schedule-meeting";
import { useState, useEffect } from 'react';
import sectionAPI from '../../api/sectionAPI';
import userAPI from '../../api/userAPI';
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import { format } from 'date-fns';
import tutorAPI from '../../api/tutorAPI';

function PersonalSchedule() {

    const styles = {
        ".jDIjzZ": {
            boxShadow: "none !important"
        }
    };

    const formatDate = (date) => {
        const day = String(date.getDate()).padStart(2, '0');
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const year = date.getFullYear();
        const hours = String(date.getUTCHours()).padStart(2, '0');
        const minutes = String(date.getUTCMinutes()).padStart(2, '0');
        const seconds = String(date.getUTCSeconds()).padStart(2, '0');
        return `${year}-${month}-${day}T${hours}%3A${minutes}%3A${seconds}Z`;
    };

    const formatSection = (date) => {
        const day = String(date.getDate()).padStart(2, '0');
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const year = date.getFullYear();
        const hours = String(date.getUTCHours()).padStart(2, '0');
        const minutes = String(date.getUTCMinutes()).padStart(2, '0');
        const seconds = String(date.getUTCSeconds()).padStart(2, '0');
        return `${year}-${month}-${day}T${hours}:${minutes}:${seconds}Z`;
    };

    // const formatSlot = (date) => {
    //     const day = String(date.getDate()).padStart(2, '0');
    //     const month = String(date.getMonth() + 1).padStart(2, '0');
    //     const year = date.getFullYear();
    //     const hours = String(date.getUTCHours()).padStart(2, '0');
    //     const minutes = String(date.getUTCMinutes()).padStart(2, '0');
    //     const seconds = String(date.getUTCSeconds()).padStart(2, '0');
    //     return `${year}-${month}-${day} ${hours}:${minutes}:${seconds}`;
    // };

    const startDate = new Date(Date.now());
    const endDate = new Date(startDate);

    startDate.setDate(startDate.getDate() - 7);
    endDate.setFullYear(endDate.getFullYear() + 1);

    const formattedStartDate = formatDate(startDate)
    const formattedEndDate = formatDate(endDate)


    const getSectionFromResponse = (sectionList) => {
        let sections = [];

        for (let i = 0; i < sectionList.length; i++) {
            const section = sectionList[i];
            let tmpArray = section.sections.$values;
            tmpArray.map((item) => {
                const startTime = new Date(item.sectionStart);
                startTime.setMinutes(startTime.getMinutes() + 59);
                const endTime = new Date(item.sectionEnd);
                endTime.setMinutes(endTime.getMinutes() + 59);
                const duration = 50;
                sections.push({
                    startTime: new Date(formatSection(startTime)),
                    endTime: new Date(formatSection(endTime)),
                    duration: duration,
                    meetUrl: item.meetUrl,
                    tutorId: item.tutorId
                });
            })
        }
        return sections;
    }

    const [sections, setSections] = useState([]);

    const [show, setShow] = useState(false);
    const handleClose = () => setShow(false);

    const [timeStart, setTimeStart] = useState('');
    const [timeEnd, setTimeEnd] = useState('');
    const [meetUrl, setMeetUrl] = useState();
    const [duration, setDuration] = useState();
    const [tutorName, setTutorName] = useState();

    const handleTimeSelect = async (timeSlot) => {
        setShow(true);

        const startTime = new Date(timeSlot.startTime);
        startTime.setMinutes(startTime.getMinutes() - 59);
        const endTime = new Date(timeSlot.availableTimeslot.endTime);
        endTime.setMinutes(endTime.getMinutes() - 59);

        setTimeStart(format(startTime, 'yyyy-MM-dd HH:mm:ss'))
        setTimeEnd(format(endTime, 'yyyy-MM-dd HH:mm:ss'))

        setMeetUrl(timeSlot.availableTimeslot.meetUrl)
        setDuration(timeSlot.availableTimeslot.duration)
        try {
            const tutorName = await tutorAPI.get(timeSlot.availableTimeslot.tutorId)
            setTutorName(tutorName.data.tutorNavigation.firstName + ' ' + tutorName.data.tutorNavigation.lastName)
        } catch (e) {
            console.log(e);
        }
    };


    useEffect(() => {
        const token = localStorage.getItem('token');
        const email = localStorage.getItem('email');

        const fetchSection = async () => {
            try {
                const user = await userAPI.getUserByEmail(email);
                const sectionsDays = await sectionAPI.getLearnerSection(user.data.id, formattedStartDate, formattedEndDate, token);
                const tmp = getSectionFromResponse(sectionsDays.data.schedule.$values)
                setSections(tmp)
            } catch (error) {
                console.error("Error fetching tutors:", error);
            }
        };
        fetchSection();

    }, []);

    return (
        <div style={{ marginTop: '5em' }}>
            <h1>Your Schedule</h1>
            <ScheduleMeeting
                borderRadius={10}
                scheduleMeetingStyles={styles}
                primaryColor="#3f5b85"
                eventDurationInMinutes={50}
                availableTimeslots={sections}
                onStartTimeSelect={handleTimeSelect}
                format_startTimeFormatString="h:00 a"
            />
            <Modal
                show={show}
                onHide={handleClose}
                backdrop="static"
                keyboard={false}
            >
                <Modal.Header closeButton>
                    <Modal.Title>Section</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <div className='body-add-new'>
                        <div className='text'>Start time: {timeStart}</div>
                        <div className='text'>End time: {timeEnd}</div>
                        <div className='text'>Meet URL: {meetUrl}</div>
                        <div className='text'>Duration: {duration}</div>
                        <div className='text'>Tutor: {tutorName}</div>
                    </div>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>
                        Close
                    </Button>
                </Modal.Footer>
            </Modal>
        </div>
    );
}

export default PersonalSchedule;