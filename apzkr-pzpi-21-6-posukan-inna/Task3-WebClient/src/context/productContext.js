import React, { createContext, useContext, useState } from 'react';

const ProductContext = createContext();

export const ProductProvider = ({ children }) => {
  const [selectedProducts, setSelectedProducts] = useState([]);
  const addSelectedProduct = (product) => {
    const existingProductIndex = selectedProducts.findIndex(
      (existingProduct) => existingProduct.productId === product.productId
    );
  
    if (existingProductIndex !== -1) {
      console.log('Product already exists, updating quantity...');
      const updatedProducts = [...selectedProducts];
      updatedProducts[existingProductIndex] = {
        ...updatedProducts[existingProductIndex],
        quantity: updatedProducts[existingProductIndex].quantity + 1,
      };
      console.log('Updated products:', updatedProducts);
      setSelectedProducts(updatedProducts);
    } else {
      console.log('Adding new product...');
      setSelectedProducts((prevProducts) => [...prevProducts, { ...product, quantity: 1 }]);
    }
  };
  
  const removeProductFromContext = (productId) => {
    setSelectedProducts((prevProducts) =>
      prevProducts.filter((product) => product.productId !== productId)
    );
  };
  
  const clearSelectedProducts = () => {
    setTimeout(() => {
      setSelectedProducts([]);
    }, 1000); 
  };
  
  return (
    <ProductContext.Provider
      value={{ selectedProducts, addSelectedProduct,clearSelectedProducts, removeProductFromContext }}>
      {children}
    </ProductContext.Provider>
  );
};

export const useProductContext = () => useContext(ProductContext);
