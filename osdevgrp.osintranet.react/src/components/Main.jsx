import { Routes, Route, Outlet } from 'react-router';
import Container from 'react-bootstrap/Container';
import Login from './Login';
import Logout from './Logout';
import UserInfo from './UserInfo';

function Main({ children }) {
    return (
        <main className="flex-shrink-0">
            <Container>
                <Routes>
                    <Route path="/" element={<Outlet />}>
                        <Route index element={children} />
                        <Route path="security/login" element={<Login />} />
                        <Route path="security/logout" element={<Logout />} />
                        <Route path="security/userinfo" element={<UserInfo />} />
                    </Route>
                </Routes>
            </Container>
        </main>
    );
}

export default Main;