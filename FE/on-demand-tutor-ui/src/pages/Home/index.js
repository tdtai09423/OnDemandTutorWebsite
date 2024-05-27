import TutorRecap from "../../Components/TutorRecap";
import FilterBar from "../../Components/FilterBar";
import './home.scss';

function Home() {
    return (
        <div>
            <h2>Main content, images, introduction of the website</h2>
            <FilterBar />
            <hr />
            <div className="tutor-recap">
                <TutorRecap />
            </div>

        </div>

    );
}

export default Home;