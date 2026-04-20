import { useState, useEffect } from 'react';
import CreateUser from './CreateUser';

export default function Dashboard({ user, onLogout }) {
  const [activeMenu, setActiveMenu] = useState('ti');
  const [activeTab, setActiveTab] = useState('home');
  const [searchQuery, setSearchQuery] = useState('');
  const [usersList, setUsersList] = useState([]);
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState('');
  
  // RH State - Recrutamento e Holerite
  const [vagas, setVagas] = useState([]);
  const [vagaForm, setVagaForm] = useState({ titulo: '', descricao: '', departamento: '' });
  const [holerites, setHolerites] = useState([]);
  
  // TI State - Inventário
  const [equipamentos, setEquipamentos] = useState([]);
  const [equipForm, setEquipForm] = useState({ nome: '', marca: '', serviceTag: '', tipo: '' });

  // Permission State
  const [permSearchUser, setPermSearchUser] = useState('');
  const [permSelectedUser, setPermSelectedUser] = useState(null);
  const [permNovoGrupo, setPermNovoGrupo] = useState('');
  const [gruposDisponiveis, setGruposDisponiveis] = useState([]);

  // Lógica de RBAC via Grupos Departamentais
  const grupos = user.grupos || [];
  
  const isGlobalAdmin = grupos.some(g => ['admin_global', 'admin', 'builtin'].includes(g.toLowerCase()));
  const isTISeniorOrPleno = grupos.some(g => ['ti senior', 'ti pleno'].includes(g.toLowerCase()));
  const isTIJunior = grupos.some(g => ['ti junior', 'ti', 'tecnologia'].includes(g.toLowerCase()));
  const isRH = grupos.some(g => ['rh', 'recursos humanos'].includes(g.toLowerCase()));
  
  const canManageUsers = isGlobalAdmin || isTISeniorOrPleno || isTIJunior;
  const canCreateUsers = isGlobalAdmin || isTISeniorOrPleno;
  const canDelegatePermissions = isGlobalAdmin || isTISeniorOrPleno;

  useEffect(() => {
      // Trazendo os grupos do backend pra não precisar digitar na mão
      fetch('http://localhost:5199/api/Grupo').then(r => r.json()).then(setGruposDisponiveis).catch(e => console.log(e));
  }, []);

  const toggleMenu = (menuName) => {
    setActiveMenu(activeMenu === menuName ? '' : menuName);
  };

  // ---------------- Funções TI ----------------
  const handleSearchTI = async (e) => {
    e.preventDefault();
    if (!searchQuery) return;
    setLoading(true); setMessage('');
    try {
      const response = await fetch(`http://localhost:5199/api/Usuario/buscar/${searchQuery}`);
      if (response.ok) {
        setUsersList(await response.json());
      } else {
        setUsersList([]); setMessage('Nenhum usuário encontrado.');
      }
    } catch (error) {
      setMessage('Erro de conexão com a API.');
    } finally { setLoading(false); }
  };

  const handleSearchPermissoes = async (e) => {
      e.preventDefault();
      if (!permSearchUser) return;
      try {
        const response = await fetch(`http://localhost:5199/api/Usuario/buscar/${permSearchUser}`);
        if (response.ok) {
          const dados = await response.json();
          if (dados.length > 0) setPermSelectedUser(dados[0]);
          else setMessage('Usuário não encontrado.');
        }
      } catch(err) { setMessage('Erro na busca'); }
  };

  const handleDelegarPermissao = async (e) => {
      e.preventDefault();
      if (!permSelectedUser || !permNovoGrupo) return;
      try {
          const res = await fetch(`http://localhost:5199/api/Usuario/${permSelectedUser.id}/delegar-grupo/${permNovoGrupo}`, { method: 'POST' });
          if(res.ok) {
              setMessage(`Pronto! O usuário ${permSelectedUser.nome} agora faz parte do grupo ${permNovoGrupo}.`);
              setPermNovoGrupo('');
          }
      } catch (err) {
          setMessage('Erro ao delegar permissão');
      }
  };

  const carregarEquipamentos = async () => {
    try {
        const res = await fetch('http://localhost:5199/api/Inventario');
        if (res.ok) setEquipamentos(await res.json());
    } catch (e) {}
  };

  const handleCriarEquipamento = async (e) => {
      e.preventDefault();
      await fetch('http://localhost:5199/api/Inventario', {
          method: 'POST',
          headers: {'Content-Type': 'application/json'},
          body: JSON.stringify(equipForm)
      });
      setMessage('Nova máquina cadastrada no inventário!');
      setEquipForm({ nome: '', marca: '', serviceTag: '', tipo: '' });
      carregarEquipamentos();
  };

  // ---------------- Funções RH ----------------
  const carregarVagas = async () => {
      try {
          const res = await fetch('http://localhost:5199/api/Recrutamento/vagas');
          if (res.ok) setVagas(await res.json());
      } catch (e) {}
  };
  
  const handleCriarVaga = async (e) => {
      e.preventDefault();
      await fetch('http://localhost:5199/api/Recrutamento/vagas', {
          method: 'POST',
          headers: {'Content-Type': 'application/json'},
          body: JSON.stringify(vagaForm)
      });
      setMessage('A vaga foi publicada com sucesso!');
      setVagaForm({ titulo: '', descricao: '', departamento: '' });
      carregarVagas();
  };

  useEffect(() => {
      if (activeTab === 'rh_vagas') carregarVagas();
      if (activeTab === 'ti_inventario') carregarEquipamentos();
  }, [activeTab]);

  return (
    <div className="app-container">
      {/* Sidebar */}
      <div className="sidebar" style={{overflowY: 'auto'}}>
        <div className="sidebar-logo">IdentityManager</div>
        
        <div style={{ padding: '0 24px', fontSize: '0.75rem', color: 'var(--text-muted)' }}>Módulos ERP</div>
        
        <a onClick={() => setActiveTab('home')} className={`nav-item ${activeTab === 'home' ? 'active' : ''}`}>Dashboard Principal</a>

        {/* Módulo T.I (Accordion) */}
        {(canManageUsers || canCreateUsers || canDelegatePermissions) && (
            <div className="nav-accordion">
                <div className="nav-item" onClick={() => toggleMenu('ti')} style={{ justifyContent: 'space-between', cursor: 'pointer', background: activeMenu === 'ti' ? 'var(--bg-tertiary)' : 'transparent' }}>
                    <span style={{fontWeight: 'bold'}}>💻 Departamento de T.I</span>
                    <span>{activeMenu === 'ti' ? '▼' : '▶'}</span>
                </div>
                {activeMenu === 'ti' && (
                    <div style={{ paddingLeft: '16px', borderLeft: '2px solid var(--border-color)', marginLeft: '12px' }}>
                        {canManageUsers && <a onClick={() => setActiveTab('ti_pesquisa')} className={`nav-item ${activeTab === 'ti_pesquisa' ? 'active' : ''}`}>Diretório AD</a>}
                        {canCreateUsers && <a onClick={() => setActiveTab('ti_create')} className={`nav-item ${activeTab === 'ti_create' ? 'active' : ''}`}>Provisionamento</a>}
                        {canDelegatePermissions && <a onClick={() => setActiveTab('ti_permissoes')} className={`nav-item ${activeTab === 'ti_permissoes' ? 'active' : ''}`}>Grupos e Acessos (RBAC)</a>}
                        <a onClick={() => setActiveTab('ti_inventario')} className={`nav-item ${activeTab === 'ti_inventario' ? 'active' : ''}`}>Inventário Físico</a>
                    </div>
                )}
            </div>
        )}

        {/* Módulo RH (Accordion) */}
        {isRH && (
            <div className="nav-accordion">
                <div className="nav-item" onClick={() => toggleMenu('rh')} style={{ justifyContent: 'space-between', cursor: 'pointer', background: activeMenu === 'rh' ? 'var(--bg-tertiary)' : 'transparent' }}>
                    <span style={{fontWeight: 'bold'}}>👥 Recursos Humanos</span>
                    <span>{activeMenu === 'rh' ? '▼' : '▶'}</span>
                </div>
                {activeMenu === 'rh' && (
                    <div style={{ paddingLeft: '16px', borderLeft: '2px solid var(--border-color)', marginLeft: '12px' }}>
                        <a onClick={() => setActiveTab('rh_vagas')} className={`nav-item ${activeTab === 'rh_vagas' ? 'active' : ''}`}>Recrutamento e Seleção</a>
                        <a onClick={() => setActiveTab('rh_holerite')} className={`nav-item ${activeTab === 'rh_holerite' ? 'active' : ''}`}>Lançamento de Holerites</a>
                    </div>
                )}
            </div>
        )}

        {/* Módulo Pessoal (Todos têm acesso) */}
        <div className="nav-accordion">
            <div className="nav-item" onClick={() => toggleMenu('pessoal')} style={{ justifyContent: 'space-between', cursor: 'pointer', background: activeMenu === 'pessoal' ? 'var(--bg-tertiary)' : 'transparent' }}>
                <span style={{fontWeight: 'bold'}}>👤 Meu Portal</span>
                <span>{activeMenu === 'pessoal' ? '▼' : '▶'}</span>
            </div>
            {activeMenu === 'pessoal' && (
                <div style={{ paddingLeft: '16px', borderLeft: '2px solid var(--border-color)', marginLeft: '12px' }}>
                    <a onClick={() => setActiveTab('meu_holerite')} className={`nav-item ${activeTab === 'meu_holerite' ? 'active' : ''}`}>Meus Contra-Cheques</a>
                </div>
            )}
        </div>

        <div style={{ marginTop: 'auto', padding: '24px' }}>
          <div style={{ fontSize: '0.85rem', fontWeight: '500' }}>{user.nome}</div>
          <div style={{ fontSize: '0.70rem', color: 'var(--text-muted)' }}>
              Último Acesso: {user.ultimoLogin ? new Date(user.ultimoLogin).toLocaleString('pt-BR') : 'Primeiro login!'}
          </div>
          <div style={{ fontSize: '0.75rem', color: 'var(--accent-primary)', marginBottom: '16px', marginTop: '4px' }}>{grupos.join(' | ') || 'Sem Grupo'}</div>
          <button onClick={onLogout} className="btn" style={{width: '100%'}}>Desconectar</button>
        </div>
      </div>

      {/* Main Content */}
      <div className="main-content animate-fade-in" style={{overflowY: 'auto'}}>
        <div className="page-header">
          <h2 className="page-title">Sistema Corporativo ERP</h2>
        </div>

        {message && <div style={{ padding: '12px', background: 'var(--bg-tertiary)', borderLeft: '4px solid var(--accent-primary)', marginBottom: '16px' }}>{message}</div>}

        {activeTab === 'home' && (
          <div className="glass-panel" style={{ padding: '24px' }}>
            <h3>Fala, {user.nome}! Bem-vindo ao ERP.</h3>
            <p style={{ marginTop: '16px', color: 'var(--text-secondary)' }}>Navegue pelo menu lateral para acessar os painéis do seu departamento.</p>
          </div>
        )}

        {/* ---------------- TELAS DE T.I ---------------- */}
        {activeTab === 'ti_pesquisa' && (
           <div className="glass-panel" style={{ padding: '24px' }}>
               <h3>Consulta de Diretório Corporativo</h3>
               <form onSubmit={handleSearchTI} style={{ display: 'flex', gap: '12px', margin: '16px 0' }}>
                 <input type="text" className="input-field" placeholder="Buscar usuário por nome..." value={searchQuery} onChange={e => setSearchQuery(e.target.value)} />
                 <button className="btn btn-primary">Buscar</button>
               </form>
               <table className="data-table">
                 <thead><tr><th>Nome</th><th>Status da Conta</th></tr></thead>
                 <tbody>
                   {usersList.map(u => (
                     <tr key={u.id}>
                       <td>{u.nome} {u.sobrenome}</td>
                       <td>{u.contaBloqueada ? 'Bloqueada (Compliance)' : 'Ativa'}</td>
                     </tr>
                   ))}
                 </tbody>
               </table>
            </div>
        )}

        {activeTab === 'ti_create' && canCreateUsers && <CreateUser token={user.token} onUserCreated={() => setActiveTab('ti_pesquisa')} />}

        {activeTab === 'ti_permissoes' && canDelegatePermissions && (
            <div className="glass-panel" style={{ padding: '24px' }}>
                <h3>Escalada de Privilégios (Access Control)</h3>
                <p style={{ color: 'var(--text-secondary)' }}>Defina grupos de permissão para liberar Módulos e Menus no painel do usuário.</p>
                
                <form onSubmit={handleSearchPermissoes} style={{ display: 'flex', gap: '12px', marginTop: '24px', maxWidth: '500px' }}>
                    <input type="text" className="input-field" placeholder="Procurar usuário por nome..." value={permSearchUser} onChange={e => setPermSearchUser(e.target.value)} required />
                    <button type="submit" className="btn btn-primary">Buscar Funcionário</button>
                </form>

                {permSelectedUser && (
                    <div style={{ marginTop: '24px', padding: '16px', border: '1px solid var(--border-color)', borderRadius: '8px' }}>
                        <h4 style={{marginBottom: '16px'}}>Delegar Acesso para: {permSelectedUser.nome}</h4>
                        <form onSubmit={handleDelegarPermissao} style={{ display: 'flex', flexDirection: 'column', gap: '12px', maxWidth: '400px' }}>
                            <label style={{fontSize: '0.85rem', color: 'var(--text-secondary)'}}>
                                Qual grupo (cargo) este usuário vai ter a partir de agora?
                            </label>
                            {/* Pegadinha: Dropdown Select em vez de digitar! */}
                            <select className="input-field" value={permNovoGrupo} onChange={e => setPermNovoGrupo(e.target.value)} required>
                                <option value="">Selecione um grupo na lista...</option>
                                {gruposDisponiveis.map(g => (
                                    <option key={g.id} value={g.nome}>{g.nome}</option>
                                ))}
                            </select>
                            <button type="submit" className="btn btn-primary" style={{backgroundColor: 'var(--success)'}}>Atribuir Nova Permissão</button>
                        </form>
                    </div>
                )}
            </div>
        )}

        {activeTab === 'ti_inventario' && (
             <div className="glass-panel" style={{ padding: '24px' }}>
                 <h3>Controle de Patrimônio (Inventário de TI)</h3>
                 <p style={{ color: 'var(--text-secondary)', marginBottom: '24px' }}>Registre os equipamentos comprados pela empresa e veja quem está usando.</p>
                 
                 <form onSubmit={handleCriarEquipamento} style={{ display: 'flex', flexDirection: 'column', gap: '12px', maxWidth: '500px', marginBottom: '32px' }}>
                     <input type="text" className="input-field" placeholder="Identificação (Ex: NB-FIN-01)" value={equipForm.nome} onChange={e => setEquipForm({...equipForm, nome: e.target.value})} required />
                     <input type="text" className="input-field" placeholder="Marca (Ex: Dell, Apple)" value={equipForm.marca} onChange={e => setEquipForm({...equipForm, marca: e.target.value})} required />
                     <input type="text" className="input-field" placeholder="Service Tag / Serial Number" value={equipForm.serviceTag} onChange={e => setEquipForm({...equipForm, serviceTag: e.target.value})} required />
                     <select className="input-field" value={equipForm.tipo} onChange={e => setEquipForm({...equipForm, tipo: e.target.value})} required>
                         <option value="">Tipo de Aparelho...</option>
                         <option value="Notebook">Notebook</option>
                         <option value="Monitor">Monitor</option>
                         <option value="Celular Corporativo">Celular Corporativo</option>
                     </select>
                     <button type="submit" className="btn btn-primary">Salvar no Estoque</button>
                 </form>

                 <h4>Ativos Registrados:</h4>
                 <table className="data-table" style={{marginTop: '16px'}}>
                     <thead><tr><th>Patrimônio</th><th>Marca</th><th>Tag</th><th>Dono (Alocado)</th><th>Status</th></tr></thead>
                     <tbody>
                         {equipamentos.length === 0 ? <tr><td colSpan="5">Nenhum equipamento cadastrado.</td></tr> : null}
                         {equipamentos.map(eq => (
                             <tr key={eq.id}>
                                 <td>{eq.nome}</td>
                                 <td>{eq.marca}</td>
                                 <td>{eq.serviceTag}</td>
                                 <td>{eq.usuarioAlocado}</td>
                                 <td><span className={eq.status === 'Em Uso' ? 'badge badge-success' : 'badge'}>{eq.status}</span></td>
                             </tr>
                         ))}
                     </tbody>
                 </table>
             </div>
        )}

        {/* ---------------- TELAS DE RH ---------------- */}
        {activeTab === 'rh_vagas' && isRH && (
             <div className="glass-panel" style={{ padding: '24px' }}>
                 <h3>Módulo de Recrutamento (Vagas)</h3>
                 <form onSubmit={handleCriarVaga} style={{ display: 'flex', flexDirection: 'column', gap: '12px', maxWidth: '500px', marginBottom: '32px' }}>
                     <input type="text" className="input-field" placeholder="Título da Vaga (ex: Engenheiro Front-End)" value={vagaForm.titulo} onChange={e => setVagaForm({...vagaForm, titulo: e.target.value})} required />
                     <textarea className="input-field" rows="3" placeholder="Requisitos e Descrição da vaga..." value={vagaForm.descricao} onChange={e => setVagaForm({...vagaForm, descricao: e.target.value})} required></textarea>
                     <input type="text" className="input-field" placeholder="Departamento Solicitante" value={vagaForm.departamento} onChange={e => setVagaForm({...vagaForm, departamento: e.target.value})} required />
                     <button type="submit" className="btn btn-primary">Disparar Vaga no Sistema</button>
                 </form>

                 <h4>Quadro de Vagas:</h4>
                 <div style={{ marginTop: '16px', display: 'grid', gap: '12px' }}>
                     {vagas.length === 0 ? <p style={{color: 'var(--text-muted)'}}>Tranquilidade no RH, não temos vagas em aberto.</p> : vagas.map(v => (
                         <div key={v.id} style={{ padding: '16px', border: '1px solid var(--border-color)', borderRadius: '8px' }}>
                             <div style={{display: 'flex', justifyContent: 'space-between', alignItems: 'center'}}>
                                <strong>{v.titulo}</strong>
                                <span className="badge badge-success">{v.status}</span>
                             </div>
                             <div style={{ fontSize: '0.8rem', color: 'var(--accent-primary)', marginTop: '4px' }}>Área: {v.departamento}</div>
                             <p style={{ fontSize: '0.85rem', color: 'var(--text-secondary)', marginTop: '8px' }}>{v.descricao}</p>
                         </div>
                     ))}
                 </div>
             </div>
        )}

        {activeTab === 'rh_holerite' && isRH && (
            <div className="glass-panel" style={{ padding: '24px' }}>
                <h3>Fechamento de Folha (Lançar Holerite)</h3>
                <p style={{color: 'var(--text-secondary)'}}>Em breve: Interface para atrelar o salário final e os bônus ao usuário no final do mês, para ele ver no portal dele!</p>
            </div>
        )}

        {activeTab === 'meu_holerite' && (
            <div className="glass-panel" style={{ padding: '24px' }}>
                <h3>Meus Documentos de RH</h3>
                <p style={{color: 'var(--text-secondary)'}}>Nenhum holerite emitido para você ainda neste mês. Espere a virada da folha!</p>
            </div>
        )}

      </div>
    </div>
  );
}
