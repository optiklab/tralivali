import { useState, useEffect } from 'react';
import { messageService, inviteService } from '../services/api';
import { encryptMessage } from '../services/crypto';
import type { Conversation, Message, Invite } from '../types';

interface MessagingProps {
  userId: string;
  username: string;
  onLogout: () => void;
}

export default function Messaging({ userId, username, onLogout }: MessagingProps) {
  const [conversations, setConversations] = useState<Conversation[]>([]);
  const [selectedConversation, setSelectedConversation] = useState<string | null>(null);
  const [messages, setMessages] = useState<Message[]>([]);
  const [newMessage, setNewMessage] = useState('');
  const [invites, setInvites] = useState<Invite[]>([]);
  const [newInviteEmail, setNewInviteEmail] = useState('');
  const [showInvites, setShowInvites] = useState(false);

  useEffect(() => {
    loadConversations();
    loadInvites();
  }, []);

  useEffect(() => {
    if (selectedConversation) {
      loadMessages(selectedConversation);
    }
  }, [selectedConversation]);

  const loadConversations = async () => {
    try {
      const data = await messageService.getConversations();
      setConversations(data);
    } catch (err) {
      console.error('Failed to load conversations', err);
    }
  };

  const loadMessages = async (conversationId: string) => {
    try {
      const data = await messageService.getMessages(conversationId);
      setMessages(data.reverse());
    } catch (err) {
      console.error('Failed to load messages', err);
    }
  };

  const loadInvites = async () => {
    try {
      const data = await inviteService.getMyInvites();
      setInvites(data);
    } catch (err) {
      console.error('Failed to load invites', err);
    }
  };

  const handleSendMessage = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!newMessage.trim() || !selectedConversation) return;

    try {
      const conversation = conversations.find((c) => c.id === selectedConversation);
      if (!conversation) return;

      const recipientId = conversation.participantIds.find((id) => id !== userId);
      if (!recipientId) return;

      // For demo purposes, using a placeholder public key
      // In production, fetch recipient's public key from the server
      const { encryptedContent, encryptedKey } = await encryptMessage(
        newMessage,
        'placeholder-public-key'
      );

      await messageService.sendMessage({
        recipientId,
        encryptedContent,
        encryptedKey,
      });

      setNewMessage('');
      loadMessages(selectedConversation);
    } catch (err) {
      console.error('Failed to send message', err);
    }
  };

  const handleCreateInvite = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!newInviteEmail.trim()) return;

    try {
      await inviteService.createInvite(newInviteEmail);
      setNewInviteEmail('');
      loadInvites();
      alert('Invite created successfully!');
    } catch (err) {
      console.error('Failed to create invite', err);
    }
  };

  return (
    <div style={{ display: 'flex', height: '100vh' }}>
      {/* Sidebar */}
      <div style={{ width: '300px', borderRight: '1px solid #ccc', padding: '20px' }}>
        <div style={{ marginBottom: '20px' }}>
          <h2>Tralivali</h2>
          <p>Welcome, {username}!</p>
          <button onClick={onLogout} style={{ marginRight: '10px' }}>
            Logout
          </button>
          <button onClick={() => setShowInvites(!showInvites)}>
            {showInvites ? 'Messages' : 'Invites'}
          </button>
        </div>

        {!showInvites ? (
          <>
            <h3>Conversations</h3>
            {conversations.map((conv) => (
              <div
                key={conv.id}
                onClick={() => setSelectedConversation(conv.id)}
                style={{
                  padding: '10px',
                  cursor: 'pointer',
                  backgroundColor: selectedConversation === conv.id ? '#e0e0e0' : 'white',
                  marginBottom: '5px',
                }}
              >
                Conversation {conv.id.substring(0, 8)}...
              </div>
            ))}
          </>
        ) : (
          <>
            <h3>Invites</h3>
            <form onSubmit={handleCreateInvite} style={{ marginBottom: '20px' }}>
              <input
                type="email"
                placeholder="Email"
                value={newInviteEmail}
                onChange={(e) => setNewInviteEmail(e.target.value)}
                style={{ width: '100%', padding: '5px', marginBottom: '5px' }}
              />
              <button type="submit">Create Invite</button>
            </form>
            {invites.map((invite) => (
              <div key={invite.id} style={{ padding: '10px', borderBottom: '1px solid #ccc' }}>
                <p>
                  <strong>Code:</strong> {invite.inviteCode}
                </p>
                <p>
                  <strong>Email:</strong> {invite.email}
                </p>
                <p>
                  <strong>Status:</strong> {invite.isUsed ? 'Used' : 'Active'}
                </p>
              </div>
            ))}
          </>
        )}
      </div>

      {/* Main Chat Area */}
      <div style={{ flex: 1, display: 'flex', flexDirection: 'column' }}>
        {selectedConversation ? (
          <>
            <div style={{ flex: 1, overflowY: 'auto', padding: '20px' }}>
              {messages.map((msg) => (
                <div
                  key={msg.id}
                  style={{
                    marginBottom: '10px',
                    textAlign: msg.senderId === userId ? 'right' : 'left',
                  }}
                >
                  <div
                    style={{
                      display: 'inline-block',
                      padding: '10px',
                      backgroundColor: msg.senderId === userId ? '#007bff' : '#e0e0e0',
                      color: msg.senderId === userId ? 'white' : 'black',
                      borderRadius: '10px',
                      maxWidth: '70%',
                    }}
                  >
                    {msg.encryptedContent.substring(0, 50)}... (encrypted)
                  </div>
                </div>
              ))}
            </div>
            <form onSubmit={handleSendMessage} style={{ padding: '20px', borderTop: '1px solid #ccc' }}>
              <input
                type="text"
                placeholder="Type a message..."
                value={newMessage}
                onChange={(e) => setNewMessage(e.target.value)}
                style={{ width: 'calc(100% - 100px)', padding: '10px' }}
              />
              <button type="submit" style={{ width: '80px', padding: '10px', marginLeft: '10px' }}>
                Send
              </button>
            </form>
          </>
        ) : (
          <div style={{ flex: 1, display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
            <p>Select a conversation to start messaging</p>
          </div>
        )}
      </div>
    </div>
  );
}
