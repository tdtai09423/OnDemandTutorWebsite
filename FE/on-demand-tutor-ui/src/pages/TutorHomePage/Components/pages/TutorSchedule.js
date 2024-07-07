import * as React from 'react';
import { ScheduleMeeting } from "react-schedule-meeting";
import { useState, useEffect } from 'react';
import sectionAPI from '../../../../api/sectionAPI';
import userAPI from '../../../../api/userAPI';
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import { format } from 'date-fns';
import orderHistoryAPI from '../../../../api/orderHistoryAPI';

function TutorSchedule() {
  // const styles = {
  //   ".jDIjzZ": {
  //     boxShadow: "none !important"
  //   }
  // };

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

  const formatSlot = (date) => {
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();
    const hours = String(date.getUTCHours()).padStart(2, '0');
    const minutes = String(date.getUTCMinutes()).padStart(2, '0');
    const seconds = String(date.getUTCSeconds()).padStart(2, '0');
    return `${year}-${month}-${day} ${hours}:${minutes}:${seconds}`;
  };

  const startDate = new Date(Date.now());
  const endDate = new Date(startDate);

  startDate.setDate(startDate.getDate() - 7);
  console.log(startDate)
  endDate.setFullYear(endDate.getFullYear() + 1);

  const formattedStartDate = formatDate(startDate);
  const formattedEndDate = formatDate(endDate);

  const getSectionFromResponse = (sectionList) => {
    let sections = [];

    for (let i = 0; i < sectionList.length; i++) {
      const section = sectionList[i];
      let tmpArray = section.sections.$values;
      tmpArray.map((item) => {
        const startTime = new Date(item.sectionStart);
        const endTime = new Date(item.sectionEnd);
        const duration = 50;
        sections.push({
          startTime: new Date(formatSection(startTime)),
          endTime: new Date(formatSection(endTime)),
          duration: duration,
          meetUrl: item.meetUrl,
          id: item.id,
          status: item.sectionStatus
        });
      });
    }
    return sections;
  };

  const [sections, setSections] = useState([]);

  const [show, setShow] = useState(false);
  const handleClose = () => {
    setShow(false);
    setShowCannotCompleteMessage(false); // Reset the message visibility on modal close
  };

  const [timeStart, setTimeStart] = useState('');
  const [timeEnd, setTimeEnd] = useState('');
  const [meetUrl, setMeetUrl] = useState();
  const [duration, setDuration] = useState();
  const [status, setStatus] = useState();
  const [sectionId, setSectionId] = useState();
  const [orderId, setOrderId] = useState();
  const [canComplete, setCanComplete] = useState(false);
  const [showCannotCompleteMessage, setShowCannotCompleteMessage] = useState(false);

  const handleTimeSelect = async (timeSlot) => {
    setShow(true);
    setTimeStart(format(new Date(timeSlot.startTime), 'yyyy-MM-dd HH:mm:ss'));
    setTimeEnd(format(new Date(timeSlot.availableTimeslot.endTime), 'yyyy-MM-dd HH:mm:ss'));
    setMeetUrl(timeSlot.availableTimeslot.meetUrl);
    setDuration(timeSlot.availableTimeslot.duration);
    setSectionId(timeSlot.availableTimeslot.id);
    setStatus(timeSlot.availableTimeslot.status);
    const now = new Date();
    const current = new Date(timeSlot.startTime);
    console.log('>>>>current', now, current);
    if (current < now) {
      setCanComplete(true);
    } else {
      setCanComplete(false);
    }

    try {
      const orderBySectionRes = await sectionAPI.getOrderBySection(timeSlot.availableTimeslot.id);
      setOrderId(orderBySectionRes.data.orderId);
    } catch (e) {
      console.log('error>>>', e);
    }
  };

  const handleComplete = async () => {
    if (canComplete) {
      let token = localStorage.getItem('token');
      const confirmRes = await orderHistoryAPI.postConfirmCompleteSection(sectionId, orderId, token);
      console.log('confirmRes');
    } else {
      console.log('cannot complete');
      setShowCannotCompleteMessage(true);
    }
  };


  const scheduleMeetingStyles = {
    timeslot: (timeslot) => {
      const today = new Date().toISOString().split('T')[0];
      console.log(timeslot)
      //const timeslotDate = timeslot.startTime.split('T')[0];
      // if (timeslotDate === today) {
      //   return {
      //     backgroundColor: 'black', // Example style for today's timeslot
      //     borderColor: '#f57c00',
      //   };
      // }
      // return {};
    },
  };

  useEffect(() => {
    const email = localStorage.getItem('email');

    const fetchSection = async () => {
      try {
        let token = localStorage.getItem('token');
        const user = await userAPI.getUserByEmail(email);
        const sectionsDays = await sectionAPI.getTutorSection(user.data.id, formattedStartDate, formattedEndDate, token);
        const tmp = getSectionFromResponse(sectionsDays.data.$values);
        setSections(tmp);
      } catch (error) {
        console.error("Error fetching tutors:", error);
      }
    };
    fetchSection();
  }, []);

  return (
    <div style={{ marginTop: '0.5em' }}>
      <h1>Your Schedule</h1>
      <ScheduleMeeting
        borderRadius={10}
        scheduleMeetingStyles={scheduleMeetingStyles}
        primaryColor="#3f5b85"
        eventDurationInMinutes={50}
        availableTimeslots={sections}
        onStartTimeSelect={handleTimeSelect}
        defaultDate={startDate}
      />
      <Modal
        show={show}
        onHide={handleClose}
        backdrop="static"
        keyboard={false}
        style={{ marginTop: '90px' }}
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
            <div className='text'>Status: {status}</div>
            {showCannotCompleteMessage && (
              <div className='text' style={{ color: 'red' }}>
                This section cannot be confirmed as complete
              </div>
            )}
          </div>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={handleClose}>
            Close
          </Button>
          <Button variant="primary" onClick={handleComplete}>
            Completed
          </Button>
        </Modal.Footer>
      </Modal>
    </div>
  );
}

export default TutorSchedule;
