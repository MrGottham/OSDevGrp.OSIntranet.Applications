import { useContext, useState, useEffect } from 'react';
import { useErrorBoundary } from 'react-error-boundary';
import { Link } from 'react-router';
import { ServiceContext } from '../contexts/ServiceContext';
import { HelperContext } from '../contexts/HelperContext';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import Card from 'react-bootstrap/Card';
import Image from 'react-bootstrap/Image';
import Loading from './Loading';
import AccoutingImage from '../assets/accounting.png';

function UserInfo() {
    const { showBoundary } = useErrorBoundary();
    const securityService = useContext(ServiceContext).securityService;
    const staticTextHelper = useContext(HelperContext).staticTextHelper;
    const [userInfo, setUserInfo] = useState();

    useEffect(() => {
        populateUserInfo()
            .catch(error => showBoundary(error));
    }, []);

    if (userInfo === undefined) {
        return (
            <>
                <Loading />
            </>
        );
    }

    return (
        <>
            <Row>
                <Col xs={12} sm={12} md={12} lg={12} xl={12} xxl={12}>
                    {getFullNameContent(userInfo.userInfo)}
                    {getMailAddressContent(userInfo.userInfo, userInfo.staticTexts, staticTextHelper)}
                </Col>
            </Row>
            <Row>
                <Col xs={12} sm={12} md={5} lg={4} xl={4} xxl={3}>
                    <Row xs={1} sm={1} md={1} lg={1} xl={1} xxl={1} className='g-3'>
                        {getPermissionsContent(userInfo.userInfo, userInfo.staticTexts, staticTextHelper)}
                        {getFinancialManagementContent(userInfo.userInfo, userInfo.staticTexts, staticTextHelper)}
                    </Row>
                </Col>
                <Col xs={12} sm={12} md={7} lg={8} xl={8} xxl={9}>
                    {getAccountingsContent(userInfo.userInfo, userInfo.staticTexts, staticTextHelper)}
                </Col>
            </Row>
        </>
    )

    function getFullNameContent(userInfo) {
        if (userInfo.name !== undefined && userInfo.name !== null && userInfo.name.length > 0) {
            return (
                <h1>{userInfo.name}</h1>
            );
        }

        if (userInfo.mailAddress !== undefined && userInfo.mailAddress !== null && userInfo.mailAddress.length > 0) {
            return (
                <h1>{userInfo.mailAddress}</h1>
            );
        }

        return (
            <>
            </>
        );
    }

    function getMailAddressContent(userInfo, staticTexts, staticTextHelper) {
        if (userInfo.mailAddress !== undefined && userInfo.mailAddress !== null && userInfo.mailAddress.length > 0) {
            return (
                <p>{staticTextHelper.getMailAddressText(staticTexts)}:&nbsp;<strong>{userInfo.mailAddress}</strong></p>
            );
        }

        return (
            <>
            </>
        );
    }

    function getPermissionsContent(userInfo, staticTexts, staticTextHelper) {
        return (
            <Col>
                <Card>
                    <Card.Body>
                        <Card.Title>{staticTextHelper.getPermissionsText(staticTexts)}</Card.Title>
                        {getPermissionContent(userInfo.hasAccountingAccess, staticTextHelper.getFinancialManagementText(staticTexts), 'mt-3')}
                        {getPermissionContent(userInfo.isAccountingAdministrator, staticTextHelper.getAdministratorText(staticTexts), 'mt-3 ps-3')}
                        {getPermissionContent(userInfo.isAccountingCreator, staticTextHelper.getCreatorText(staticTexts), 'mt-3 ps-3')}
                        {getPermissionContent(userInfo.isAccountingModifier, staticTextHelper.getModifierText(staticTexts), 'mt-3 ps-3')}
                        {getPermissionContent(userInfo.isAccountingViewer, staticTextHelper.getViewerText(staticTexts), 'mt-3 ps-3')}
                        {getPermissionContent(userInfo.hasCommonDataAccess, staticTextHelper.getCommonDataText(staticTexts), 'mt-3')}
                    </Card.Body>
                </Card>
            </Col>
        );
    }

    function getPermissionContent(permission, permissionName, className) {
        if (permission === undefined || permission === null || permission === false) {
            return (
                <>
                </>
            );
        }

        return (
            <Card.Text className={className}><span><i className='fa-solid fa-check'></i>&nbsp;{permissionName}</span></Card.Text>
        );
    }

    function getFinancialManagementContent(userInfo, staticTexts, staticTextHelper) {
        if (userInfo.hasAccountingAccess === undefined || userInfo.hasAccountingAccess === null || userInfo.hasAccountingAccess === false) {
            return (
                <>
                </>
            );
        }

        return (
            <Col>
                <Card>
                    <Card.Body>
                        <Card.Title>{staticTextHelper.getFinancialManagementText(staticTexts)}</Card.Title>
                        <Card.Text className='mt-3'>{staticTextHelper.getPrimaryAccountingText(staticTexts)}</Card.Text>
                        {getPrimaryAccountingContent(userInfo.accountings, userInfo.defaultAccountingNumber)}
                    </Card.Body>
                </Card>
            </Col>
        );
    }

    function getPrimaryAccountingContent(accountings, defaultAccountingNumber) {
        if (accountings === undefined || accountings === null || defaultAccountingNumber === undefined || defaultAccountingNumber === null) {
            return (
                <>
                </>
            );
        }

        const found = accountings.find(accounting => accounting.number === defaultAccountingNumber);
        if (found === undefined || found === null) {
            return (
                <>
                </>
            );
        }

        return (
            <Card.Text><strong>{found.name}</strong></Card.Text>
        );
    }

    function getAccountingsContent(userInfo, staticTexts, staticTextHelper) {
        if (userInfo.hasAccountingAccess === undefined || userInfo.hasAccountingAccess === null || userInfo.hasAccountingAccess === false) {
            return (
                <>
                </>
            );
        }

        return (
            <>
                <Row className='pt-3 pt-sm-3 pt-md-0 pt-lg-0 pt-xl-0 pt-xxl-0'>
                    <Col xs={12} sm={12} md={12} lg={12} xl={12} xxl={12}>
                        <h2>{staticTextHelper.getAccountingsText(staticTexts)}</h2>
                    </Col>
                </Row>
                <Row xs={1} sm={1} md={1} lg={2} xl={3} xxl={3} className='g-3'>
                    {userInfo.accountings.map((accounting, index) => (
                        <Col key={index}>
                            <Card className='h-100'>
                                <Card.Img variant='top' as={Image} src={AccoutingImage} fluid />
                                <Card.Body>
                                    <Card.Title><Card.Link as={Link} to={`/accountings/${accounting.number}`}>{accounting.name}</Card.Link></Card.Title>
                                    {getPermissionContent(userInfo.isAccountingModifier && accountingNumberInAccountings(accounting.number, userInfo.modifiableAccountings), staticTextHelper.getModifierText(staticTexts), 'mt-3')}
                                    {getPermissionContent(userInfo.isAccountingViewer && accountingNumberInAccountings(accounting.number, userInfo.viewableAccountings), staticTextHelper.getViewerText(staticTexts), 'mt-3')}
                                </Card.Body>
                            </Card>
                        </Col>
                    ))}
                </Row>
            </>
        );
    }

    function accountingNumberInAccountings(accountingNumber, accountings) {
        if (accountingNumber === undefined || accountingNumber === null || accountings === undefined || accountings === null) {
            return false;
        }

        return accountings.some(accounting => accounting.number === accountingNumber);
    }

    async function populateUserInfo() {
        const json = await securityService.getUserInfo();
        setUserInfo(json);
    }
}

export default UserInfo;