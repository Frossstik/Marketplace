import { useCart } from '../context/CartContext';
import { Link } from 'react-router-dom';

const mockProducts = [
  { id: '1', name: 'Товар A', price: 100 },
  { id: '2', name: 'Товар B', price: 150 },
  { id: '3', name: 'Товар C', price: 250 },
];

const Products = () => {
  const { addToCart } = useCart();

  return (
    <div className="py-12">
      <h2 className="text-3xl font-bold text-center mb-8">Товары</h2>
      <div className="grid sm:grid-cols-2 lg:grid-cols-3 gap-6 max-w-6xl mx-auto">
        {mockProducts.map(product => (
          <div key={product.id} className="p-6 border rounded shadow">
            <h3 className="text-xl font-semibold mb-2">
              <Link to={`/product/${product.id}`} className="text-blue-700 hover:underline">
                {product.name}
              </Link>
            </h3>
            <p className="text-blue-600 font-bold mb-4">{product.price} ₽</p>
            <button
              onClick={() =>
                addToCart({
                  productId: product.id,
                  productName: product.name,
                  price: product.price,
                  quantity: 1,
                })
              }
              className="w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700"
            >
              Добавить в корзину
            </button>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Products;
