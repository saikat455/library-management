import React, { useState, useEffect, useCallback } from "react";
import { userAPI } from "./services/api";
import UserTable from "./components/UserTable";
import UserForm from "./components/UserForm";
import type { User, MessageState } from "./types";

const App: React.FC = () => {
  const [users, setUsers] = useState<User[]>([]);
  const [loading, setLoading] = useState<boolean>(false);
  const [formLoading, setFormLoading] = useState<boolean>(false);
  const [message, setMessage] = useState<MessageState>({ text: "", type: "" });

  const showMessage = useCallback(
    (text: string, type: "success" | "error" | "info"): void => {
      setMessage({ text, type });
      setTimeout(() => setMessage({ text: "", type: "" }), 5000);
    },
    []
  );

  const fetchUsers = useCallback(async (): Promise<void> => {
    try {
      setLoading(true);
      const response = await userAPI.fetchUsers();
      setUsers(response.data);
      showMessage(
        `Loaded ${response.count} users ${response.cached ? "(from cache)" : ""
        }`,
        "success"
      );
    } catch (error) {
      console.error("Error fetching users:", error);
      showMessage("Failed to fetch users", "error");
    } finally {
      setLoading(false);
    }
  }, [showMessage]);

  useEffect(() => {
    fetchUsers();
  }, [fetchUsers]);

  const handleCreateUser = async (
    userData: Omit<User, "id" | "timeStamp">
  ): Promise<void> => {
    try {
      setFormLoading(true);
      await userAPI.createUser(userData);
      showMessage("User created successfully!", "success");
      await fetchUsers();
    } catch (error) {
      console.error("Error creating user:", error);
      showMessage("Failed to create user", "error");
    } finally {
      setFormLoading(false);
    }
  };

  const handleCreateBulkUsers = async (): Promise<void> => {
    if (!window.confirm("This will create 10,000 users. Continue?")) {
      return;
    }

    try {
      setLoading(true);
      showMessage("Creating 10,000 users... Please wait", "info");
      const response = await userAPI.createBulkUsers();
      showMessage(response.message, "success");
      await fetchUsers();
    } catch (error) {
      console.error("Error creating bulk users:", error);
      showMessage("Failed to create bulk users", "error");
    } finally {
      setLoading(false);
    }
  };

  const getMessageStyles = () => {
    const baseStyles = "px-4 py-3 rounded-lg mb-6 font-medium text-sm border";
    switch (message.type) {
      case "success":
        return `${baseStyles} bg-green-50 text-green-800 border-green-200`;
      case "error":
        return `${baseStyles} bg-red-50 text-red-800 border-red-200`;
      case "info":
        return `${baseStyles} bg-blue-50 text-blue-800 border-blue-200`;
      default:
        return baseStyles;
    }
  };

  return (
    <div className="min-h-screen bg-gray-50">
      <header className="bg-white border-b border-gray-200">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-6">
          <h1 className="text-2xl font-bold text-gray-900">
            Library Management System
          </h1>
          <p className="text-gray-600 text-sm mt-1">Manage your users efficiently</p>
        </div>
      </header>

      <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">

        {message.text && (
          <div className={getMessageStyles()}>{message.text}</div>
        )}

        <div className="flex flex-wrap gap-3 mb-6">
          <button
            onClick={fetchUsers}
            className="px-5 py-2.5 bg-white text-gray-700 font-medium rounded-lg border border-gray-300 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-blue-500 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
            disabled={loading}
          >
            {loading ? "Loading..." : "Refresh Users"}
          </button>
          <button
            onClick={handleCreateBulkUsers}
            className="px-5 py-2.5 bg-purple-600 text-white font-medium rounded-lg hover:bg-purple-700 focus:outline-none focus:ring-2 focus:ring-purple-500 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
            disabled={loading}
          >
            Create 10K Users
          </button>
        </div>

        <UserForm onSubmit={handleCreateUser} loading={formLoading} />

        <UserTable users={users} loading={loading} />
      </main>
    </div>
  );
};

export default App;