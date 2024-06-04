import React from 'react';
import 'bootstrap/dist/css/bootstrap.min.css'
import { useState } from 'react';
import { useEffect } from 'react';
import learnerAPI from "../../../../api/learnerAPI";


function LearnerReview({ id }) {

    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');

    useEffect(() => {
        const fetchLearner = async () => {
            try {
                const learner = await learnerAPI.get(id);
                setFirstName(learner.data.learnerNavigation.firstName);
                setLastName(learner.data.learnerNavigation.lastName);

            } catch (error) {
                console.error("Error fetching tutors:", error);
            }
        };
        fetchLearner();


    }, [id]);

    return (
        <span style={{ fontSize: '13px' }}>{firstName} {lastName} : </span>
    );
}

export default LearnerReview;