import Home from "../pages/Home";
import Following from "../pages/Following";
import TutorDetail from "../pages/TutorDetail";
import Login from "../pages/Login";
import SignUpStudent from "../pages/SignUpStudent";
import SignUpTutor from "../pages/SignUpTutor";


//Public Route
const publicRoute = [
    { path: '/', component: Home },
    { path: '/following', component: Following },
    { path: '/tutor-detail', component: TutorDetail },
    { path: '/login', component: Login },
    { path: '/sign-up-student', component: SignUpStudent },
    { path: '/sign-up-tutor', component: SignUpTutor },

];

const privateRoute = [
];

export { publicRoute, privateRoute };