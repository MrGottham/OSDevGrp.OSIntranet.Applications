import { useContext, useState, useEffect } from 'react';
import { useErrorBoundary } from 'react-error-boundary';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCheck } from '@fortawesome/free-solid-svg-icons';
import { ServiceContext } from '../contexts/ServiceContext';
import { HelperContext } from '../contexts/HelperContext';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import Card from 'react-bootstrap/Card';
import Loading from './Loading';
import AccountingCard from './AccountingCard';

function UserInfo() {
    const { showBoundary } = useErrorBoundary();
    const securityService = useContext(ServiceContext).securityService;
    const authorizationHelper = useContext(HelperContext).authorizationHelper;
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
                    {getFullNameContent(userInfo)}
                    {getMailAddressContent(userInfo, userInfo.staticTexts, staticTextHelper)}
                </Col>
            </Row>
            <Row>
                <Col xs={12} sm={12} md={5} lg={4} xl={4} xxl={3}>
                    <Row xs={1} sm={1} md={1} lg={1} xl={1} xxl={1} className='g-3'>
                        {getPermissionsContent(userInfo, authorizationHelper, userInfo.staticTexts, staticTextHelper)}
                        {getFinancialManagementContent(userInfo, authorizationHelper, userInfo.staticTexts, staticTextHelper)}
                    </Row>
                </Col>
                <Col xs={12} sm={12} md={7} lg={8} xl={8} xxl={9}>
                    {getAccountingsContent(userInfo, authorizationHelper, userInfo.staticTexts, staticTextHelper)}
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

    function getPermissionsContent(userInfo, authorizationHelper, staticTexts, staticTextHelper) {
        return (
            <Col>
                <Card>
                    <Card.Body>
                        <Card.Title>{staticTextHelper.getPermissionsText(staticTexts)}</Card.Title>
                        {getPermissionContent(authorizationHelper.hasAccountingAccess(userInfo), staticTextHelper.getFinancialManagementText(staticTexts), 'mt-3')}
                        {getPermissionContent(authorizationHelper.isAccountingAdministrator(userInfo), staticTextHelper.getAdministratorText(staticTexts), 'mt-3 ps-3')}
                        {getPermissionContent(authorizationHelper.isAccountingCreator(userInfo), staticTextHelper.getCreatorText(staticTexts), 'mt-3 ps-3')}
                        {getPermissionContent(authorizationHelper.isAccountingModifier(userInfo), staticTextHelper.getModifierText(staticTexts), 'mt-3 ps-3')}
                        {getPermissionContent(authorizationHelper.isAccountingViewer(userInfo), staticTextHelper.getViewerText(staticTexts), 'mt-3 ps-3')}
                        {getPermissionContent(authorizationHelper.hasCommonDataAccess(userInfo), staticTextHelper.getCommonDataText(staticTexts), 'mt-3')}
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
            <Card.Text className={className}><span><FontAwesomeIcon icon={faCheck} />&nbsp;{permissionName}</span></Card.Text>
        );
    }

    function getFinancialManagementContent(userInfo, authorizationHelper, staticTexts, staticTextHelper) {
        if (authorizationHelper.hasAccountingAccess(userInfo) === false) {
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

    function getAccountingsContent(userInfo, authorizationHelper, staticTexts, staticTextHelper) {
        if (authorizationHelper.hasAccountingAccess(userInfo) === false) {
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
                    {userInfo.accountings.map((accounting, index) => getAccountingContent(accounting, index, userInfo, authorizationHelper, staticTexts, staticTextHelper))}
                </Row>
            </>
        );
    }

    function getAccountingContent(accounting, index, userInfo, authorizationHelper, staticTexts, staticTextHelper) {
        if (authorizationHelper.isAccountingViewer(userInfo, accounting.number) === false) {
            return (
                <>
                </>
            );
        }

        return (
            <Col key={index}>
                <AccountingCard accounting={accounting}>
                    {getPermissionContent(authorizationHelper.isAccountingModifier(userInfo, accounting.number), staticTextHelper.getModifierText(staticTexts), 'mt-3')}
                    {getPermissionContent(authorizationHelper.isAccountingViewer(userInfo, accounting.number), staticTextHelper.getViewerText(staticTexts), 'mt-3')}
                </AccountingCard>
            </Col>
        );
    }

    async function populateUserInfo() {
        const json = await securityService.getUserInfo();
        setUserInfo(json);
    }
}

export default UserInfo;