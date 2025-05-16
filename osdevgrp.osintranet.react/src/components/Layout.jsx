import { useContext, useState, useEffect } from 'react';
import { ServiceContext } from '../contexts/ServiceContext';
import Loading from './Loading';
import Navigation from './Navigation';
import Main from './Main';
import Footer from './Footer';

function Layout({ children }) {
    const homeService = useContext(ServiceContext).homeService;
    const [layoutContext, setLayoutContext] = useState();

    useEffect(() => {
        populateLayoutContext();
    }, []);

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

    async function populateLayoutContext() {
        const json = await homeService.getLayoutContext();
        document.title = json.title;
        setLayoutContext(json);
    }
}

export default Layout