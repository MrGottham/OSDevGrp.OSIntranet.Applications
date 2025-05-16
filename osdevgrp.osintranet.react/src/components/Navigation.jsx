import { useContext } from 'react';
import { Link } from 'react-router';
import { HelperContext } from '../contexts/HelperContext';
import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import NavDropdown from 'react-bootstrap/NavDropdown';

function buildStartNavLink(layoutContext, staticTextHelper) {
    return (
        <Nav.Link as={Link} to="/">{staticTextHelper.getStaticTextByKey(layoutContext.staticTexts, 'Start')}</Nav.Link>
    );
}

function buildAccountingsNavigationContent(accountings) {
    if (accountings === undefined || accountings === null || accountings.length === 0) {
        return (
            <>
            </>
        );
    }

    const accountingsNavigationContent = accountings.map(accounting => {
        return (
            <NavDropdown.Item key={accounting.number} as={Link} to={`/accountings/${accounting.number}`}>{accounting.name}</NavDropdown.Item>
        );
    });

    return (
        <>
            <NavDropdown.Divider />
            {accountingsNavigationContent}
        </>
    );
}

function buildFinancialManagementNavigationContent(layoutContext, authorizationHelper, staticTextHelper) {
    if (authorizationHelper.hasAccountingAccess(layoutContext) === false) {
        return (
            <>
            </>
        );
    }

    return (
        <NavDropdown title={staticTextHelper.getStaticTextByKey(layoutContext.staticTexts, 'FinancialManagement')} id="navbarFinancialManagementDropdown">
            <NavDropdown.Item as={Link} to="/accountings">{staticTextHelper.getStaticTextByKey(layoutContext.staticTexts, 'Accountings')}</NavDropdown.Item>
            {buildAccountingsNavigationContent(layoutContext.userInfo.accountings)}
        </NavDropdown>
    );
}

function buildPrimaryNavigationContent(layoutContext, authorizationHelper, staticTextHelper) {
    if (authorizationHelper.authenticatedUser(layoutContext)) {
        return (
            <Nav className="me-auto">
                {buildStartNavLink(layoutContext, staticTextHelper)}
                {buildFinancialManagementNavigationContent(layoutContext, authorizationHelper, staticTextHelper)}
            </Nav>
        );
    }

    return (
        <Nav className="me-auto">
            {buildStartNavLink(layoutContext, staticTextHelper)}
        </Nav>
    );
}

function buildSecondaryNavigationContent(layoutContext, authorizationHelper, staticTextHelper) {
    if (authorizationHelper.authenticatedUser(layoutContext)) {
        return (
            <Nav className="justify-content-end">
                <Nav.Link href="/security/userinfo">{layoutContext.userInfo.name}</Nav.Link>
                <Nav.Link as={Link} to="/security/logout">{staticTextHelper.getStaticTextByKey(layoutContext.staticTexts, 'Logout')}</Nav.Link>
            </Nav>
        );
    }

    return (
        <Nav className="justify-content-end">
            <Nav.Link as={Link} to="/security/login">{staticTextHelper.getStaticTextByKey(layoutContext.staticTexts, 'Login')}</Nav.Link>
        </Nav>
    );
}

function Navigation({ layoutContext }) {
    const helperContext = useContext(HelperContext);
    const authorizationHelper = helperContext.authorizationHelper;
    const staticTextHelper = helperContext.staticTextHelper;

    return (
        <header>
            <Navbar expand="md lg xl xxl" className="bg-body-tertiary">
                <Container>
                    <Navbar.Brand as={Link} to="/">{layoutContext.title}</Navbar.Brand>
                    <Navbar.Toggle aria-controls="layout-navbar-nav" />
                    <Navbar.Collapse id="layout-navbar-nav">
                        {buildPrimaryNavigationContent(layoutContext, authorizationHelper, staticTextHelper)}
                        {buildSecondaryNavigationContent(layoutContext, authorizationHelper, staticTextHelper)}
                    </Navbar.Collapse>
                </Container>
            </Navbar>
        </header>
    );
}

export default Navigation;