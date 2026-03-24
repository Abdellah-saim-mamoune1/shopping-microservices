import React, { useState, useEffect } from 'react';
import { useAuth } from '../contexts/AuthContext';
import { orderAPI } from '../api';
import { Order, OrderItem } from '../types';
import './Orders.css';

const Orders: React.FC = () => {
  const { isAuthenticated } = useAuth();
  const [orders, setOrders] = useState<Order[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    if (isAuthenticated) {
      loadOrders();
    }
  }, [isAuthenticated]);

  const loadOrders = async () => {
    setLoading(true);
    setError('');
    try {
      const data = await orderAPI.getMyOrders();
      setOrders(data);
    } catch (err: any) {
         console.log(err);
      setError(err.response?.data?.title || 'Failed to load orders');
    } finally {
      setLoading(false);
    }
  };

  if (!isAuthenticated) {
    return <div className="orders-container">Please login to view your orders.</div>;
  }

  if (loading) return <div className="orders-container">Loading orders...</div>;
  if (error) return <div className="orders-container error">{error}</div>;

  return (
    <div className="orders-container">
      <h2>Your Orders</h2>
      {orders.length === 0 ? (
        <p>You haven't placed any orders yet.</p>
      ) : (
        <div className="orders-list">
          {orders.map((order) => (
            <div key={order.id} className="order-card">
              <div className="order-header">
                <h3>Order #{order.id}</h3>
                <span className="order-status">{order.status}</span>
              </div>
              <p>Date: {new Date(order.createdAt).toLocaleDateString()}</p>
              <p>Total: ${order.totalPrice.toFixed(2)}</p>
              <div className="order-items">
                {order.items.map((item: OrderItem) => (
                  <div key={item.productId} className="order-item">
                    <img src={item.imageUrl} alt={item.name} className="order-item-image" />
                    <div>
                      <p>{item.name}</p>
                      <p>Qty: {item.quantity} x ${item.totalPrice.toFixed(2)}</p>
                    </div>
                  </div>
                ))}
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default Orders;