import { Button, Dropdown } from 'react-bootstrap';
import { Link as RouterLink, useNavigate } from 'react-router-dom';
import { Box, Link, Stack } from '@mui/material';
import images from '../../../assets/images';
import { BorderBottom, PersonCircle } from 'react-bootstrap-icons'
import logoutAPI from '../../../api/logoutAPI';

const TOP_NAV_HEIGHT = 85;

function TopNav() {

  const navigate = useNavigate();

  const HandleLogOut = async () => {
    localStorage.removeItem('token');
    localStorage.removeItem('email');
    let res = await logoutAPI();
    console.log(res);
    navigate("/")
  }

  return (
    <Box
      component="header"
      sx={{
        backgroundColor: 'white',
        color: 'common.white',
        position: 'fixed',
        width: '100%',
        zIndex: (theme) => theme.zIndex.appBar,
        boxShadow: 'rgba(0, 0, 0, 0.24) 0px 3px 3px'
      }}
    >
      <Stack
        direction="row"
        justifyContent="space-between"
        sx={{
          minHeight: TOP_NAV_HEIGHT,
          px: 3
        }}
      >
        <Stack
          alignItems="center"
          direction="row"
          spacing={3}
        >
          <Box
            component={RouterLink}
            to="/"
            sx={{
              display: 'inline-flex',
              height: 24,
              width: 24
            }}
          >
            <img src={images.logo} alt="OnDemandTutorLogo" />
          </Box>
        </Stack>
        <Stack
          alignItems="center"
          direction="row"
          spacing={2}
        >
          <Dropdown align="" className='log-out-button'>
            <Button
              className="loginButton text-black border border-2 border-dark"
              variant=""
              onClick={HandleLogOut}
              style={{
                width: '90%',
                position: 'relative',
                float: 'inline-end',
                marginRight: '10px'
              }}>
              <span className="loginContent">Log Out</span>
            </Button>
          </Dropdown>
        </Stack>
      </Stack>
    </Box>
  );
}

export default TopNav;
