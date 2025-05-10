import { useContext } from 'react';
import { HelperContext } from '../contexts/HelperContext';
import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';

function buildStartNavLink(layoutContext, staticTextHelper) {
    return (
        <Nav.Link href="#home">{staticTextHelper.getStaticTextByKey(layoutContext.staticTexts, 'Start')}</Nav.Link>
    );
}

function buildPrimaryNavigationContent(layoutContext, staticTextHelper) {
    if (layoutContext.userInfo) {
        return (
            <Nav className="me-auto">
                {buildStartNavLink(layoutContext, staticTextHelper)}
            </Nav>
        );
    }

    return (
        <Nav className="me-auto">
            {buildStartNavLink(layoutContext, staticTextHelper)}
        </Nav>
    );
}

function buildSecondaryNavigationContent(layoutContext, staticTextHelper) {
    if (layoutContext.userInfo) {
        return (
            <Nav className="justify-content-end">
                <Nav.Link href="#userInfo">{layoutContext.userInfo.name}</Nav.Link>
                <Nav.Link href="#logout">{staticTextHelper.getStaticTextByKey(layoutContext.staticTexts, 'Logout')}</Nav.Link>
            </Nav>
        );
    }

    return (
        <Nav className="justify-content-end">
            <Nav.Link href="#login">{staticTextHelper.getStaticTextByKey(layoutContext.staticTexts, 'Login')}</Nav.Link>
        </Nav>
    );
}

function Navigation({ layoutContext }) {
    const staticTextHelper = useContext(HelperContext).staticTextHelper;

    return (
        <header>
            <Navbar expand="md lg xl xxl" className="bg-body-tertiary">
                <Container>
                    <Navbar.Brand href="#home">{layoutContext.title}</Navbar.Brand>
                    <Navbar.Toggle aria-controls="layout-navbar-nav" />
                    <Navbar.Collapse id="layout-navbar-nav">
                        {buildPrimaryNavigationContent(layoutContext, staticTextHelper)}
                        {buildSecondaryNavigationContent(layoutContext, staticTextHelper)}
                    </Navbar.Collapse>
                </Container>
            </Navbar>
        </header>
    );
}

export default Navigation;