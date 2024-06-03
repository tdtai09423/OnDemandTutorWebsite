import apiClient from './apiClient';

const SignUpStudentAPI = (firstName,lastName,email, password,confirmPassword,learnerAge,learnerPicture) => {
    console.log("goi api ");
    return apiClient.post("/api/Account/learner-register", { firstName,lastName,email, password,confirmPassword,learnerAge,learnerPicture});
}

export default SignUpStudentAPI;