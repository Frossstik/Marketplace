import { useState } from 'react';

const SellerCreateProduct = () => {
  const [form, setForm] = useState({
    name: '',
    description: '',
    price: '',
    count: '',
  });

  const [images, setImages] = useState<File[]>([]);
  const [previewUrls, setPreviewUrls] = useState<string[]>([]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setForm(prev => ({ ...prev, [name]: value }));
  };

  const handleImageChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const files = e.target.files;
    if (!files) return;

    const newFiles = Array.from(files);
    setImages(prev => [...prev, ...newFiles]);

    const urls = newFiles.map(file => URL.createObjectURL(file));
    setPreviewUrls(prev => [...prev, ...urls]);
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    const productData = {
      ...form,
      price: Number(form.price),
      count: Number(form.count),
      images,
    };

    console.log('Создание товара:', productData);
    alert('Товар создан (мок). Форма отправлена в консоль.');
  };

  return (
    <div className="max-w-3xl mx-auto py-12">
      <h2 className="text-3xl font-bold text-center mb-8">Создание товара</h2>

      <form onSubmit={handleSubmit} className="space-y-6">
        <input
          name="name"
          placeholder="Название товара"
          value={form.name}
          onChange={handleChange}
          required
          className="w-full border px-4 py-2 rounded"
        />

        <textarea
          name="description"
          placeholder="Описание"
          value={form.description}
          onChange={handleChange}
          required
          className="w-full border px-4 py-2 rounded h-32 resize-none"
        />

        <div className="flex gap-4">
          <input
            name="price"
            type="number"
            placeholder="Цена, ₽"
            value={form.price}
            onChange={handleChange}
            required
            className="w-1/2 border px-4 py-2 rounded"
          />
          <input
            name="count"
            type="number"
            placeholder="Количество"
            value={form.count}
            onChange={handleChange}
            required
            className="w-1/2 border px-4 py-2 rounded"
          />
        </div>

        <div>
          <label className="block mb-2 font-medium">Фотографии</label>
          <input
            type="file"
            accept="image/*"
            multiple
            onChange={handleImageChange}
            className="block"
          />
          {previewUrls.length > 0 && (
            <div className="mt-4 grid grid-cols-2 md:grid-cols-3 gap-4">
              {previewUrls.map((url, i) => (
                <img
                  key={i}
                  src={url}
                  alt={`preview-${i}`}
                  className="w-full h-32 object-cover rounded border"
                />
              ))}
            </div>
          )}
        </div>

        <button
          type="submit"
          className="bg-blue-600 text-white px-6 py-2 rounded hover:bg-blue-700"
        >
          Создать товар
        </button>
      </form>
    </div>
  );
};

export default SellerCreateProduct;
