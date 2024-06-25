import * as React from 'react';
import { ScheduleMeeting } from "react-schedule-meeting";


function PersonalSchedule() {

    const availableTimeslots = [0, 1, 2, 3, 4, 5].map((id) => {
        return {
            id,
            startTime: new Date(
                new Date(new Date().setDate(new Date().getDate() + id)).setHours(
                    9,
                    0,
                    0,
                    0
                )
            ),
            endTime: new Date(
                new Date(new Date().setDate(new Date().getDate() + id)).setHours(
                    17,
                    0,
                    0,
                    0
                )
            )
        };
    });
    const styles = {
        ".jDIjzZ": {
            boxShadow: "none !important"
        }
    };

    return (
        <div className="App">
            <ScheduleMeeting
                borderRadius={10}
                scheduleMeetingStyles={styles}
                primaryColor="#3f5b85"
                eventDurationInMinutes={30}
                availableTimeslots={availableTimeslots}
                onStartTimeSelect={console.log}
            />
        </div>
    );
}

export default PersonalSchedule;