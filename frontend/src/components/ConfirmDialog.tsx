import { useI18n } from '../contexts/I18nContext';

interface Props {
  onConfirm: () => void;
  onCancel: () => void;
  loading?: boolean;
}

export default function ConfirmDialog({ onConfirm, onCancel, loading }: Props) {
  const { t } = useI18n();
  return (
    <div className="confirm-overlay">
      <div className="confirm-box">
        <div className="confirm-icon">🗑️</div>
        <h3 className="confirm-title">{t('confirmDeleteTitle')}</h3>
        <p className="confirm-text">{t('confirmDeleteText')}</p>
        <div className="confirm-actions">
          <button id="confirm-cancel-btn" className="btn btn-ghost" onClick={onCancel} disabled={loading}>{t('cancel')}</button>
          <button id="confirm-delete-btn" className="btn btn-danger" onClick={onConfirm} disabled={loading}>
            {loading ? t('loading') : t('delete')}
          </button>
        </div>
      </div>
    </div>
  );
}
