import { Col, Row } from "react-bootstrap";

function HomePage() {
    return (
        <Col>
            <Row xs={1} className="jumbotron mt-3">
                <h1>Hotelio</h1>
                <p>Welcome!</p>
            </Row>
        </Col>
    );
}

export default HomePage;