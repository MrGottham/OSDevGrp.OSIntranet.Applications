import { Link } from 'react-router';
import Card from 'react-bootstrap/Card';
import Image from 'react-bootstrap/Image';
import AccoutingImage from '../assets/accounting.png';

function AccountingCard({ accounting, children }) {
    return (
        <Card className='h-100'>
            <Card.Img variant='top' as={Image} src={AccoutingImage} fluid />
            <Card.Body>
                <Card.Title><Card.Link as={Link} to={`/accountings/${accounting.number}`}>{accounting.name}</Card.Link></Card.Title>
                {children}
            </Card.Body>
        </Card>
    );
}

export default AccountingCard;