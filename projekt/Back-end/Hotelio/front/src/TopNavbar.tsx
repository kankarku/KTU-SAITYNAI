import { useState } from 'react';
import { Button, Container, Modal, Nav, Navbar } from 'react-bootstrap';
import { Link, useNavigate } from 'react-router-dom';
import { logout, useAuthDispatch, useAuthState } from './AuthProvider';
import { useAxiosContext } from './AxiosInstanceProvider';

function TopNavbar(props: any) {
    const authState = useAuthState();

    let userSection;
    if (authState.user) {
        userSection =
            <LoggedInUser />
    } else {
        userSection =
            <Nav>
                <Link className="link-light mx-1" to={'/register'}>Register</Link>
                <Link className="link-light mx-1" to={'/login'}>Login</Link>
            </Nav>
    }

    return (
        <Navbar collapseOnSelect expand="lg" variant="dark" bg="dark" className="border-bottom border-secondary">
            <Container>
                <Navbar.Brand href='/'>Hotelio</Navbar.Brand>
                <Navbar.Toggle />
                <Navbar.Collapse>
                    <Nav className="me-auto">
                        <Link className="link-light mx-1" to={'/hotels'}>Hotels</Link>
                    </Nav>
                    {userSection}
                </Navbar.Collapse>
            </Container>
        </Navbar>
    );
}

function LoggedInUser(props: any) {
    const authState = useAuthState();
    const setAuthState = useAuthDispatch();
    const axios = useAxiosContext();
    const navigate = useNavigate();

    let [showSignout, setShowSignout] = useState<boolean>(false);

    const handleShow = () => setShowSignout(true);
    const handleClose = () => setShowSignout(false);
    const handleSignout = async () => {
        if (authState.user) {
            await logout(setAuthState, axios);

            setShowSignout(false);
            navigate("/");
        }
    }

    return (
        <Nav>
            <Button variant="secondary" onClick={e => handleShow()}>Sign Out</Button>
            <Modal className="text-light" backdrop={true} show={showSignout} onHide={handleClose}>
                <Modal.Header className="bg-dark">
                    <Modal.Title>Sign Out</Modal.Title>
                </Modal.Header>
                <Modal.Body className="bg-dark">Are you sure you want to sign out?</Modal.Body>
                <Modal.Footer className="bg-dark">
                    <Button variant="secondary" onClick={handleClose}>
                        No
                    </Button>
                    <Button variant="primary" onClick={e => handleSignout()}>
                        Yes
                    </Button>
                </Modal.Footer>
            </Modal>
        </Nav>    
    );
}

export default TopNavbar;