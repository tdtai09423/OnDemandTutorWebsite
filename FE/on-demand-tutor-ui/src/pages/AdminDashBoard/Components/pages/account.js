import { useCallback, useState, useEffect } from 'react';
import { Box, Button, Card, Container, Divider, Stack, Typography } from '@mui/material';
import { OrdersSearch } from '../sections/orders/orders-search.js';
import userAPI from '../../../../api/userAPI.js';
import { AccountsTable } from '../sections/orders/account-table.js';


function AdminAccount() {
  const [mode, setMode] = useState('table');
  const [query, setQuery] = useState('');
  const [page, setPage] = useState(1);
  const [rowsPerPage, setRowsPerPage] = useState(10);

  const [users, setUser] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const token = localStorage.getItem('token');
        const userList = await userAPI.getAll(token, page + 1, rowsPerPage);
        console.log(userList.data.response.items.$values);
        setUser(userList.data.response.items.$values);
      } catch (error) {
        console.error("Error fetching tutors:", error);
      }
    };
    fetchData();
  }, [page, rowsPerPage]);

  const handleModeChange = useCallback(
    (event, value) => {
      if (value) {
        setMode(value);
      }
    },
    []
  );

  const handleQueryChange = useCallback(
    (value) => {
      setQuery(value);
    },
    []
  );

  const handleChangePage = useCallback(
    (event, value) => {
      console.log(value)
      setPage(value);
    },
    []
  );

  const handleChangeRowsPerPage = useCallback(
    (event) => {
      console.log(event.target.value)
      setRowsPerPage(parseInt(event.target.value, 10));
      setPage(1);
    },
    []
  );

  return (
    <>
      <Box
        sx={{
          flexGrow: 1,
          py: 8
        }}
      >
        <Container maxWidth="xl">
          <Stack spacing={3}>
            <Stack
              alignItems="flex-start"
              direction="row"
              justifyContent="space-between"
              spacing={3}
            >
              <Typography variant="h4">
                Account
              </Typography>
            </Stack>
            <div>
              <Card>
                <OrdersSearch
                  mode={mode}
                  onModeChange={handleModeChange}
                  onQueryChange={handleQueryChange}
                  query={query}
                />
                <Divider />
                <AccountsTable
                  count={users.length}
                  items={users}
                  page={page}
                  rowsPerPage={rowsPerPage}
                  onPageChange={handleChangePage}
                  onRowsPerPageChange={handleChangeRowsPerPage}
                />
              </Card>
            </div>
          </Stack>
        </Container>
      </Box>
    </>
  );
};

export default AdminAccount;
