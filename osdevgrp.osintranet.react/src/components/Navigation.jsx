import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import { getStaticTextByKey } from '../helpers/StaticTextHelper';

function buildStartNavLink(layoutContext) {
    return (
        <Nav.Link href="#home">{getStaticTextByKey(layoutContext.staticTexts, 'Start')}</Nav.Link>
    );
}

function buildPrimaryNavigationContent(layoutContext) {
    if (layoutContext.userInfo) {
        return (
            <Nav className="me-auto">
                {buildStartNavLink(layoutContext)}
            </Nav>
        );
    }

    return (
        <Nav className="me-auto">
            {buildStartNavLink(layoutContext)}
        </Nav>
    );
}

function buildSecondaryNavigationContent(layoutContext) {
    if (layoutContext.userInfo) {
        return (
            <Nav className="justify-content-end">
                <Nav.Link href="#userInfo">{layoutContext.userInfo.name}</Nav.Link>
                <Nav.Link href="#logout">{getStaticTextByKey(layoutContext.staticTexts, 'Logout')}</Nav.Link>
            </Nav>
        );
    }

    return (
        <Nav className="justify-content-end">
            <Nav.Link href="#login">{getStaticTextByKey(layoutContext.staticTexts, 'Login')}</Nav.Link>
        </Nav>
    );
}

function Navigation({ layoutContext }) {
    return (
        <header>
            <Navbar expand="md lg xl xxl" className="bg-body-tertiary">
                <Container>
                    <Navbar.Brand href="#home">{layoutContext.title}</Navbar.Brand>
                    <Navbar.Toggle aria-controls="layout-navbar-nav" />
                    <Navbar.Collapse id="layout-navbar-nav">
                        {buildPrimaryNavigationContent(layoutContext)}
                        {buildSecondaryNavigationContent(layoutContext)}
                    </Navbar.Collapse>
                </Container>
            </Navbar>
        </header>
    );
}

export default Navigation;