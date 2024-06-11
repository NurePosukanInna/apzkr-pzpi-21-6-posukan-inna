import axios from 'axios';
import { BASE_URL } from '../utils/apiConfig';

export const loginUser = async (userData) => {
  try {
    const response = await axios.post(`${BASE_URL}/User/login`, userData);
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const registerUser = async (userData) => {
  try {
    const response = await axios.post(`${BASE_URL}/User/register`, userData);
    return response.data; 
  } catch (error) {
    throw error; 
  }
};

