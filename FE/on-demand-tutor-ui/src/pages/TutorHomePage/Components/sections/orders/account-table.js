import PropTypes from 'prop-types';
import {
  Divider,
  Button,
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
import Combobox from "react-widgets/Combobox";
import 'react-widgets/styles.css';
import { Scrollbar } from '../../../../../Components/scrollbar.js';
import { CheckCircleFill, XCircleFill } from 'react-bootstrap-icons'

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
let statuss = [
  { id: 'Disable', name: 'Disable' },
  { id: 'Enable', name: 'Enable' }
];



export const AccountsTable = (props) => {
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
                Created Date
              </TableCell>
              <TableCell>
                Full Name
              </TableCell>
              <TableCell>
                Email Verify
              </TableCell>
              <TableCell>
                Role ID
              </TableCell>
              <TableCell>
                Status
              </TableCell>
              <TableCell />
            </TableRow>
          </TableHead>
          <TableBody>
            {items.map((account) => {
              const id = account.id;
              const isEmailVerified = account.isEmailVerified;
              const status = account.status;
              const roleId = account.roleId;
              let accountStatus = '';
              status ? accountStatus = 'Enable' : accountStatus = 'Disable';
              let emailVerified = '';
              isEmailVerified ? emailVerified = 'Verified' : emailVerified = 'Not verified';

              return (
                <TableRow key={id}>
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
                      01/01/2002
                    </Typography>
                  </TableCell>
                  <TableCell>
                    {account.fullName}
                  </TableCell>
                  <TableCell>
                    {/* <Stack
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
                       <Typography variant="body2">
                        {certiStatus.label}
                      </Typography> 
                       <Combobox
                        data={Certificate}
                        dataKey='id'
                        textField='name'
                        defaultValue={certiStatus.label}
                        style={{ width: '10em' }}
                      /> 
                    </Stack> */}
                    <Stack
                      alignItems="center"
                      direction="row"
                      spacing={1}
                    >
                      {status ? (
                        <CheckCircleFill style={{ color: 'green', marginRight: '5px' }}>

                        </CheckCircleFill>
                      ) : (
                        <XCircleFill style={{ color: 'red', marginRight: '5px' }}>

                        </XCircleFill>
                      )}
                      {emailVerified}
                    </Stack>
                  </TableCell>
                  <TableCell>
                    {roleId}
                  </TableCell>
                  <TableCell>
                    <Stack
                      alignItems="center"
                      direction="row"
                      spacing={1}
                    >
                      {status ? (
                        <CheckCircleFill style={{ color: 'green' }}>

                        </CheckCircleFill>
                      ) : (
                        <XCircleFill style={{ color: 'red' }}>

                        </XCircleFill>
                      )}
                      {/* <Typography variant="body2">
                        status
                      </Typography> */}
                      <Combobox
                        data={statuss}
                        dataKey='id'
                        textField='name'
                        defaultValue={accountStatus}
                        style={{ width: '10em' }}
                      />
                    </Stack>

                  </TableCell>
                  <TableCell align="right">
                    <Button
                      color="primary"
                      size="large"
                      variant="contained"
                    >
                      Update
                    </Button>
                  </TableCell>
                </TableRow>
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

AccountsTable.propTypes = {
  items: PropTypes.array,
  page: PropTypes.number,
  rowsPerPage: PropTypes.number,
  onPageChange: PropTypes.func,
  onRowsPerPageChange: PropTypes.func
};
