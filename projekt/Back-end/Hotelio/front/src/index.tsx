import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import ErrorPage from './ErrorPage';
import HomePage from './HomePage';
import LoginPage from './UserComponents/Login';
import RegisterPage from './UserComponents/RegisterPage';
import { AxiosInstanceProvider } from './AxiosInstanceProvider';
import { AuthProvider } from './AuthProvider';
import HotelList from './HotelList';
import HotelView from './HotelView';

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);

const router = createBrowserRouter([
    {
        path: '/',
        element: <App />,
        errorElement: <ErrorPage />,
        children: [
            {
                path: '/',
                element: <HomePage />
            },
            {
                path: '/login',
                element: <LoginPage />
            },
            {
                path: '/register',
                element: <RegisterPage />
            },
            {
                path: '/hotels',
                element: <HotelList />
            },
            {
                path: '/hotel/:hotelId',
                element: <HotelView />
            }
        ],
    }
]);

root.render(
    <AuthProvider>
        <AxiosInstanceProvider>
            <RouterProvider router={router} />
        </AxiosInstanceProvider>
    </AuthProvider>
);
reportWebVitals();