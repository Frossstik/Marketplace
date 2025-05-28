import { Link, useNavigate } from 'react-router-dom';
import { useCart } from '../context/CartContext';
import { useMutation } from '@apollo/client';
import { DELETE_PRODUCT } from '../api/graphql/mutations/productsMutations';

type ProductCardProps = {
  id: string;
  name: string;
  price: number;
  description?: string;
  imagePaths?: string[];
  categoryName?: string;
  sellerName?: string;
  count?: number;
  showEditDelete?: boolean;
};

const getImageSrc = (path?: string): string => {
  if (!path) return '/MarketplaceLogo.jpg';
  return path.startsWith('data:image') ? path : '/MarketplaceLogo.jpg';
};

const ProductCard = ({
  id,
  name,
  price,
  description,
  imagePaths,
  categoryName,
  sellerName,
  count,
  showEditDelete,
}: ProductCardProps) => {
  const { addToCart } = useCart();
  const navigate = useNavigate();

  const [deleteProduct] = useMutation(DELETE_PRODUCT, {
    variables: { input: { id } },
    refetchQueries: ['ProductsByCreator', 'GetProducts'],
  });

  const previewImage = getImageSrc(imagePaths?.[0]);

  const handleAdd = () => {
    addToCart({
      productId: id,
      productName: name,
      price,
      quantity: 1,
    });
  };

  const handleEdit = () => {
    navigate(`/seller/edit-product/${id}`);
  };

  const handleDelete = async () => {
    if (confirm('Вы уверены, что хотите удалить этот товар?')) {
      try {
        await deleteProduct();
      } catch (error) {
        console.error('Ошибка при удалении товара:', error);
      }
    }
  };

  return (
    <div className="border rounded shadow hover:shadow-lg transition overflow-hidden flex flex-col">
      <Link to={`/product/${id}`}>
        <img
          src={previewImage}
          alt={name}
          className="w-full h-48 object-cover"
          onError={(e) => {
            (e.target as HTMLImageElement).src = '/MarketplaceLogo.jpg';
          }}
        />
        <div className="p-4 space-y-2">
          <h3 className="text-lg font-bold truncate">{name}</h3>
          <p className="text-blue-600 font-semibold">{price} ₽</p>
        </div>
      </Link>

      {showEditDelete ? (
        <div className="flex justify-between mt-auto px-4 pb-4 gap-2">
          <button
            onClick={handleEdit}
            className="bg-yellow-500 text-white px-3 py-1 rounded hover:bg-yellow-600 w-full"
          >
            Изменить
          </button>
          <button
            onClick={handleDelete}
            className="bg-red-600 text-white px-3 py-1 rounded hover:bg-red-700 w-full"
          >
            Удалить
          </button>
        </div>
      ) : (
        <button
          onClick={handleAdd}
          disabled={count === 0}
          className="mt-auto bg-green-600 text-white py-2 w-full hover:bg-green-700 disabled:opacity-50"
        >
          {count === 0 ? 'Нет в наличии' : 'В корзину'}
        </button>
      )}
    </div>
  );
};

export default ProductCard;
