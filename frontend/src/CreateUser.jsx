import { useState, useEffect } from 'react';

export default function CreateUser({ token, onUserCreated }) {
  const [formData, setFormData] = useState({
    nome: '', sobrenome: '', email: '', cpf: '', telefone: '', dataNascimento: '',
    organizacaoNome: 'Sede', departamentoNome: '', unidadeNome: 'Matriz', grupoNome: ''
  });
  
  // Guardando os dados que vêm do banco (O tal do selectbox dinâmico!)
  const [departamentos, setDepartamentos] = useState([]);
  const [grupos, setGrupos] = useState([]);

  const [loading, setLoading] = useState(false);
  const [msg, setMsg] = useState('');
  const [error, setError] = useState(false);

  useEffect(() => {
    // Pegadinha do banco: Puxando as listas pro usuário não ter que adivinhar o nome exato!
    fetch('http://localhost:5199/api/Departamento').then(r => r.json()).then(d => {
        setDepartamentos(d);
        if(d.length > 0) setFormData(f => ({...f, departamentoNome: d[0].nome}));
    }).catch(()=>console.log("Deu ruim na busca de departamentos"));
    
    fetch('http://localhost:5199/api/Grupo').then(r => r.json()).then(g => {
        setGrupos(g);
        if(g.length > 0) setFormData(f => ({...f, grupoNome: g[0].nome}));
    }).catch(()=>console.log("Deu ruim na busca de grupos"));
  }, []);

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true); setMsg(''); setError(false);
    try {
      const response = await fetch('http://localhost:5199/api/Usuario', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${token}` },
        body: JSON.stringify(formData)
      });
      if (response.ok) {
        setMsg('Show! Usuário provisionado no AD!');
        setFormData({ ...formData, nome: '', sobrenome: '', email: '', cpf: '' });
        if (onUserCreated) setTimeout(onUserCreated, 1500);
      } else {
        const text = await response.text();
        setError(true); setMsg(text || 'Erro ao criar usuário.');
      }
    } catch (err) {
      setError(true); setMsg('Erro de conexão com o servidor.');
    } finally { setLoading(false); }
  };

  return (
    <div className="glass-panel animate-fade-in" style={{ padding: '24px', maxWidth: '800px' }}>
      <h3 style={{ marginBottom: '24px', fontSize: '1.2rem', fontWeight: '500' }}>Provisionamento de Novo Usuário</h3>
      
      {msg && (
        <div style={{ padding: '12px', borderRadius: '8px', backgroundColor: error ? 'var(--danger-bg)' : 'var(--success-bg)', color: error ? 'var(--danger)' : 'var(--success)', marginBottom: '16px' }}>
          {msg}
        </div>
      )}

      <form onSubmit={handleSubmit} style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '20px' }}>
        <div>
            <label style={{display: 'block', marginBottom: '8px', fontSize: '0.8rem', color: 'var(--text-secondary)'}}>Nome</label>
            <input type="text" name="nome" placeholder="Ex: Andre" className="input-field" value={formData.nome} onChange={handleChange} required />
        </div>
        <div>
            <label style={{display: 'block', marginBottom: '8px', fontSize: '0.8rem', color: 'var(--text-secondary)'}}>Sobrenome</label>
            <input type="text" name="sobrenome" placeholder="Ex: Silva" className="input-field" value={formData.sobrenome} onChange={handleChange} required />
        </div>
        <div>
            <label style={{display: 'block', marginBottom: '8px', fontSize: '0.8rem', color: 'var(--text-secondary)'}}>E-mail Corporativo</label>
            <input type="email" name="email" placeholder="andre@empresa.com" className="input-field" value={formData.email} onChange={handleChange} required />
        </div>
        <div>
            <label style={{display: 'block', marginBottom: '8px', fontSize: '0.8rem', color: 'var(--text-secondary)'}}>CPF</label>
            <input type="text" name="cpf" placeholder="Apenas números" className="input-field" value={formData.cpf} onChange={handleChange} required />
        </div>
        <div>
            <label style={{display: 'block', marginBottom: '8px', fontSize: '0.8rem', color: 'var(--text-secondary)'}}>Telefone</label>
            <input type="text" name="telefone" placeholder="(11) 99999-9999" className="input-field" value={formData.telefone} onChange={handleChange} required />
        </div>
        <div>
            <label style={{display: 'block', marginBottom: '8px', fontSize: '0.8rem', color: 'var(--text-secondary)'}}>Data de Nascimento</label>
            <input type="datetime-local" name="dataNascimento" className="input-field" value={formData.dataNascimento} onChange={handleChange} required />
        </div>
        
        <div style={{ gridColumn: '1 / -1', height: '1px', backgroundColor: 'var(--border-color)', margin: '16px 0' }}></div>
        
        {/* Usando selects no lugar de inputs soltos */}
        <div>
            <label style={{display: 'block', marginBottom: '8px', fontSize: '0.8rem', color: 'var(--text-secondary)'}}>Organização (Tenant)</label>
            <input type="text" name="organizacaoNome" placeholder="Organização" className="input-field" value={formData.organizacaoNome} onChange={handleChange} required />
        </div>
        <div>
            <label style={{display: 'block', marginBottom: '8px', fontSize: '0.8rem', color: 'var(--text-secondary)'}}>Departamento</label>
            <select name="departamentoNome" className="input-field" value={formData.departamentoNome} onChange={handleChange} required>
                <option value="">Selecione um departamento...</option>
                {departamentos.map(d => <option key={d.id} value={d.nome}>{d.nome}</option>)}
            </select>
        </div>
        <div>
            <label style={{display: 'block', marginBottom: '8px', fontSize: '0.8rem', color: 'var(--text-secondary)'}}>Unidade Organizacional (OU)</label>
            <input type="text" name="unidadeNome" placeholder="Ex: Matriz, Filial SP" className="input-field" value={formData.unidadeNome} onChange={handleChange} required />
        </div>
        <div>
            <label style={{display: 'block', marginBottom: '8px', fontSize: '0.8rem', color: 'var(--text-secondary)'}}>Grupo de Permissão / Cargo</label>
            <select name="grupoNome" className="input-field" value={formData.grupoNome} onChange={handleChange} required>
                <option value="">Selecione um grupo de segurança...</option>
                {grupos.map(g => <option key={g.id} value={g.nome}>{g.nome}</option>)}
            </select>
        </div>
        
        <div style={{ gridColumn: '1 / -1', marginTop: '16px' }}>
          <button type="submit" className="btn btn-primary" disabled={loading} style={{ padding: '12px 24px' }}>
            {loading ? 'Provisionando...' : 'Provisionar no Active Directory'}
          </button>
        </div>
      </form>
    </div>
  );
}
