import axios from 'axios';
import { BASE_URL } from '../utils/apiConfig';

export const fetchSuppliers = async () => {
    try {
      const response = await axios.get(`${BASE_URL}/Supplier`);
      console.log(response.data); 
      return response.data;
    } catch (error) {
      console.error('Error fetching categories:', error);
      return [];
    }
};
