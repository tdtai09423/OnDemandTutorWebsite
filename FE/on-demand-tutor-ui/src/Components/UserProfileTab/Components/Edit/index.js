
import { useEffect, useState } from 'react';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import axios from 'axios';
import { toast } from 'react-toastify';
const EditProfile = ({ userInformation }) => {

    console.log("userInformation>>>>>>>>>>>>>>>>", userInformation);
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [age, setAge] = useState('');
    const [email, setEmail] = useState('');
    const [avatar, setAvatar] = useState('');
    const [password, setPassword] = useState('');
    const [nationality, setNationality] = useState('');
    const [description, setDescription] = useState('');
    const [newPassword, setNewPassword] = useState('');
    const [confirmNewPassword, setConfirmNewPassword] = useState('');


    const [show, setShow] = useState(false);
    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);
    useEffect(() => {

        if (userInformation.roleId === 'TUTOR') {
            setFirstName(userInformation.firstName);
            setLastName(userInformation.lastName);
            setAge(userInformation.age);
            setEmail(userInformation.email);
            setAvatar(userInformation.avatar);
            setNationality(userInformation.nationality);
            setDescription(userInformation.description);
            console.log("userID>>>>>>>>>", userInformation.id)
            console.log("tutorIn4444444444", userInformation)

        } else if (userInformation.roleId === 'LEARNER') {
            setFirstName(userInformation.firstName);
            setLastName(userInformation.lastName);
            setAge(userInformation.age);
            setEmail(userInformation.email);
            setAvatar(userInformation.avatar);
            setDescription(null);
            setNationality(null);
            console.log("userID>>>>>>>>>", userInformation.id)
            console.log("learnerIn4444444444", userInformation)
        }




    }, [userInformation])


    const handlePreviewImage = (e) => {
        const file = e.target.files[0];

        file.preview = URL.createObjectURL(file);
        setAvatar(file)
    }
    const handEditProfile = async () => {
        const confirmed = window.confirm('Are you sure to edit your profile?');
        if (confirmed) {

            if (userInformation.roleId === 'TUTOR') {
                let formData = new FormData();
                formData.append('Age', age);
                formData.append('FirstName', firstName);
                formData.append('LastName', lastName);
                // formData.append('Email', email);
                formData.append('PasswordModel.CurrentPassword', password);
                formData.append('PasswordModel.Password', newPassword);
                formData.append('PasswordModel.ConfirmPassword', confirmNewPassword);
                formData.append('Nationality', nationality);
                formData.append('Description', description);
                formData.append('Image', avatar)
                console.log(formData);
                let url = 'https://localhost:7010/api/Account/update-tutor?accountId=' + userInformation.id;
                let res = await axios.put(url, formData, {
                    headers: {
                        'Content-Type': 'multipart/form-data',
                    },
                });
                toast.success(res.message);

            } else if (userInformation.roleId === 'LEARNER') {
                console.log("learnerneeee")
                let formData = new FormData();
                formData.append('Age', age);

                avatar ? formData.append('Image', avatar) : formData.append('Image', '')
                formData.append('FirstName', firstName);
                formData.append('LastName', lastName);
                // formData.append('Email', email);
                password ? formData.append('PasswordModel.CurrentPassword', password) : formData.append('PasswordModel.CurrentPassword', '');
                newPassword ? formData.append('PasswordModel.Password', newPassword) : formData.append('PasswordModel.Password', '');
                confirmNewPassword ? formData.append('PasswordModel.ConfirmPassword', confirmNewPassword) : formData.append('PasswordModel.CurrentPassword', '');
                for (const [key, value] of formData) {
                    console.log("formdata>>><<<>><><><>", `${key}: ${value}`);
                }

                try {
                    let url = 'https://localhost:7010/api/Account/update-learner?accountId=' + userInformation.id;
                    let res = await axios.put(url, formData, {
                        headers: {
                            'Content-Type': 'multipart/form-data',
                        },
                    });
                } catch (error) {
                    toast.error(error.message);
                    console.error("Edit profile error<><><><><><>:", error);
                }


            }


        }
        window.location.reload();

    }

    return (
        <>
            <Button onClick={handleShow} className='editprofile'>
                Edit Profile
            </Button>

            <Modal
                style={{ marginTop: '90px' }}
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
                        {/* <div className='text'>Email</div>
                        <input type='text' placeholder='Email' className='form-control' value={email} onChange={(event) => setEmail(event.target.value)} /> */}
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