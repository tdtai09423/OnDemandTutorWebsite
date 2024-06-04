import { Button } from 'react-bootstrap';
import { CaretRightFill, CaretLeftFill } from 'react-bootstrap-icons'

function ScheduleTab() {
    return (
        <div className="container">
            <div className="">
                <Button className="loginButton text-black border border-2 border-dark" variant="">
                    <CaretLeftFill className="schedule-icon"></CaretLeftFill>
                </Button>
                <Button className="loginButton text-black border border-2 border-dark" variant="" style={{ marginLeft: '5px' }}>
                    <CaretRightFill className="schedule-icon"></CaretRightFill>
                </Button>
            </div>
            <div className="row">
                <div className="col-md-12">
                    <div className="schedule-table">
                        <table className="table bg-white">
                            <thead>
                                <tr>
                                    <th>Monday</th>
                                    <th>Tuesday</th>
                                    <th>Wednesday</th>
                                    <th>Thursday</th>
                                    <th>Friday</th>
                                    <th>Saturday</th>
                                    <th className="last">Sunday</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td className="active">
                                        <div className="hover">
                                            <p>08:00</p>
                                        </div>
                                        <div className="hover">
                                            <p>10:00</p>
                                        </div>
                                    </td>
                                    <td className="active">
                                        <div className="hover">
                                            <p>08:00</p>
                                        </div>
                                        <div className="hover">
                                            <p>10:00</p>
                                        </div>
                                    </td>
                                    <td className="active">
                                        <div className="hover">
                                            <p>08:00</p>
                                        </div>
                                        <div className="hover">
                                            <p>10:00</p>
                                        </div>
                                    </td>
                                    <td className="active">
                                        <div className="hover">
                                            <p>08:00</p>
                                        </div>
                                        <div className="hover">
                                            <p>10:00</p>
                                        </div>
                                    </td>
                                    <td className="active">
                                        <div className="hover">
                                            <p>08:00</p>
                                        </div>
                                        <div className="hover">
                                            <p>10:00</p>
                                        </div>
                                    </td>
                                    <td className="active">
                                        <div className="hover">
                                            <p>08:00</p>
                                        </div>
                                        <div className="hover">
                                            <p>10:00</p>
                                        </div>
                                    </td>
                                    <td className="active">
                                        <div className="hover">
                                            <p>08:00</p>
                                        </div>
                                        <div className="hover">
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