import { useAuth } from '../contexts/AuthContext';
import { useI18n } from '../contexts/I18nContext';
import { useNavigate } from 'react-router-dom';

export default function Navbar() {
  const { user, logout } = useAuth();
  const { t, language, setLanguage } = useI18n();
  const navigate = useNavigate();

  return (
    <nav className="navbar">
      <div className="navbar-brand">
        <div className="navbar-logo">S</div>
        <span className="navbar-title">S-System</span>
      </div>

      <div className="navbar-center">
        <span className="navbar-page">{t('dashboard')}</span>
      </div>

      <div className="navbar-actions">
        <button
          id="lang-toggle-btn"
          className="lang-toggle"
          onClick={() => setLanguage(language === 'en' ? 'ckb' : 'en')}
          title={t('language')}
        >
          🌐 {language === 'en' ? 'کوردی' : 'EN'}
        </button>

        {user && (
          <div className="navbar-user">
            <div className="user-avatar">{user.username[0].toUpperCase()}</div>
            <div className="user-info">
              <span className="user-name">{user.username}</span>
              <span className={`badge ${user.role === 'Admin' ? 'badge-admin' : 'badge-user'}`}>
                {t(user.role === 'Admin' ? 'admin' : 'user')}
              </span>
            </div>
          </div>
        )}

        <button
          id="logout-btn"
          className="btn btn-ghost btn-sm"
          onClick={() => { logout(); navigate('/login'); }}
        >
          <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
            <path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4"/>
            <polyline points="16 17 21 12 16 7"/>
            <line x1="21" y1="12" x2="9" y2="12"/>
          </svg>
          {t('logout')}
        </button>
      </div>
    </nav>
  );
}
