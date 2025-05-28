import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useQuery } from '@apollo/client';
import { jwtDecode } from 'jwt-decode';
import ProductCard from '../components/ProductCard';
import { PRODUCTS_BY_CREATOR } from '../api/graphql/queries/productsQueries';

type DecodedToken = {
  email: string;
  exp: number;
  sub: string;
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'?: string | string[];
};

const MyProducts = () => {
  const navigate = useNavigate();
  const [creatorId, setCreatorId] = useState<string | null>(null);
  const [role, setRole] = useState<string | null>(null);
  const [loadingToken, setLoadingToken] = useState(true);

  useEffect(() => {
    const token = localStorage.getItem('token');
    if (!token) {
      navigate('/login');
      return;
    }

    try {
      const decoded = jwtDecode<DecodedToken>(token);
      const roleClaim = decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      const extractedRole = Array.isArray(roleClaim) ? roleClaim[0] : roleClaim;

      setRole(extractedRole ?? null);
      setCreatorId(decoded.sub); // sub = user ID
    } catch (e) {
      console.error('Ошибка при декодировании токена:', e);
      localStorage.removeItem('token');
      navigate('/login');
    } finally {
      setLoadingToken(false);
    }
  }, [navigate]);

  const { data, loading: loadingQuery } = useQuery(PRODUCTS_BY_CREATOR, {
    variables: { creatorId },
    skip: !creatorId,
    fetchPolicy: 'network-only',
  });

  const products = data?.products?.nodes ?? [];

  if (loadingToken || loadingQuery) return <p className="text-center py-12">Загрузка...</p>;

  if (role !== 'Admin' && role !== 'Seller') {
    return <p className="text-center py-12 text-red-600">Нет доступа</p>;
  }

  return (
    <div className="max-w-7xl mx-auto py-12 px-4">
      <h2 className="text-3xl font-bold mb-6 text-center">Мои товары</h2>
      {products.length === 0 ? (
        <p className="text-center">Вы ещё не добавили ни одного товара.</p>
      ) : (
        <div className="grid sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
          {products.map((p: any) => (
            <ProductCard
              key={p.id}
              id={p.id}
              name={p.name}
              price={p.price}
              description={p.description}
              imagePaths={p.imagePaths}
              categoryName={p.category?.name ?? '—'}
              sellerName={p.creator?.companyName ?? '—'}
              showEditDelete
            />
          ))}
        </div>
      )}
    </div>
  );
};

export default MyProducts;
