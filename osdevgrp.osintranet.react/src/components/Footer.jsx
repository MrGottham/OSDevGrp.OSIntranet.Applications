import Container from 'react-bootstrap/Container';
import { getStaticTextByKey } from '../helpers/StaticTextHelper';

function Footer({ layoutContext }) {
    return (
        <footer className="footer mt-auto py-3 bg-body-tertiary">
            <Container>
                <div className="d-block d-sm-block d-md-none d-lg-none d-xl-none d-xxl-none">
                    <span className="text-body-secondary">{layoutContext.title} - {getStaticTextByKey(layoutContext.staticTexts, 'BuildInfo')}</span>
                </div>
                <div className="d-none d-sm-none d-md-block d-lg-block d-xl-block d-xxl-block">
                    <span className="text-body-secondary">{layoutContext.title} - {getStaticTextByKey(layoutContext.staticTexts, 'BuildInfo')} - {getStaticTextByKey(layoutContext.staticTexts, 'BuildInfo')}</span>
                </div>
            </Container>
        </footer>
    );
}

export default Footer;