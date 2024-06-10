import ChartPieIcon from "@heroicons/react/24/solid/ChartPieIcon"
import UserCircleIcon from "@heroicons/react/24/solid/UserCircleIcon"
import ExclamationTriangleIcon from "@heroicons/react/24/solid/ExclamationTriangleIcon"
import ShoppingCartIcon from "@heroicons/react/24/solid/ShoppingCartIcon"
import { SvgIcon } from '@mui/material';

export const items = [
  {
    href: '/admin-dash-board',
    icon: (
      <SvgIcon>
        <ChartPieIcon />
      </SvgIcon>
    ),
    label: 'Home'
  },
  {
    href: '/admin-dash-board-order',
    icon: (
      <SvgIcon>
        <ShoppingCartIcon />
      </SvgIcon>
    ),
    label: 'Orders'
  },
  {
    href: '/admin-dash-board-account',
    icon: (
      <SvgIcon>
        <UserCircleIcon />
      </SvgIcon>
    ),
    label: 'Account'
  },
  {
    href: '/admin-dash-board/404',
    icon: (
      <SvgIcon>
        <ExclamationTriangleIcon />
      </SvgIcon>
    ),
    label: 'Error'
  }
];
