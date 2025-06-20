import TutorRecap from "../../Components/TutorRecap";
import FilterBar from "../../Components/FilterBar";
import './home.scss';
import tutorAPI from "../../api/tutorAPI";
import { useEffect, useState } from "react";
import { Image } from "react-bootstrap";
import userAPI from "../../api/userAPI";
import { useNavigate } from "react-router-dom";
import majorAPI from "../../api/majorAPI";
import learnerAPI from "../../api/learnerAPI";

function Home() {
    const [tutors, setTutors] = useState([]);
    const [majors, setMajors] = useState([]);
    const [user, setUser] = useState({});
    const [tutorFavouriteList, setTutorFavouriteList] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchTutors = async () => {
            try {
                const email = localStorage.getItem('email');
                const user = await userAPI.getUserByEmail(email);
                setUser(user.data);
                if (user.data.roleId === 'LEARNER') {

                } else if (user.data.roleId === 'TUTOR') {
                    navigate('/tutor-page');
                } else if (user.data.roleId === 'ADMIN') {
                    navigate('/admin-dash-board');
                }
                const tutorList = await tutorAPI.getApproved();
                setTutors(tutorList.data.response.items.$values);
                const tutorFavouriteList = await learnerAPI.getFavourite(user.data.id);
                setTutorFavouriteList(tutorFavouriteList.data.$values);

            } catch (error) {
                console.error("Error fetching tutors:", error);
            }
        };
        const fetchMajor = async () => {
            try {
                const majorsRes = await majorAPI.getAll();
                setMajors(majorsRes.data.response.items.$values);

            } catch (error) {
                console.error("Error fetching tutors:", error);
            }
        };
        fetchTutors();
        fetchMajor();
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
                    <Image className="main-img"
                        src="https://static.preply.com/static/ssr/_next/static/images/hero-23-0802150dbe518540999c5757ad16d400.jpg"
                        style={{ zIndex: '5', position: 'relative' }}
                    />
                    <Image className="banner-image" src="https://static.preply.com/static/ssr/_next/static/images/hero-23-0802150dbe518540999c5757ad16d400.jpg" />
                    <Image className="banner-image" src="https://static.preply.com/static/ssr/_next/static/images/hero-23-0802150dbe518540999c5757ad16d400.jpg" />
                    <Image className="banner-image" src="https://static.preply.com/static/ssr/_next/static/images/hero-23-0802150dbe518540999c5757ad16d400.jpg" />
                </div>
            </div>
            <FilterBar
                majors={majors}
            />
            <hr />
            <div className="tutor-recap">
                {tutors.map((tutor) => (
                    <TutorRecap tutorFavouriteList={tutorFavouriteList} tutor={tutor} key={tutor.tutorId} />
                ))}
            </div>
        </div>
    );
}

export default Home;