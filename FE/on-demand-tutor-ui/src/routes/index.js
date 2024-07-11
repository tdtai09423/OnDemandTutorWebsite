import Home from "../pages/Home";
import Following from "../pages/Following";
import TutorDetail from "../pages/TutorDetail";
import Login from "../pages/Login";
import SignUpStudent from "../pages/SignUpStudent";
import SignUpTutor from "../pages/SignUpTutor";
import SendReport from "../pages/SendReport";
import Policy from "../pages/Policy";
import AdminDashBoard from "../pages/AdminDashBoard";
import UserProfile from "../pages/UserProfile";
import DashboardLayout from '../Components/Layout/DashboardLayout/index.js'
import AdminOrder from '../pages/AdminDashBoard/Components/pages/order.js'
import VerifyCode from "../pages/VerifyCode/index.js";
import AdminAccount from '../pages/AdminDashBoard/Components/pages/account.js'
import Certificate from "../pages/AdminDashBoard/Components/pages/certificate.js";
import OrderHistory from "../pages/OrderHistory/index.js";
import FavoriteTutor from "../pages/FavoriteTutor/index.js";
import Payment from "../pages/Payment/index.js";
import PaymentSuccess from "../pages/PaymentSuccess/index.js";
import OrderListTutor from "../pages/OrderListTutor/index.js";
import ForgotPassword from "../pages/ForgotPassword/index.js";
import VerifyAccount from "../pages/ForgotPassword/VerifyAccount/index.js";
import ChangePassword from "../pages/ForgotPassword/ChangePassword/index.js";
import PersonalSchedule from "../pages/PersonalSchedule/index.js";
import TutorSideLayout from "../Components/Layout/TutorSideLayout/index.js";
import TutorSchedule from "../pages/TutorHomePage/Components/pages/TutorSchedule.js";
import TutorProfile from "../pages/TutorHomePage/Components/pages/TutorProfile.js";
import TutorNotification from "../pages/TutorHomePage/Components/pages/TutorNotification.js";
import TutorRevenue from "../pages/TutorHomePage/Components/pages/TutorRevenue.js";
import TutorProfilePage from "../pages/TutorHomePage/Components/pages/TutorProfilePage.js";
import ChatWindow from "../Components/ChatWindow/chat-window.js";

const publicRoute = [
    { path: '/', component: Home },
    { path: '/following', component: Following },
    { path: '/tutor-detail', component: TutorDetail },
    { path: '/login', component: Login, layout: null },
    { path: '/sign-up-student', component: SignUpStudent, layout: null },
    { path: '/sign-up-tutor', component: SignUpTutor, layout: null },
    { path: '/send-report', component: SendReport, layout: null },
    { path: '/policy', component: Policy, layout: null },
    { path: '/admin-dash-board', component: AdminDashBoard, layout: DashboardLayout, role: 'ADMIN' },
    { path: '/admin-dash-board-order', component: AdminOrder, layout: DashboardLayout },
    { path: '/admin-dash-board-account', component: AdminAccount, layout: DashboardLayout },
    { path: '/admin-dash-board-certificate', component: Certificate, layout: DashboardLayout },
    { path: '/user-profile', component: UserProfile },
    { path: '/tutor-profile', component: UserProfile, layout: TutorSideLayout },
    { path: '/verify-code', component: VerifyCode },
    { path: '/order-history', component: OrderHistory },
    { path: '/favorite-tutor', component: FavoriteTutor },
    { path: '/payment', component: Payment },
    { path: '/payment-success', component: PaymentSuccess },
    { path: '/order-list', component: OrderListTutor, layout: TutorSideLayout },
    { path: '/forgot-password', component: ForgotPassword },
    { path: '/verify-account', component: VerifyAccount },
    { path: '/change-password', component: ChangePassword },
    { path: '/personal-schedule', component: PersonalSchedule },
    { path: '/tutor-page', component: TutorSchedule, layout: TutorSideLayout, role: 'TUTOR' },
    { path: '/tutor-notificattion', component: TutorNotification, layout: TutorSideLayout, role: 'TUTOR' },
    { path: '/tutor-revenue', component: TutorRevenue, layout: TutorSideLayout, role: 'TUTOR' },
    { path: '/tutor-profile-page', component: TutorProfilePage, layout: TutorSideLayout, role: 'TUTOR' },
    { path: '/chat-learner', component: ChatWindow },
    { path: '/chat-tutor', component: ChatWindow, layout: TutorSideLayout, role: 'TUTOR' }
];

const privateRoute = [
];

export { publicRoute, privateRoute };