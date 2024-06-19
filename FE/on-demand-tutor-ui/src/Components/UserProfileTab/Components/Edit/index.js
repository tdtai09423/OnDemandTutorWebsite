
import { useEffect, useState } from 'react';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import learnerAPI from '../../../../api/learnerAPI';
import tutorAPI from '../../../../api/tutorAPI';
import axios from 'axios';
import { toast } from 'react-toastify';
const EditProfile = ({ user }) => {

    console.log("user>>>>>>>>>>>>>>>>", user);
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [age, setAge] = useState('');
    const [email, setEmail] = useState('');
    const [avatar, setAvatar] = useState('');
    const [password, setPassword] = useState('');
    const [nationality, setNationality] = useState('');
    const [description, setDescription] = useState('');
    const [roleIdd, setRoleIdd] = useState('');
    const [newPassword, setNewPassword] = useState('');
    const [confirmNewPassword, setConfirmNewPassword] = useState('');


    const [show, setShow] = useState(false);
    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);
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


    const handlePreviewImage = (e) => {
        const file = e.target.files[0];

        file.preview = URL.createObjectURL(file);
        setAvatar(file)
    }
    const handEditProfile = async () => {
        const confirmed = window.confirm('Are you sure to edit this course?');
        if (confirmed) {
            try {
                if (user.roleId === 'TUTOR') {
                    let formData = new FormData();
                    formData.append('Age', age);
                    formData.append('FirstName', firstName);
                    formData.append('LastName', lastName);
                    formData.append('Email', email);
                    formData.append('PasswordModel.CurrentPassword', password);
                    formData.append('PasswordModel.Password', newPassword);
                    formData.append('PasswordModel.ConfirmPassword', confirmNewPassword);
                    formData.append('Nationality', nationality);
                    formData.append('Description', description);
                    formData.append('Image', avatar)
                    console.log(formData);
                    let res = await axios.post("https://localhost:7010/api/Account/update-tutor", formData, {
                        headers: {
                            'Content-Type': 'multipart/form-data',
                        },
                    });
                    toast.success(res.message);

                } else if (user.roleId === 'LEARNER') {
                    console.log("learnerneeee")
                    let formData = new FormData();
                    formData.append('Age', age);
                    formData.append('FirstName', firstName);
                    formData.append('LastName', lastName);
                    formData.append('Email', email);
                    formData.append('PasswordModel.CurrentPassword', password);
                    formData.append('PasswordModel.Password', newPassword);
                    formData.append('PasswordModel.ConfirmPassword', confirmNewPassword);
                    formData.append('Image', avatar)
                    console.log("formdata>>><<<>><><><>", formData)
                    let res = await axios.post("https://localhost:7010/api/Account/update-learner", formData, {
                        headers: {
                            'Content-Type': 'multipart/form-data',
                        },
                    });
                    toast.success(res.message);
                }

            } catch (error) {
                toast.error(error.message);
                console.error("Edit profile error<><><><><><>:", error);
            }
        }

    }

    return (
        <>
            <Button onClick={handleShow} className='editprofile'>
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
                        <div className='text'>Age</div>
                        <input type='number' placeholder='Age' className='form-control' value={age} onChange={(event) => setAge(event.target.value)} />
                        <div className='text'>Email</div>
                        <input type='text' placeholder='Email' className='form-control' value={email} onChange={(event) => setEmail(event.target.value)} />
                        <div className='text'>Current Password</div>
                        <input type='password' placeholder='Current Password' className='form-control' value={password} onChange={(event) => setPassword(event.target.value)} />
                        <div className='text'>New Password</div>
                        <input type='password' placeholder='New Password' className='form-control' value={newPassword} onChange={(event) => setNewPassword(event.target.value)} />
                        <div className='text'>Confirm New Password</div>
                        <input type='password' placeholder='Confirm New Password' className='form-control' value={confirmNewPassword} onChange={(event) => setConfirmNewPassword(event.target.value)} />

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