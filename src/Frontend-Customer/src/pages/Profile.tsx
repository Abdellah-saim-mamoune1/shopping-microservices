import React, { useState, useEffect } from 'react';
import { useAuth } from '../contexts/AuthContext';
import { userAPI } from '../api';
import { User } from '../types';
import './Profile.css';

const Profile: React.FC = () => {
  const { user, isAuthenticated, isLoadingUserData, refreshUserData } = useAuth();
  const [editing, setEditing] = useState(false);
  const [formData, setFormData] = useState<Partial<User>>({});
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
console.log("uuuu"+user);
  useEffect(() => {
    if (user) {
      setFormData(user);
    }
  }, [user]);

  const handleEdit = () => {
    setEditing(true);
  };

  const handleSave = async () => {
    setLoading(true);
    setError('');
    try {
      await userAPI.updateProfile(formData);
      await refreshUserData();
      setEditing(false);
    } catch (err: any) {
      setError(err.response?.data?.title || 'Failed to update profile');
    } finally {
      setLoading(false);
    }
  };

  const handleCancel = () => {
    setFormData(user || {});
    setEditing(false);
    setError('');
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  if (!isAuthenticated) {
    return <div className="profile-container">Please login to view your profile.</div>;
  }

  if (isLoadingUserData || !user) {
    return <div className="profile-container">Loading profile...</div>;
  }

  return (
    <div className="profile-container">
      <h2>Your Profile</h2>
      <div className="profile-card">
        <div className="profile-field">
          <label>First Name:</label>
          {editing ? (
            <input
              type="text"
              name="firstName"
              value={formData.firstName || ''}
              onChange={handleChange}
            />
          ) : (
            <span>{user.firstName}</span>
          )}
        </div>
        <div className="profile-field">
          <label>Last Name:</label>
          {editing ? (
            <input
              type="text"
              name="lastName"
              value={formData.lastName || ''}
              onChange={handleChange}
            />
          ) : (
            <span>{user.lastName}</span>
          )}
        </div>
        <div className="profile-field">
          <label>Email:</label>
          {editing ? (
            <input
              type="email"
              name="email"
              value={formData.email || ''}
              onChange={handleChange}
            />
          ) : (
            <span>{user.email}</span>
          )}
        </div>
        <div className="profile-field">
          <label>Phone Number:</label>
          {editing ? (
            <input
              type="tel"
              name="phoneNumber"
              value={formData.phoneNumber || ''}
              onChange={handleChange}
            />
          ) : (
            <span>{user.phoneNumber}</span>
          )}
        </div>
        <div className="profile-field">
          <label>Account Type:</label>
          <span>{user.type}</span>
        </div>
        <div className="profile-field">
          <label>Member Since:</label>
          <span>{new Date(user.createdAt).toLocaleDateString()}</span>
        </div>
        {error && <div className="error">{error}</div>}
        <div className="profile-actions">
          {editing ? (
            <>
              <button onClick={handleSave} disabled={loading}>
                {loading ? 'Saving...' : 'Save'}
              </button>
              <button onClick={handleCancel}>Cancel</button>
            </>
          ) : (
            <button onClick={handleEdit}>Edit Profile</button>
          )}
        </div>
      </div>
    </div>
  );
};

export default Profile;