import { useContext, useState, useEffect } from 'react';
import { useErrorBoundary } from 'react-error-boundary';
import { ServiceContext } from '../contexts/ServiceContext';
import { HelperContext } from '../contexts/HelperContext';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import Stack from 'react-bootstrap/Stack';
import Loading from './Loading';
import AccountingSummary from './AccountingSummary';

function Home() {
    const { showBoundary } = useErrorBoundary();
    const homeService = useContext(ServiceContext).homeService;
    const authorizationHelper = useContext(HelperContext).authorizationHelper;
    const staticTextHelper = useContext(HelperContext).staticTextHelper;
    const [layoutContext, setLayoutContext] = useState();

    useEffect(() => {
        populateLayoutContext()
            .catch(error => showBoundary(error));
    }, []);

    if (layoutContext === undefined) {
        return (
            <Loading />
        );
    }

    return (
        <>
            <Row>
                <Col xs={12} sm={12} md={8} lg={9} xl={9} xxl={10}>
                    {getPrimaryContent(layoutContext, authorizationHelper, staticTextHelper)}
                </Col>
                <Col className='d-none d-md-block d-lg-block d-xl-bock d-xxl-block' md={4} lg={3} xl={3} xxl={2}>
                    {getSecondaryContent()}
                </Col>
            </Row>
            <Row className='d-block d-sm-block d-md-none d-lg-none d-xl-none d-xxl-none'>
                <Col xs={12} sm={12} md={12} lg={12} xl={12} xxl={12}>
                    {getSecondaryContent()}
                </Col>
            </Row>
        </>
    );

    function getPrimaryContent(layoutContext, authorizationHelper, staticTextHelper) {
        if (authorizationHelper.authenticatedUser(layoutContext.userInfo) === false) {
            return (
                <>
                </>
            );
        }

        return (
            <Stack gap={0}>
                <h1>{staticTextHelper.getMyOverviewText(layoutContext.staticTexts)}</h1>
                <hr />
                {getAccountingSummaryContent(layoutContext.userInfo, authorizationHelper)}
            </Stack>
        );
    }

    function getAccountingSummaryContent(userInfo, authorizationHelper) {
        if (authorizationHelper.hasAccountingAccess(userInfo) === false || userInfo.defaultAccountingNumber === undefined || userInfo.defaultAccountingNumber === null) {
            return (
                <>
                </>
            );
        }

        return (
            <AccountingSummary accountingNumber={userInfo.defaultAccountingNumber} />
        )
    }

    function getSecondaryContent() {
        return (
            <>
            </>
        );
    }

    async function populateLayoutContext() {
        const json = await homeService.getLayoutContext();
        setLayoutContext(json);
    }
}

export default Home;