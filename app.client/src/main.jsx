import { StrictMode } from 'react';
import ReactDOM from 'react-dom/client';
import { GoogleOAuthProvider } from '@react-oauth/google';
import './index.css';
import App from './App.jsx';

ReactDOM.createRoot(document.getElementById('root')).render(
    <StrictMode>
        <GoogleOAuthProvider clientId="446631909658-vvbdjfbfqhhs0en5inpl4glf6m21uh2m.apps.googleusercontent.com">
            <App />
        </GoogleOAuthProvider>
    </StrictMode>
);
