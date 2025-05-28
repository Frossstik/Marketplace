import { gql, useQuery } from '@apollo/client';
import { useEffect } from 'react';
import { jwtDecode } from 'jwt-decode';
import { useNavigate } from 'react-router-dom';
import { GET_ORDERS } from '../api/graphql/queries/ordersQueries';

const Orders = () => {
  const navigate = useNavigate();

  const token = localStorage.getItem('token');
  const decoded = token ? jwtDecode<{ sub: string }>(token) : null;
  const userId = decoded?.sub;

  const { data, loading, error } = useQuery(GET_ORDERS, {
    variables: { userId },
    skip: !userId,
    fetchPolicy: 'network-only',
  });

  useEffect(() => {
    if (!userId) navigate('/login');
  }, [userId, navigate]);

  if (loading) return <p className="text-center py-12">Загрузка заказов...</p>;
  if (error) return <p className="text-center text-red-500">Ошибка: {error.message}</p>;

  // Создаём копию массива и сортируем по дате (новые — первыми)
  const orders = [...(data?.userOrders ?? [])].sort(
    (a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime()
  );

  return (
    <div className="max-w-4xl mx-auto py-12">
      <h2 className="text-3xl font-bold mb-6 text-center">История заказов</h2>

      {orders.length === 0 ? (
        <p className="text-center text-gray-500">У вас пока нет заказов.</p>
      ) : (
        orders.map((order: any) => (
          <div key={order.id} className="border rounded p-4 mb-6 shadow-sm">
            <div className="flex justify-between mb-2">
              <span className="font-medium">ID заказа: {order.id}</span>
              <span className="text-sm text-gray-600">
                {new Date(order.createdAt).toLocaleString()}
              </span>
            </div>
            <p className="mb-2 text-gray-700">
              Статус: <strong>{order.status}</strong>
            </p>
            <p className="mb-2">Сумма: <strong>{order.totalPrice} ₽</strong></p>

            <div className="mt-3 space-y-1">
              {order.items.map((item: any, idx: number) => (
                <div key={idx} className="text-sm text-gray-800">
                  - {item.productName} × {item.quantity} шт. по {item.unitPrice} ₽
                </div>
              ))}
            </div>
          </div>
        ))
      )}
    </div>
  );
};

export default Orders;
