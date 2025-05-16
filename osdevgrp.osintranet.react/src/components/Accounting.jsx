import { useParams } from 'react-router';

function Accounting() {
    const accountingNumber = useParams().accountingNumber;

    return (
        <p>Accounting Component ({accountingNumber})!</p>
    );
}

export default Accounting;