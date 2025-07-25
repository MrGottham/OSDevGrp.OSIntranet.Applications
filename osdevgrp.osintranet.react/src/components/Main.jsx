import { useContext } from 'react';
import { Routes, Route, Outlet } from 'react-router';
import { HelperContext } from '../contexts/HelperContext';
import Container from 'react-bootstrap/Container';
import NotImplemented from './NotImplemented';
import AccessDenied from './AccessDenied';
import Login from './Login';
import Logout from './Logout';
import UserInfo from './UserInfo';
import Accountings from './Accountings';
import Accounting from './Accounting';
import EditAccounting from './EditAccounting';

function Main({ layoutContext, children }) {
    const authorizationHelper = useContext(HelperContext).authorizationHelper;

    return (
        <main className='flex-shrink-0'>
            <Container>
                <Routes>
                    <Route path='/' element={<Outlet />}>
                        <Route index element={children} />
                        <Route path='home/notimplemented' element={<NotImplemented />} />
                        <Route path='security/login' element={authorizationHelper.authenticatedUser(layoutContext.userInfo) == false ? <Login /> : <AccessDenied />} />
                        <Route path='security/logout' element={authorizationHelper.authenticatedUser(layoutContext.userInfo) ? <Logout /> : <AccessDenied />} />
                        <Route path='security/userinfo' element={authorizationHelper.authenticatedUser(layoutContext.userInfo) ? <UserInfo /> : <AccessDenied />} />
                        <Route path='accountings' element={authorizationHelper.hasAccountingAccess(layoutContext.userInfo) ? <Accountings /> : <AccessDenied />} />
                        <Route path='accountings/add' element={authorizationHelper.isAccountingCreator(layoutContext.userInfo) ? <NotImplemented /> : <AccessDenied />} />
                        <Route path='accountings/:accountingNumber' element={authorizationHelper.isAccountingViewer(layoutContext.userInfo) ? <Accounting /> : <AccessDenied />} />
                        <Route path='accountings/:accountingNumber/edit' element={authorizationHelper.isAccountingModifier(layoutContext.userInfo) ? <EditAccounting /> : <AccessDenied />} />
                        <Route path='accountings/:accountingNumber/remove' element={authorizationHelper.isAccountingModifier(layoutContext.userInfo) ? <NotImplemented /> : <AccessDenied />} />
                    </Route>
                </Routes>
            </Container>
        </main>
    );
}

export default Main;