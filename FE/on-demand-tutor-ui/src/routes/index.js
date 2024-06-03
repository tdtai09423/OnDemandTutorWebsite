import Home from "../pages/Home";
import Following from "../pages/Following";
import TutorDetail from "../pages/TutorDetail";
import Login from "../pages/Login";
import SignUpStudent from "../pages/SignUpStudent";
import SignUpTutor from "../pages/SignUpTutor";
import TutorProfile from "../pages/TutorProfile";
import SendReport from "../pages/SendReport";
import Policy from "../pages/Policy";
import AdminDashBoard from "../pages/AdminDashBoard";

//Public Route
const publicRoute = [
    { path: '/', component: Home },
    { path: '/following', component: Following },
    { path: '/tutor-detail', component: TutorDetail },
    { path: '/login', component: Login },
    { path: '/sign-up-student', component: SignUpStudent },
    { path: '/sign-up-tutor', component: SignUpTutor },
    { path: '/tutor-profile', component: TutorProfile },
    { path: '/send-report', component: SendReport, layout: null },
    { path: '/policy', component: Policy, layout: null },
    { path: '/admin-dash-board', component: AdminDashBoard, layout: null },


];

const privateRoute = [
];

export { publicRoute, privateRoute };