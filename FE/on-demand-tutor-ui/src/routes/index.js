import Home from "../pages/Home";
import TutorDetail from "../pages/TutorDetail";
import Following from "../pages/Following"

//Public Route
const publicRoute = [
    { path: '/', component: Home },
    { path: '/following', component: Following, layout: null },
    { path: '/tutor-detail', component: TutorDetail },
];

const privateRoute = [
];

export { publicRoute, privateRoute };