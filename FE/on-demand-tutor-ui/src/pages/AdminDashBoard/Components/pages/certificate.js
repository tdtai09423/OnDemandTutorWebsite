import { useCallback, useState, useEffect } from 'react';
import { Box, Card, Container, Divider, Stack, Typography } from '@mui/material';
import { OrdersSearch } from '../sections/orders/orders-search.js';
import tutorAPI from '../../../../api/tutorAPI.js';
import { CertiTable } from '../sections/orders/certificate-table.js';


function Certificate() {
    const [mode, setMode] = useState('table');
    const [query, setQuery] = useState('');
    const [page, setPage] = useState(0);
    const [rowsPerPage, setRowsPerPage] = useState(5);

    const [tutors, setTutors] = useState([]);

    const fetchData = async () => {
        try {
            const token = localStorage.getItem('token');
            const approvedResponse = await tutorAPI.getApproved();
            const page = 1;
            const pageSize = 10;
            const pendingResponse = await tutorAPI.getPending(token, page, pageSize);
            const rejectedResponse = await tutorAPI.getRejected(token, page, pageSize);

            const approvedTutors = approvedResponse.data.response.items.$values;
            const pendingTutors = pendingResponse.data.response.items.$values;
            const rejectedTutors = rejectedResponse.data.response.items.$values;

            const mergedTutors = [...approvedTutors, ...pendingTutors, ...rejectedTutors].sort((a, b) => a.tutorId - b.tutorId);
            console.log(mergedTutors)
            setTutors(mergedTutors);
        } catch (error) {
            console.error("Error fetching tutors:", error);
        }
    };

    useEffect(() => {
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
                                Certificates
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
                                <CertiTable
                                    count={tutors.length}
                                    items={tutors}
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

export default Certificate;
