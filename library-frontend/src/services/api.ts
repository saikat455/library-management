import axios, { type AxiosInstance, type AxiosResponse } from "axios";
import type {
  User,
  FetchUsersResponse,
  CreateUserResponse,
  CreateBulkUsersResponse,
} from "../types";

const API_BASE_URL = "http://localhost:5289/api";

const api: AxiosInstance = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

export const userAPI = {
  // Fetch all users
  fetchUsers: async (): Promise<FetchUsersResponse> => {
    const response: AxiosResponse<FetchUsersResponse> = await api.get(
      "/users/fetch-users"
    );
    return response.data;
  },

  // Create single user
  createUser: async (
    userData: Omit<User, "id" | "timeStamp">
  ): Promise<CreateUserResponse> => {
    const response: AxiosResponse<CreateUserResponse> = await api.post(
      "/users/create-user",
      userData
    );
    return response.data;
  },

  // Create bulk users
  createBulkUsers: async (): Promise<CreateBulkUsersResponse> => {
    const response: AxiosResponse<CreateBulkUsersResponse> = await api.post(
      "/users/create-bulk-users"
    );
    return response.data;
  },

  // Clear cache
  clearCache: async (): Promise<{ message: string }> => {
    const response: AxiosResponse<{ message: string }> = await api.get(
      "/users/clear-cache"
    );
    return response.data;
  },
};

export default api;