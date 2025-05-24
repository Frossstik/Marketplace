import { useQuery } from '@apollo/client';
import { GET_CATEGORIES } from '../api/graphql/queries/categories';
import { GET_NEWEST_PRODUCTS } from '../api/graphql/queries/products';

const Home = () => {
  const { data: categoriesData } = useQuery(GET_CATEGORIES);
  const { data: productsData } = useQuery(GET_NEWEST_PRODUCTS);

  const categories = categoriesData?.categories || [];
  const products = productsData?.products?.edges?.map((e: { node: any; }) => e.node) || [];

  return (
    <div>
      <h2 className="text-3xl font-bold mb-6 text-center">Добро пожаловать в Marketplace!</h2>

      {/* Категории */}
      <section className="mb-12">
        <h3 className="text-xl font-semibold mb-4">Категории</h3>
        <ul className="flex gap-4 flex-wrap">
          {categories.map((cat: any) => (
            <li key={cat.id} className="px-4 py-2 bg-gray-200 rounded">{cat.name}</li>
          ))}
        </ul>
      </section>

      {/* Новые товары */}
      <section>
        <h3 className="text-xl font-semibold mb-4">Новые товары</h3>
        <div className="grid sm:grid-cols-2 lg:grid-cols-4 gap-4">
          {products.map((product: any) => (
            <div key={product.id} className="border p-4 rounded shadow-sm">
              <h4 className="font-semibold">{product.name}</h4>
              <p className="text-blue-600 font-bold">{product.price} ₽</p>
            </div>
          ))}
        </div>
      </section>
    </div>
  );
};

export default Home;
