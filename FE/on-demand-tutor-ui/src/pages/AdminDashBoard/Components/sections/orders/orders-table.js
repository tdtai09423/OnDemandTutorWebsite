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
import OrdersTableRow from './orders-table-row.js'

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
let statuss = [
  { id: 'Disable', name: 'Disable' },
  { id: 'Enable', name: 'Enable' }
];

const formatDate = (date) => {
  const day = String(date.getDate()).padStart(2, '0');
  const month = String(date.getMonth() + 1).padStart(2, '0');
  const year = date.getFullYear();
  const hours = String(date.getUTCHours()).padStart(2, '0');
  const minutes = String(date.getUTCMinutes()).padStart(2, '0');
  const seconds = String(date.getUTCSeconds()).padStart(2, '0');
  return `${year}-${month}-${day} ${hours}:${minutes}`;
};



export const OrdersTable = (props) => {
  const {
    count = 0,
    items = [],
    onPageChange = () => { },
    page = 0,
    rowsPerPage = 0,
    onRowsPerPageChange
  } = props;

  console.log(items)

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
                Order Date
              </TableCell>
              <TableCell>
                Learner
              </TableCell>
              <TableCell>
                Status
              </TableCell>
              <TableCell>
                Tutor
              </TableCell>
              <TableCell>
                Total
              </TableCell>
              <TableCell />
            </TableRow>
          </TableHead>
          <TableBody>
            {items.map((order) => {
              const id = order.orderId;
              const isCompleted = order.isCompleted;

              return (
                <OrdersTableRow
                  key={id}
                  id={id}
                  order={order}
                  isCompleted={isCompleted}
                />
              );
            })}
          </TableBody>
        </Table>
      </Scrollbar>
      <Divider />
      <TablePagination
        rowsPerPageOptions={[10, 25]}
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

OrdersTable.propTypes = {
  items: PropTypes.array,
  page: PropTypes.number,
  rowsPerPage: PropTypes.number,
  onPageChange: PropTypes.func,
  onRowsPerPageChange: PropTypes.func
};

