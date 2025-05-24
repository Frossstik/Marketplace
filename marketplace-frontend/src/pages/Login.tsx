import { useState } from 'react';

const Login = () => {
  const [form, setForm] = useState({ email: '', password: '' });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    console.log('Login:', form);
    alert('Вход выполнен (мок)');
  };

  const handleOAuthClick = (provider: 'google' | 'yandex') => {
    console.log(`OAuth via ${provider}`);
    alert(`OAuth через ${provider} (заглушка)`);
    // можно заменить на: window.location.href = `/auth/${provider}`;
  };

  return (
    <div className="max-w-md mx-auto py-12">
      <h2 className="text-3xl font-bold mb-6 text-center">Вход</h2>

      {/* OAuth buttons */}
      <div className="space-y-3 mb-8">
        <button
          onClick={() => handleOAuthClick('google')}
          className="w-full flex items-center justify-center gap-2 bg-white border border-gray-300 py-2 rounded hover:bg-gray-50"
        >
          <img
            src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/google/google-original.svg"
            alt="Google"
            className="w-5 h-5"
          />
          <span>Войти через Google</span>
        </button>
        <button
          onClick={() => handleOAuthClick('yandex')}
          className="w-full flex items-center justify-center gap-2 bg-white border border-gray-300 py-2 rounded hover:bg-gray-50"
        >
          <img
            src="https://upload.wikimedia.org/wikipedia/commons/0/0b/Yandex_icon.svg"
            alt="Yandex"
            className="w-5 h-5"
          />
          <span>Войти через Yandex</span>
        </button>
      </div>

      {/* Email/password login */}
      <form onSubmit={handleSubmit} className="space-y-4">
        <input
          name="email"
          type="email"
          placeholder="Email"
          value={form.email}
          onChange={handleChange}
          className="w-full border border-gray-300 rounded px-4 py-2"
          required
        />
        <input
          name="password"
          type="password"
          placeholder="Пароль"
          value={form.password}
          onChange={handleChange}
          className="w-full border border-gray-300 rounded px-4 py-2"
          required
        />
        <button
          type="submit"
          className="w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700"
        >
          Войти
        </button>
      </form>
    </div>
  );
};

export default Login;
