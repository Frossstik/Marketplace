import { Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { useCart } from '../context/CartContext';

const Header = () => {
  const { isAuthenticated, logout } = useAuth();
  const { cartCount } = useCart();

  return (
    <header className="bg-white shadow">
      <nav className="max-w-7xl mx-auto px-4 py-4 flex justify-between items-center">
        <Link to="/">
          <img src="/FullLogoMarketplace.jpg" alt="Logo" className="h-10" />
        </Link>

        <div className="flex gap-6 items-center">
          <Link to="/products" className="hover:text-blue-600">Товары</Link>
          
          <Link to="/categories" className="hover:text-blue-600">Категории</Link>

          <Link to="/cart" className="relative hover:text-blue-600">
            Корзина
            {cartCount > 0 && (
              <span className="absolute -top-2 -right-3 bg-red-500 text-white rounded-full text-xs w-5 h-5 flex items-center justify-center">
                {cartCount}
              </span>
            )}
          </Link>

          {isAuthenticated ? (
            <>
              <Link to="/profile" className="hover:text-blue-600">Личный кабинет</Link>
              <button onClick={logout} className="text-red-500 hover:underline">Выйти</button>
            </>
          ) : (
            <>
              <Link to="/login" className="hover:text-blue-600">Войти</Link>
              <Link to="/register" className="hover:text-blue-600">Регистрация</Link>
            </>
          )}
        </div>
      </nav>
    </header>
  );
};

export default Header;
