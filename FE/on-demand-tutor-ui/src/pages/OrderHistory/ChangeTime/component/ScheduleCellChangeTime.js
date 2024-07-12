import { useEffect, useState } from "react";
import {
    isSameDay,
    set
} from "date-fns";
import '../../../TutorDetail/components/ScheduleTab/ScheduleTab.scss';
import sectionAPI from "../../../../api/sectionAPI";
import orderHistoryAPI from "../../../../api/orderHistoryAPI";



function ScheduleCellChangeTime({ curriculum, day, selectedDate, formattedDate, orderId }) {

    const Jtoken = localStorage.getItem('token');

    const handleOnClick = async (sectionFree) => {
        console.log(orderId);
        console.log(sectionFree.sectionStart);
        console.log(Jtoken);
        const confirmed = window.confirm('Are you sure to change order time ?');
        if (confirmed) {
            try {
                await orderHistoryAPI.putTimeOrder(orderId, sectionFree.sectionStart, Jtoken);
            } catch (error) {
                console.error('Error updating data:', error);
                alert('Đã xảy ra lỗi');
            }
            window.location.reload();
        }
    }

    const originalDate = new Date(day);

    const newDate = new Date(originalDate);
    newDate.setDate(newDate.getDate() + 1);

    const formatDate = (date) => {
        const day = date.getDate().toString().padStart(2, '0');
        const month = date.getMonth() + 1;
        const year = date.getFullYear();
        return `${year}-0${month}-${day}`;
    };

    const formatSection = (date) => {
        const hours = date.getHours().toString().padStart(2, '0');
        const minutes = date.getMinutes().toString().padStart(2, '0');
        return `${hours}:${minutes}`;
    };

    const formattedOriginalDate = formatDate(originalDate);
    const formattedNewDate = formatDate(newDate);

    const [sections, setSection] = useState([])
    const [free, setFree] = useState([{
        $id: '1',
        sectionStart: formattedOriginalDate + "T07:00:00"
    }, {
        $id: '2',
        sectionStart: formattedOriginalDate + "T08:00:00"
    }, {
        $id: '3',
        sectionStart: formattedOriginalDate + "T09:00:00"
    }, {
        $id: '4',
        sectionStart: formattedOriginalDate + "T10:00:00"
    }, {
        $id: '5',
        sectionStart: formattedOriginalDate + "T11:00:00"
    }, {
        $id: '6',
        sectionStart: formattedOriginalDate + "T12:00:00"
    }, {
        $id: '7',
        sectionStart: formattedOriginalDate + "T13:00:00"
    }, {
        $id: '8',
        sectionStart: formattedOriginalDate + "T14:00:00"
    }, {
        $id: '9',
        sectionStart: formattedOriginalDate + "T15:00:00"
    }, {
        $id: '10',
        sectionStart: formattedOriginalDate + "T16:00:00"
    }, {
        $id: '11',
        sectionStart: formattedOriginalDate + "T17:00:00"
    }, {
        $id: '12',
        sectionStart: formattedOriginalDate + "T18:00:00"
    }, {
        $id: '13',
        sectionStart: formattedOriginalDate + "T19:00:00"
    }, {
        $id: '14',
        sectionStart: formattedOriginalDate + "T20:00:00"
    }, {
        $id: '15',
        sectionStart: formattedOriginalDate + "T21:00:00"
    }, {
        $id: '16',
        sectionStart: formattedOriginalDate + "T22:00:00"
    }, {
        $id: '17',
        sectionStart: formattedOriginalDate + "T23:00:00"
    }])

    useEffect(() => {
        const fetchSection = async () => {
            try {
                const sectionsDay = await sectionAPI.getTutorSection(curriculum.tutorId, formattedOriginalDate, formattedNewDate);
                if (sectionsDay.data.$values[0]) {
                    setSection(sectionsDay.data.$values[0].sections.$values);
                }

            } catch (error) {
                console.error("Error fetching tutors:", error);
            }
        };
        fetchSection();
    }, []);

    return (
        <div
            className={`col cell `}
            key={day}
        >
            <div className="number">{formattedDate}</div>
            <div className="sections-day" style={{ marginTop: '10px', marginBottom: '1.5em', borderLeft: '1px solid black', borderRight: '1px solid black' }}>
                {free.map((sectionFree) => {
                    if (!sections.some(section => section.sectionStart === sectionFree.sectionStart)) {
                        return (
                            <div key={sectionFree.$id} className="sectionFree" onClick={() => handleOnClick(sectionFree)}>{formatSection(new Date(sectionFree.sectionStart))}</div>
                        );
                    } else {
                        return (
                            <div key={sectionFree.$id} className="sectionBooked" disable>{formatSection(new Date(sectionFree.sectionStart))}</div>
                        );
                    }
                })}

                {/* <div className="section-day">Hello</div>
                <div className="section-day">Hello</div> */}
            </div>

        </div>
    );
}

export default ScheduleCellChangeTime;