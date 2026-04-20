import { useState } from 'react';

export default function Login({ onLogin }) {
  const [upn, setUpn] = useState('');
  const [password, setPassword] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [requirePasswordChange, setRequirePasswordChange] = useState(false);

  const handleLoginSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    
    try {
      const response = await fetch('http://localhost:5199/api/Auth/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ Upn: upn, Senha: password })
      });
      
      const data = await response.json().catch(() => null);

      if (response.ok) {
        onLogin(data); // data tem: nome, email, upn, token, grupos
      } else {
        if (response.status === 401 && data?.mensagem === "É necessário alterar a senha") {
          setRequirePasswordChange(true);
        } else {
          setError(data?.mensagem || data || 'Falha no login. Verifique suas credenciais.');
        }
      }
    } catch (err) {
      setError('Erro ao conectar com o servidor. A API dotnet está rodando?');
    } finally {
      setLoading(false);
    }
  };

  const handleChangePassword = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    
    try {
      const response = await fetch('http://localhost:5199/api/Auth/alterar-senha', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ Upn: upn, SenhaAtual: password, NovaSenha: newPassword })
      });
      
      if (response.ok) {
        setPassword(newPassword);
        setRequirePasswordChange(false);
        setError('Senha alterada com sucesso! Clique em Entrar novamente.');
      } else {
        const data = await response.text();
        setError(data || 'Erro ao alterar a senha.');
      }
    } catch (err) {
      setError('Erro ao conectar com o servidor.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{ height: '100vh', display: 'flex', alignItems: 'center', justifyContent: 'center', backgroundImage: 'radial-gradient(circle at 50% -20%, #1a2a44, #0f111a)' }}>
      <div className="glass-panel animate-fade-in" style={{ width: '400px', padding: '40px', textAlign: 'center' }}>
        <div style={{ marginBottom: '32px' }}>
          <div style={{ width: '64px', height: '64px', backgroundColor: 'var(--accent-primary)', borderRadius: '16px', display: 'flex', alignItems: 'center', justifyContent: 'center', margin: '0 auto 16px auto', boxShadow: 'var(--shadow-glow)' }}>
            <svg width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
              <path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z"/>
            </svg>
          </div>
          <h1 style={{ fontSize: '1.5rem', fontWeight: '600' }}>Identity Manager</h1>
          <p style={{ color: 'var(--text-secondary)', fontSize: '0.9rem', marginTop: '8px' }}>
            {requirePasswordChange ? 'Ação necessária de segurança' : 'Acesse o diretório corporativo'}
          </p>
        </div>

        {error && (
           <div style={{ padding: '10px', backgroundColor: 'var(--bg-tertiary)', borderLeft: '4px solid var(--accent-primary)', borderRadius: '4px', marginBottom: '16px', fontSize: '0.85rem' }}>
             {error}
           </div>
        )}

        {!requirePasswordChange ? (
          <form onSubmit={handleLoginSubmit} style={{ display: 'flex', flexDirection: 'column', gap: '16px' }}>
            <input
              type="text"
              className="input-field"
              placeholder="UPN (ex: admin)"
              value={upn}
              onChange={(e) => setUpn(e.target.value)}
              required
            />
            <input
              type="password"
              className="input-field"
              placeholder="Senha"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
            <button type="submit" className="btn btn-primary" style={{ width: '100%', marginTop: '8px' }} disabled={loading}>
              {loading ? 'Autenticando...' : 'Entrar'}
            </button>
          </form>
        ) : (
          <form onSubmit={handleChangePassword} style={{ display: 'flex', flexDirection: 'column', gap: '16px' }}>
            <div style={{ textAlign: 'left', color: 'var(--text-secondary)', fontSize: '0.85rem', marginBottom: '8px' }}>
              Este é o seu primeiro login. Por motivos de segurança e compliance, você deve definir uma nova senha.
            </div>
            <input
              type="password"
              className="input-field"
              placeholder="Nova Senha"
              value={newPassword}
              onChange={(e) => setNewPassword(e.target.value)}
              required
            />
            <button type="submit" className="btn btn-primary" style={{ width: '100%', marginTop: '8px' }} disabled={loading}>
              {loading ? 'Salvando...' : 'Definir Nova Senha e Continuar'}
            </button>
          </form>
        )}
      </div>
    </div>
  );
}
