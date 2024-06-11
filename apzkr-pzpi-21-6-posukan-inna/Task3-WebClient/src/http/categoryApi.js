import axios from 'axios';
import { BASE_URL } from '../utils/apiConfig';

export const fetchCategories = async () => {
    try {
      const response = await axios.get(`${BASE_URL}/Category`);
      console.log(response.data); 
      return response.data;
    } catch (error) {
      console.error('Error fetching categories:', error);
      return [];
    }
};
