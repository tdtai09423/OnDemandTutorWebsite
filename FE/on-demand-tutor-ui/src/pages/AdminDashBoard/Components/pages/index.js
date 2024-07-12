import ShoppingBagIcon from '@heroicons/react/24/solid/ShoppingBagIcon';
import User from '@heroicons/react/24/solid/UserIcon';
import Users from '@heroicons/react/24/solid/UserGroupIcon';
import {
  Avatar,
  Button,
  Box,
  Container,
  Stack,
  SvgIcon,
  Typography,
  Unstable_Grid2 as Grid
} from '@mui/material';
import { OverviewKpi } from '../sections/overview/overview-kpi.js';
import { OverviewSummary } from '../sections/overview/overview-summary.js';
import { useState, useEffect } from 'react';
import tutorAPI from '../../../../api/tutorAPI.js';
import learnerAPI from '../../../../api/learnerAPI.js';
import orderAPI from '../../../../api/orderAPI.js';


const now = new Date();

function HomeAdmin() {

  const [orders, setOrders] = useState([]);
  const [tutors, setTutors] = useState([]);
  const [learners, setLearners] = useState([]);
  const [orderRevenue, setOrderRevenue] = useState([])

  const [chartOptions, setChartOptions] = useState();
  function getMonthName(monthNumber) {
    const months = [
      "January", "February", "March", "April",
      "May", "June", "July", "August",
      "September", "October", "November", "December"
    ];

    const adjustedMonthNumber = monthNumber - 1;

    return months[adjustedMonthNumber];
  }
  useEffect(() => {
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

        const orderList = await orderAPI.getAll();
        setOrders(orderList.data.$values);
        //set order for revenue
        const learnerList = await learnerAPI.getAll(token);
        console.log(learnerList.data.response.items.$values)
        setLearners(learnerList.data.response.items.$values);

      } catch (error) {
        console.error("Error fetching tutors:", error);
      }
    };
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
            <div>
              <Typography variant="h4">
                Reports
              </Typography>
            </div>
            <div>
              <Grid
                container
                spacing={3}
              >
                <Grid
                  xs={12}
                  md={4}
                >
                  <OverviewSummary
                    icon={
                      <Avatar
                        sx={{
                          backgroundColor: 'primary.main',
                          color: 'primary.contrastText',
                          height: 56,
                          width: 56
                        }}
                      >
                        <SvgIcon>
                          <ShoppingBagIcon />
                        </SvgIcon>
                      </Avatar>
                    }
                    label='Orders'
                    value={orders.length}
                  />
                </Grid>
                <Grid
                  xs={12}
                  md={4}
                >
                  <OverviewSummary
                    icon={
                      <Avatar
                        sx={{
                          backgroundColor: 'primary.main',
                          color: 'primary.contrastText',
                          height: 56,
                          width: 56
                        }}
                      >
                        <SvgIcon>
                          <User />
                        </SvgIcon>
                      </Avatar>
                    }
                    label='Tutors'
                    value={tutors.length}
                  />
                </Grid>
                <Grid
                  xs={12}
                  md={4}
                >
                  <OverviewSummary
                    icon={
                      <Avatar
                        sx={{
                          backgroundColor: 'primary.main',
                          color: 'primary.contrastText',
                          height: 56,
                          width: 56
                        }}
                      >
                        <SvgIcon>
                          <Users />
                        </SvgIcon>
                      </Avatar>
                    }
                    label='Learner'
                    value={learners.length}
                  />
                </Grid>
                <Grid xs={12}>
                  
                </Grid>
                {/* <Grid xs={12}>
                  <OverviewLatestCustomers
                    customers={[
                      {
                        id: 'a105ac46530704806ca58ede',
                        amountSpent: 684.45,
                        avatar: '/assets/avatars/avatar-fabiano-jorioz.jpg',
                        createdAt: subDays(subHours(subMinutes(now, 7), 3), 2).getTime(),
                        isOnboarded: true,
                        name: 'Fabiano Jorioz',
                        orders: 2
                      },
                      {
                        id: '126ed71fc9cbfabc601c56c5',
                        amountSpent: 0,
                        avatar: '/assets/avatars/avatar-meggie-heinonen.jpg',
                        createdAt: subDays(subHours(subMinutes(now, 7), 3), 2).getTime(),
                        isOnboarded: false,
                        name: 'Meggie Heinonen',
                        orders: 0
                      },
                      {
                        id: 'aafaeb0545357922aff32a7b',
                        amountSpent: 32.25,
                        avatar: '/assets/avatars/avatar-sean-picott.jpg',
                        createdAt: subDays(subHours(subMinutes(now, 11), 2), 3).getTime(),
                        isOnboarded: true,
                        name: 'Sean Picott',
                        orders: 1
                      },
                      {
                        id: '16b526d9e0fefe53f7eba66b',
                        amountSpent: 0,
                        avatar: '/assets/avatars/avatar-bell-covely.jpg',
                        createdAt: subDays(subHours(subMinutes(now, 18), 9), 5).getTime(),
                        isOnboarded: true,
                        name: 'Bell Covely',
                        orders: 0
                      },
                      {
                        id: 'fe035356923629912236d9a2',
                        amountSpent: 125.70,
                        avatar: '/assets/avatars/avatar-giraud-lamlin.jpg',
                        createdAt: subDays(subHours(subMinutes(now, 19), 18), 7).getTime(),
                        isOnboarded: false,
                        name: 'Giraud Lamlin',
                        orders: 1
                      }
                    ]}
                  />
                </Grid>*/}
              </Grid>
            </div>
          </Stack>
        </Container>
      </Box>
    </>
  );
};

export default HomeAdmin;
