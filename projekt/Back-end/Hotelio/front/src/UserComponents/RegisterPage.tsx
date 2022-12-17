import { useState } from "react";
import { Container } from "react-bootstrap";
import { register, useAuthDispatch } from "../AuthProvider";

function RegisterPage(props: any) {
    const authDispatch = useAuthDispatch();

    let [email, setEmail] = useState<string | null>(null);
    let [password, setPassword] = useState<string | null>(null);
    let [username, setUsername] = useState<string | null>(null);

    let [successRegistering, setSuccessRegistering] = useState<boolean | null>(null);

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        if (!email || !password || !username) return;

        const success = await register(authDispatch, { Email: email, Username: username, Password: password  });
        setSuccessRegistering(success);
    };

    let responseMessage = undefined;
    if (successRegistering != null && successRegistering == true) {
        responseMessage =
            <div className="jumbotron m-2">
                <p className="text-success">Success</p>
            </div>;
    } else if (successRegistering != null && successRegistering == false) {
        responseMessage =
            <div className="jumbotron m-2">
                <p className="text-danger">Failed</p>
            </div>;
    }

    return (
        <Container className="justify-content-center">
            <h1 className="text-center m-3">Register</h1>
            <form method="post" className="mw-100" onSubmit={e => handleSubmit(e)}>
                <div className="form-group mb-3">
                    <label htmlFor="email" className="form-label">Email</label>
                    <input type="text" className="form-control" name="email" id="email" onChange={e => setEmail(e.target.value)} />
                </div>
                <div className="form-group mb-3">
                    <label htmlFor="password" className="form-label">Password</label>
                    <input type="password" className="form-control" name="password" id="password" onChange={e => setPassword(e.target.value)} />
                </div>
                <div className="form-group mb-3">
                    <label htmlFor="username" className="form-label">Username(useless)</label>
                    <input type="text" className="form-control" name="username" id="roleSusernameecret" onChange={e => setUsername(e.target.value)} />
                </div>
                <input type="submit" className="btn btn-primary" />
            </form>
            {responseMessage}
        </Container>
    );
}

export default RegisterPage;