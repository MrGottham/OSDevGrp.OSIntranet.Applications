import { useContext } from 'react';
import { HelperContext } from '../contexts/HelperContext';
import Container from 'react-bootstrap/Container';

function Footer({ layoutContext }) {
    const staticTextHelper = useContext(HelperContext).staticTextHelper;

    return (
        <footer className='footer mt-auto py-3 bg-body-tertiary'>
            <Container>
                <div className='d-block d-sm-block d-md-none d-lg-none d-xl-none d-xxl-none'>
                    <span className='text-body-secondary'>{layoutContext.title} - {staticTextHelper.getBuildInfoText(layoutContext.staticTexts)}</span>
                </div>
                <div className='d-none d-sm-none d-md-block d-lg-block d-xl-block d-xxl-block'>
                    <span className='text-body-secondary'>{layoutContext.title} - {staticTextHelper.getCopyrightText(layoutContext.staticTexts)} - {staticTextHelper.getBuildInfoText(layoutContext.staticTexts)}</span>
                </div>
            </Container>
        </footer>
    );
}

export default Footer;