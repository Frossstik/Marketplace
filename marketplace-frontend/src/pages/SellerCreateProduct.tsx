import { gql, useMutation, useQuery } from '@apollo/client';
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { jwtDecode } from 'jwt-decode';

const GET_CATEGORIES = gql`
  query GetCategories {
    categories {
      id
      name
    }
  }
`;

const CREATE_CATEGORY = gql`
  mutation CreateCategory($input: CreateCategoryInput!) {
    createCategory(input: $input) {
      uuid
    }
  }
`;

const CREATE_PRODUCT = gql`
  mutation CreateProduct($input: CreateProductInput!) {
    createProduct(input: $input) {
      product {
        id
      }
    }
  }
`;

type DecodedToken = {
  sub: string;
};

const SellerCreateProduct = () => {
  const navigate = useNavigate();

  const [creatorId, setCreatorId] = useState<string | null>(null);
  const [form, setForm] = useState({
    name: '',
    description: '',
    price: '',
    count: '',
    categoryName: '',
    imagePaths: [] as string[],
  });

  const { data: categoryData, refetch: refetchCategories } = useQuery(GET_CATEGORIES);
  const categories = categoryData?.categories ?? [];

  const [createCategory] = useMutation(CREATE_CATEGORY);
  const [createProduct, { loading }] = useMutation(CREATE_PRODUCT, {
    onCompleted: () => {
      alert('Товар успешно создан!');
      navigate('/products');
    },
    onError: (error) => {
      console.error('Ошибка при создании товара:', error.message);
      alert('Ошибка при создании товара: ' + error.message);
    },
  });

  useEffect(() => {
    const token = localStorage.getItem('token');
    if (!token) return;

    try {
      const decoded = jwtDecode<DecodedToken>(token);
      setCreatorId(decoded.sub);
    } catch (e) {
      console.error('Ошибка при декодировании токена:', e);
    }
  }, []);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const handleImageChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const files = Array.from(e.target.files ?? []);
    if (!files.length) return;

    Promise.all(
      files.map((file) => {
        return new Promise<string>((resolve, reject) => {
          const reader = new FileReader();
          reader.onloadend = () => {
            const result = reader.result as string;
            console.log('📷 Загружено изображение:', result.slice(0, 100)); //лог
            if (result.startsWith('data:image/')) {
              resolve(result);
            } else {
              reject(new Error('Файл не является изображением'));
            }
          };
          reader.onerror = reject;
          reader.readAsDataURL(file);
        });
      })
    )
      .then((images) => {
        setForm((prev) => ({
          ...prev,
          imagePaths: [...prev.imagePaths, ...images],
        }));
      })
      .catch((err) => {
        console.error('Ошибка загрузки изображения:', err);
        alert('Некоторые изображения не были загружены. Проверьте формат файлов.');
      });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!creatorId) {
      alert('Пользователь не авторизован');
      return;
    }

    const categoryName = form.categoryName.trim();
    let categoryId = categories.find(
      (c: any) => c.name.toLowerCase() === categoryName.toLowerCase()
    )?.id;

    if (!categoryId) {
      try {
        const result = await createCategory({
          variables: {
            input: {
              input: {
                name: categoryName,
              },
            },
          },
        });

        categoryId = result?.data?.createCategory?.uuid;
        await refetchCategories();
      } catch (error: any) {
        console.error('Ошибка при создании категории:', error.message);
        alert('Не удалось создать категорию');
        return;
      }
    }

    const normalizedImagePaths = form.imagePaths.map((path) => {
      if (typeof path !== 'string') return ''; // защита от мусора
      if (path.startsWith('data:image') && !path.includes(',')) {
        // в теории такого быть не должно, но вдруг
        return path;
      }

      if (
        path.startsWith('data:image/jpeg') ||
        path.startsWith('data:image/png') ||
        path.startsWith('data:image/webp')
      ) {
        return path; // всё в порядке
      }

      if (path.startsWith('base64,')) {
        return 'data:image/jpeg;' + path; // крайне редкий случай
      }

      return path;
    });

    const payload = {
      name: form.name,
      description: form.description,
      price: parseFloat(form.price),
      count: parseInt(form.count),
      imagePaths: normalizedImagePaths,
      categoryId,
    };

    console.log('📤 Отправляем товар:', payload); //Лог

    await createProduct({
      variables: {
        input: {
          input: payload,
        },
      },
    });
  };

  return (
    <div className="max-w-xl mx-auto py-12">
      <h2 className="text-3xl font-bold mb-6 text-center">Создание товара</h2>

      <form onSubmit={handleSubmit} className="space-y-4">
        <input
          name="name"
          placeholder="Название"
          value={form.name}
          onChange={handleChange}
          required
          className="w-full border px-4 py-2 rounded"
        />
        <textarea
          name="description"
          placeholder="Описание"
          value={form.description}
          onChange={handleChange}
          className="w-full border px-4 py-2 rounded h-24"
        />
        <input
          name="price"
          type="number"
          placeholder="Цена"
          value={form.price}
          onChange={handleChange}
          required
          className="w-full border px-4 py-2 rounded"
        />
        <input
          name="count"
          type="number"
          placeholder="Количество"
          value={form.count}
          onChange={handleChange}
          required
          className="w-full border px-4 py-2 rounded"
        />
        <input
          name="categoryName"
          placeholder="Категория"
          value={form.categoryName}
          onChange={handleChange}
          required
          className="w-full border px-4 py-2 rounded"
        />

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Изображения (первая будет превью)
          </label>
          <input
            type="file"
            accept="image/*"
            multiple
            onChange={handleImageChange}
            className="w-full border px-4 py-2 rounded bg-white"
          />
        </div>

        {form.imagePaths.length > 0 && (
          <div className="grid grid-cols-3 gap-2">
            {form.imagePaths.map((img, i) => (
              <div key={i} className="relative">
                <img src={img} className="w-full h-24 object-cover border rounded" alt={`img-${i}`} />
                <button
                  type="button"
                  onClick={() =>
                    setForm((prev) => ({
                      ...prev,
                      imagePaths: prev.imagePaths.filter((_, index) => index !== i),
                    }))
                  }
                  className="absolute top-1 right-1 bg-red-600 text-white text-xs px-1 rounded"
                >
                  ✕
                </button>
              </div>
            ))}
          </div>
        )}

        <button
          type="submit"
          disabled={loading}
          className="w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700 disabled:opacity-50"
        >
          {loading ? 'Создание...' : 'Создать товар'}
        </button>
      </form>
    </div>
  );
};

export default SellerCreateProduct;
