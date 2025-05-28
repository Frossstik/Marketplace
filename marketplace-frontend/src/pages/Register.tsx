import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { gql, useMutation } from '@apollo/client';

const REGISTER_MUTATION = gql`
  mutation Register($input: RegisterInput!) {
    register(input: $input) {
      authResponse {
        userId
      }
    }
  }
`;

const Register = () => {
  const [form, setForm] = useState({
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    confirm: '',
    role: 'CLIENT',
    companyName: '',
  });

  const navigate = useNavigate();
  const [register, { loading }] = useMutation(REGISTER_MUTATION);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setForm(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (form.password !== form.confirm) {
      alert('Пароли не совпадают');
      return;
    }

    try {
      const { data } = await register({
        variables: {
          input: {
            input: {
              firstName: form.firstName,
              lastName: form.lastName,
              email: form.email,
              password: form.password,
              role: form.role,
              companyName: form.companyName,
            },
          },
        },
      });

      alert(`Регистрация успешна! ID: ${data.register.authResponse.userId}`);
      navigate('/login');
    } catch (error) {
      console.error(error);
      alert('Ошибка при регистрации');
    }
  };

  return (
    <div className="max-w-md mx-auto py-12">
      <h2 className="text-3xl font-bold text-center mb-6">Регистрация</h2>
      <form onSubmit={handleSubmit} className="space-y-4">
        <input
          name="firstName"
          placeholder="Имя"
          value={form.firstName}
          onChange={handleChange}
          className="w-full border px-4 py-2 rounded"
          required
        />
        <input
          name="lastName"
          placeholder="Фамилия"
          value={form.lastName}
          onChange={handleChange}
          className="w-full border px-4 py-2 rounded"
          required
        />
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
        <input
          name="confirm"
          type="password"
          placeholder="Подтвердите пароль"
          value={form.confirm}
          onChange={handleChange}
          className="w-full border px-4 py-2 rounded"
          required
        />
        <select
          name="role"
          value={form.role}
          onChange={handleChange}
          className="w-full border px-4 py-2 rounded"
        >
          <option value="CLIENT">Клиент</option>
          <option value="SELLER">Продавец</option>
          {/* <option value="ADMIN">Администратор</option> */}
        </select>
        <input
          name="companyName"
          placeholder="Название компании"
          value={form.companyName}
          onChange={handleChange}
          className="w-full border px-4 py-2 rounded"
          required={form.role !== 'CLIENT'}
        />

        <button
          type="submit"
          disabled={loading}
          className="w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700 disabled:opacity-50"
        >
          {loading ? 'Регистрация...' : 'Зарегистрироваться'}
        </button>
      </form>
    </div>
  );
};

export default Register;
