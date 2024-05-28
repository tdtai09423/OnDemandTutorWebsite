import TutorRecap from "../../Components/TutorRecap";
import FilterBar from "../../Components/FilterBar";
import './home.scss';
import { useEffect, useState } from "react";
import tutorAPI from "../../api/tutorAPI";

function Home() {

    const [tutorList, setTutorList] = useState([]);

    useEffect(() => {
        const fetchTutor = async () => {
            const response = await tutorAPI.getAll();
            const tutorList = response.data;
            setTutorList(tutorList);
            console.log(tutorList);
        }

        fetchTutor();
    }, [])

    return (
        <div>
            <h2>Main content, images, introduction of the website</h2>
            <FilterBar />
            <hr />
            <div className="tutor-recap">
                {tutorList.map((tutor, index) => (
                    <TutorRecap
                        key={index}
                        tutorInfo={{
                            image: tutor.image,
                            name: tutor.name,
                            language: tutor.language,
                            activeStudents: tutor.activeStudents,
                            lessons: tutor.lessons,
                            speaks: tutor.speaks,
                            description: tutor.description,
                            price: tutor.price,
                            duration: tutor.duration
                        }}
                    />
                ))}
            </div>
        </div>
    );
}

export default Home;