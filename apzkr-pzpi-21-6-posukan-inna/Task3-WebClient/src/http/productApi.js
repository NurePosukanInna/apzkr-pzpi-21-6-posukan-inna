import axios from 'axios';
import { BASE_URL } from '../utils/apiConfig';

export async function addProduct(productData) {
  try {
    const response = await axios.post(`${BASE_URL}/Product`, productData);
    return response.data;
  } catch (error) {
    console.error('Error adding product:', error);
    return null;
  }
}

export const fetchCurrencies = async () => {
  try {
    const response = await axios.get('https://open.er-api.com/v6/latest/USD');
    if (response.data && response.data.rates) {
      const availableCurrencies = Object.keys(response.data.rates);
      return availableCurrencies;
    } else {
      console.error('Error fetching currencies: Invalid API response');
      return [];
    }
  } catch (error) {
    console.error('Error fetching currencies:', error.message);
    return [];
  }
};

export async function deleteProduct(productId) {
  try {
    const response = await axios.delete(`${BASE_URL}/Product/${productId}`);
    return response.data;
  } catch (error) {
    console.error('Error deleting product:', error);
    return null;
  }
}

export async function updateProduct(productId, productData) {
  try {
    const response = await axios.put(`${BASE_URL}/Product/${productId}`, productData);
    return response.data;
  } catch (error) {
    console.error('Error updating product:', error);
    return null;
  }
}

export async function fetchAllProducts(shopId) {
  try {
    const response = await axios.get(`${BASE_URL}/Product/${shopId}`);
    const products = response.data;
    return products;
  } catch (error) {
    console.error('Error fetching products:', error);
    return null;
  }
}
export async function fetchDefectiveProductsForStore(shopId) {
  try {
    const response = await axios.get(`${BASE_URL}/DefectiveProducts/${shopId}`);
    const defectiveProducts = response.data;
    return defectiveProducts;
  } catch (error) {
    console.error('Error fetching defective products:', error);
    return null;
  }
}