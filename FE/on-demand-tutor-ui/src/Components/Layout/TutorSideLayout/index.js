import { styled } from '@mui/material/styles';
import { Footer } from './footer.js';
import { SideNav } from './side-nav.js';
import TopNav from './top-nav.js'

const SIDE_NAV_WIDTH = 250;
const TOP_NAV_HEIGHT = 90;

const LayoutRoot = styled('div')(({ theme }) => ({
    backgroundColor: theme.palette.background.default,
    display: 'flex',
    flex: '1 1 auto',
    maxWidth: '100%',
    paddingTop: TOP_NAV_HEIGHT,
    [theme.breakpoints.up('lg')]: {
        paddingLeft: SIDE_NAV_WIDTH + 50,
        paddingRight: 40
    }
}));

const LayoutContainer = styled('div')({
    display: 'flex',
    flex: '1 1 auto',
    flexDirection: 'column',
    width: '100%'
});

function TutorSideLayout({ children }) {

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

export default TutorSideLayout;
