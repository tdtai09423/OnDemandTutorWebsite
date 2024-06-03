import React, { useEffect } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css'
import './TutorDetail.scss'
import { useState } from 'react';
import tutorAPI from '../../api/tutorAPI';
import TutorDetailTab from '../../Components/TutorDetailTab';// Import Bootstrap components
import { useSearchParams } from 'react-router-dom';

function TutorDetail(param) {

    const [tutor, setTutor] = useState({});
    const [certi, setCerti] = useState([]);
    const [searchParam] = useSearchParams();
    const tutorId = searchParam.get('tutorId');

    useEffect(() => {
        console.log("TutorDetail");
        console.log(tutorId);
        const fetchTutors = async () => {
            try {
                const tutor = await tutorAPI.get(tutorId);
                const certi = await tutorAPI.getCerti(tutorId);
                setTutor(tutor.data);
            } catch (error) {
                console.error("Error fetching tutors:", error);
            }
        };
        fetchTutors();
    }, []);

    return (

        <TutorDetailTab
            tutorParam={tutor}
            tutorCerti={certi}
        />
    );
};

export default TutorDetail;