import { useQuery } from '@apollo/client';
import { useState } from 'react';
import { GET_PRODUCTS } from '../api/graphql/queries/productsQueries';
import ProductCard from '../components/ProductCard';

const Products = () => {
  const pageSize = 25;
  const [allProducts, setAllProducts] = useState<any[]>([]);
  const [endCursor, setEndCursor] = useState<string | null>(null);
  const [hasNextPage, setHasNextPage] = useState<boolean>(true);
  const [search, setSearch] = useState('');

  const { loading, error, fetchMore } = useQuery(GET_PRODUCTS, {
    variables: { first: pageSize, after: null },
    fetchPolicy: 'cache-and-network',
    notifyOnNetworkStatusChange: true,
    onCompleted: (data) => {
      const newEdges = data?.products?.edges ?? [];
      setAllProducts(newEdges.map((edge: any) => edge.node));
      setEndCursor(data?.products?.pageInfo?.endCursor ?? null);
      setHasNextPage(data?.products?.pageInfo?.hasNextPage ?? false);
    },
  });

  const loadMore = () => {
    if (!hasNextPage || !endCursor || loading) return;

    fetchMore({
      variables: {
        first: pageSize,
        after: endCursor,
      },
    }).then(({ data }) => {
      const newEdges = data?.products?.edges ?? [];
      setAllProducts((prev) => [...prev, ...newEdges.map((edge: any) => edge.node)]);
      setEndCursor(data?.products?.pageInfo?.endCursor ?? null);
      setHasNextPage(data?.products?.pageInfo?.hasNextPage ?? false);
    });
  };

  const filtered = allProducts.filter((p) =>
    p.name.toLowerCase().includes(search.toLowerCase())
  );

  const sortedProducts = filtered.sort(
    (a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime()
  );

  return (
    <div className="max-w-7xl mx-auto py-12 px-4">
      <h2 className="text-3xl font-bold mb-8">Все товары</h2>

      <input
        type="text"
        placeholder="Поиск по товарам..."
        value={search}
        onChange={(e) => setSearch(e.target.value)}
        className="mb-6 w-full border px-4 py-2 rounded"
      />

      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6 mb-8">
        {sortedProducts.map((p: any) => (
          <ProductCard
            key={p.id}
            id={p.id}
            name={p.name}
            price={p.price}
            description={p.description}
            imagePaths={p.imagePaths}
            categoryName={p.category?.name ?? '—'}
            sellerName={p.creator?.companyName ?? '—'}
            count={p.count}
          />
        ))}
      </div>

      {hasNextPage && (
        <div className="text-center">
          <button
            onClick={loadMore}
            disabled={loading}
            className="px-6 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 disabled:bg-blue-300"
          >
            {loading ? 'Загрузка...' : 'Загрузить ещё'}
          </button>
        </div>
      )}
    </div>
  );
};

export default Products;
