import { SvgIcon } from '@mui/material';
import ScheduleCard from './components/schedule-card.js'
import ProfileCard from './components/profile-card.js'
import NotificationCard from './components/notification-card.js'
import OrderListCard from './components/order-list.js';
import TutorRevenueCard from './components/tutor-revenue-card.js';
import TutorProfilePageCard from './components/tutor-page-card.js';

export const items = [
  {
    href: '/tutor-page',
    icon: (
      <ScheduleCard />
    ),
    key: 'schedule'
  },
  {
    href: '/tutor-profile',
    icon: (
      <ProfileCard />
    ),
    key: 'profile'
  },
  {
    href: '/tutor-notificattion',
    icon: (
      <NotificationCard />
    ),
    key: 'notification'
  }, {
    href: '/order-list',
    icon: (
      <OrderListCard />
    ),
    key: 'notification'
  },
  {
    href: '/tutor-revenue',
    icon: (
      <TutorRevenueCard />
    ),
    key: 'revenue'
  },
  {
    href: '/tutor-profile-page',
    icon: (
      <TutorProfilePageCard />
    ),
    key: 'tutorProfilePage'
  }
];
