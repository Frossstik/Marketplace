import { jwtDecode } from 'jwt-decode';
import { useNavigate } from 'react-router-dom';
import { useEffect, useState } from 'react';

type DecodedToken = {
  email: string;
  exp: number;
  sub: string;
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'?: string | string[];
};

const Profile = () => {
  const navigate = useNavigate();
  const [email, setEmail] = useState('');
  const [role, setRole] = useState('');
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const token = localStorage.getItem('token');
    if (!token) {
      navigate('/login');
      return;
    }

    try {
      const decoded = jwtDecode<DecodedToken>(token);

      setEmail(decoded.email);

      const roleClaim = decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

      if (Array.isArray(roleClaim)) {
        setRole(roleClaim[0]);
      } else if (typeof roleClaim === 'string') {
        setRole(roleClaim);
      } else {
        setRole('Неизвестно');
      }
    } catch (e) {
      console.error('Ошибка при декодировании токена:', e);
      localStorage.removeItem('token');
      navigate('/login');
    } finally {
      setLoading(false);
    }
  }, [navigate]);

  const handleLogout = () => {
    localStorage.clear();
    navigate('/login');
  };

  if (loading) return <p className="text-center py-12">Загрузка...</p>;

  return (
    <div className="max-w-lg mx-auto py-12">
      <h2 className="text-3xl font-bold mb-6 text-center">Личный кабинет</h2>

      <div className="space-y-4 text-lg">
        <p><strong>Email:</strong> {email}</p>
        <p><strong>Роль:</strong> {role}</p>
      </div>

      <button
        onClick={() => navigate('/orders')}
        className="mt-4 w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700"
      >
        История заказов
      </button>

      {(role === 'Admin' || role === 'Seller') && (
        <div>
          <button
            onClick={() => navigate('/seller/create-product')}
            className="w-full bg-green-600 text-white py-2 rounded hover:bg-green-700"
          >
            Создать товар
          </button>
          <button
            onClick={() => navigate('/seller/my-products')}
            className="w-full bg-indigo-600 text-white py-2 rounded hover:bg-indigo-700"
          >
            Мои товары
          </button>
        </div>
      )}

      <button
        onClick={handleLogout}
        className="mt-4 w-full bg-red-600 text-white py-2 rounded hover:bg-red-700"
      >
        Выйти
      </button>
    </div>
  );
};

export default Profile;
