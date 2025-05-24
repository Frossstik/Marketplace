const NotFound = () => {
  return (
    <div className="text-center py-24">
      <h1 className="text-5xl font-bold text-gray-800 mb-4">404</h1>
      <p className="text-lg text-gray-600 mb-6">Упс! Страница не найдена.</p>
      <a href="/" className="text-blue-600 hover:underline">
        Вернуться на главную
      </a>
    </div>
  );
};

export default NotFound;
