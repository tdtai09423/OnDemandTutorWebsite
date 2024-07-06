import {
    Box,
    Button,
    Link,
    Stack,
    TableCell,
    TableRow
} from '@mui/material';
import Combobox from "react-widgets/Combobox";
import 'react-widgets/styles.css';
import TutorCertificates from './TutorCertificates.js';
import { useEffect, useState } from 'react';
import tutorAPI from '../../../../../api/tutorAPI.js';


const statusMap = {
    1: {
        color: 'success.main',
        label: 'Approved',
        id: 1
    },
    0: {
        color: 'warning.main',
        label: 'Pending',
        id: 0
    },
    2: {
        color: 'error.main',
        label: 'Rejected',
        id: 2
    }
};
let Certificate = [
    { id: 0, name: 'Pending' },
    { id: 1, name: 'Approved' },
    { id: 2, name: 'Rejected' }
];

function CertificateTableRow({ account }) {
    const [status, setStatus] = useState('');

    const tutorId = account.tutorNavigation.id;
    const certiStatus = statusMap[account.certiStatus];

    useEffect(() => {
        setStatus(certiStatus);
    }, [])

    const handleChangeStatus = (e) => {
        setStatus(e.name);
    };


    const handleUpdate = async (id) => {
        console.log(id);
        console.log(status);
        try {
            await tutorAPI.putCertificateStatus(id, status);
        } catch (error) {
            console.error('Error updating data:', error);
            alert('Đã xảy ra lỗi');
        }
        window.location.reload();

    };

    return (
        <TableRow key={tutorId}>
            <TableCell>
                <Link
                    color="inherit"
                    href="#"
                    underline="none"
                    variant="subtitle2"
                >
                    #{tutorId}
                </Link>
            </TableCell>
            <TableCell>
                {account.tutorNavigation.firstName + ' ' + account.tutorNavigation.lastName}
            </TableCell>
            <TableCell>
                <TutorCertificates
                    tutorId={tutorId}
                />
            </TableCell>
            <TableCell>
                <Stack
                    alignItems="center"
                    direction="row"
                    spacing
                    ={1}
                >
                    <Box
                        sx={{
                            backgroundColor: certiStatus.color,
                            borderRadius: '50%',
                            height: 8,
                            width: 8
                        }}
                    />
                    <Combobox
                        data={Certificate}
                        dataKey='id'
                        textField='name'
                        defaultValue={certiStatus.label}
                        style={{ width: '10em' }}
                        onChange={(e) => handleChangeStatus(e)}
                        className="custom-combobox"
                    />
                </Stack>
            </TableCell>
            <TableCell align="right">
                <Button
                    color="primary"
                    size="large"
                    variant="contained"
                    onClick={() => handleUpdate(tutorId)}
                >
                    Update
                </Button>
            </TableCell>
        </TableRow>
    );
}

export default CertificateTableRow;