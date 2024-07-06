import PropTypes from 'prop-types';
import {
    Divider,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TablePagination,
    TableRow
} from '@mui/material';
import 'react-widgets/styles.css';
import { Scrollbar } from '../../../../../Components/scrollbar.js';
import CertificateTableRow from './certificate-table-row.js';

const statusMap = {
    1: {
        color: 'success.main',
        label: 'Approved',
        id: 1
    },
    0: {
        color: 'error.main',
        label: 'Denied',
        id: 0
    },
    2: {
        color: 'warning.main',
        label: 'Pending',
        id: 2
    }
};
let Certificate = [
    { id: 0, name: 'Denied' },
    { id: 1, name: 'Approved' },
    { id: 2, name: 'Pending' }
];

export const CertiTable = (props) => {
    const {
        count = 0,
        items = [],
        onPageChange = () => { },
        page = 0,
        rowsPerPage = 0,
        onRowsPerPageChange
    } = props;

    return (
        <div>
            <Scrollbar>
                <Table sx={{ minWidth: 800 }}>
                    <TableHead>
                        <TableRow>
                            <TableCell>
                                ID
                            </TableCell>
                            <TableCell>
                                Full Name
                            </TableCell>
                            <TableCell>
                                Certificates
                            </TableCell>
                            <TableCell>
                                Status
                            </TableCell>
                            <TableCell />
                        </TableRow>
                    </TableHead>
                    <TableBody className='table-body'>
                        {items.map((account) => {
                            return (
                                <CertificateTableRow
                                    account={account}
                                />
                            );

                        })}
                    </TableBody>
                </Table>
            </Scrollbar>
            <Divider />
            <TablePagination
                rowsPerPageOptions={[5, 10, 25]}
                component="div"
                count={count}
                rowsPerPage={rowsPerPage}
                page={page}
                onPageChange={onPageChange}
                onRowsPerPageChange={onRowsPerPageChange}
            />
        </div>
    );
};

CertiTable.propTypes = {
    items: PropTypes.array,
    page: PropTypes.number,
    rowsPerPage: PropTypes.number,
    onPageChange: PropTypes.func,
    onRowsPerPageChange: PropTypes.func,
};
