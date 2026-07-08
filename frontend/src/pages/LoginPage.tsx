import { useState, FormEvent } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import { useI18n } from '../contexts/I18nContext';
import api from '../api/axios';

export default function LoginPage() {
  const navigate  = useNavigate();
  const { login } = useAuth();
  const { t, language, setLanguage } = useI18n();

  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [errorMsg, setErrorMsg] = useState('');
  const [loading,  setLoading]  = useState(false);

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    if (!username.trim() || !password.trim()) { setErrorMsg(t('required')); return; }
    setErrorMsg('');
    setLoading(true);
    try {
      const res = await api.post<{ token: string }>('/api/auth/login', {
        username: username.trim(),
        password,
      });
      login(res.data.token);
      navigate('/dashboard', { replace: true });
    } catch (err: unknown) {
      const status = (err as { response?: { status?: number } }).response?.status;
      setErrorMsg(status === 401 ? t('loginFailed') : t('networkError'));
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="login-page">
      <div className="bg-orbs">
        <div className="bg-orb bg-orb-1" />
        <div className="bg-orb bg-orb-2" />
        <div className="bg-orb bg-orb-3" />
      </div>

      <div className="login-card">
        <div className="login-lang-toggle">
          <button
            id="login-lang-btn"
            className="lang-toggle"
            onClick={() => setLanguage(language === 'en' ? 'ckb' : 'en')}
          >
            🌐 {language === 'en' ? 'کوردی' : 'English'}
          </button>
        </div>

        <div className="login-logo">S</div>

        <div className="login-heading">
          <h1 className="login-title">{t('welcome')}</h1>
          <p className="login-subtitle">{t('loginSubtitle')}</p>
        </div>

        <form className="login-form" onSubmit={handleSubmit} noValidate>
          {errorMsg && (
            <div id="login-error" className="login-error" role="alert">{errorMsg}</div>
          )}

          <div className="form-group">
            <label className="form-label" htmlFor="username">{t('username')}</label>
            <input
              id="username" type="text" className="form-input"
              value={username} onChange={(e) => setUsername(e.target.value)}
              placeholder={t('usernamePlaceholder')} autoComplete="username" autoFocus
            />
          </div>

          <div className="form-group">
            <label className="form-label" htmlFor="password">{t('password')}</label>
            <input
              id="password" type="password" className="form-input"
              value={password} onChange={(e) => setPassword(e.target.value)}
              placeholder={t('passwordPlaceholder')} autoComplete="current-password"
            />
          </div>

          <button
            id="login-submit-btn"
            type="submit"
            className="btn btn-primary btn-lg"
            style={{ width: '100%', justifyContent: 'center', marginTop: 4 }}
            disabled={loading}
          >
            {loading ? t('loading') : t('login')}
          </button>
        </form>

        <p className="login-footer">{t('loginWithAdmin')}</p>
      </div>
    </div>
  );
}
