import { useContext } from 'react';
import { Navigate } from 'react-router';
import { ServiceContext } from '../contexts/ServiceContext'

function Logout() {
    const authenticateService = useContext(ServiceContext).authenticateService;

    return (
        <Navigate replace to={window.location.href = authenticateService.getLogoutUrl(window.location)} />
    );
}

export default Logout;