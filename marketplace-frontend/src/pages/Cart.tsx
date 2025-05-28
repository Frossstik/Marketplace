import { useNavigate } from 'react-router-dom';
import { useCart } from '../context/CartContext';

const Cart = () => {
  const { cartItems, removeFromCart, clearCart, getTotalPrice } = useCart();
  const navigate = useNavigate();

  if (cartItems.length === 0) {
    return <p className="text-center py-12 text-lg">🛒 Корзина пуста</p>;
  }

  return (
    <div className="max-w-3xl mx-auto py-12">
      <h2 className="text-3xl font-bold mb-6">Ваша корзина</h2>

      <div className="space-y-4">
        {cartItems.map((item) => (
          <div
            key={item.productId}
            className="flex justify-between items-center border p-4 rounded"
          >
            <div>
              <p className="font-semibold">{item.productName}</p>
              <p>
                {item.price} ₽ × {item.quantity} ={' '}
                <span className="font-bold">{item.price * item.quantity} ₽</span>
              </p>
            </div>
            <button
              onClick={() => removeFromCart(item.productId)}
              className="text-red-600 hover:underline"
            >
              Удалить
            </button>
          </div>
        ))}
      </div>

      <div className="mt-6 text-lg font-semibold">
        Общая сумма: {getTotalPrice()} ₽
      </div>

      <div className="flex justify-between items-center mt-6 gap-4">
        <button
          onClick={() => clearCart()}
          className="bg-gray-300 text-black px-4 py-2 rounded hover:bg-gray-400"
        >
          Очистить корзину
        </button>

        <button
          onClick={() => navigate('/checkout')}
          className="bg-blue-600 text-white px-6 py-2 rounded hover:bg-blue-700"
        >
          Перейти к оформлению
        </button>
      </div>
    </div>
  );
};

export default Cart;
