import apiClient from './apiClient';

const SignUpStudentAPI = (learnerAge,learnerPicture,firstName,lastName,email, password,confirmPassword) => {
    console.log("goi api ");
    
    return apiClient.post("/api/Account/learner-register", { learnerAge,learnerPicture,firstName,lastName,email, password,confirmPassword});
}

export default SignUpStudentAPI;