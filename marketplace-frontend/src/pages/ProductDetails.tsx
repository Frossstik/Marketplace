import { useParams } from 'react-router-dom';
import { useQuery } from '@apollo/client';
import { GET_PRODUCT } from '../api/graphql/queries/productsQueries';
import { useCart } from '../context/CartContext';
import ImageCarousel from '../components/ImageCarousel';

const ProductDetails = () => {
  const { addToCart } = useCart();
  const { id } = useParams<{ id: string }>();

  const { data, loading, error } = useQuery(GET_PRODUCT, {
    variables: { id },
  });

  if (loading) return <p className="text-center py-12">Загрузка товара...</p>;
  if (error || !data?.productById) return <p className="text-center py-12">Ошибка: товар не найден</p>;

  const p = data.productById;
  console.log('📥 Получен продукт с imagePaths:', p.imagePaths);

  return (
    <div className="max-w-4xl mx-auto py-12 px-4 space-y-8">
      {p.imagePaths?.length ? (
        <ImageCarousel images={p.imagePaths} />
      ) : (
        <div className="w-full h-64 bg-gray-100 flex items-center justify-center text-gray-400 rounded">
          Нет изображений
        </div>
      )}

      <div className="space-y-2">
        <h2 className="text-3xl font-bold">{p.name}</h2>
        <p><strong>Цена:</strong> {p.price} ₽</p>
        <p><strong>Категория:</strong> {p.category?.name}</p>
        <p><strong>Продавец:</strong> {p.creator?.companyName || '—'}</p>
        <p><strong>Описание:</strong> {p.description || '—'}</p>
        <p><strong>Количество:</strong> {p.count}</p>
        <button
          onClick={() => {
            addToCart({
              productId: p.id,
              productName: p.name,
              price: p.price,
              quantity: 1,
            });
          }}
          className="mt-4 bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
        >
          Добавить в корзину
        </button>
      </div>
    </div>
  );
};

export default ProductDetails;
