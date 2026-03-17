export interface AuthResponse {
  userId: string;
  name: string;
  email: string;
  provider: string;
  providerUserId: string;
  isNewUser: boolean;
  isNewProvider: boolean;
  accessToken: string;
  expiresAtUtc: string;
}

export interface LoginPayload {
  idToken: string;
  invitationCode?: string;
}
