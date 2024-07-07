import { useCallback, useState, useEffect } from 'react';
import { subHours, subMinutes } from 'date-fns';
import { Box, Button, Card, Container, Divider, Stack, Typography } from '@mui/material';
import { OrdersSearch } from '../sections/orders/orders-search.js';
import { OrdersTable } from '../sections/orders/orders-table.js';


function TutorProfile() {
  const [mode, setMode] = useState('table');
  const [query, setQuery] = useState('');
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(5);

  const [users, setUser] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
    };
    fetchData();
  }, []);

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
      setPage(value);
    },
    []
  );

  const handleChangeRowsPerPage = useCallback(
    (event) => {
      setRowsPerPage(parseInt(event.target.value, 10));
      setPage(0);
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
                TutorProfile
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
              </Card>
            </div>
          </Stack>
        </Container>
      </Box>
    </>
  );
};

export default TutorProfile;
