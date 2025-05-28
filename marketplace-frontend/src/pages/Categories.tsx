import { useQuery } from '@apollo/client';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { GET_CATEGORIES } from '../api/graphql/queries/categoriesQueries';

const Categories = () => {
  const { data, loading, error } = useQuery(GET_CATEGORIES);
  const [search, setSearch] = useState('');
  const navigate = useNavigate();

  const categories = data?.categories || [];

  const filtered = categories.filter((c: any) =>
    c.name.toLowerCase().includes(search.toLowerCase())
  );

  if (loading) return <p className="text-center py-12">Загрузка...</p>;
  if (error) return <p className="text-center text-red-500">Ошибка: {error.message}</p>;

  return (
    <div className="max-w-4xl mx-auto py-12 px-4">
      <h2 className="text-3xl font-bold mb-6">Категории</h2>

      <input
        type="text"
        placeholder="Поиск категорий..."
        value={search}
        onChange={(e) => setSearch(e.target.value)}
        className="mb-6 w-full border px-4 py-2 rounded"
      />

      <ul className="space-y-2">
        {filtered.map((cat: any) => (
          <li
            key={cat.id}
            onClick={() => navigate(`/category/${cat.id}`)}
            className="p-3 bg-gray-100 rounded hover:bg-gray-200 cursor-pointer"
          >
            {cat.name}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default Categories;
