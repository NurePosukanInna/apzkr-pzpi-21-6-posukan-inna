import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import { AuthProvider } from './context/authContext';
import { ProductProvider } from './context/productContext';
import { LanguageProvider } from './context/languageContext';
import './i18n';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
    <AuthProvider>
      <ProductProvider>
        <LanguageProvider>
        <App />
        </LanguageProvider>
      </ProductProvider>
    </AuthProvider>
  </React.StrictMode>
);
