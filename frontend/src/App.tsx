import { useState } from 'react';
import Login from './components/Login';
import Messaging from './components/Messaging';
import './App.css';

function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [userId, setUserId] = useState('');
  const [username, setUsername] = useState('');

  const handleLoginSuccess = (_token: string, uid: string, uname: string) => {
    setIsAuthenticated(true);
    setUserId(uid);
    setUsername(uname);
  };

  const handleLogout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('userId');
    setIsAuthenticated(false);
    setUserId('');
    setUsername('');
  };

  return (
    <div>
      {!isAuthenticated ? (
        <Login onSuccess={handleLoginSuccess} />
      ) : (
        <Messaging userId={userId} username={username} onLogout={handleLogout} />
      )}
    </div>
  );
}

export default App;
