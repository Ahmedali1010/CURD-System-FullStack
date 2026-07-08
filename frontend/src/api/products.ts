import api from './axios';

export interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  createdAt: string;
  updatedAt: string;
}

export interface ProductPayload {
  name: string;
  description?: string;
  price: number;
}

export const productApi = {
  getAll:  ()                             => api.get<Product[]>('/api/products'),
  getById: (id: number)                   => api.get<Product>(`/api/products/${id}`),
  create:  (data: ProductPayload)         => api.post<Product>('/api/products', data),
  update:  (id: number, data: ProductPayload) => api.put<Product>(`/api/products/${id}`, data),
  remove:  (id: number)                   => api.delete(`/api/products/${id}`),
};
