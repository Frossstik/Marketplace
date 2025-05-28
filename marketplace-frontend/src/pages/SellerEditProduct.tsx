import { useQuery, useMutation } from '@apollo/client';
import { useParams, useNavigate } from 'react-router-dom';
import { GET_PRODUCT } from '../api/graphql/queries/productsQueries';
import { UPDATE_PRODUCT } from '../api/graphql/mutations/productsMutations';
import { GET_CATEGORIES } from '../api/graphql/queries/categoriesQueries';
import { useEffect, useState } from 'react';

const SellerEditProduct = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { data: productData, loading: productLoading } = useQuery(GET_PRODUCT, {
    variables: { id },
    skip: !id,
  });

  const { data: categoriesData } = useQuery(GET_CATEGORIES);

  const [formData, setFormData] = useState({
    name: '',
    description: '',
    price: '',
    count: '',
    imagePaths: '',
    categoryId: '',
  });

  useEffect(() => {
    if (productData?.productById) {
      const p = productData.productById;
      setFormData({
        name: p.name ?? '',
        description: p.description ?? '',
        price: p.price?.toString() ?? '',
        count: p.count?.toString() ?? '',
        imagePaths: p.imagePaths?.join('\n') ?? '',
        categoryId: p.categoryId ?? '',
      });
    }
  }, [productData]);

  const [updateProduct] = useMutation(UPDATE_PRODUCT);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await updateProduct({
        variables: {
          input: {
            input: {
              id,
              name: formData.name,
              description: formData.description,
              price: parseFloat(formData.price),
              count: parseInt(formData.count),
              imagePaths: formData.imagePaths.split('\n'),
              categoryId: formData.categoryId,
            },
          },
        },
      });
      navigate('/seller/my-products');
    } catch (err) {
      console.error('Ошибка при обновлении товара:', err);
      alert('Ошибка при обновлении товара.');
    }
  };

  if (productLoading) return <p className="text-center py-12">Загрузка...</p>;

  return (
    <div className="max-w-2xl mx-auto py-12">
      <h2 className="text-3xl font-bold mb-6 text-center">Редактировать товар</h2>
      <form onSubmit={handleSubmit} className="space-y-4">
        <input name="name" value={formData.name} onChange={handleChange} required placeholder="Название" className="input" />
        <textarea name="description" value={formData.description} onChange={handleChange} placeholder="Описание" className="input" />
        <input type="number" name="price" value={formData.price} onChange={handleChange} required placeholder="Цена" className="input" />
        <input type="number" name="count" value={formData.count} onChange={handleChange} required placeholder="Количество" className="input" />
        {/* <textarea name="imagePaths" value={formData.imagePaths} onChange={handleChange} placeholder="Ссылки на изображения (по строке)" className="input" /> */}

        <select name="categoryId" value={formData.categoryId} onChange={handleChange} required className="input">
          <option value="" disabled>Выберите категорию</option>
          {categoriesData?.categories.map((c: any) => (
            <option key={c.id} value={c.id}>{c.name}</option>
          ))}
        </select>

        <button type="submit" className="w-full bg-green-600 text-white py-2 rounded hover:bg-green-700">
          Сохранить изменения
        </button>
      </form>
    </div>
  );
};

export default SellerEditProduct;
