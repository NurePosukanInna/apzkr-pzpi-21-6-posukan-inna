import axios from 'axios';
import { BASE_URL } from '../utils/apiConfig';

export async function fetchAllSupplierRequests(userId) {
    try {
      const response = await axios.get(`${BASE_URL}/SupplierRequest/${userId}`);
      return response.data;
    } catch (error) {
      console.error('Error fetching supplier requests:', error);
      throw error; 
    }
}

export async function fetchAllSupplierRequestsForEmployee(employeeId) {
    try {
      const response = await axios.get(`${BASE_URL}/SupplierRequest/employee/${employeeId}`);
      return response.data;
    } catch (error) {
      console.error('Error fetching supplier requests:', error);
      throw error; 
    }
}
export async function deleteSupplierRequest(requestId) {
    try {
        const response = await axios.delete(`${BASE_URL}/SupplierRequest/${requestId}`);
        return response.data;
    } catch (error) {
        console.error('Error deleting supplier request:', error);
        throw error; 
    }
}
export async function updateSupplierRequest(requestId, updateData) {
    try {
        const response = await axios.put(`${BASE_URL}/SupplierRequest/${requestId}`, updateData);
        return response.data;
    } catch (error) {
        console.error('Error updating supplier request:', error);
        throw error; 
    }
}
