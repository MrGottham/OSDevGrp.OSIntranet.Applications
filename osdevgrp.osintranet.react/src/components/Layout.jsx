import { useEffect, useState } from 'react';
import Navigation from './Navigation'
import Main from './Main'
import Footer from './Footer'

function Layout({ children }) {
    const [layoutContext, setLayoutContext] = useState();

    useEffect(() => {
        populateLayoutContext();
    }, []);

    if (layoutContext === undefined) {
        return (
            <>
            </>
        );
    }

    return (
        <>
            <Navigation layoutContext={layoutContext} />
            <Main children={children} />
            <Footer layoutContext={layoutContext} />
        </>
    );

    async function populateLayoutContext() {
        const response = await fetch('/api/home/index');
        if (response.ok) {
            const json = await response.json();
            document.title = json.title;
            setLayoutContext(json);
        }
    }
}

export default Layout