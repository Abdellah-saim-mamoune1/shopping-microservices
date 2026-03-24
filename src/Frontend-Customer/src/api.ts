import axios from 'axios';
import { LoginRequest, RegisterRequest, User, Cart, Order, AuthTokens, CartItem, BasketCheckoutDto } from './types';

const getClientIP = async () => {
  const res = await fetch("https://api.ipify.org?format=json");
  const data = await res.json();
  return data.ip;
};

const API_BASE_URL = 'http://localhost:7000';

const api = axios.create({
   baseURL: API_BASE_URL,
});

// Add token and clientIP to requests
api.interceptors.request.use(async (config) => {
  const token = localStorage.getItem('accessToken');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  
  const clientIP = await getClientIP();
  config.headers.clientIP = clientIP;
  
  return config;
});

// Handle token refresh on 401
api.interceptors.response.use(
  (response) => response,
  async (error) => {
    if (error.response?.status === 401) {
      const refreshToken = localStorage.getItem('refreshToken');
      const userId = localStorage.getItem('userId');
      if (refreshToken && userId) {
        try {
          const refreshResponse = await api.post(`auth/refresh?userId=${userId}&refreshToken=${refreshToken}`);
          const newTokens: AuthTokens = refreshResponse.data;
          localStorage.setItem('accessToken', newTokens.accessToken);
          localStorage.setItem('refreshToken', newTokens.refreshToken);
          localStorage.setItem('expiresAt', newTokens.expiresAt);
          // Retry original request
          error.config.headers.Authorization = `Bearer ${newTokens.accessToken}`;
          return axios(error.config);
        } catch (refreshError) {
          // Refresh failed, logout
          localStorage.clear();
          window.location.href = '/login';
        }
      } else {
        localStorage.clear();
        window.location.href = '/login';
      }
    }
    return Promise.reject(error);
  }
);

export const authAPI = {
  login: async (data: LoginRequest): Promise<AuthTokens> => {
    const response = await api.post(`auth/login`, data);
    console.log('login response:', response.data);
    return response.data.data;
  },

  register: async (data: RegisterRequest): Promise<void> => {
    await api.post(`auth/register-client`, data);
  },
};

export const userAPI = {
  getProfile: async (): Promise<User> => {
    const response = await api.get('/client');
      console.log('getProfile response:', response.data);
    return response.data.data;
  },

  updateProfile: async (data: Partial<User>): Promise<User> => {
    const response = await api.put('/client', data);
    return response.data.data;
  },
};

export const cartAPI = {
  getCart: async (): Promise<Cart> => {
    const response = await api.get('/basket');
    console.log('getCart response:', response.data);
    return response.data.data;
  },

  addItem: async (item: CartItem): Promise<void> => {
    
    const res= await api.post('/basket/item/',item);
    console.log(res);
  },

  updateItem: async (productId: string, quantity: number): Promise<void> => {
    await api.put('/basket/item', { productId, quantity });
  },

  deleteItem: async (productId: string): Promise<void> => {
    const d=await api.delete(`/basket/item/${productId}`);
    console.log("ddddd"+d);
  },

  checkout: async (checkoutData: BasketCheckoutDto): Promise<void> => {
  
  try{
    await api.post('/basket/checkout', checkoutData);
  }catch(err: any){
  if (err.response) {
    // 🔥 Server responded with error (4xx, 5xx)
    console.log('Status:', err.response.status);
    console.log('Data:', err.response.data);
    console.log('Headers:', err.response.headers);
  } else if (err.request) {
    // ❌ Request sent but no response
    console.log('No response received:', err.request);
  } else {
    // ❌ Something else (setup error)
    console.log('Error:', err.message);
  }
  }
  },
};

export const orderAPI = {
  getMyOrders: async (): Promise<Order[]> => {
   
    const response = await api.get('/orders');
     
    return response.data.data;
    }
  
};

export const fetchProducts = async (
  pageNumber: number = 1,
  pageSize: number = 20
) => {
  const response = await api.get(`/catalog/${pageNumber},${pageSize}`
  );
  if (!response.data.success) {
    throw new Error(response.data.title || 'API error');
  }
  return response.data.data;
};