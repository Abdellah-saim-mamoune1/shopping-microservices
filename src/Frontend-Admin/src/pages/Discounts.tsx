import React, { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { apiService } from '../services/api';
import { Coupon, DiscountFormData } from '../types';
import { useToast } from '../contexts/ToastContext';
import Modal from '../components/Modal';
import { Plus, Edit, Trash2 } from 'lucide-react';

const Discounts: React.FC = () => {
  const [discounts, setDiscounts] = useState<Coupon[]>([]);
  const [loading, setLoading] = useState(false);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingDiscount, setEditingDiscount] = useState<Coupon | null>(null);
  const { showToast } = useToast();

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<DiscountFormData>();

  const fetchDiscounts = async () => {
    setLoading(true);
    try {
      const response = await apiService.getDiscounts();
      if (response.data.success) {
        setDiscounts(response.data.data || []);
      } else {
        showToast('Failed to load discounts', 'error');
      }
    } catch (error) {
      showToast('Error loading discounts', 'error');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchDiscounts();
  }, []);

  const openCreateModal = () => {
    setEditingDiscount(null);
    reset();
    setIsModalOpen(true);
  };

  const openEditModal = (discount: Coupon) => {
    setEditingDiscount(discount);
    reset({
      bookId: discount.bookId,
      name: discount.name,
      amount: discount.amount,
    });
    setIsModalOpen(true);
  };

  const handleSubmitDiscount = async (data: DiscountFormData) => {
    try {
      if (editingDiscount) {
        await apiService.updateDiscount(editingDiscount.id, data);
        showToast('Discount updated successfully', 'success');
      } else {
        await apiService.createDiscount(data);
        showToast('Discount created successfully', 'success');
      }
      setIsModalOpen(false);
      fetchDiscounts();
    } catch (error) {
      showToast('Error saving discount', 'error');
    }
  };

  const handleDeleteDiscount = async (discountId: string) => {
    if (!confirm('Are you sure you want to delete this discount?')) return;

    try {
      await apiService.deleteDiscount(discountId);
      showToast('Discount deleted successfully', 'success');
      fetchDiscounts();
    } catch (error) {
      showToast('Error deleting discount', 'error');
    }
  };

  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <h1 className="text-2xl font-bold text-gray-900">Discounts Management</h1>
        <button
          onClick={openCreateModal}
          className="bg-blue-600 text-white px-4 py-2 rounded-md hover:bg-blue-700 flex items-center"
        >
          <Plus className="w-4 h-4 mr-2" />
          Add Discount
        </button>
      </div>

      {/* Discounts Table */}
      <div className="bg-white shadow rounded-lg overflow-hidden">
        <div className="overflow-x-auto">
          <table className="min-w-full divide-y divide-gray-200">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Book ID
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Name
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Discount Amount
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Actions
                </th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {loading ? (
                <tr>
                  <td colSpan={4} className="px-6 py-4 text-center">
                    <div className="flex justify-center">
                      <div className="animate-spin rounded-full h-6 w-6 border-b-2 border-blue-600"></div>
                    </div>
                  </td>
                </tr>
              ) : discounts.length === 0 ? (
                <tr>
                  <td colSpan={4} className="px-6 py-4 text-center text-gray-500">
                    No discounts found
                  </td>
                </tr>
              ) : (
                discounts.map((discount) => (
                  <tr key={discount.id} className="hover:bg-gray-50">
                    <td className="px-6 py-4 whitespace-nowrap text-sm font-mono text-gray-900">
                      {discount.bookId}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                      {discount.name}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                      ${discount.amount.toFixed(2)}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm font-medium">
                      <button
                        onClick={() => openEditModal(discount)}
                        className="text-blue-600 hover:text-blue-900 mr-4"
                      >
                        <Edit className="w-4 h-4" />
                      </button>
                      <button
                        onClick={() => handleDeleteDiscount(discount.id)}
                        className="text-red-600 hover:text-red-900"
                      >
                        <Trash2 className="w-4 h-4" />
                      </button>
                    </td>
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </div>
      </div>

      {/* Discount Modal */}
      <Modal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        title={editingDiscount ? 'Edit Discount' : 'Add New Discount'}
      >
        <form onSubmit={handleSubmit(handleSubmitDiscount)} className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700">Book ID</label>
            <input
              {...register('bookId', { required: 'Book ID is required' })}
              placeholder="Enter the book ID to apply discount"
              className="mt-1 block w-full border border-gray-300 rounded-md px-3 py-2 focus:ring-blue-500 focus:border-blue-500"
            />
            {errors.bookId && <p className="mt-1 text-sm text-red-600">{errors.bookId.message}</p>}
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700">Discount Name</label>
            <input
              {...register('name', { required: 'Name is required' })}
              placeholder="e.g., Summer Sale, New Release Discount"
              className="mt-1 block w-full border border-gray-300 rounded-md px-3 py-2 focus:ring-blue-500 focus:border-blue-500"
            />
            {errors.name && <p className="mt-1 text-sm text-red-600">{errors.name.message}</p>}
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700">Discount Amount ($)</label>
            <input
              {...register('amount', {
                required: 'Amount is required',
                valueAsNumber: true,
                min: { value: 0, message: 'Amount must be positive' }
              })}
              type="number"
              step="0.01"
              min="0"
              placeholder="0.00"
              className="mt-1 block w-full border border-gray-300 rounded-md px-3 py-2 focus:ring-blue-500 focus:border-blue-500"
            />
            {errors.amount && <p className="mt-1 text-sm text-red-600">{errors.amount.message}</p>}
          </div>

          <div className="flex justify-end space-x-3 pt-4">
            <button
              type="button"
              onClick={() => setIsModalOpen(false)}
              className="px-4 py-2 border border-gray-300 rounded-md text-sm font-medium text-gray-700 hover:bg-gray-50"
            >
              Cancel
            </button>
            <button
              type="submit"
              className="px-4 py-2 bg-blue-600 border border-transparent rounded-md text-sm font-medium text-white hover:bg-blue-700"
            >
              {editingDiscount ? 'Update' : 'Create'}
            </button>
          </div>
        </form>
      </Modal>
    </div>
  );
};

export default Discounts;