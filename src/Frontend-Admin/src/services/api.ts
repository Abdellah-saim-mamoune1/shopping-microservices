import axios, { AxiosInstance, AxiosResponse } from 'axios';
import { ApiResponse } from '../types';

const BASE_URL = 'http://localhost:7000';

class ApiService {
  private api: AxiosInstance;

  constructor() {
    this.api = axios.create({
      baseURL: BASE_URL,
    
    });

    // Request interceptor to add auth token
    this.api.interceptors.request.use(
      (config) => {
        const token = localStorage.getItem('accessToken');
        if (token) {
          config.headers.Authorization = `Bearer ${token}`;
        }

         config.headers.clientIP = "123.234.232.12";
        return config;
      },
      (error) => Promise.reject(error)
    );

    // Response interceptor to handle token refresh
    this.api.interceptors.response.use(
      (response) => response,
      async (error) => {
        const originalRequest = error.config;

        if (error.response?.status === 401 && !originalRequest._retry) {
          originalRequest._retry = true;

          try {
            const refreshToken = localStorage.getItem('refreshToken');
            const userId = localStorage.getItem('userId');

            if (refreshToken && userId) {
              const response = await axios.post(`${BASE_URL}/auth/refresh`, {
                userId: parseInt(userId),
                refreshToken,
              });

              const { accessToken, refreshToken: newRefreshToken } = response.data.data;

              localStorage.setItem('accessToken', accessToken);
              localStorage.setItem('refreshToken', newRefreshToken);

              originalRequest.headers.Authorization = `Bearer ${accessToken}`;
              return this.api(originalRequest);
            }
          } catch (refreshError) {
            // Refresh failed, logout user
            localStorage.removeItem('accessToken');
            localStorage.removeItem('refreshToken');
            localStorage.removeItem('userId');
            localStorage.removeItem('user');
            window.location.href = '/login';
          }
        }

        return Promise.reject(error);
      }
    );
  }

  // Auth endpoints
  async login(credentials: { account: string; password: string }): Promise<AxiosResponse<ApiResponse<any>>> {
    return this.api.post('/auth/login', credentials);
  }

  async registerEmployee(data: any): Promise<AxiosResponse<ApiResponse<any>>> {
    return this.api.post('/auth/register-employee', data);
  }

  // Catalog endpoints
  async getBooks(pageSize: number, pageNumber: number): Promise<AxiosResponse<ApiResponse<any>>> {
     const b=this.api.get(`/catalog/${pageNumber},${pageSize}`);
     console.log('API Response:', b);
     return b;
  }

  async searchBooks(keyword: string): Promise<AxiosResponse<ApiResponse<any>>> {
    return this.api.get(`/catalog/search?keyword=${encodeURIComponent(keyword)}`);
  }

  async createBook(book: any): Promise<AxiosResponse<ApiResponse<any>>> {
    return this.api.post('/catalog/admin', book);
  }

  async updateBook(bookId: string, book: any): Promise<AxiosResponse<ApiResponse<any>>> {
    return this.api.put(`/catalog/admin/${bookId}`, book);
  }

  async deleteBook(bookId: string): Promise<AxiosResponse<ApiResponse<any>>> {
    return this.api.delete(`/catalog/admin/${bookId}`);
  }

  // Orders endpoints
  async getOrders(pageNumber: number, pageSize: number): Promise<AxiosResponse<ApiResponse<any>>> {
    return this.api.get(`/orders/all/${pageNumber},${pageSize}`);
  }

  async getOrderStats(): Promise<AxiosResponse<ApiResponse<any>>> {
    return this.api.get('/orders/stats');
  }

  async updateOrderStatus(orderId: string, status: string): Promise<AxiosResponse<ApiResponse<any>>> {
    return this.api.put(`/orders/${orderId},${status}`);
  }

  // Discount endpoints
  async getDiscounts(): Promise<AxiosResponse<ApiResponse<any>>> {
    return this.api.get('/discount');
  }

  async createDiscount(discount: any): Promise<AxiosResponse<ApiResponse<any>>> {
    return this.api.post('/discount', discount);
  }

  async updateDiscount(id: string, discount: any): Promise<AxiosResponse<ApiResponse<any>>> {
    return this.api.put(`/discount/${id}`, discount);
  }

  async deleteDiscount(id: string): Promise<AxiosResponse<ApiResponse<any>>> {
    return this.api.delete(`/discount/${id}`);
  }

  // User endpoints
  async getProfile(): Promise<AxiosResponse<ApiResponse<any>>> {
    return this.api.get('/employee');
  }

  async updateProfile(id: number, data: any): Promise<AxiosResponse<ApiResponse<any>>> {
    return this.api.put(`/employee/${id}`, data);
  }
}

export const apiService = new ApiService();