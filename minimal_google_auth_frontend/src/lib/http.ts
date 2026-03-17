import type { AuthResponse, LoginPayload } from '../types/auth';

const API_BASE_URL = 'https://localhost:7114';

export class ApiError extends Error {
  constructor(
    message: string,
    public readonly status: number,
  ) {
    super(message);
    this.name = 'ApiError';
  }
}

export async function loginWithGoogle(payload: LoginPayload): Promise<AuthResponse> {
  const response = await fetch(`${API_BASE_URL}/auth/google`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      ...(payload.invitationCode ? { 'Invitation-Code': payload.invitationCode } : {}),
    },
    body: JSON.stringify({
      idToken: payload.idToken,
    }),
  });

  const text = await response.text();

  if (!response.ok) {
    throw new ApiError(text || 'Authentication failed.', response.status);
  }

  if (!text.trim()) {
    throw new ApiError('Empty authentication response.', response.status);
  }

  return JSON.parse(text) as AuthResponse;
}
