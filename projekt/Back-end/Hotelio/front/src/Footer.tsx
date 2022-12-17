import { Container } from "react-bootstrap";

function Footer(props: any) {
    return (
        <footer className="bg-dark text-center text-white border-top border-secondary mt-3">
            <Container className="text-center text-md-start mt-5">
                <div className="row mt-2">
                    <div className="col-md-3 col-lg-4 col-xl-3 mx-auto mb-4">
                        <h6 className="text-uppercase fw-bold mb-4">
                            footer
                        </h6>
                        <p>
                            This is a footer
                        </p>
                    </div>
                    <div className="col-md-2 col-lg-2 col-xl-2 mx-auto mb-4">
                        footer
                    </div>
                </div>
            </Container>
        </footer>
    );
}

export default Footer;