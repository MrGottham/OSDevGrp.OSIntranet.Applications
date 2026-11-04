import Toast from 'react-bootstrap/Toast';
import ToastContainer from 'react-bootstrap/ToastContainer';
import Markdown from 'react-markdown';

const variants = {
    Information: 'info',
    Warning: 'warning',
    Error: 'danger'
};

function Notifications({ notifications, onDismiss }) {
    return (
        <ToastContainer position='top-end' className='p-3 notifications__toast-container'>
            {notifications.map(notification => (
                <Toast
                    key={notification.identifier}
                    onClose={() => onDismiss(notification.identifier)}
                    show={true}
                    autohide={false}
                    bg={variants[notification.type]}
                    className='notifications__toast'
                >
                    <Toast.Header closeButton className='notifications__toast-header'>{notification.header}</Toast.Header>
                    <Toast.Body className='text-dark'>
                        {notification.format === 'Markdown' ? <Markdown>{notification.content}</Markdown> : notification.content}
                    </Toast.Body>
                </Toast>
            ))}
        </ToastContainer>
    );
}

export default Notifications;