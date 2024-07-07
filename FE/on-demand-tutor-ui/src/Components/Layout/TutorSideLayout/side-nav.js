import { Link as RouterLink, matchPath, useLocation } from 'react-router-dom';
import { Drawer, List, ListItem, ListItemIcon, ListItemText } from '@mui/material';
import { items } from './config';
import { useState } from 'react';
import Tab from 'react-bootstrap/Tab';
import Tabs from 'react-bootstrap/Tabs';

const SIDE_NAV_WIDTH = 250;
const TOP_NAV_HEIGHT = 90;

export const SideNav = () => {

  const [key, setKey] = useState('schedule');
  const location = useLocation();

  return (
    <Drawer
      open
      variant="permanent"
      PaperProps={{
        sx: {
          backgroundColor: 'background.default',
          display: 'flex',
          flexDirection: 'column',
          height: `calc(100% - ${TOP_NAV_HEIGHT + 15}px)`,
          p: 1,
          top: TOP_NAV_HEIGHT,
          width: SIDE_NAV_WIDTH,
          zIndex: (theme) => theme.zIndex.appBar - 100,
          left: '15px',
          borderRadius: '10px',
          boxShadow: 'rgba(0, 0, 0, 0.24) 0px 3px 8px'
        }
      }}
    >
      <List sx={{ width: '100%', top: '10px' }}>
        {items.map((item) => {
          const active = matchPath({ path: item.href, end: true }, location.pathname);

          return (
            <ListItem
              eventKey={item.key}
              disablePadding
              component={RouterLink}
              key={item.href}
              to={item.href}
              sx={{
                flexDirection: 'column',
                px: 2,
                py: 1.5
              }}
            >
              <div>
                <ListItemIcon
                  sx={{
                    minWidth: 'auto',
                    color: active ? 'primary.main' : 'neutral.400',
                  }}
                >
                  {item.icon}
                </ListItemIcon>
              </div>
            </ListItem>
          );
        })}
      </List>
    </Drawer >
  );
};
