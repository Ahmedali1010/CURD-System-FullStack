import { useState, useEffect, useCallback } from 'react';
import { useI18n } from '../contexts/I18nContext';
import { useAuth } from '../contexts/AuthContext';
import Navbar from '../components/Navbar';
import ProductModal from '../components/ProductModal';
import ConfirmDialog from '../components/ConfirmDialog';
import ToastContainer from '../components/Toast';
import type { ToastItem, ToastType } from '../components/Toast';
import { productApi } from '../api/products';
import type { Product, ProductPayload } from '../api/products';

const fmt    = (d: string) => new Date(d).toLocaleDateString(undefined, { year:'numeric', month:'short', day:'numeric' });
const fmtUSD = (n: number) => new Intl.NumberFormat('en-US', { style:'currency', currency:'USD' }).format(n);

export default function DashboardPage() {
  const { t } = useI18n();
  const { isAdmin } = useAuth();

  const [products,     setProducts]     = useState<Product[]>([]);
  const [loading,      setLoading]      = useState(true);
  const [modalOpen,    setModalOpen]    = useState(false);
  const [editProduct,  setEditProduct]  = useState<Product | null>(null);
  const [deleteTarget, setDeleteTarget] = useState<Product | null>(null);
  const [deleting,     setDeleting]     = useState(false);
  const [toasts,       setToasts]       = useState<ToastItem[]>([]);

  const addToast = useCallback((message: string, type: ToastType) => {
    setToasts(prev => [...prev, { id: crypto.randomUUID(), message, type }]);
  }, []);

  const dismissToast = useCallback((id: string) => {
    setToasts(prev => prev.filter(t => t.id !== id));
  }, []);

  const load = useCallback(async () => {
    try {
      setLoading(true);
      const res = await productApi.getAll();
      setProducts(res.data);
    } catch {
      addToast(t('networkError'), 'error');
    } finally {
      setLoading(false);
    }
  }, [addToast, t]);

  useEffect(() => { load(); }, [load]);

  const handleSave = async (data: ProductPayload) => {
    try {
      if (editProduct) {
        await productApi.update(editProduct.id, data);
        addToast(t('productUpdated'), 'success');
      } else {
        await productApi.create(data);
        addToast(t('productCreated'), 'success');
      }
      await load();
    } catch {
      addToast(t('operationFailed'), 'error');
      throw new Error('save failed');
    }
  };

  const handleDelete = async () => {
    if (!deleteTarget) return;
    setDeleting(true);
    try {
      await productApi.remove(deleteTarget.id);
      addToast(t('productDeleted'), 'success');
      setDeleteTarget(null);
      await load();
    } catch {
      addToast(t('operationFailed'), 'error');
    } finally {
      setDeleting(false);
    }
  };

  return (
    <>
      <div className="bg-orbs">
        <div className="bg-orb bg-orb-1" />
        <div className="bg-orb bg-orb-2" />
      </div>

      <div className="app-wrapper">
        <Navbar />

        <main className="dashboard-page">
          <div className="dashboard-header">
            <div>
              <h1 className="dashboard-title">{t('products')}</h1>
              <p className="dashboard-subtitle">{products.length} {t('productsCount')}</p>
            </div>
            {isAdmin && (
              <button id="add-product-btn" className="btn btn-primary" onClick={() => { setEditProduct(null); setModalOpen(true); }}>
                <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5" strokeLinecap="round">
                  <line x1="12" y1="5" x2="12" y2="19"/><line x1="5" y1="12" x2="19" y2="12"/>
                </svg>
                {t('addProduct')}
              </button>
            )}
          </div>

          <div className="dashboard-card">
            <div className="card-header">
              <span className="card-title">{t('products')}</span>
              {!loading && <span className="card-count">{products.length} {t('productsCount')}</span>}
            </div>

            {loading ? (
              <div className="spinner-wrap"><div className="spinner" /><span>{t('loading')}</span></div>
            ) : products.length === 0 ? (
              <div className="empty-state">
                <span className="empty-icon">📦</span>
                <p className="empty-text">{t('noProducts')}</p>
                {isAdmin && <button className="btn btn-primary" onClick={() => { setEditProduct(null); setModalOpen(true); }}>{t('addProduct')}</button>}
              </div>
            ) : (
              <div className="table-wrapper">
                <table className="data-table">
                  <thead>
                    <tr>
                      <th style={{ width:60 }}>{t('id')}</th>
                      <th>{t('name')}</th>
                      <th>{t('description')}</th>
                      <th>{t('price')}</th>
                      <th>{t('createdAt')}</th>
                      {isAdmin && <th style={{ width:180, textAlign:'end' }}>{t('actions')}</th>}
                    </tr>
                  </thead>
                  <tbody>
                    {products.map(p => (
                      <tr key={p.id}>
                        <td style={{ color:'var(--text-muted)', fontSize:12 }}>#{p.id}</td>
                        <td style={{ fontWeight:600, color:'var(--text-primary)' }}>{p.name}</td>
                        <td style={{ maxWidth:200, overflow:'hidden', textOverflow:'ellipsis', whiteSpace:'nowrap' }}>{p.description || '—'}</td>
                        <td className="price-cell">{fmtUSD(p.price)}</td>
                        <td style={{ fontSize:13 }}>{fmt(p.createdAt)}</td>
                        {isAdmin && (
                          <td>
                            <div className="actions-cell">
                              <button id={`edit-btn-${p.id}`} className="btn btn-success btn-sm"
                                onClick={() => { setEditProduct(p); setModalOpen(true); }}>
                                <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round">
                                  <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"/>
                                  <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"/>
                                </svg>
                                {t('edit')}
                              </button>
                              <button id={`delete-btn-${p.id}`} className="btn btn-danger btn-sm"
                                onClick={() => setDeleteTarget(p)}>
                                <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round">
                                  <polyline points="3 6 5 6 21 6"/>
                                  <path d="M19 6l-1 14H6L5 6"/><path d="M10 11v6M14 11v6"/>
                                  <path d="M9 6V4h6v2"/>
                                </svg>
                                {t('delete')}
                              </button>
                            </div>
                          </td>
                        )}
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            )}
          </div>
        </main>
      </div>

      {modalOpen && (
        <ProductModal
          product={editProduct}
          onSave={handleSave}
          onClose={() => { setModalOpen(false); setEditProduct(null); }}
        />
      )}

      {deleteTarget && (
        <ConfirmDialog onConfirm={handleDelete} onCancel={() => setDeleteTarget(null)} loading={deleting} />
      )}

      <ToastContainer toasts={toasts} onDismiss={dismissToast} />
    </>
  );
}
