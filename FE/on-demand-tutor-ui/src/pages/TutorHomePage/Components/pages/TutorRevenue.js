import { useState, useEffect } from 'react';
import { Box, Card, Container, Divider, Stack, Typography } from '@mui/material';
import { CertiTable } from '../sections/orders/certificate-table.js';


function TutorRevenue() {
    const [mode, setMode] = useState('table');
    const [query, setQuery] = useState('');
    const [page, setPage] = useState(0);
    const [rowsPerPage, setRowsPerPage] = useState(5);



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
                                Revenue
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

export default TutorRevenue;
