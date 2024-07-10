import PropTypes from 'prop-types';
import {
    Divider,
    Link,
    Stack,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TablePagination,
    TableRow,
    Typography
} from '@mui/material';
import { Scrollbar } from '../../../../../Components/scrollbar.js';
import { CheckCircleFill, XCircleFill } from 'react-bootstrap-icons'
import { useEffect, useState } from 'react';
import tutorAPI from '../../../../../api/tutorAPI.js';
import learnerAPI from '../../../../../api/learnerAPI.js';
import curriculumAPI from '../../../../../api/curriculumAPI.js';

function OrdersTableRow({ id, order, isCompleted }) {

    const [learner, setLearner] = useState('');
    const [tutor, setTutor] = useState('');

    const formatDate = (date) => {
        const day = String(date.getDate()).padStart(2, '0');
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const year = date.getFullYear();
        const hours = String(date.getUTCHours()).padStart(2, '0');
        const minutes = String(date.getUTCMinutes()).padStart(2, '0');
        const seconds = String(date.getUTCSeconds()).padStart(2, '0');
        return `${year}-${month}-${day} ${hours}:${minutes}`;
    };

    useEffect(() => {
        const fetchData = async () => {
            try {
                const learnerRes = await learnerAPI.get(order.learnerId);
                const curriculum = await curriculumAPI.getCurriculumByOrderId(id);
                const tutorRes = await tutorAPI.get(curriculum.data.curriculum.tutorId);
                setLearner(learnerRes.data.learnerNavigation.firstName + ' ' + learnerRes.data.learnerNavigation.lastName);
                setTutor(tutorRes.data.tutorNavigation.firstName + ' ' + tutorRes.data.tutorNavigation.lastName);
            } catch (error) {
                console.error('Error fetching data:', error);
            }
        }
        fetchData();
    }, [])

    return (
        <TableRow>
            <TableCell>
                <Link
                    color="inherit"
                    href="#"
                    underline="none"
                    variant="subtitle2"
                >
                    #{id}
                </Link>
            </TableCell>
            <TableCell>
                <Typography
                    color="inherit"
                    variant="inherit"
                >
                    {formatDate(new Date(order.orderDate))}
                </Typography>
            </TableCell>
            <TableCell>
                {learner}
            </TableCell>
            <TableCell>
                <Stack
                    alignItems="center"
                    direction="row"
                    spacing={1}
                >
                    {isCompleted ? (
                        <div className='d-flex align-items-center'>
                            <CheckCircleFill style={{ color: 'green', marginRight: '5px' }}>

                            </CheckCircleFill>
                            <div>Completed</div>
                        </div>
                    ) : (
                        <div className='d-flex align-items-center'>
                            <XCircleFill style={{ color: 'red', marginRight: '5px' }}>

                            </XCircleFill>
                            <div>Not complete</div>
                        </div>
                    )}
                </Stack>
            </TableCell>
            <TableCell>
                {tutor}
            </TableCell>
            <TableCell>
                $ {order.total}
            </TableCell>
            <TableCell align="right">
            </TableCell>
        </TableRow>
    );
}

export default OrdersTableRow;