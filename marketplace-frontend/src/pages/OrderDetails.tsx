import { gql, useQuery } from '@apollo/client';
import { useEffect, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { jwtDecode } from 'jwt-decode';
import { GET_ORDER_BY_ID } from '../api/graphql/queries/ordersQueries';

type DecodedToken = {
  sub: string;
};

const OrderDetails = () => {
  const { state } = useLocation();
  const navigate = useNavigate();
  const [userId, setUserId] = useState<string | null>(null);
  const orderId = state?.orderId;

  useEffect(() => {
    const token = localStorage.getItem('token');
    if (token) {
      const decoded = jwtDecode<DecodedToken>(token);
      setUserId(decoded.sub);
    } else {
      navigate('/login');
    }
  }, [navigate]);

  const { data, loading, error } = useQuery(GET_ORDER_BY_ID, {
    variables: {
      orderId,
      userId,
    },
    skip: !orderId || !userId,
    fetchPolicy: 'network-only',
  });

  if (loading) return <p className="text-center py-8">Загрузка заказа...</p>;
  if (error) return <p className="text-center py-8 text-red-500">Ошибка загрузки заказа</p>;

  const order = data?.orderById;

  if (!order) {
    return <p className="text-center py-8 text-gray-500">Заказ не найден.</p>;
  }

  return (
    <div className="max-w-3xl mx-auto py-12">
      <h2 className="text-3xl font-bold mb-6 text-center">Детали заказа</h2>

      <div className="border p-6 rounded shadow">
        <div className="flex justify-between mb-3">
          <span className="font-semibold text-lg">Заказ № {order.id}</span>
          <span className="text-sm text-gray-500">
            {new Date(order.createdAt).toLocaleString()}
          </span>
        </div>

        <div className="mb-3">
          <span className="text-sm">Статус: </span>
          <span className="font-semibold">{order.status}</span>
        </div>

        <ul className="text-sm space-y-1 mb-4">
          {order.items.map((item: any, i: number) => (
            <li key={i}>
              {item.productName} — {item.quantity} × {item.unitPrice} ₽
            </li>
          ))}
        </ul>

        <div className="text-right font-bold text-lg">
          Итого: {order.totalPrice} ₽
        </div>
      </div>
    </div>
  );
};

export default OrderDetails;
