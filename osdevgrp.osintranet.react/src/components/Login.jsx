import { useContext } from 'react';
import { Navigate } from 'react-router';
import { ServiceContext } from '../contexts/ServiceContext';

function Login() {
    const authenticateService = useContext(ServiceContext).authenticateService;

    return (
        <Navigate replace to={window.location.href = authenticateService.getLoginUrl(window.location)} />
    );
}

export default Login;