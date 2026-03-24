import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import { User, Cart, AuthTokens, CartItem } from '../types';
import { userAPI, cartAPI } from '../api';

interface AuthContextType {
  user: User | null;
  cart: Cart | null;
  isAuthenticated: boolean;
  isLoadingUserData: boolean;
  login: (tokens: AuthTokens, userId: string) => void;
  logout: () => void;
  updateCart: (cart: Cart) => void;
  clearCart: () => void;
  addToCart: (item: CartItem) => Promise<void>;
  updateCartItem: (productId: string, quantity: number) => Promise<void>;
  removeFromCart: (productId: string) => Promise<void>;
  refreshUserData: () => Promise<void>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};

interface AuthProviderProps {
  children: ReactNode;
}

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);
  const [cart, setCart] = useState<Cart | null>(null);
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [isLoadingUserData, setIsLoadingUserData] = useState(false);

  useEffect(() => {
    const token = localStorage.getItem('accessToken');
    const userId = localStorage.getItem('userId');
    console.log('AuthContext useEffect, token exists:', !!token, 'userId:', userId);
    if (token && userId) {
      setIsAuthenticated(true);
      refreshUserData();
    }
  }, []);

  const login = (tokens: AuthTokens, userId: string) => {
    localStorage.setItem('accessToken', tokens.accessToken);
    localStorage.setItem('refreshToken', tokens.refreshToken);
    localStorage.setItem('expiresAt', tokens.expiresAt);
    localStorage.setItem('userId', userId);
    setIsAuthenticated(true);
    refreshUserData();
  };

  const logout = () => {
    localStorage.clear();
    setUser(null);
    setCart(null);
    setIsAuthenticated(false);
  };

  const updateCart = (newCart: Cart) => {
    setCart(newCart);
  };

  const clearCart = () => {
    setCart(null);
  };

  const addToCart = async (item: CartItem) => {
    if (!isAuthenticated) {
      window.location.href = '/login';
      return;
    }
    console.log('Adding to cart:', item);
    await cartAPI.addItem(item);
    await refreshUserData();
  };

  const updateCartItem = async (productId: string, quantity: number) => {
    await cartAPI.updateItem(productId, quantity);
    await refreshUserData();
  };

  const removeFromCart = async (productId: string) => {
    await cartAPI.deleteItem(productId);
    await refreshUserData();
  };

  const refreshUserData = async () => {
    if (!isAuthenticated) {

console.log("sssssssssssssss");
//return;
    }
    console.log('refreshUserData called');
    setIsLoadingUserData(true);
    try {
      console.log('fetching user and cart');
      const [userData, cartData] = await Promise.all([
        userAPI.getProfile(),
        cartAPI.getCart()
      ]);
      console.log('fetched data:', { userData, cartData });
      setUser(userData);
      setCart(cartData);
    } catch (error) {
      console.error('Failed to refresh user data:', error);
      setUser(null);
      setCart(null);
    } finally {
      setIsLoadingUserData(false);
    }
  };

  const value: AuthContextType = {
    user,
    cart,
    isAuthenticated,
    clearCart,
    isLoadingUserData,
    login,
    logout,
    updateCart,
    addToCart,
    updateCartItem,
    removeFromCart,
    refreshUserData,
  };

  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  );
};