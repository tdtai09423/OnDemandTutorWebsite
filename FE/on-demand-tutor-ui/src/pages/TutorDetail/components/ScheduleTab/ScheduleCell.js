import { useEffect, useState } from "react";
import {
    isSameDay
} from "date-fns";
import './ScheduleTab.scss';
import { Row, Col } from "react-bootstrap";
import { ArrowLeftShort, ArrowRightShort } from 'react-bootstrap-icons'
import sectionAPI from "../../../../api/sectionAPI";



function ScheduleCell({ tutorId, day, selectedDate, formattedDate }) {

    // useEffect(() => {
    //     const fetchSection = async () => {
    //         try {
    //             const sectionsDay = await sectionAPI.getTutorSection(tutorId);
    //         } catch (error) {
    //             console.error("Error fetching tutors:", error);
    //         }
    //     };
    //     fetchSection();
    // }, []);
    // Tạo đối tượng Date từ chuỗi ngày tháng của bạn
    const originalDate = new Date(day);

    // Lùi thời gian đi 24 tiếng
    const newDate = new Date(originalDate);
    newDate.setHours(newDate.getHours() + 24);

    // Xuất ra console để kiểm tra kết quả
    console.log('Original Date:', originalDate);
    console.log('New Date:', newDate);

    console.log(day)
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
                <div className="section-day">Hello</div>
                <div className="section-day">Hello</div>
            </div>

        </div>
    );
}

export default ScheduleCell;