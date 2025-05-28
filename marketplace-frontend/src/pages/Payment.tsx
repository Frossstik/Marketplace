import { gql, useMutation, useLazyQuery } from '@apollo/client';
import { useLocation, useNavigate } from 'react-router-dom';
import { useState } from 'react';
import { paymentClient } from '../api/clients/paymentClient';

const PROCESS_PAYMENT = gql`
  mutation ProcessPayment($input: PaymentInput!) {
    processPayment(input: $input) {
      id
      status
      transactionId
      failureReason
    }
  }
`;

const GET_PAYMENT_STATUS = gql`
  query PaymentStatus($paymentId: UUID!) {
    paymentStatus(paymentId: $paymentId)
  }
`;

const Payment = () => {
  const { state } = useLocation();
  const navigate = useNavigate();

  const { orderId, amount } = state || {};
  const [method, setMethod] = useState<'CARD' | 'YOO_MONEY'>('CARD');
  const [details, setDetails] = useState({
    card_number: '',
    expiry_date: '',
    cvv: '',
    yoomoney_wallet: '',
  });

  const [paymentId, setPaymentId] = useState<string | null>(null);
  const [statusMessage, setStatusMessage] = useState<string | null>(null);
  const [error, setError] = useState<string | null>(null);

  const [processPayment, { loading }] = useMutation(PROCESS_PAYMENT, {
    client: paymentClient,
    onCompleted: (data) => {
      const { id, status, transactionId, failureReason } = data.processPayment;
      setPaymentId(id);

      if (status === 'COMPLETED') {
        setStatusMessage(`✅ Оплата прошла. Транзакция: ${transactionId}`);
        setTimeout(() => {
          navigate('/order', {
            state: { orderId }
          });
        }, 2000);
      } else if (status === 'FAILED') {
        setError(`❌ Ошибка: ${failureReason}`);
      } else {
        setStatusMessage('⏳ Ожидается подтверждение...');
      }
    },
    onError: (err) => {
      setError(`❌ Ошибка GraphQL: ${err.message}`);
    },
  });

  const [fetchStatus] = useLazyQuery(GET_PAYMENT_STATUS, {
    client: paymentClient,
    onCompleted: (data) => {
      setStatusMessage(`ℹ️ Статус: ${data.paymentStatus}`);
    },
    onError: (err) => {
      setError(`❌ Ошибка статуса: ${err.message}`);
    },
  });

  const handleSubmit = () => {
    if (!orderId || !amount) {
      setError('Некорректные данные заказа');
      return;
    }

    const base = {
      orderId,
      amount,
      currency: 'RUB',
      method,
    };

    const input =
      method === 'CARD'
        ? {
            ...base,
            card_details: {
              card_number: details.card_number,
              expiry_date: details.expiry_date,
              cvv: details.cvv,
            },
          }
        : {
            ...base,
            yoomoney_wallet: details.yoomoney_wallet,
          };

    processPayment({ variables: { input } });
  };

  return (
    <div className="max-w-lg mx-auto py-12">
      <h2 className="text-3xl font-bold text-center mb-6">Оплата</h2>

      <select
        value={method}
        onChange={(e) => setMethod(e.target.value as 'CARD' | 'YOO_MONEY')}
        className="w-full mb-4 border p-2 rounded"
      >
        <option value="CARD">Карта</option>
        <option value="YOO_MONEY">ЮMoney</option>
      </select>

      {method === 'CARD' && (
        <div className="space-y-3">
          <input placeholder="Номер карты" className="w-full border p-2 rounded"
            value={details.card_number}
            onChange={(e) => setDetails({ ...details, card_number: e.target.value })}
          />
          <input placeholder="MM/YY" className="w-full border p-2 rounded"
            value={details.expiry_date}
            onChange={(e) => setDetails({ ...details, expiry_date: e.target.value })}
          />
          <input placeholder="CVV" className="w-full border p-2 rounded"
            value={details.cvv}
            onChange={(e) => setDetails({ ...details, cvv: e.target.value })}
          />
        </div>
      )}

      {method === 'YOO_MONEY' && (
        <input placeholder="ЮMoney кошелек" className="w-full border p-2 rounded mt-2"
          value={details.yoomoney_wallet}
          onChange={(e) => setDetails({ ...details, yoomoney_wallet: e.target.value })}
        />
      )}

      <button
        onClick={handleSubmit}
        disabled={loading}
        className="mt-6 w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700"
      >
        {loading ? 'Обработка оплаты...' : `Оплатить ${amount} ₽`}
      </button>

      {paymentId && (
        <button
          onClick={() => fetchStatus({ variables: { paymentId } })}
          className="mt-4 w-full bg-gray-200 text-black py-2 rounded hover:bg-gray-300"
        >
          Проверить статус
        </button>
      )}

      {statusMessage && <p className="mt-4 text-green-700">{statusMessage}</p>}
      {error && <p className="mt-4 text-red-600">{error}</p>}
    </div>
  );
};

export default Payment;
