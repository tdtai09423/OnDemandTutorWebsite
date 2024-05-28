import Home from "../pages/Home";
import Following from "../pages/Following";
import TutorDetail from "../pages/TutorDetail";
import Login from "../pages/Login";
import SignUpStudent from "../pages/SignUpStudent";
import SignUpTutor from "../pages/SignUpTutor";
import UserProfile from "../pages/UserProfile";

//Public Route
const publicRoute = [
    { path: '/', component: Home },
    { path: '/following', component: Following, layout: null },
    { path: '/tutor-detail', component: TutorDetail },
    { path: '/login', component: Login },
    { path: '/sign-up-student', component: SignUpStudent },
    { path: '/sign-up-tutor', component: SignUpTutor },
    { path: '/user-profile', component: UserProfile },
];

const privateRoute = [
];

export { publicRoute, privateRoute };