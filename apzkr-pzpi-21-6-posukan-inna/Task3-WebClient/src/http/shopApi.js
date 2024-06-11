import axios from 'axios';
import { BASE_URL } from '../utils/apiConfig';

export async function fetchAllStores(userId) {
    try {
        const response = await axios.get(`${BASE_URL}/Store/${userId}`);
        const stores = response.data;
        return stores;
    } catch (error) {
        console.error('Error fetching store data:', error);
        return null;
    }
}

export async function createStore(storeData) {
    try {
        const response = await axios.post(`${BASE_URL}/Store`, storeData);
        const createdStore = response.data;
        if (createdStore) {
            await createSensor(createdStore.storeId);
        }
        return createdStore;
    } catch (error) {
        console.error('Error creating store:', error);
        return null;
    }
}

export async function fetchStoreById(storeId) {
    try {
        const response = await axios.get(`${BASE_URL}/Store/${storeId}`);
        const store = response.data;
        return store;
    } catch (error) {
        console.error('Error fetching store by ID:', error);
        return null;
    }
}

async function createSensor(storeId) {
    try {
        await axios.post(`${BASE_URL}/Sensor`, { storeId, temperature: 0, humidity: 0 });
    } catch (error) {
        console.error('Error creating sensor:', error);
    }
}
