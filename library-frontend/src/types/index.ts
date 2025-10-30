export interface User {
  id: number;
  name: string;
  age: number;
  email: string;
  timeStamp: string;
}

export interface UserFormData {
  name: string;
  age: string;
  email: string;
}

export interface FormErrors {
  name?: string;
  age?: string;
  email?: string;
}

export interface FetchUsersResponse {
  count: number;
  data: User[];
  cached: boolean;
}

export interface CreateUserResponse {
  id: number;
  name: string;
  age: number;
  email: string;
  timeStamp: string;
}

export interface CreateBulkUsersResponse {
  message: string;
  count: number;
}

export interface MessageState {
  text: string;
  type: 'success' | 'error' | 'info' | '';
}

export interface ApiError {
  message: string;
  statusCode?: number;
}