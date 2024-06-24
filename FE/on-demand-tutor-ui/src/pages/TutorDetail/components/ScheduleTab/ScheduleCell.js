import { useEffect, useState } from "react";
import {
    isSameDay,
    set
} from "date-fns";
import './ScheduleTab.scss';
import { useNavigate } from 'react-router-dom'
import sectionAPI from "../../../../api/sectionAPI";



function ScheduleCell({ tutorId, day, selectedDate, formattedDate }) {

    const Jtoken = localStorage.getItem('token');
    console.log(Jtoken);
    const navigate = useNavigate();

    const handleOnClick = (sectionFree) => {
        if (Jtoken) {
            const course = 'Basic English Language Course';
            const time = sectionFree.sectionStart + 'Z';
            console.log(sectionFree.sectionStart);
            const searchParams = new URLSearchParams();
            searchParams.set('course', course);
            searchParams.set('time', time);
            navigate("/payment?tutorId=" + tutorId + '&' + searchParams.toString())
        } else {
            navigate("/login")
        }
    }

    const originalDate = new Date(day);

    const newDate = new Date(originalDate);
    newDate.setDate(newDate.getDate() + 1);

    const formatDate = (date) => {
        const day = date.getDate();
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
                const sectionsDay = await sectionAPI.getTutorSection(tutorId, formattedOriginalDate, formattedNewDate);
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
            className={`col cell ${isSameDay(day, new Date())
                ? "today"
                : isSameDay(day, selectedDate)
                    ? "selected"
                    : ""
                }`}
            key={day}
        >
            <div className="number">{formattedDate}</div>
            <div className="sections-day">
                {free.map((sectionFree) => {
                    if (!sections.some(section => section.sectionStart === sectionFree.sectionStart)) {
                        return (
                            <div key={free.$id} className="sectionFree" onClick={() => handleOnClick(sectionFree)}>{formatSection(new Date(sectionFree.sectionStart))}</div>
                        );
                    } else {
                        return (
                            <div key={free.$id} className="sectionBooked" disable>{formatSection(new Date(sectionFree.sectionStart))}</div>
                        );
                    }
                })}

                {/* <div className="section-day">Hello</div>
                <div className="section-day">Hello</div> */}
            </div>

        </div>
    );
}

export default ScheduleCell;