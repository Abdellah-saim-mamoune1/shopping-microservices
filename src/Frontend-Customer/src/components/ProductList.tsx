import React, { useState, useEffect } from 'react';
import { Book, PaginatedBooks } from '../types';
import { fetchProducts } from '../api';
import { useAuth } from '../contexts/AuthContext';
import './ProductList.css';

const ProductList: React.FC = () => {
  const [products, setProducts] = useState<Book[]>([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [selectedBook, setSelectedBook] = useState<Book | null>(null);

  const { addToCart } = useAuth();

  const pageSize = 20;

  useEffect(() => {
    const loadProducts = async () => {
      setLoading(true);
      setError(null);
      try {
        const data: PaginatedBooks = await fetchProducts(currentPage, pageSize);
        setProducts(data.books);
        setTotalPages(data.totalPages);
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Failed to load products');
      } finally {
        setLoading(false);
      }
    };

    loadProducts();
  }, [currentPage]);

  const handlePageChange = (page: number) => {
    setCurrentPage(page);
  };

  // ✅ Helpers
  const getDiscountedPrice = (price: number, discountAmount?: number) => {
    if (!discountAmount || discountAmount <= 0) return price;
    return price - discountAmount;
  };

  const getDiscountPercentage = (price: number, discountAmount?: number) => {
    if (!discountAmount || discountAmount <= 0) return 0;
    return Math.round((discountAmount / price) * 100);
  };

  if (loading) return <div className="loading">Loading...</div>;
  if (error) return <div className="error">{error}</div>;

  return (
    <div className="product-list-container">
      <h2 style={{ paddingBottom: '10px' }}>Books</h2>

      <div className="product-grid">
        {products.map((book) => (
          <div
            key={book.id}
            className="product-card"
            onClick={() => setSelectedBook(book)}
          >
            <img src={book.imageUrl} alt={book.name} className="product-image" />

            <h3>{book.name}</h3>
            <p className="authors">{book.authors.join(', ')}</p>

            {/* Price Section */}
            <div className="price-container">
              {book.discountAmount && book.discountAmount > 0 ? (
                <>
                  <span className="original-price">
                    ${book.price.toFixed(2)}
                  </span>

                  <span className="discounted-price">
                    ${getDiscountedPrice(book.price, book.discountAmount).toFixed(2)}
                  </span>

                  <span className="discount-badge">
                    -{getDiscountPercentage(book.price, book.discountAmount)}%
                  </span>
                </>
              ) : (
                <span className="price">${book.price.toFixed(2)}</span>
              )}
            </div>

            <p className="rating">
              ⭐ {book.averageRating.toFixed(1)} ({book.ratingsCount} reviews)
            </p>
          </div>
        ))}
      </div>

      {/* Pagination */}
      <div className="pagination">
        <button
          onClick={() => handlePageChange(currentPage - 1)}
          disabled={currentPage === 1}
        >
          Previous
        </button>

        <span>Page {currentPage} of {totalPages}</span>

        <button
          onClick={() => handlePageChange(currentPage + 1)}
          disabled={currentPage === totalPages}
        >
          Next
        </button>
      </div>

      {/* Modal */}
      {selectedBook && (
        <div className="modal-overlay" onClick={() => setSelectedBook(null)}>
          <div className="modal-content" onClick={(e) => e.stopPropagation()}>
            <img
              src={selectedBook.imageUrl}
              alt={selectedBook.name}
              className="modal-image"
            />

            <h2 className="modal-title">{selectedBook.name}</h2>

            <p className="modal-authors">
              <strong>Authors:</strong> {selectedBook.authors.join(', ')}
            </p>

            <p className="modal-category">
              <strong>Category:</strong> {selectedBook.category}
            </p>

            {/*  Modal Price */}
            <div className="modal-price">
              <strong>Price: </strong>

              {selectedBook.discountAmount && selectedBook.discountAmount > 0 ? (
                <>
                  <span className="original-price" style={{ marginRight: '10px' }}>
                    ${selectedBook.price.toFixed(2)}
                  </span>

                  <span className="discounted-price" style={{ marginRight: '10px' }}>
                    ${getDiscountedPrice(
                      selectedBook.price,
                      selectedBook.discountAmount
                    ).toFixed(2)}
                  </span>

                  <span className="discount-badge">
                    ({getDiscountPercentage(
                      selectedBook.price,
                      selectedBook.discountAmount
                    )}% OFF)
                  </span>
                </>
              ) : (
                <span>${selectedBook.price.toFixed(2)}</span>
              )}
            </div>

            <p className="modal-rating">
              <strong>Rating:</strong> ⭐ {selectedBook.averageRating.toFixed(1)} (
              {selectedBook.ratingsCount} reviews)
            </p>

            <p className="modal-summary">{selectedBook.description}</p>

            <div className="modal-buttons">
              <button
                onClick={() => {
                  addToCart({
                    productId: selectedBook.id,
                    name: selectedBook.name,
                    price:  Number(
                          (selectedBook.price - (selectedBook.discountAmount || 0)).toFixed(2)),
                    quantity: 1,
                    imageUrl: selectedBook.imageUrl,
                    category: selectedBook.category,
                    description: selectedBook.description,
                    discountAmount: selectedBook.discountAmount,
                  });
                  setSelectedBook(null);
                }}
              >
                Add to Cart
              </button>

              <button
                className="close-btn"
                onClick={() => setSelectedBook(null)}
              >
                Close
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default ProductList;