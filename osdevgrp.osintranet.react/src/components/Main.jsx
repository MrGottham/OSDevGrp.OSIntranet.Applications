import { useContext } from 'react';
import { Routes, Route, Outlet } from 'react-router';
import { HelperContext } from '../contexts/HelperContext';
import Container from 'react-bootstrap/Container';
import AccessDenied from './AccessDenied'
import Login from './Login';
import Logout from './Logout';
import UserInfo from './UserInfo';
import Accountings from './Accountings';
import Accounting from './Accounting'

function Main({ layoutContext, children }) {
    const authorizationHelper = useContext(HelperContext).authorizationHelper;

    return (
        <main className="flex-shrink-0">
            <Container>
                <Routes>
                    <Route path="/" element={<Outlet />}>
                        <Route index element={children} />
                        <Route path="security/login" element={authorizationHelper.authenticatedUser(layoutContext) == false ? <Login /> : <AccessDenied />} />
                        <Route path="security/logout" element={authorizationHelper.authenticatedUser(layoutContext) ? <Logout /> : <AccessDenied />} />
                        <Route path="security/userinfo" element={authorizationHelper.authenticatedUser(layoutContext) ? <UserInfo /> : <AccessDenied />} />
                        <Route path="accountings" element={authorizationHelper.hasAccountingAccess(layoutContext) ? <Accountings /> : <AccessDenied />} />
                        <Route path="accountings/:accountingNumber" element={authorizationHelper.hasAccountingAccess(layoutContext) ? <Accounting /> : <AccessDenied />} />
                    </Route>
                </Routes>
            </Container>
        </main>
    );
}

export default Main;