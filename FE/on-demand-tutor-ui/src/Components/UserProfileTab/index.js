import './style.scss';
import { Image } from 'react-bootstrap';
import { useState, useEffect } from 'react';
import tutorAPI from '../../api/tutorAPI';
import learnerAPI from '../../api/learnerAPI';
import EditProfile from './Components/Edit';



function UserProfileTab({ user }) {

    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [age, setAge] = useState();
    const [email, setEmail] = useState();
    const [avatar, setAvatar] = useState();
    const [nationality, setNationality] = useState();
    const [description, setDescription] = useState('');



    useEffect(() => {
        const fetchUser = async () => {
            try {
                if (user.roleId === 'TUTOR') {
                    const tutor = await tutorAPI.get(user.id);
                    console.log(tutor);
                    setFirstName(tutor.data.tutorNavigation.firstName);
                    setLastName(tutor.data.tutorNavigation.lastName);
                    setAge(tutor.data.tutorAge);
                    setEmail(tutor.data.tutorEmail);
                    setAvatar(tutor.data.tutorPicture);
                    setNationality(tutor.data.nationality);
                    setDescription(tutor.data.tutorDescription);
                } else if (user.roleId === 'LEARNER') {
                    const learner = await learnerAPI.get(user.id);
                    console.log(learner);
                    setFirstName(learner.data.learnerNavigation.firstName);
                    setLastName(learner.data.learnerNavigation.lastName);
                    setAge(learner.data.learnerAge);
                    setEmail(learner.data.learnerEmail);
                    setAvatar('https://villagesonmacarthur.com/wp-content/uploads/2020/12/Blank-Avatar.png');
                    setNationality(null);
                }

            } catch (error) {
                console.error("Error fetching user:", error);
            }
        };
        fetchUser();

    }, [user])
    

    return (
        <section className="h-100 gradient-custom-2">
            <div className="containerRoot py-5 h-100">
                <div className="row d-flex justify-content-center containerRoot02">
                    <div className="col col-lg-9 col-xl-8">
                        <div className="card">
                            <div className="rounded-top text-white d-flex flex-row" style={{ backgroundColor: '#6c757d', height: '200px' }}>
                                <div className="ms-4 mt-5 d-flex flex-column" style={{ width: '150px' }}>
                                    {/* avatar */}
                                    <Image src={avatar}
                                        className="img-fluid img-thumbnail mt-4 mb-2"
                                        style={{ width: '150px', zIndex: 1 }} />                                
                                </div>
                                {/* first name, last name, nation      */}
                                <div className="ms-3" style={{ marginTop: '130px' }}>
                                    <h5>{firstName} {lastName}</h5>
                                    {(user.roleId === 'TUTOR') ? <p>{nationality}</p> : <p>Student</p>}
                                    
                                </div>
                                
                            </div>
                            <div className="p-4 text-black bg-body-tertiary">
                                <EditProfile user={user}/>
                                <div className="d-flex justify-content-end text-center py-1 text-body">
                                    {nationality ? (<div>
                                        <p className="mb-1 h5">2</p>
                                        <p className="small text-muted mb-0">lesson</p>
                                    </div>) : <div></div>}


                                </div>
                            </div>
                            <div className="card-body p-4 text-black">
                                <div className="mb-5 text-body">
                                    <p className="lead fw-normal mb-1">About</p>
                                    <div className="p-4 bg-body-tertiary">
                                        <p className="font-italic mb-1">Age : {age}</p>
                                        <p className="font-italic mb-1">Email : {email}</p>
                                        <hr />
                                        {nationality ? (<p className="font-italic mb-0">{description}</p>) : <p>Learner of preply</p>}

                                    </div>
                                </div>

                                <div className="row g-2">
                                    <div className="col mb-2">

                                    </div>
                                    <div className="col mb-2">

                                    </div>
                                </div>
                                <div className="row g-2">
                                    <div className="col">

                                    </div>
                                    <div className="col">

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    );
}

export default UserProfileTab;