import axios from 'axios';
import type { AuthResponse, Invite, Message, Conversation } from '../types';

const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api';

const api = axios.create({
  baseURL: API_BASE_URL,
});

// Add token to requests
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export const authService = {
  register: async (data: {
    username: string;
    email: string;
    password: string;
    inviteCode: string;
    publicKey: string;
    privateKeyEncrypted: string;
  }): Promise<AuthResponse> => {
    const response = await api.post<AuthResponse>('/auth/register', data);
    return response.data;
  },

  login: async (email: string, password: string): Promise<AuthResponse> => {
    const response = await api.post<AuthResponse>('/auth/login', { email, password });
    return response.data;
  },
};

export const inviteService = {
  createInvite: async (email: string): Promise<Invite> => {
    const response = await api.post<Invite>('/invites', { email });
    return response.data;
  },

  getMyInvites: async (): Promise<Invite[]> => {
    const response = await api.get<Invite[]>('/invites');
    return response.data;
  },
};

export const messageService = {
  sendMessage: async (data: {
    recipientId: string;
    encryptedContent: string;
    encryptedKey: string;
  }): Promise<Message> => {
    const response = await api.post<Message>('/messages', data);
    return response.data;
  },

  getMessages: async (conversationId: string, limit = 50): Promise<Message[]> => {
    const response = await api.get<Message[]>(`/messages/conversations/${conversationId}`, {
      params: { limit },
    });
    return response.data;
  },

  getConversations: async (): Promise<Conversation[]> => {
    const response = await api.get<Conversation[]>('/messages/conversations');
    return response.data;
  },
};

export default api;
