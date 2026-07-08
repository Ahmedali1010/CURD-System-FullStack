import React, { createContext, useContext, useState, useCallback } from 'react';
import { jwtDecode } from 'jwt-decode';

interface DecodedToken {
  sub: string;
  username: string;
  role: string;
  exp: number;
}

export interface AuthUser {
  id: string;
  username: string;
  role: 'Admin' | 'User';
}

interface AuthContextValue {
  user: AuthUser | null;
  token: string | null;
  login: (token: string) => void;
  logout: () => void;
  isAdmin: boolean;
}

const AuthContext = createContext<AuthContextValue | null>(null);

function parseToken(token: string): AuthUser | null {
  try {
    const decoded = jwtDecode<DecodedToken>(token);
    if (decoded.exp * 1000 < Date.now()) return null;
    return { id: decoded.sub, username: decoded.username, role: decoded.role as 'Admin' | 'User' };
  } catch {
    return null;
  }
}

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [token, setToken] = useState<string | null>(() => localStorage.getItem('token'));
  const [user,  setUser]  = useState<AuthUser | null>(() => {
    const t = localStorage.getItem('token');
    return t ? parseToken(t) : null;
  });

  const login = useCallback((newToken: string) => {
    const parsed = parseToken(newToken);
    if (parsed) {
      localStorage.setItem('token', newToken);
      setToken(newToken);
      setUser(parsed);
    }
  }, []);

  const logout = useCallback(() => {
    localStorage.removeItem('token');
    setToken(null);
    setUser(null);
  }, []);

  return (
    <AuthContext.Provider value={{ user, token, login, logout, isAdmin: user?.role === 'Admin' }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth(): AuthContextValue {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error('useAuth must be inside <AuthProvider>');
  return ctx;
}
