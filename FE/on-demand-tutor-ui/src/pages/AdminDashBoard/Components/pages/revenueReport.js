import { useCallback, useState, useEffect } from 'react';
import { subHours, subMinutes } from 'date-fns';
import { Box, Button, Card, Container, Divider, Stack, Typography } from '@mui/material';
import { OrdersSearch } from '../sections/orders/orders-search.js';
import { OrdersTable } from '../sections/orders/orders-table.js';
import orderAPI from '../../../../api/orderAPI.js';
import { AgCharts } from 'ag-charts-react';
const now = new Date();

function RevenueReport() {
    const [chartOptions, setChartOptions] = useState();
    const [orders, setOrders] = useState([]);
    const [year, setYear] = useState(2024);
    function getMonthName(monthNumber) {
        const months = [
            "January", "February", "March", "April",
            "May", "June", "July", "August",
            "September", "October", "November", "December"
        ];

        const adjustedMonthNumber = monthNumber - 1;

        return months[adjustedMonthNumber];
    }
    const formatDate = (date) => {
        const day = date.getDate();
        const month = date.getMonth() + 1;
        const year = date.getFullYear();
        return month;
    };

    useEffect(() => {
        const fetchData = async () => {
            try {
                const orderList = await orderAPI.getAll();
                setOrders(orderList.data.$values);
                const ordersData = orderList.data.$values
                console.log("ORDERS IN REVENUE", ordersData)
                const arr = [12];
                for (let mon = 0; mon < 12; mon++) {
                    try {
                        let totalTmp = 0;
                        let totalOrder = 0;
                        let mon1 = mon + 1;
                        let monthlyTotals = {}; // Initialize an empty object to store monthly totals
                        let monthText = getMonthName(mon1);
                        console.log(`ADDDATA RES ${monthText}`);
                        ordersData.forEach(item => {
                            let isAccepted = item.orderStatus === "Accepted";
                            // const date = item.orderDate;
                            const month = formatDate(new Date(item.orderDate)); // Get the month (1-12)

                            if (month === mon1 && isAccepted) {
                                totalTmp += item.total;
                                totalOrder += 1;
                            }
                            // monthlyTotals[month].total += item.total;
                            // monthlyTotals[month].totalOrder += 1;
                        });
                        monthlyTotals = { month: monthText, total: totalTmp, totalOrder: totalOrder };
                        console.log(monthlyTotals);
                        arr[mon] = monthlyTotals;
                        console.log("ARRRRRR", arr)
                    } catch (error) {
                        console.log("Error in loop", error)
                    }

                }

                setChartOptions({
                    data: arr,
                    // Series: Defines which chart type and data to use
                    series: [{ type: 'bar', xKey: 'month', yKey: 'total', yName: 'Total' },
                    ]
                })
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
                <div className='chart-container'>
                    <AgCharts options={chartOptions} />
                </div>
            </Box>
        </>
    );
};

export default RevenueReport;
