import React, { useEffect } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css'
import './TutorDetail.scss'
import { useState } from 'react';
import tutorAPI from '../../api/tutorAPI';
import TutorDetailTab from '../../Components/TutorDetailTab';// Import Bootstrap components

function TutorDetail() {

    const [tutor, setTutor] = useState({});

    // useEffect(() => {
    //     const fetchTutors = async () => {
    //         try {
    //             const tutor = await tutorAPI.get(1);
    //             setTutor(tutor.data);
    //             console.log(tutor.data);
    //         } catch (error) {
    //             console.error("Error fetching tutors:", error);
    //         }
    //     };
    //     fetchTutors();
    // }, []);
    useEffect(() => {
        setTutor({
            firstName: "John",
            lastName: "Doe",
            email: '',
            description: "Hello",
            certificated: ["Certificate1", "Certificate2", "Certificate3"]

        })
    }, []);

    return (

        <TutorDetailTab
            tutorParam={tutor}
        />
    );
};

export default TutorDetail;