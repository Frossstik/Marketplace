const Header = () => (
  <header className="bg-white shadow-sm">
    <nav className="max-w-7xl mx-auto px-4 py-4 flex justify-between items-center">
      <div className="flex items-center">
        <a href="/" className="mr-3">
          <img 
            src="/FullLogoMarketplace.jpg"
            alt="Logo" 
            className="h-12 w-50"
          />
        </a>
        {/* <h1 className="text-2xl font-bold text-blue-600">Marketplace</h1> */}
      </div>
      <div className="space-x-6">
        <a href="/" className="text-gray-700 hover:text-blue-600">Главная</a>
        <a href="/products" className="text-gray-700 hover:text-blue-600">Товары</a>
        <a href="/cart" className="text-gray-700 hover:text-blue-600">Корзина</a>
        <a href="/login" className="text-gray-700 hover:text-blue-600">Вход</a>
        <a href="/register" className="text-gray-700 hover:text-blue-600">Регистрация</a>
      </div>
    </nav>
  </header>
);

export default Header;