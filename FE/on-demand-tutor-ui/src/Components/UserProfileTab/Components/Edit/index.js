
import { useEffect, useState } from 'react';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';


function EditProfile({user}) {
    
    console.log("user>>>>>>>>>>>>>>>>", user);
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [age, setAge] = useState('');
    const [email, setEmail] = useState('');
    const [avatar, setAvatar] = useState('');
    const [nationality, setNationality] = useState('');
    const [description, setDescription] = useState('');
    const [roleIdd, setRoleIdd] = useState('');
    
    

    const [show, setShow] = useState(false);
    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);

    useEffect(() => {
        if (show) {
            if (user.roleId === 'TUTOR') {
                
                console.log(user);
                setRoleIdd(user.roleId)
                setFirstName(user.firstName);
                setLastName(user.lastName);
                setAge(user.tutorAge);
                setEmail(user.tutorEmail);
                setAvatar(user.tutorPicture);
                setNationality(user.nationality);
                setDescription(user.tutorDescription);
            } else if (user.roleId === 'LEARNER') {
                
                console.log(user);
                setRoleIdd(user.roleId)
                setFirstName(user.firstName);
                setLastName(user.lastName);
                setAge(user.learnerAge);
                setEmail(user.learnerEmail);
                setAvatar('https://villagesonmacarthur.com/wp-content/uploads/2020/12/Blank-Avatar.png');
                setNationality(null);
            }

        }
    }, [user])


    const handlePreviewImage = (e) => {
        const file = e.target.files[0];

        file.preview = URL.createObjectURL(file);
        setAvatar(file)
    }
    const handEditProfile = async () => {
        const confirmed = window.confirm('Are you sure to edit this course?');
        if (roleIdd === '')
            if (user.roleId === 'TUTOR') {
                
                console.log(user);
                setRoleIdd(user.roleId)
                setFirstName(user.firstName);
                setLastName(user.lastName);
                setAge(user.tutorAge);
                setEmail(user.tutorEmail);
                setAvatar(user.tutorPicture);
                setNationality(user.nationality);
                setDescription(user.tutorDescription);
            } else if (user.roleId === 'LEARNER') {
                
                console.log(user);
                setRoleIdd(user.roleId)
                setFirstName(user.firstName);
                setLastName(user.lastName);
                setAge(user.learnerAge);
                setEmail(user.learnerEmail);
                setAvatar('https://villagesonmacarthur.com/wp-content/uploads/2020/12/Blank-Avatar.png');
                setNationality(null);
            }

    }

    return (
        <>
            <Button  onClick={handleShow} className='editprofile'>
                Edit Profile
            </Button>

            <Modal
                show={show}
                onHide={handleClose}
                backdrop="static"
                keyboard={false}
            >
                <Modal.Header closeButton>
                    <Modal.Title>Edit Profile</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <div className='body-add-new'>

                        {/* <div className='text'>Description</div>
                        <input type='text' placeholder='Course description' className='form-control' value={desc} onChange={(event) => setDesc(event.target.value)} />


                        <div className='text'>Picture</div>
                        <div>
                            <input
                                type='file' onChange={handlePreviewImage}
                            />
                            {image && (
                                <img src={image.preview} alt='' width="50%" />
                            )}
                        </div> */}
                        <div className='text'>First Name</div>
                        <input type='text' placeholder='First Name' className='form-control' value={firstName} onChange={(event) => setFirstName(event.target.value)} />
                        <div className='text'>Last Name</div>
                        <input type='text' placeholder='Last Name' className='form-control' value={lastName} onChange={(event) => setLastName(event.target.value)} />



                        <div className='text'>Avatar</div>
                        <div>
                            <input
                                type='file' onChange={handlePreviewImage}
                            />
                            {avatar && (
                                <img src={avatar.preview} alt='' width="50%" />
                            )}
                        </div>

                    </div>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>
                        Close
                    </Button>
                    <Button variant="primary" onClick={() => handEditProfile()}>Edit Profile</Button>
                </Modal.Footer>
            </Modal>
        </>
    );
}

export default EditProfile;