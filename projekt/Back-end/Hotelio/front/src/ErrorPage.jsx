import { useRouteError } from "react-router-dom";

export default function ErrorPage() {
    const error = useRouteError();

    return (
        <div id="error-page">
            <h1>Something Went wrong</h1>
            <p>
                <i>{error.statusText || error.message}</i>
            </p>
        </div>
    );
}