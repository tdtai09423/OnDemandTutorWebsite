import { useCallback, useState, useEffect } from 'react';
import { subHours, subMinutes } from 'date-fns';
import { Box, Button, Card, Container, Divider, Stack, Typography } from '@mui/material';
import { OrdersSearch } from '../sections/orders/orders-search.js';
import { OrdersTable } from '../sections/orders/orders-table.js';
import orderAPI from '../../../../api/orderAPI.js';

const now = new Date();

function AdminOrder() {
  const [mode, setMode] = useState('table');
  const [query, setQuery] = useState('');
  const [page, setPage] = useState(1);
  const [rowsPerPage, setRowsPerPage] = useState(10);

  const [orders, setOrder] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const orderList = await orderAPI.getAll();
        console.log(orderList.data.$values);
        setOrder(orderList.data.$values);
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
                Orders
              </Typography>
              <Button
                color="primary"
                size="large"
                variant="contained"
              >
                Add
              </Button>
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
                <OrdersTable
                  count={orders.length}
                  items={orders}
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

export default AdminOrder;
