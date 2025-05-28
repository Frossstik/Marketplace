import { useState } from 'react';
import { gql, useLazyQuery } from '@apollo/client';
import { useNavigate } from 'react-router-dom';

const LOGIN_QUERY = gql`
  query Login($email: String!, $password: String!) {
    login(input: {
      email: $email,
      password: $password
    }) {
      token
    }
  }
`;

const Login = () => {
  const [form, setForm] = useState({ email: '', password: '' });
  const navigate = useNavigate();

  const [login, { loading, error, data }] = useLazyQuery(LOGIN_QUERY, {
    fetchPolicy: 'no-cache',
    onCompleted: (data) => {
      const token = data?.login?.token;
      if (token) {
        localStorage.setItem('token', token);
        alert('Вход выполнен!');
        navigate('/profile');
      }
    },
    onError: () => {
      alert('Неверный логин или пароль');
    },
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    login({
      variables: {
        email: form.email,
        password: form.password,
      },
    });
  };

  return (
    <div className="max-w-md mx-auto py-12">
      <h2 className="text-3xl font-bold text-center mb-6">Вход</h2>
      <form onSubmit={handleSubmit} className="space-y-4">
        <input
          name="email"
          type="email"
          placeholder="Email"
          value={form.email}
          onChange={handleChange}
          className="w-full border px-4 py-2 rounded"
          required
        />
        <input
          name="password"
          type="password"
          placeholder="Пароль"
          value={form.password}
          onChange={handleChange}
          className="w-full border px-4 py-2 rounded"
          required
        />
        <button
          type="submit"
          disabled={loading}
          className="w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700"
        >
          {loading ? 'Вход...' : 'Войти'}
        </button>
      </form>

      {error && (
        <p className="mt-4 text-red-500 text-sm text-center">
          {error.message}
        </p>
      )}
    </div>
  );
};

export default Login;
