import React, { useState } from "react";
import { GoogleOAuthProvider, GoogleLogin } from "@react-oauth/google";
import { jwtDecode } from "jwt-decode";
import "./App.css";
import "./index.css";
export default function App() {
    const [user, setUser] = useState(null);

    const handleSuccess = async (credentialResponse) => {
        const token = credentialResponse.credential;
        const decoded = jwtDecode(token);
        console.log("Google user data:", decoded);

        try {
            const res = await fetch("http://localhost:5150/api/auth/google", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ token }),
            });

            const data = await res.json();

            const proxyPictureUrl = `http://localhost:5150/api/auth/userphoto?url=${encodeURIComponent(data.picture)}`;

            setUser({ ...data, picture: proxyPictureUrl }); //spread + перезапис властивостей

        } catch (error) {
            console.error("Login error:", error);
        }
    };

    const handleError = () => console.log("Login Failed");

    if (user) {
        return (
            <div className="text-center mt-8">
                <h1 className="text-3xl font-bold mb-6">User Profile</h1>
                <img src={user.picture} alt={user.name} className="rounded-full mx-auto" />
                <h2 className="text-2xl mt-4">{user.name}</h2>
                <p className="text-gray-700">{user.email}</p>
            </div>
        );
    }
    console.log("User state:", user);

    return (
        <GoogleOAuthProvider clientId="446631909658-7hodsgfqhug6mtk2bqmcksfi13b2tr69.apps.googleusercontent.com">
            <h1 className="text-2xl font-bold ">Google Login</h1>
            <div className="mt-4">
                <GoogleLogin onSuccess={handleSuccess} onError={handleError} />
            </div>
        </GoogleOAuthProvider>
    );
}
