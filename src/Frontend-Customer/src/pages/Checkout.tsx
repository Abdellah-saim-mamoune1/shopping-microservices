import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import { cartAPI } from '../api';
import { BasketCheckoutDto } from '../types';
import './Checkout.css';

const Checkout: React.FC = () => {
  const navigate = useNavigate();
  const { cart, isAuthenticated, isLoadingUserData, clearCart } = useAuth();
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const [billingAddress, setBillingAddress] = useState({
    firstName: '',
    lastName: '',
    emailAddress: '',
    addressLine: '',
    country: '',
    state: '',
    zipCode: '',
  });

  const [paymentInfo, setPaymentInfo] = useState({
    cardName: '',
    cardNumber: '',
    expiration: '',
    cvv: '',
  });

  const [paymentMethod, setPaymentMethod] = useState<number>(1); // Default to credit card

  if (!isAuthenticated) {
    return (
      <div className="checkout-container">
        <div className="error-message">
          Please login to checkout. <a href="/login">Go to login</a>
        </div>
      </div>
    );
  }

  if (isLoadingUserData || !cart) {
    return <div className="checkout-container">Loading checkout...</div>;
  }

  if (cart.items.length === 0) {
    return (
      <div className="checkout-container">
        <div className="empty-cart-message">
          Your cart is empty. <a href="/">Continue shopping</a>
        </div>
      </div>
    );
  }

  const handleAddressChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setBillingAddress((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const validateForm = (): boolean => {
    if (
      !billingAddress.firstName ||
      !billingAddress.lastName ||
      !billingAddress.emailAddress ||
      !billingAddress.addressLine ||
      !billingAddress.country ||
      !billingAddress.state ||
      !billingAddress.zipCode
    ) {
      setError('Please fill in all billing address fields');
      return false;
    }

    if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(billingAddress.emailAddress)) {
      setError('Please enter a valid email address');
      return false;
    }

    if (
      !paymentInfo.cardName ||
      !paymentInfo.cardNumber ||
      !paymentInfo.expiration ||
      !paymentInfo.cvv
    ) {
      setError('Please fill in all payment information fields');
      return false;
    }

    // Basic card number validation (16 digits)
    if (paymentInfo.cardNumber.replace(/\s/g, '').length !== 16) {
      setError('Card number must be 16 digits');
      return false;
    }

    // Basic expiry validation (MM/YY format)
    if (!/^\d{2}\/\d{2}$/.test(paymentInfo.expiration)) {
      setError('Expiry date must be in MM/YY format');
      return false;
    }

    // Basic CVV validation (3-4 digits)
    if (!/^\d{3,4}$/.test(paymentInfo.cvv)) {
      setError('CVV must be 3-4 digits');
      return false;
    }

    return true;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);

    if (!validateForm()) {
      return;
    }

    setIsSubmitting(true);
    try {
      // Create checkout payload matching BasketCheckoutDto structure
      const checkoutData: BasketCheckoutDto = {
        items: cart.items.map(item => ({
          productId: item.productId,
          name: item.name,
          category: item.category,
          quantity: item.quantity,
          imageUrl: item.imageUrl,
          price: item.price,
        })),
        totalPrice: cart.totalPrice,
        firstName: billingAddress.firstName,
        lastName: billingAddress.lastName,
        emailAddress: billingAddress.emailAddress,
        addressLine: billingAddress.addressLine,
        country: billingAddress.country,
        state: billingAddress.state,
        zipCode: billingAddress.zipCode,
        cardName: paymentInfo.cardName,
        cardNumber: paymentInfo.cardNumber.replace(/\s/g, ''), // Remove spaces
        expiration: paymentInfo.expiration,
        cvv: paymentInfo.cvv,
        paymentMethod: paymentMethod,
      };

      await cartAPI.checkout(checkoutData);
      // Clear cart and redirect to orders
      clearCart();
      navigate('/orders', { replace: true });
    } catch (err: any) {
      setError(err.response?.data?.message || 'Checkout failed. Please try again.');
      console.error('Checkout error:', err);
    } finally {
      setIsSubmitting(false);
    }
  };

  const handlePaymentMethodChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    setPaymentMethod(parseInt(e.target.value));
  };

 
  return (
    <div className="checkout-container">
      <h2>Checkout</h2>

      <div className="checkout-content">
        {/* Order Summary */}
        <div className="order-summary">
          <h3>Order Summary</h3>
          <div className="summary-items">
            {cart.items.map((item) => (
              <div key={item.productId} className="summary-item">
                <div className="item-info">
                  <span className="item-name">{item.name}</span>
                  <span className="item-quantity">x{item.quantity}</span>
                </div>
                <span className="item-total">
                  ${(item.price * item.quantity).toFixed(2)}
                </span>
              </div>
            ))}
          </div>
          <div className="summary-total">
            <h4>Total: ${cart.totalPrice.toFixed(2)}</h4>
          </div>
        </div>

        {/* Checkout Form */}
        <form className="checkout-form" onSubmit={handleSubmit}>
          {error && <div className="error-message">{error}</div>}

          {/* Billing Address Section */}
          <div className="form-section">
            <h3>Billing Address</h3>
            <div className="form-row">
              <div className="form-group">
                <label htmlFor="firstName">First Name *</label>
                <input
                  type="text"
                  id="firstName"
                  name="firstName"
                  value={billingAddress.firstName}
                  onChange={handleAddressChange}
                  placeholder="John"
                  disabled={isSubmitting}
                />
              </div>
              <div className="form-group">
                <label htmlFor="lastName">Last Name *</label>
                <input
                  type="text"
                  id="lastName"
                  name="lastName"
                  value={billingAddress.lastName}
                  onChange={handleAddressChange}
                  placeholder="Doe"
                  disabled={isSubmitting}
                />
              </div>
            </div>

            <div className="form-group">
              <label htmlFor="emailAddress">Email Address *</label>
              <input
                type="email"
                id="emailAddress"
                name="emailAddress"
                value={billingAddress.emailAddress}
                onChange={handleAddressChange}
                placeholder="john.doe@example.com"
                disabled={isSubmitting}
              />
            </div>

            <div className="form-group">
              <label htmlFor="addressLine">Address Line *</label>
              <input
                type="text"
                id="addressLine"
                name="addressLine"
                value={billingAddress.addressLine}
                onChange={handleAddressChange}
                placeholder="123 Main Street"
                disabled={isSubmitting}
              />
            </div>

            <div className="form-row">
              <div className="form-group">
                <label htmlFor="country">Country *</label>
                <input
                  type="text"
                  id="country"
                  name="country"
                  value={billingAddress.country}
                  onChange={handleAddressChange}
                  placeholder="United States"
                  disabled={isSubmitting}
                />
              </div>
              <div className="form-group">
                <label htmlFor="state">State *</label>
                <input
                  type="text"
                  id="state"
                  name="state"
                  value={billingAddress.state}
                  onChange={handleAddressChange}
                  placeholder="NY"
                  disabled={isSubmitting}
                />
              </div>
            </div>

            <div className="form-group">
              <label htmlFor="zipCode">Zip Code *</label>
              <input
                type="text"
                id="zipCode"
                name="zipCode"
                value={billingAddress.zipCode}
                onChange={handleAddressChange}
                placeholder="10001"
                disabled={isSubmitting}
              />
            </div>
          </div>

          {/* Payment Information Section */}
          <div className="form-section">
            <h3>Payment Method</h3>
            <div className="form-group">
              <label htmlFor="paymentMethod">Select Payment Method *</label>
              <select
                id="paymentMethod"
                name="paymentMethod"
                value={paymentMethod}
                onChange={handlePaymentMethodChange}
                disabled={isSubmitting}
                className="payment-select"
              >
                <option value={1}>Credit Card</option>
                <option value={2}>Debit Card</option>
                <option value={3}>PayPal</option>
                <option value={4}>Bank Transfer</option>
              </select>
            </div>
          </div>

          {/* Payment Information Section */}
          <div className="form-section">
            <h3>Payment Information</h3>
            <div className="form-group">
              <label htmlFor="cardName">Cardholder Name *</label>
              <input
                type="text"
                id="cardName"
                name="cardName"
                value={paymentInfo.cardName}
                onChange={(e) => setPaymentInfo(prev => ({ ...prev, cardName: e.target.value }))}
                placeholder="John Doe"
                disabled={isSubmitting}
              />
            </div>

            <div className="form-group">
              <label htmlFor="cardNumber">Card Number *</label>
              <input
                type="text"
                id="cardNumber"
                name="cardNumber"
                value={paymentInfo.cardNumber}
                onChange={(e) => setPaymentInfo(prev => ({ ...prev, cardNumber: e.target.value.replace(/\D/g, '') }))}
                placeholder="4111111111111111"
                maxLength={19}
                disabled={isSubmitting}
              />
            </div>

            <div className="form-row">
              <div className="form-group">
                <label htmlFor="expiration">Expiry Date (MM/YY) *</label>
                <input
                  type="text"
                  id="expiration"
                  name="expiration"
                  value={paymentInfo.expiration}
                  onChange={(e) => setPaymentInfo(prev => ({ ...prev, expiration: e.target.value }))}
                  placeholder="12/25"
                  maxLength={5}
                  disabled={isSubmitting}
                />
              </div>
              <div className="form-group">
                <label htmlFor="cvv">CVV *</label>
                <input
                  type="text"
                  id="cvv"
                  name="cvv"
                  value={paymentInfo.cvv}
                  onChange={(e) => setPaymentInfo(prev => ({ ...prev, cvv: e.target.value }))}
                  placeholder="123"
                  maxLength={4}
                  disabled={isSubmitting}
                />
              </div>
            </div>
          </div>

          {/* Action Buttons */}
          <div className="form-actions">
            <button
              type="button"
              className="back-btn"
              onClick={() => navigate('/cart')}
              disabled={isSubmitting}
            >
              Back to Cart
            </button>
            <button type="submit" className="submit-btn" disabled={isSubmitting}>
              {isSubmitting ? 'Processing...' : 'Place Order'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default Checkout;
