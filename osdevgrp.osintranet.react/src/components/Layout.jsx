import { useContext, useState, useEffect, useCallback } from 'react';
import { useErrorBoundary } from 'react-error-boundary';
import { ServiceContext } from '../contexts/ServiceContext';
import Loading from './Loading';
import Navigation from './Navigation';
import Main from './Main';
import Footer from './Footer';

function Layout({ children }) {
    const { showBoundary } = useErrorBoundary();
    const homeService = useContext(ServiceContext).homeService;
    const [layoutContext, setLayoutContext] = useState();
    const populateLayoutContext = useCallback(async () => {
        const json = await homeService.getLayoutContext();
        document.title = json.title;
        setLayoutContext(json);
    }, [homeService]);

    useEffect(() => {
        async function fetchLayoutContext() {
            populateLayoutContext()
                .catch(error => showBoundary(error));
        }
        fetchLayoutContext();
    }, [populateLayoutContext, showBoundary]);

    if (layoutContext === undefined) {
        return (
            <>
                <Loading />
            </>
        );
    }

    return (
        <>
            <Navigation layoutContext={layoutContext} />
            <Main layoutContext={layoutContext} children={children} />
            <Footer layoutContext={layoutContext} />
        </>
    );
}

export default Layout;