import TutorRecap from "../../Components/TutorRecap";
import FilterBar from "../../Components/FilterBar";
import './home.scss';
import tutorAPI from "../../api/tutorAPI";
import { useEffect, useState } from "react";
import { Image } from "react-bootstrap";

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
            <div class="row banner">
                <div className="col-lg-6">
                    <h1>Unlock your potential with the best language tutors.</h1>
                    <p>Learn a new language with the help of our expert tutors.</p>
                    <button className="banner-btn">Get started</button>
                </div>
                <div className="col-lg-6 image-container">
                    <Image className="main-img" src="https://static.preply.com/static/ssr/_next/static/images/hero-23-0802150dbe518540999c5757ad16d400.jpg" />
                    <Image className="banner-image" src="https://static.preply.com/static/ssr/_next/static/images/hero-23-0802150dbe518540999c5757ad16d400.jpg" />
                    <Image className="banner-image" src="https://static.preply.com/static/ssr/_next/static/images/hero-23-0802150dbe518540999c5757ad16d400.jpg" />
                    <Image className="banner-image" src="https://static.preply.com/static/ssr/_next/static/images/hero-23-0802150dbe518540999c5757ad16d400.jpg" />
                </div>
            </div>
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