import { useCart } from '../context/CartContext';

const Cart = () => {
  const { cartItems, removeFromCart } = useCart();

  const total = cartItems.reduce(
    (sum, item) => sum + item.price * item.quantity,
    0
  );

  return (
    <div className="py-12 max-w-2xl mx-auto">
      <h2 className="text-3xl font-bold text-center mb-6">Корзина</h2>
      {cartItems.length === 0 ? (
        <p className="text-center">Корзина пуста.</p>
      ) : (
        <>
          <ul className="space-y-4">
            {cartItems.map(item => (
              <li key={item.productId} className="border p-4 rounded shadow-sm">
                <div className="flex justify-between items-center">
                  <div>
                    <h4 className="font-semibold">{item.productName}</h4>
                    <p className="text-sm text-gray-500">
                      {item.price} ₽ × {item.quantity}
                    </p>
                  </div>
                  <button
                    onClick={() => removeFromCart(item.productId)}
                    className="text-red-500 hover:underline"
                  >
                    Удалить
                  </button>
                </div>
              </li>
            ))}
          </ul>
          <div className="text-right mt-6 text-lg font-bold">
            Итого: {total} ₽
          </div>
        </>
      )}
    </div>
  );
};

export default Cart;
