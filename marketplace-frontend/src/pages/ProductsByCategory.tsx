import { useParams } from "react-router-dom";
import { gql, useQuery } from "@apollo/client";
import ProductCard from "../components/ProductCard";
import { PRODUCTS_BY_CATEGORY } from "../api/graphql/queries/productsQueries";

const ProductsByCategory = () => {
  const { categoryId } = useParams<{ categoryId: string }>();
  const { data, loading, error } = useQuery(PRODUCTS_BY_CATEGORY, {
    variables: { categoryId },
    skip: !categoryId,
  });

  if (loading) return <p className="text-center py-12">Загрузка товаров...</p>;
  if (error || !data?.products) return <p className="text-center py-12">Ошибка при загрузке</p>;

  const products = data.products.nodes;

  return (
    <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-6 p-6">
      {products.map((p: any) => (
        <ProductCard
          key={p.id}
          id={p.id}
          name={p.name}
          price={p.price}
          description={p.description}
          count={p.count}
          imagePaths={p.imagePaths}
          categoryName={p.category?.name}
          sellerName={p.creator?.companyName}
        />
      ))}
    </div>
  );
};

export default ProductsByCategory;
