import React, { useState } from 'react';
import { useAuth } from '../context/authContext';
import { loginUser, registerUser } from '../http/authApi';
import authLogo from '../assets/auth-logo.png'; 
import '../styles/auth.css';

const Auth = () => {
  const { handleLogin } = useAuth();
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [isLoginMode, setLoginMode] = useState(true);
  const [showPassword, setShowPassword] = useState(false);
  const [error, setError] = useState('');

  const handleToggleMode = () => {
    setLoginMode((prevMode) => !prevMode);
  };

  const handleTogglePasswordVisibility = () => {
    setShowPassword((prevShowPassword) => !prevShowPassword);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      let responseData;
      if (isLoginMode) {
        responseData = await loginUser({ email, password });
      } else {
        responseData = await registerUser({ email, password });
      }

      const token = responseData.token;
      handleLogin(token);

      console.log(`${isLoginMode ? 'Login' : 'Registration'} successful. Token:`, token);
      window.alert(`${isLoginMode ? 'Login' : 'Registration'} successful`);

      if (isLoginMode) {
        window.location.replace('/dashboard');
      } else {
        window.location.replace('/');
      }
    } catch (error) {
      console.error(`Error during ${isLoginMode ? 'login' : 'registration'}:`, error);
      setError(error.message);
    }
  };

  return (
    <div className="auth-container">
      <div className="auth-content">
        <h2 className="auth-title">{isLoginMode ? 'Login' : 'Registration'}</h2>
        {isLoginMode && <h3 className="auth-info">Welcome back! Please log in.</h3>}
        <form className="auth-form" onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="email">Email:</label>
            <input
              className="form-control"
              type="email"
              id="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
          </div>
          <div className="form-group">
            <label htmlFor="password">Password:</label>
            <div className="password-input-container">
              <input
                className="form-control"
                type={showPassword ? 'text' : 'password'}
                id="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
              />
              <i
                className={`eye-icon ${showPassword ? 'fas fa-eye-slash' : 'fas fa-eye'}`}
                onClick={handleTogglePasswordVisibility}
              ></i>
            </div>
          </div>
          <button className="btn btn-success" type="success" style={{ width: '100%', padding: '10px' }}>
            {isLoginMode ? 'Login' : 'Register'}
          </button>
        </form>
        <p className="form-toggle" onClick={handleToggleMode}>
          {isLoginMode ? "Don't have an account?" : 'Already have an account?'}
        </p>
      </div>
      <div className="auth-image-container">
        <img src={authLogo} alt="Auth Logo" className="auth-logo" />
      </div>
    </div>
  );
};

export default Auth;
