import { useContext, useEffect } from 'react';
import { ServiceContext } from '../contexts/ServiceContext';

function Login() {
    const authenticateService = useContext(ServiceContext).authenticateService;

    useEffect(() => {
        window.location.href = authenticateService.getLoginUrl(window.location);
    }, [authenticateService]);

    return null;
}

export default Login;