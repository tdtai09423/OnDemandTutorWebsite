import { Button } from 'react-bootstrap';
import { CaretRightFill, CaretLeftFill } from 'react-bootstrap-icons'

function ScheduleTab() {
    return (
        <div class="container">
            <div class="">
                <Button className="loginButton text-black border border-2 border-dark" variant="">
                    <CaretLeftFill className="schedule-icon"></CaretLeftFill>
                </Button>
                <Button className="loginButton text-black border border-2 border-dark" variant="" style={{ marginLeft: '5px' }}>
                    <CaretRightFill className="schedule-icon"></CaretRightFill>
                </Button>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="schedule-table">
                        <table class="table bg-white">
                            <thead>
                                <tr>
                                    <th>Monday</th>
                                    <th>Tuesday</th>
                                    <th>Wednesday</th>
                                    <th>Thursday</th>
                                    <th>Friday</th>
                                    <th>Saturday</th>
                                    <th class="last">Sunday</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td class="active">
                                        <div class="hover">
                                            <p>08:00</p>
                                        </div>
                                        <div class="hover">
                                            <p>10:00</p>
                                        </div>
                                    </td>
                                    <td class="active">
                                        <div class="hover">
                                            <p>08:00</p>
                                        </div>
                                        <div class="hover">
                                            <p>10:00</p>
                                        </div>
                                    </td>
                                    <td class="active">
                                        <div class="hover">
                                            <p>08:00</p>
                                        </div>
                                        <div class="hover">
                                            <p>10:00</p>
                                        </div>
                                    </td>
                                    <td class="active">
                                        <div class="hover">
                                            <p>08:00</p>
                                        </div>
                                        <div class="hover">
                                            <p>10:00</p>
                                        </div>
                                    </td>
                                    <td class="active">
                                        <div class="hover">
                                            <p>08:00</p>
                                        </div>
                                        <div class="hover">
                                            <p>10:00</p>
                                        </div>
                                    </td>
                                    <td class="active">
                                        <div class="hover">
                                            <p>08:00</p>
                                        </div>
                                        <div class="hover">
                                            <p>10:00</p>
                                        </div>
                                    </td>
                                    <td class="active">
                                        <div class="hover">
                                            <p>08:00</p>
                                        </div>
                                        <div class="hover">
                                            <p>10:00</p>
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default ScheduleTab;