import apiClient from './apiClient';

const SignUptutorAPI = (firstName,lastName,email, password,confirmPassword,tutorAge,nationality, tutorDescription, tutorPicture,majorId,certificateLink) => {
    console.log("goi api ");
    return apiClient.post("/Account/tutor-register", { firstName,lastName,email, password,confirmPassword,tutorAge,nationality, tutorDescription, tutorPicture,majorId,certificateLink});
}

export default SignUptutorAPI;