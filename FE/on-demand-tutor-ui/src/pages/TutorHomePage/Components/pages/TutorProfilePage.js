import TutorDetailTab from "../../../../Components/TutorDetailTab";
import { useEffect, useState } from "react";
import userAPI from "../../../../api/userAPI";


function TutorProfilePage({ role }) {

    const [id, setId] = useState();

    useEffect(() => {
        const email = localStorage.getItem('email');

        const fetchData = async () => {
            try {
                const userRes = await userAPI.getUserByEmail(email);
                setId(userRes.data.id)
            } catch (error) {
                console.error("Error fetching tutors:", error);
            }
        };
        fetchData();
    }, []);


    return (
        <>
            <TutorDetailTab
                tutorId={id}
                roleUser={role}
            />
        </>
    );
};

export default TutorProfilePage;
