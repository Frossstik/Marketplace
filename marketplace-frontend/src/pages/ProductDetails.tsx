import { useParams } from "react-router-dom";
import ImageCarousel from "../components/ImageCarousel";
import { useCart } from "../context/CartContext";

// мок-товар
const mockProduct = {
  id: "1",
  name: "Товар A",
  description: "Описание товара A. Lorem ipsum dolor sit amet.",
  price: 100,
  images: [
    "https://via.placeholder.com/600x400?text=Фото+1",
    "https://via.placeholder.com/600x400?text=Фото+2",
    "https://via.placeholder.com/600x400?text=Фото+3",
  ],
};

const ProductDetails = () => {
  const { id } = useParams(); // пригодится позже
  const { addToCart } = useCart();

  return (
    <div className="max-w-4xl mx-auto py-12 px-4">
      <div className="grid md:grid-cols-2 gap-8">
        {/* Карусель */}
        <ImageCarousel images={mockProduct.images} />

        {/* Детали */}
        <div>
          <h2 className="text-3xl font-bold mb-4">{mockProduct.name}</h2>
          <p className="text-gray-700 mb-6">{mockProduct.description}</p>
          <p className="text-2xl font-bold text-blue-600 mb-6">
            {mockProduct.price} ₽
          </p>
          <button
            onClick={() =>
              addToCart({
                productId: mockProduct.id,
                productName: mockProduct.name,
                price: mockProduct.price,
                quantity: 1,
              })
            }
            className="bg-blue-600 text-white px-6 py-2 rounded hover:bg-blue-700"
          >
            Добавить в корзину
          </button>
        </div>
      </div>
    </div>
  );
};

export default ProductDetails;
