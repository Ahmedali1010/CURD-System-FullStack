import { useState, useEffect } from 'react';
import { useI18n } from '../contexts/I18nContext';
import type { Product, ProductPayload } from '../api/products';

interface Props {
  product?: Product | null;
  onSave: (data: ProductPayload) => Promise<void>;
  onClose: () => void;
}

export default function ProductModal({ product, onSave, onClose }: Props) {
  const { t } = useI18n();
  const isEdit = !!product;

  const [name,        setName]        = useState(product?.name ?? '');
  const [description, setDescription] = useState(product?.description ?? '');
  const [price,       setPrice]       = useState(String(product?.price ?? ''));
  const [errors,      setErrors]      = useState<Record<string, string>>({});
  const [saving,      setSaving]      = useState(false);

  useEffect(() => {
    const fn = (e: KeyboardEvent) => { if (e.key === 'Escape') onClose(); };
    window.addEventListener('keydown', fn);
    return () => window.removeEventListener('keydown', fn);
  }, [onClose]);

  const validate = () => {
    const errs: Record<string, string> = {};
    if (!name.trim()) errs.name = t('required');
    const p = parseFloat(price);
    if (isNaN(p) || p < 0) errs.price = t('invalidPrice');
    setErrors(errs);
    return Object.keys(errs).length === 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!validate()) return;
    setSaving(true);
    try {
      await onSave({ name: name.trim(), description: description.trim(), price: parseFloat(price) });
      onClose();
    } finally {
      setSaving(false);
    }
  };

  return (
    <div className="modal-overlay" onClick={(e) => { if (e.target === e.currentTarget) onClose(); }}>
      <div className="modal-content" role="dialog" aria-modal="true">
        <div className="modal-header">
          <h2 className="modal-title">{isEdit ? t('updateProduct') : t('createProduct')}</h2>
          <button id="modal-close-btn" className="modal-close" onClick={onClose} aria-label="Close">✕</button>
        </div>

        <form onSubmit={handleSubmit}>
          <div className="modal-body">
            <div className="form-group">
              <label className="form-label" htmlFor="product-name">{t('productName')}</label>
              <input
                id="product-name" className="form-input" type="text" autoFocus
                value={name} onChange={(e) => setName(e.target.value)}
                placeholder={t('namePlaceholder')}
              />
              {errors.name && <span className="form-error">{errors.name}</span>}
            </div>

            <div className="form-group">
              <label className="form-label" htmlFor="product-desc">{t('productDescription')}</label>
              <textarea
                id="product-desc" className="form-input form-textarea" rows={3}
                value={description} onChange={(e) => setDescription(e.target.value)}
                placeholder={t('descriptionPlaceholder')}
              />
            </div>

            <div className="form-group">
              <label className="form-label" htmlFor="product-price">{t('productPrice')}</label>
              <input
                id="product-price" className="form-input" type="number" min="0" step="0.01"
                value={price} onChange={(e) => setPrice(e.target.value)}
                placeholder={t('pricePlaceholder')}
              />
              {errors.price && <span className="form-error">{errors.price}</span>}
            </div>
          </div>

          <div className="modal-footer">
            <button id="modal-cancel-btn" type="button" className="btn btn-ghost" onClick={onClose} disabled={saving}>
              {t('cancel')}
            </button>
            <button id="modal-save-btn" type="submit" className="btn btn-primary" disabled={saving}>
              {saving ? t('loading') : isEdit ? t('saveChanges') : t('createProduct')}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
