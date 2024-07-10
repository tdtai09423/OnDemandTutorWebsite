import React, { useState, useEffect } from 'react';
import { Box, Card, Container, Divider, Stack, Typography } from '@mui/material';
import { AgCharts } from 'ag-charts-react';
import './TutoeRevenue.scss';
import RevenueAPI from '../../../../api/RevenueAPI';
import userAPI from '../../../../api/userAPI';
function TutorRevenue() {
    // { month: 'Jan', avgTemp: 2.3, total: 162000 },
        // { month: 'Mar', avgTemp: 6.3, total: 302000 },
        // { month: 'May', avgTemp: 16.2, total: 800000 },
        // { month: 'Jul', avgTemp: 22.8, total: 1254000 },
        // { month: 'Sep', avgTemp: 14.5, total: 950000 },
        // { month: 'Nov', avgTemp: 8.9, total: 200000 },
    const token = localStorage.getItem('token');
    const email = localStorage.getItem('email');
    // const [arrdata, setArrdata] = useState([]);
    // const [data, setData] = useState();
    const [chartOptions, setChartOptions] = useState();
    const monthArr= [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];

    

    function getMonthName(monthNumber) {
        const months = [
            "January", "February", "March", "April",
            "May", "June", "July", "August",
            "September", "October", "November", "December"
        ];
    
        const adjustedMonthNumber = monthNumber - 1;
    
        return months[adjustedMonthNumber];
    }
    // const addData = (res, mon) => {
        
        
        
    // }


    const fetchData = async () => {
        try {
            const user = await userAPI.getUserByEmail(email);
            const tutorId = user.data.id;
            let arr = [12];
            for (let mon = 0; mon < 12; mon++) {
                try {
                    const mon1 = mon + 1;
                    const res = await RevenueAPI.getRevenueByMonth(tutorId, mon1, 2024, token);
                    console.log("res",res)
                    const month = getMonthName(mon1);
                    console.log(`ADDDATA RES ${month}`, res);
        
                    const total = res.data.totalAmount;
                    const totalOrder = res.data.totalBookings;
                    // setData({ month: month, total: total });
                    const tempData = { month: month, total: total , totalOrder: totalOrder};
                    // setArrdata(prevData => [...prevData, data]);
                    arr[mon] = tempData;
                } catch (error) {
                    console.error(`Error fetching revenue for ${mon}:`, error);
                }
            }
            console.log("Arr>>>>>>>>>>>",arr)
            setChartOptions({
                data : arr,
                // Series: Defines which chart type and data to use
                series: [{ type: 'bar', xKey: 'month', yKey: 'total',yName: 'Total' },
                ]
            })
            
            
        } catch (error) {
            console.error("Error fetching tutors:", error);
        }
    };

    useEffect(() => {
         fetchData();
        
            
    }, []);


    return (
        <>
            <div className='chart-container'>
                <AgCharts options={chartOptions} />
            </div>
          
        </>
    );
};

export default TutorRevenue;