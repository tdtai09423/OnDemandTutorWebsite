import { useCallback, useState, useEffect } from 'react';
import { Box, Card, Container, Divider, Stack, Typography } from '@mui/material';
import { OrdersSearch } from '../sections/orders/orders-search.js';
import tutorAPI from '../../../../api/tutorAPI.js';
import { CertiTable } from '../sections/orders/certificate-table.js';


function TutorNotification() {
    const [mode, setMode] = useState('table');
    const [query, setQuery] = useState('');
    const [page, setPage] = useState(0);
    const [rowsPerPage, setRowsPerPage] = useState(5);

    const [tutors, setTutors] = useState([]);

    const fetchData = async () => {
        try {

        } catch (error) {
            console.error("Error fetching tutors:", error);
        }
    };

    useEffect(() => {
        fetchData();
    }, []);


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
                                Notification
                            </Typography>
                        </Stack>
                        <div>
                            <Card>

                                <Divider />
                                <CertiTable

                                />
                            </Card>
                        </div>
                    </Stack>
                </Container>
            </Box>
        </>
    );
};

export default TutorNotification;
