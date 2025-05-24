import { useCart } from '../context/CartContext';
import { useNavigate } from 'react-router-dom';
import { useState } from 'react';

const Payment = () => {
  const { cartItems } = useCart();
  const navigate = useNavigate();
  const [form, setForm] = useState({
    cardNumber: '',
    expiry: '',
    cvv: '',
  });

  const total = cartItems.reduce(
    (sum, item) => sum + item.price * item.quantity,
    0
  );

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    console.log('Оплата:', form);
    console.log('Товары:', cartItems);
    alert('Оплата прошла успешно!');
    navigate('/');
  };

  return (
    <div className="max-w-md mx-auto py-12">
      <h2 className="text-3xl font-bold text-center mb-6">Оплата</h2>
      <p className="text-center mb-4 text-lg">Сумма: {total} ₽</p>
      <form onSubmit={handleSubmit} className="space-y-4">
        <input
          name="cardNumber"
          placeholder="Номер карты"
          value={form.cardNumber}
          onChange={handleChange}
          required
          className="w-full border px-4 py-2 rounded"
        />
        <div className="flex gap-4">
          <input
            name="expiry"
            placeholder="MM/YY"
            value={form.expiry}
            onChange={handleChange}
            required
            className="w-1/2 border px-4 py-2 rounded"
          />
          <input
            name="cvv"
            placeholder="CVV"
            value={form.cvv}
            onChange={handleChange}
            required
            className="w-1/2 border px-4 py-2 rounded"
          />
        </div>
        <button
          type="submit"
          className="w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700"
        >
          Оплатить
        </button>
      </form>
    </div>
  );
};

export default Payment;
