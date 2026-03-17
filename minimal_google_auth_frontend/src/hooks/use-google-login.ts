import { useMutation } from '@tanstack/react-query';
import type { CredentialResponse } from '@react-oauth/google';
import { loginWithGoogle } from '../lib/http';
import { useAuthStore } from '../store/auth-store';

export function useGoogleLoginMutation() {
  const setSession = useAuthStore((state) => state.setSession);

  return useMutation({
    mutationFn: async ({ credential, invitationCode }: { credential?: string; invitationCode?: string }) => {
      if (!credential) {
        throw new Error('Google credential was not returned.');
      }

      const result = await loginWithGoogle({
        idToken: credential,
        invitationCode: invitationCode?.trim() || undefined,
      });

      setSession(result);
      return result;
    },
  });
}

export function getGoogleCredential(response: CredentialResponse) {
  return response.credential;
}
