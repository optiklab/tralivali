export interface User {
  id: string;
  username: string;
  email: string;
  publicKey: string;
}

export interface AuthResponse {
  token: string;
  userId: string;
  username: string;
}

export interface Message {
  id: string;
  senderId: string;
  recipientId: string;
  conversationId: string;
  encryptedContent: string;
  encryptedKey: string;
  sentAt: string;
  deliveredAt?: string;
  readAt?: string;
}

export interface Conversation {
  id: string;
  participantIds: string[];
  lastMessageId: string;
  createdAt: string;
  updatedAt: string;
}

export interface Invite {
  id: string;
  inviterUserId: string;
  inviteCode: string;
  email: string;
  createdAt: string;
  expiresAt: string;
  isUsed: boolean;
}
