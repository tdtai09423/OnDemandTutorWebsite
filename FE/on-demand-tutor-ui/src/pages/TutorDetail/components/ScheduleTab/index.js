import { useEffect, useState, searchParam } from "react";
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
import './ScheduleTab.scss';
import { Row, Col } from "react-bootstrap";
import { ArrowLeftShort, ArrowRightShort } from 'react-bootstrap-icons'
import sectionAPI from "../../../../api/sectionAPI";
import ScheduleCell from "./ScheduleCell";

function Calendar({ tutorId, subject }) {
    const [currentMonth, setCurrentMonth] = useState(new Date());
    const [currentWeek, setCurrentWeek] = useState(getWeek(currentMonth));
    const [selectedDate, setSelectedDate] = useState(new Date());

    useEffect(() => {
        console.log(tutorId, subject);
    }, []);

    const changeWeekHandle = (btnType) => {
        //console.log("current week", currentWeek);
        if (btnType === "prev") {
            //console.log(subWeeks(currentMonth, 1));
            setCurrentMonth(subWeeks(currentMonth, 1));
            setCurrentWeek(getWeek(subWeeks(currentMonth, 1)));
        }
        if (btnType === "next") {
            //console.log(addWeeks(currentMonth, 1));
            setCurrentMonth(addWeeks(currentMonth, 1));
            setCurrentWeek(getWeek(addWeeks(currentMonth, 1)));
        }
    };

    const renderHeader = () => {
        const dateFormat = "MMM yyyy";
        // console.log("selected day", selectedDate);
        return (
            <div className="header row flex-middle">
                <div className="col col-start">

                </div>
                <div className="col col-center">
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
                <div className="col col-center" key={i}>
                    {format(addDays(startDate, i), dateFormat)}
                </div>
            );
        }
        return <div className="days row">{days}</div>;
    };

    const fetchSections = async (tutorId, start, end) => {
        try {
            const sections = await sectionAPI.get(tutorId, start, end);

        } catch (error) {
            console.error("Error fetching tutors:", error);
        }
    };

    const renderCells = () => {
        const startDate = startOfWeek(currentMonth, { weekStartsOn: 1 });
        const endDate = lastDayOfWeek(currentMonth, { weekStartsOn: 1 });
        const dateFormat = "d";
        const rows = [];
        let days = [];
        let day = startDate;
        let formattedDate = "";
        while (day <= endDate) {
            for (let i = 0; i < 7; i++) {

                formattedDate = format(day, dateFormat);
                days.push(
                    <ScheduleCell
                        tutorId={tutorId}
                        day={day}
                        selectedDate={selectedDate}
                        formattedDate={formattedDate}
                        subject={subject}
                    />
                );
                day = addDays(day, 1);
            }

            rows.push(
                <div className="row" key={day}>
                    {days}
                </div>
            );
            days = [];
        }
        return <div className="body">{rows}</div>;
    };
    const renderFooter = () => {
        return (
            <Row className="footer row">
                <Col md={3} className="icon-wrapper prev">
                    <button className="icon" onClick={() => changeWeekHandle("prev")}>
                        <ArrowLeftShort style={{ fontSize: '30px' }}></ArrowLeftShort>prev week
                    </button>
                </Col>
                <Col md={6} className="current">Current week: {currentWeek}</Col>
                <Col md={3} className="icon-wrapper next" onClick={() => changeWeekHandle("next")}>
                    <button className="icon">next week<ArrowRightShort style={{ fontSize: '30px' }}></ArrowRightShort></button>
                </Col>
            </Row>
        );
    };
    return (
        <div className="calendar">
            {renderHeader()}
            {renderDays()}
            {renderCells()}
            {renderFooter()}
        </div>
    );
};

export default Calendar;
/**
 * Header:
 * icon for switching to the previous month,
 * formatted date showing current month and year,
 * another icon for switching to next month
 * icons should also handle onClick events to change a month
 */
