import axios from 'axios';
import { BASE_URL } from '../utils/apiConfig';

export async function fetchAllEmployees(userId) {
  try {
    const response = await axios.get(`${BASE_URL}/Employee/${userId}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching employee data:', error);
    throw error; 
  }
}


export async function addEmployee(employeeData) {
  try {
    const response = await axios.post(`${BASE_URL}/Employee`, employeeData);
    return response.data;
  } catch (error) {
    console.error('Error adding employee:', error);
    throw error; 
  }
}

export async function deleteEmployee(id) {
    try {
      const response = await axios.delete(`${BASE_URL}/Employee/${id}`);
      return response.data;
    } catch (error) {
      console.error('Error deleting employee:', error);
      throw error; 
    }
  }
  
  export async function updateEmployee(id, updatedEmployeeData) {
    try {
      const response = await axios.put(`${BASE_URL}/Employee/${id}`, updatedEmployeeData);
      return response.data;
    } catch (error) {
      console.error('Error updating employee:', error);
      throw error; 
    }
  }
  
  export async function fetchStoresByEmployeeId(employeeId) {
    try {
      const response = await axios.get(`${BASE_URL}/Employee/${employeeId}/stores`);
      return response.data;
    } catch (error) {
      console.error('Error fetching stores for employee:', error);
      throw error; 
    }
  }