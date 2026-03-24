export interface Book {
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
}

export interface PaginatedBooks {
  books: Book[];
  totalPages: number;
  pageNumber: number;
  pageSize: number;
}

export interface ApiResponse<T> {
  success: boolean;
  statusCode: number;
  title: string;
  data: T;
  errors: any;
}

export interface User {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  type: string;
  createdAt: string;
}

export interface CartItem {
  productId: string;
  name: string;
  price: number;
  quantity: number;
  imageUrl: string;
  category: string;
  description: string;
  discountAmount: number;
}



export interface OrderItem {
  productId: string;
  name: string;
  totalPrice: number;
  quantity: number;
  imageUrl: string;
  category: string;
  description: string;
  discountAmount: number;
}


export interface Cart {
  items: CartItem[];
  userId:number;
  totalPrice: number;
}

export interface Order {
  id: string;
  userId: string;
  createdAt: string;
  totalPrice: number;
  status: string;
  items: OrderItem[];
}

export interface AuthTokens {
  id: string;
  accessToken: string;
  refreshToken: string;
  expiresAt: string;
}

export interface BasketCheckoutDto {
  items: BasketItemDto[];
  totalPrice: number;
  firstName: string;
  lastName: string;
  emailAddress: string;
  addressLine: string;
  country: string;
  state: string;
  zipCode: string;
  cardName: string;
  cardNumber: string;
  expiration: string;
  cvv: string;
  paymentMethod: number;
}

export interface BasketItemDto {
  productId: string;
  name: string;
  category: string;
  quantity: number;
  imageUrl: string;
  price: number;
}

export interface LoginRequest {
  account: string;
  password: string;
}

export interface RegisterRequest {
  firstName: string;
  lastName: string;
  phoneNumber: string;
  account: string;
  password: string;
}