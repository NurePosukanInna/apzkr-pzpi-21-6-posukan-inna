import axios from 'axios';
import { BASE_URL } from '../utils/apiConfig';

export const getSubscriptionTypes = async () => {
    try {
      const response = await axios.get(`${BASE_URL}/Subscription`);
      return response.data;
    } catch (error) {
      console.error('Error fetching subscription types:', error);
      throw error;
    }
  };
  export const addSubscriptionToUser = async (userId, subscriptionData) => {
    try {
      const response = await axios.post(`${BASE_URL}/Subscription/${userId}`, subscriptionData);
      return response.data;
    } catch (error) {
      console.error('Error adding subscription to user:', error);
      throw error;
    }
  };
  export const getActiveSubscriptionsForUser = async (userId) => {
    try {
      const response = await axios.get(`${BASE_URL}/Subscription/${userId}`);
      return response.data;
    } catch (error) {
      console.error('Error fetching active subscriptions for user:', error);
      throw error;
    }
};
