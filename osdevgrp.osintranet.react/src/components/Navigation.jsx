import { useContext } from 'react';
import { Link } from 'react-router';
import { HelperContext } from '../contexts/HelperContext';
import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import NavDropdown from 'react-bootstrap/NavDropdown';

function buildStartNavLink(layoutContext, staticTextHelper) {
    return (
        <Nav.Link as={Link} to='/'>{staticTextHelper.getStartText(layoutContext.staticTexts)}</Nav.Link>
    );
}

function buildAccountingCreatorNavigationContent(layoutContext, authorizationHelper, staticTextHelper) {
    if (authorizationHelper.isAccountingCreator(layoutContext.userInfo) === false) {
        return (
            <>
            </>
        );
    }

    return (
        <>
            <NavDropdown.Divider />
            <NavDropdown.Item as={Link} to={`/accountings/add`}>{staticTextHelper.getCreateNewAccountingText(layoutContext.staticTexts)}</NavDropdown.Item>
        </>
    );
}

function buildAccountingsNavigationContent(layoutContext, authorizationHelper) {
    if (layoutContext.userInfo.accountings === undefined || layoutContext.userInfo.accountings === null || layoutContext.userInfo.accountings.length === 0) {
        return (
            <>
            </>
        );
    }

    const accountingsNavigationContent = layoutContext.userInfo.accountings.map(accounting => {
        if (authorizationHelper.isAccountingViewer(layoutContext.userInfo, accounting.number) === false) {
            return (
                <>
                </>
            );
        }

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
    if (authorizationHelper.hasAccountingAccess(layoutContext.userInfo) === false) {
        return (
            <>
            </>
        );
    }

    return (
        <NavDropdown title={staticTextHelper.getFinancialManagementText(layoutContext.staticTexts)} id='navbarFinancialManagementDropdown'>
            <NavDropdown.Item as={Link} to='/accountings'>{staticTextHelper.getAccountingsText(layoutContext.staticTexts)}</NavDropdown.Item>
            {buildAccountingCreatorNavigationContent(layoutContext, authorizationHelper, staticTextHelper)}
            {buildAccountingsNavigationContent(layoutContext, authorizationHelper)}
        </NavDropdown>
    );
}

function buildPrimaryNavigationContent(layoutContext, authorizationHelper, staticTextHelper) {
    if (authorizationHelper.authenticatedUser(layoutContext.userInfo)) {
        return (
            <Nav className='me-auto'>
                {buildStartNavLink(layoutContext, staticTextHelper)}
                {buildFinancialManagementNavigationContent(layoutContext, authorizationHelper, staticTextHelper)}
            </Nav>
        );
    }

    return (
        <Nav className='me-auto'>
            {buildStartNavLink(layoutContext, staticTextHelper)}
        </Nav>
    );
}

function buildSecondaryNavigationContent(layoutContext, authorizationHelper, staticTextHelper) {
    if (authorizationHelper.authenticatedUser(layoutContext.userInfo)) {
        return (
            <Nav className='justify-content-end'>
                <Nav.Link as={Link} to='/security/userinfo'>{layoutContext.userInfo.name}</Nav.Link>
                <Nav.Link as={Link} to='/security/logout'>{staticTextHelper.getLogoutText(layoutContext.staticTexts)}</Nav.Link>
            </Nav>
        );
    }

    return (
        <Nav className='justify-content-end'>
            <Nav.Link as={Link} to='/security/login'>{staticTextHelper.getLoginText(layoutContext.staticTexts)}</Nav.Link>
        </Nav>
    );
}

function Navigation({ layoutContext }) {
    const helperContext = useContext(HelperContext);
    const authorizationHelper = helperContext.authorizationHelper;
    const staticTextHelper = helperContext.staticTextHelper;

    return (
        <header>
            <Navbar expand='md lg xl xxl' className='bg-body-tertiary'>
                <Container>
                    <Navbar.Brand as={Link} to='/'>{layoutContext.title}</Navbar.Brand>
                    <Navbar.Toggle aria-controls='layout-navbar-nav' />
                    <Navbar.Collapse id='layout-navbar-nav'>
                        {buildPrimaryNavigationContent(layoutContext, authorizationHelper, staticTextHelper)}
                        {buildSecondaryNavigationContent(layoutContext, authorizationHelper, staticTextHelper)}
                    </Navbar.Collapse>
                </Container>
            </Navbar>
        </header>
    );
}

export default Navigation;