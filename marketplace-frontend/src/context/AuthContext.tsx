import { createContext, useContext, useEffect, useState } from 'react';
import { jwtDecode } from 'jwt-decode';

type DecodedToken = {
  id: string;
  email: string;
  role: string;
  sub: string;
  exp: number;
};

type AuthContextType = {
  isAuthenticated: boolean;
  user: DecodedToken | null;
  login: (token: string) => void;
  logout: () => void;
};

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [user, setUser] = useState<DecodedToken | null>(null);

  useEffect(() => {
    const token = localStorage.getItem('token');
    if (token) {
      try {
        const decoded = jwtDecode<DecodedToken>(token);
        if (decoded.exp * 1000 > Date.now()) {
          setUser(decoded);
        } else {
          localStorage.removeItem('token');
        }
      } catch {
        localStorage.removeItem('token');
      }
    }
  }, []);

  const login = (token: string) => {
    localStorage.setItem('token', token);
    const decoded = jwtDecode<DecodedToken>(token);
    setUser(decoded);
  };

  const logout = () => {
    localStorage.removeItem('token');
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{ isAuthenticated: !!user, user, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error('useAuth must be used inside AuthProvider');
  return ctx;
};
