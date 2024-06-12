import PropTypes from 'prop-types';
import EllipsisVerticalIcon from '@heroicons/react/24/solid/EllipsisVerticalIcon';
import {
  Box,
  Divider,
  IconButton,
  Link,
  Stack,
  SvgIcon,
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

const statusMap = {
  1: {
    color: 'success.main',
    label: 'Approved'
  },
  0: {
    color: 'error.main',
    label: 'Denied'
  },
  Pending: {
    color: 'warning.main',
    label: 'Pending'
  }
};

export const AccountsTable = (props) => {
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
                Created Date
              </TableCell>
              <TableCell>
                Name
              </TableCell>
              <TableCell>
                Certificate status
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
              const id = account.tutorNavigation.id;
              const certiStatus = statusMap[account.certiStatus];
              const status = account.tutorNavigation.status;
              const roleId = account.tutorNavigation.roleId;

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
                    {account.tutorNavigation.firstName}
                  </TableCell>
                  <TableCell>
                    <Stack
                      alignItems="center"
                      direction="row"
                      spacing={1}
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
                      <Typography variant="body2">
                        {status ? 'Enable' : 'Disable'}
                      </Typography>
                    </Stack>

                  </TableCell>
                  <TableCell align="right">
                    <IconButton>
                      <SvgIcon fontSize="small">
                        <EllipsisVerticalIcon />
                      </SvgIcon>
                    </IconButton>
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
