// API Response wrapper
export interface ApiResponse<T> {
  success: boolean;
  statusCode: number;
  title?: string;
  data?: T;
  errors?: ValidationError[];
}

export interface ValidationError {
  field: string;
  message: string;
}

// Auth types
export interface LoginDto {
  account: string;
  password: string;
}

export interface AuthResponse {
  id: number;
  role: string;
  accessToken: string;
  refreshToken: string;
  expiresAt: string;
}

export interface EmployeeRegistrationDto {
  firstName: string;
  lastName: string;
  phoneNumber: string;
  account: string;
  password: string;
}

// Book types
export interface BookDto {
  name: string;
  category: string;
  summary: string;
  description: string;
  authors: string[];
  imageUrl: string;
  price: number;
  pagesCount: number;
  averageRating: number;
  ratingsCount: number;
  publishedAt?: string;
  quantity: number;
}

export interface BookGetDto {
  id: string;
  name: string;
  category: string;
  summary: string;
  description: string;
  authors: string[];
  imageUrl: string;
  price: number;
  discountAmount: number;
  pagesCount: number;
  averageRating: number;
  ratingsCount: number;
  publishedAt?: string;
  quantity: number;
}

export interface GetPaginatedBooksDto {
  books: BookGetDto[];
  totalPages: number;
  pageNumber: number;
  pageSize: number;
}

// Order types
export interface OrderCheckoutDto {
  items: Item[];
  totalPrice: number;
  firstName: string;
  lastName: string;
  emailAddress: string;
  addressLine: string;
  country: string;
  state: string;
  zipCode: string;
  paymentMethod: number;
}

export interface OrdersPaginatedGet {
  orders: Order[];
  totalPages: number;
  pageNumber: number;
  pageSize: number;
}

export interface OrderStatsDto {
  totalOrders: number;
  totalRevenue: number;
}

export interface Order {
  id: string;
  status: string;
  userId: number;
  items: Item[];
  totalPrice: number;
  firstName: string;
  lastName: string;
  emailAddress: string;
  addressLine: string;
  country: string;
  state: string;
  zipCode: string;
  paymentMethod: number;
  createdAt: string;
}

export interface Item {
  productId: string;
  name: string;
  category: string;
  quantity: number;
  imageUrl: string;
  totalPrice: number;
}

// Discount types
export interface CouponDto {
  bookId: string;
  name: string;
  amount: number;
}

export interface Coupon {
  id: string;
  bookId: string;
  name: string;
  amount: number;
}

// User types
export interface UserGetDto {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  type: string;
  createdAt: string;
}

export interface UserUpdateDto {
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
}

// Auth context types
export interface AuthContextType {
  user: AuthResponse | null;
  login: (credentials: LoginDto) => Promise<void>;
  logout: () => void;
  isAuthenticated: boolean;
  isLoading: boolean;
}

// Form types
export interface BookFormData extends BookDto {}

export interface DiscountFormData extends CouponDto {}

export interface ProfileFormData extends UserUpdateDto {}