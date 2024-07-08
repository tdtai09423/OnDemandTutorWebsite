import { useCallback, useState, useEffect } from 'react';
import { Box, Card, Container, Divider, Stack, Typography } from '@mui/material';
import { OrdersSearch } from '../sections/orders/orders-search.js';
import userAPI from '../../../../api/userAPI.js';
import { CertiTable } from '../sections/orders/certificate-table.js';
import NotificationCom from '../../../../Components/Layout/components/NotificationCom/index.js';
import NotificationAPI from '../../../../api/notificationAPI.js';


function TutorNotification() {
    const [notifications, setNotifications] = useState([]);

    const [tutors, setTutors] = useState([]);
    const [userId, setUserId] = useState();

    const fetchData = async () => {
        try {
            const Jtoken = localStorage.getItem('token');
            const email = localStorage.getItem('email');
            const user = await userAPI.getUserByEmail(email);
            setUserId(user.data.id);
            let res = await NotificationAPI.getNotification(user.data.id);
            setNotifications(res);
            console.log("NOTIFICATION", res.data.notificationList.$values);
            setNotifications(res.data.notificationList.$values)
        } catch (error) {
            console.log(error);
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
                            <NotificationCom notificationInfo={notifications} />
                        </div>
                    </Stack>
                </Container>
            </Box>
        </>
    );
};

export default TutorNotification;
