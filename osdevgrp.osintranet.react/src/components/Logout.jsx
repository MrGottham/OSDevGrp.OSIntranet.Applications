import { useContext, useEffect } from 'react';
import { ServiceContext } from '../contexts/ServiceContext';

function Logout() {
    const authenticateService = useContext(ServiceContext).authenticateService;

    useEffect(() => {
        window.location.href = authenticateService.getLogoutUrl(window.location);
    }, [authenticateService]);

    return null;
}

export default Logout;