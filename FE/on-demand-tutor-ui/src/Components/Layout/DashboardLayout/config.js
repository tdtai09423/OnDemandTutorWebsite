import ChartPieIcon from "@heroicons/react/24/solid/ChartPieIcon"
import UserCircleIcon from "@heroicons/react/24/solid/UserCircleIcon"
import ShoppingCartIcon from "@heroicons/react/24/solid/ShoppingCartIcon"
import CheckBadgeIcon from "@heroicons/react/24/solid/CheckBadgeIcon"
import { SvgIcon } from '@mui/material';
export const items = [
  {
    href: '/admin-dash-board',
    icon: (
      <SvgIcon>
        <ChartPieIcon />
      </SvgIcon>
    ),
    label: 'Revenue'
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
    href: '/admin-dash-board-certificate',
    icon: (
      <SvgIcon>
        <CheckBadgeIcon />
      </SvgIcon>
    ),
    label: 'Certificate'
  }
];
