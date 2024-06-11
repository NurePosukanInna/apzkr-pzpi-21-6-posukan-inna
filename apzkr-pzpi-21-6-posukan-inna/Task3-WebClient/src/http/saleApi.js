import axios from 'axios';
import { BASE_URL } from '../utils/apiConfig';

export async function addSale(saleData) {
  try {
    const response = await axios.post(`${BASE_URL}/Sale`, saleData);
    return response.data;
  } catch (error) {
    console.error('Error adding sale:', error);
    throw error;
  }
}
export async function getSalesByStoreId(storeId) {
  try {
    const response = await axios.get(`${BASE_URL}/Sale/${storeId}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching sales by store id:', error);
    throw error;
  }
}