import { useCart } from '../context/CartContext';
import { useNavigate } from 'react-router-dom';

const Checkout = () => {
  const { cartItems } = useCart();
  const navigate = useNavigate();

  const total = cartItems.reduce(
    (sum, item) => sum + item.price * item.quantity,
    0
  );

  const handleNext = () => {
    navigate('/payment');
  };

  return (
    <div className="max-w-2xl mx-auto py-12">
      <h2 className="text-3xl font-bold text-center mb-6">Оформление заказа</h2>
      {cartItems.length === 0 ? (
        <p className="text-center">Корзина пуста.</p>
      ) : (
        <>
          <ul className="space-y-4">
            {cartItems.map(item => (
              <li key={item.productId} className="border p-4 rounded shadow">
                <div className="flex justify-between">
                  <div>
                    <h4 className="font-semibold">{item.productName}</h4>
                    <p className="text-sm text-gray-500">
                      {item.price} ₽ × {item.quantity}
                    </p>
                  </div>
                  <div className="font-bold">
                    {item.price * item.quantity} ₽
                  </div>
                </div>
              </li>
            ))}
          </ul>
          <div className="text-right mt-6 text-lg font-bold">
            Итого: {total} ₽
          </div>
          <button
            onClick={handleNext}
            className="mt-6 w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700"
          >
            Перейти к оплате
          </button>
        </>
      )}
    </div>
  );
};

export default Checkout;
