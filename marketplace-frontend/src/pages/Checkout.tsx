import { useEffect, useRef } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { useCart } from '../context/CartContext';
import { useMutation } from '@apollo/client';
import { jwtDecode } from 'jwt-decode';
import { CREATE_ORDER } from '../api/graphql/mutations/ordersMutations';

const Checkout = () => {
  const { state } = useLocation();
  const navigate = useNavigate();
  const { cartItems, getTotalPrice, clearCart } = useCart();
  const [createOrder] = useMutation(CREATE_ORDER);

  const calledRef = useRef(false); // 👈 флаг для защиты

  useEffect(() => {
    if (calledRef.current) return;
    calledRef.current = true;

    const token = localStorage.getItem('token');
    if (!token) {
      navigate('/login');
      return;
    }

    const decoded: any = jwtDecode(token);
    const userId = decoded.sub;

    const items = cartItems.map(item => ({
      productId: item.productId,
      productName: item.productName,
      unitPrice: item.price,
      quantity: item.quantity,
    }));

    if (!items.length) {
      alert("Корзина пуста, заказ не будет создан.");
      navigate('/cart');
      return;
    }

    console.log('CreateOrder payload:', {
      input: {
        userId,
        items,
      }
    });

    createOrder({
      variables: {
        input: {
          userId,
          items,
        },
      },
    })
      .then(({ data }) => {
        clearCart();
        navigate('/payment', {
          state: {
            orderId: data.createOrder.uuid,
            amount: getTotalPrice(),
            userId,
          },
        });
      })
      .catch(error => {
        console.error('❌ Ошибка при создании заказа:', error);
        alert('Не удалось оформить заказ. Попробуйте позже.');
        navigate('/cart');
      });
  }, [cartItems, clearCart, createOrder, getTotalPrice, navigate]);

  return (
    <div className="max-w-2xl mx-auto py-12 text-center">
      <h2 className="text-2xl font-bold">Оформляем заказ...</h2>
      <p className="text-gray-500 mt-4">Пожалуйста, подождите...</p>
    </div>
  );
};

export default Checkout;
