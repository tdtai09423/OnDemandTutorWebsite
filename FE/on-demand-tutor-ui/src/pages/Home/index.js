import TutorRecap from "../../Components/TutorRecap";
import FilterBar from "../../Components/FilterBar";
import './home.scss';
import tutorAPI from "../../api/tutorAPI";
import { useEffect, useState } from "react";

function Home() {
    const [tutors, setTutors] = useState([]);

    useEffect(() => {
        const fetchTutors = async () => {
            try {
                const tutorList = await tutorAPI.getAll();
                setTutors(tutorList.data.$values);
                console.log(tutorList.data.$values);
            } catch (error) {
                console.error("Error fetching tutors:", error);
            }
        };
        fetchTutors();
    }, []);



    return (
        <div>
            <h2>Main content, images, introduction of the website</h2>
            <FilterBar />
            <hr />
            <div className="tutor-recap">
                {tutors.map((tutor) => (
                    <TutorRecap tutor={tutor} key={tutor.tutorId} />
                ))}
            </div>
        </div>
    );
}

export default Home;