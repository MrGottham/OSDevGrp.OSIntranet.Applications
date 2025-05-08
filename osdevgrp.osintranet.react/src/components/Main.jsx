import Container from 'react-bootstrap/Container';

function Main({ children }) {
    return (
        <main className="flex-shrink-0">
            <Container>
                {children}
            </Container>
        </main>
    );
}

export default Main;