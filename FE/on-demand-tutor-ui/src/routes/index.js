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
import UserProfile from "../pages/UserProfile";
import DashboardLayout from '../Components/Layout/DashboardLayout/index.js'
import AdminOrder from '../pages/AdminDashBoard/Components/pages/order.js'
import VerifyCode from "../pages/VerifyCode/index.js";
import AdminAccount from '../pages/AdminDashBoard/Components/pages/account.js'
import Certificate from "../pages/AdminDashBoard/Components/pages/certificate.js";
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
    { path: '/admin-dash-board', component: AdminDashBoard, layout: DashboardLayout },
    { path: '/admin-dash-board-order', component: AdminOrder, layout: DashboardLayout },
    { path: '/admin-dash-board-account', component: AdminAccount, layout: DashboardLayout },
    { path: '/admin-dash-board-certificate', component: Certificate, layout: DashboardLayout },
    { path: '/user-profile', component: UserProfile },
    { path: '/verify-code', component: VerifyCode },
    { path: '/order-history', component: OrderHistory },



];

const privateRoute = [
];

export { publicRoute, privateRoute };