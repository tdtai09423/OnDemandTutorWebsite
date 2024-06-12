import apiClient from './apiClient';

// const SignUpStudentAPI = (learnerAge,learnerPicture,firstName,lastName,email, password,confirmPassword) => {
//     console.log("goi api ");

//     return apiClient.post("/api/Account/learner-register", { learnerAge,learnerPicture,firstName,lastName,email, password,confirmPassword});
// }
const SignUpStudentAPI = (FormData) => {
    console.log("goi api ");

    return apiClient.post("/Account/learner-register", { FormData });
}

export default SignUpStudentAPI;