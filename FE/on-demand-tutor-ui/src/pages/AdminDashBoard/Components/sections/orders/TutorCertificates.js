import tutorAPI from '../../../../../api/tutorAPI.js';
import { useEffect, useState } from 'react';

function TutorCertificates({ tutorId }) {
    const [certificates, setCertificates] = useState([]);

    const fetchCertificate = async (id) => {
        try {
            const certiList = await tutorAPI.getCerti(id);
            setCertificates(certiList.data.$values);
        } catch (error) {
            console.error("Error fetching tutors:", error);
        }
    };

    useEffect(() => {
        fetchCertificate(tutorId);
    }, []);

    return (
        <div>
            {certificates.map((item) => (
                <div key={item.$id}>{item.tutorCertificate}</div>
            ))}
        </div>
    );
}

export default TutorCertificates;