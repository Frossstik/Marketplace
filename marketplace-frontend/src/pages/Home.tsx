import { useQuery, gql } from '@apollo/client';
import { Link } from 'react-router-dom';
import { GET_NEWEST_PRODUCTS } from '../api/graphql/queries/productsQueries';
import ProductCard from '../components/ProductCard';

const GET_CATEGORIES = gql`
  query {
    categories {
      id
      name
      createdAt
    }
  }
`;

const Home = () => {
  const { data: categoriesData } = useQuery(GET_CATEGORIES);
  const { data: productsData } = useQuery(GET_NEWEST_PRODUCTS);

  const categories =
    categoriesData?.categories
      ?.slice()
      .sort((a: any, b: any) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime())
      .slice(0, 3) || [];

  const products =
    productsData?.products?.edges?.map((e: { node: any }) => e.node) || [];

  return (
    <div className="max-w-7xl mx-auto py-12 px-4">
      <h2 className="text-3xl font-bold mb-6 text-center">Добро пожаловать в Marketplace!</h2>

      {/* Категории */}
      <section className="mb-12">
        <h3 className="text-xl font-semibold mb-4">Последние категории</h3>
        <ul className="flex gap-4 flex-wrap">
          {categories.map((cat: any) => (
            <li key={cat.id}>
              <Link
                to={`/category/${cat.id}`}
                className="px-4 py-2 bg-gray-200 rounded hover:bg-gray-300 transition"
              >
                {cat.name}
              </Link>
            </li>
          ))}
        </ul>
      </section>

      {/* Продукты */}
      <section>
        <h3 className="text-xl font-semibold mb-4">Последние товары</h3>
        <div className="grid sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
          {[...products].reverse().map((p: any) => (
            <ProductCard
              key={p.id}
              id={p.id}
              name={p.name}
              price={p.price}
              description={p.description}
              count={p.count}
              imagePaths={p.imagePaths}
              categoryName={p.category?.name ?? '—'}
              sellerName={p.creator?.companyName ?? '—'}
            />
          ))}
        </div>

      </section>
    </div>
  );
};

export default Home;
