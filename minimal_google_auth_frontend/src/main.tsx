import React from 'react';
import ReactDOM from 'react-dom/client';
import { QueryClientProvider } from '@tanstack/react-query';
import { GoogleOAuthProvider } from '@react-oauth/google';
import { App } from './App';
import { queryClient } from './lib/query-client';
import './styles.css';
import './i18n';

const clientId = import.meta.env.VITE_GOOGLE_CLIENT_ID;

if (!clientId) {
  throw new Error('VITE_GOOGLE_CLIENT_ID is required in .env.local');
}

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <GoogleOAuthProvider clientId={clientId}>
      <QueryClientProvider client={queryClient}>
        <App />
      </QueryClientProvider>
    </GoogleOAuthProvider>
  </React.StrictMode>,
);
