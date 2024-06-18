import React from 'react';
import 'bootstrap/dist/css/bootstrap.min.css'
import './TutorDetail.scss'

import TutorDetailTab from '../../Components/TutorDetailTab';// Import Bootstrap components
import { useSearchParams } from 'react-router-dom';

function TutorDetail(param) {

    const [searchParam] = useSearchParams();
    const tutorId = searchParam.get('tutorId');

    return (

        <TutorDetailTab
            tutorId={tutorId}
        />
    );
};

export default TutorDetail;