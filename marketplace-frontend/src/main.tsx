import React from 'react';
import ReactDOM from 'react-dom/client';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { ApolloProvider } from '@apollo/client';
import { marketplaceClient } from './api/clients/webClient';
import './index.css';
import Layout from './components/Layout';
import Home from './pages/Home';
import NotFound from './pages/NotFound';
import Login from './pages/Login';
import Register from './pages/Register';
import Products from './pages/Products';
import Cart from './pages/Cart';
import ProductDetails from './pages/ProductDetails';
import Checkout from './pages/Checkout';
import Payment from './pages/Payment';
import { CartProvider } from './context/CartContext';
import SellerCreateProduct from './pages/SellerCreateProduct';
import Profile from './pages/Profile';
import OrderDetails from './pages/OrderDetails';
import Orders from './pages/Orders';
import { AuthProvider, useAuth } from './context/AuthContext';
import Categories from './pages/Categories';
import ProductsByCategory from './pages/ProductsByCategory';
import MyProducts from './pages/MyProducts';
import SellerEditProduct from './pages/SellerEditProduct';

const AppRoutes = () => {
  const { user } = useAuth();

  return (
    <Routes>
      <Route path="/" element={<Layout />}>
        <Route index element={<Home />} />
        <Route path="*" element={<NotFound />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/products" element={<Products />} />
        <Route path="/categories" element={<Categories />} />
        <Route path="/cart" element={<Cart />} />
        <Route path="/checkout" element={<Checkout />} />
        <Route path="/payment" element={<Payment />} />
        <Route path="/product/:id" element={<ProductDetails />} />
        <Route path="/seller/create-product" element={<SellerCreateProduct />} />
        <Route path="/profile" element={<Profile />} />
        <Route path="/order" element={<OrderDetails />} />
        <Route path="/orders" element={<Orders />} />
        <Route path="/category/:categoryId" element={<ProductsByCategory />} />
        <Route path="/seller/my-products" element={<MyProducts />} />
        <Route path="/seller/edit-product/:id" element={<SellerEditProduct />} />
      </Route>

    </Routes>
  );
};

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <ApolloProvider client={marketplaceClient}>
      <CartProvider>
        <AuthProvider>
          <BrowserRouter>
            <AppRoutes />
          </BrowserRouter>
        </AuthProvider>
      </CartProvider>
    </ApolloProvider>
  </React.StrictMode>
);
