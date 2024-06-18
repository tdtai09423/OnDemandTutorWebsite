import { styled } from '@mui/material/styles';
import { Footer } from './footer';
import { SideNav } from './side-nav';
import { TopNav } from './top-nav';

const SIDE_NAV_WIDTH = 75;
const TOP_NAV_HEIGHT = 64;

const LayoutRoot = styled('div')(({ theme }) => ({
    backgroundColor: theme.palette.background.default,
    display: 'flex',
    flex: '1 1 auto',
    maxWidth: '100%',
    paddingTop: TOP_NAV_HEIGHT,
    [theme.breakpoints.up('lg')]: {
        paddingLeft: SIDE_NAV_WIDTH
    }
}));

const LayoutContainer = styled('div')({
    display: 'flex',
    flex: '1 1 auto',
    flexDirection: 'column',
    width: '100%'
});

function DashboardLayout({ children }) {

    return (
        <>
            <TopNav />
            <SideNav />
            <LayoutRoot>
                <LayoutContainer>
                    {children}
                    <Footer />
                </LayoutContainer>
            </LayoutRoot>
        </>
    );
};

export default DashboardLayout;
