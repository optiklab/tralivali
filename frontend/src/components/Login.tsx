import { useState } from 'react';
import { authService } from '../services/api';
import { generateKeyPair, encryptPrivateKey } from '../services/crypto';

interface LoginProps {
  onSuccess: (token: string, userId: string, username: string) => void;
}

export default function Login({ onSuccess }: LoginProps) {
  const [isRegister, setIsRegister] = useState(false);
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [username, setUsername] = useState('');
  const [inviteCode, setInviteCode] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      const response = await authService.login(email, password);
      localStorage.setItem('token', response.token);
      localStorage.setItem('userId', response.userId);
      onSuccess(response.token, response.userId, response.username);
    } catch (err: any) {
      setError(err.response?.data?.message || 'Login failed');
    } finally {
      setLoading(false);
    }
  };

  const handleRegister = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      // Generate encryption keys
      const { publicKey, privateKey } = await generateKeyPair();
      const privateKeyEncrypted = await encryptPrivateKey(privateKey, password);

      const response = await authService.register({
        username,
        email,
        password,
        inviteCode,
        publicKey,
        privateKeyEncrypted,
      });

      localStorage.setItem('token', response.token);
      localStorage.setItem('userId', response.userId);
      localStorage.setItem('privateKey', privateKeyEncrypted);
      onSuccess(response.token, response.userId, response.username);
    } catch (err: any) {
      setError(err.response?.data?.message || 'Registration failed');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{ maxWidth: '400px', margin: '50px auto', padding: '20px' }}>
      <h1>Tralivali</h1>
      <p>Secure Messaging Platform</p>

      {error && <div style={{ color: 'red', marginBottom: '10px' }}>{error}</div>}

      <form onSubmit={isRegister ? handleRegister : handleLogin}>
        {isRegister && (
          <>
            <input
              type="text"
              placeholder="Username"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              required
              style={{ width: '100%', padding: '10px', marginBottom: '10px' }}
            />
            <input
              type="text"
              placeholder="Invite Code"
              value={inviteCode}
              onChange={(e) => setInviteCode(e.target.value)}
              required
              style={{ width: '100%', padding: '10px', marginBottom: '10px' }}
            />
          </>
        )}
        <input
          type="email"
          placeholder="Email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
          style={{ width: '100%', padding: '10px', marginBottom: '10px' }}
        />
        <input
          type="password"
          placeholder="Password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
          style={{ width: '100%', padding: '10px', marginBottom: '10px' }}
        />
        <button
          type="submit"
          disabled={loading}
          style={{
            width: '100%',
            padding: '10px',
            backgroundColor: '#007bff',
            color: 'white',
            border: 'none',
            cursor: loading ? 'not-allowed' : 'pointer',
          }}
        >
          {loading ? 'Loading...' : isRegister ? 'Register' : 'Login'}
        </button>
      </form>

      <p style={{ marginTop: '20px', textAlign: 'center' }}>
        {isRegister ? 'Already have an account?' : "Don't have an account?"}
        <button
          onClick={() => setIsRegister(!isRegister)}
          style={{ marginLeft: '5px', background: 'none', border: 'none', color: '#007bff', cursor: 'pointer' }}
        >
          {isRegister ? 'Login' : 'Register'}
        </button>
      </p>
    </div>
  );
}
