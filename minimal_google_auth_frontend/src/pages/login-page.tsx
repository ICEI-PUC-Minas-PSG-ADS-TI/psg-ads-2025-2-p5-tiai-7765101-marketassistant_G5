import { GoogleLogin } from '@react-oauth/google';
import { useTranslation } from 'react-i18next';
import { GoogleMark } from '../components/google-mark';
import { getErrorMessage } from '../lib/error-message';
import { getGoogleCredential, useGoogleLoginMutation } from '../hooks/use-google-login';
import { useAuthStore } from '../store/auth-store';

function getInvitationCodeFromPath(pathname: string) {
  const trimmedPath = pathname.replace(/^\/+|\/+$/g, '');

  if (!trimmedPath) {
    return '';
  }

  const [firstSegment] = trimmedPath.split('/');
  return decodeURIComponent(firstSegment ?? '');
}

export function LoginPage() {
  const { t } = useTranslation();
  const session = useAuthStore((state) => state.session);
  const clearSession = useAuthStore((state) => state.clearSession);
  const loginMutation = useGoogleLoginMutation();
  const invitationCode = getInvitationCodeFromPath(window.location.pathname);

  const errorMessage = loginMutation.error
    ? getErrorMessage(loginMutation.error, t('genericError'))
    : null;

  return (
    <main className="flex min-h-screen items-center justify-center px-4 py-10">
      <section className="w-full max-w-md rounded-[2rem] border border-white/70 bg-white/85 p-8 shadow-2xl shadow-amber-950/10 backdrop-blur md:p-10">
        <div className="mb-8 flex items-center justify-between gap-3">
          <div className="flex items-center gap-3">
            <div className="flex h-11 w-11 items-center justify-center rounded-2xl bg-amber-100">
              <GoogleMark />
            </div>
            <div>
              <p className="text-xs font-semibold uppercase tracking-[0.3em] text-amber-700">Market API</p>
              <h1 className="text-2xl font-bold text-slate-900 md:text-3xl">{t('title')}</h1>
            </div>
          </div>

          {session ? (
            <button className="btn btn-sm btn-ghost" onClick={clearSession} type="button">
              {t('clear')}
            </button>
          ) : null}
        </div>

        <div className="space-y-5">
          <div className="rounded-2xl border border-slate-200 bg-slate-50 p-4">
            <GoogleLogin
              text="signin_with"
              theme="outline"
              shape="pill"
              size="large"
              locale="pt-BR"
              onSuccess={(response) => {
                void loginMutation.mutateAsync({
                  credential: getGoogleCredential(response),
                  invitationCode,
                });
              }}
              onError={() => {
                loginMutation.reset();
              }}
            />
          </div>

          {loginMutation.isPending ? (
            <div className="alert border-0 bg-amber-100 text-amber-950">
              <span>{t('loading')}</span>
            </div>
          ) : null}

          {errorMessage ? (
            <div className="alert border-0 bg-rose-100 text-rose-900">
              <span>{errorMessage}</span>
            </div>
          ) : null}

          {session ? (
            <div className="alert border-0 bg-emerald-100 text-emerald-900">
              <div className="flex flex-col items-start gap-2">
                <span className="font-semibold">{t('successTitle')}</span>
                <span className="text-sm">{t('successDescription')}</span>
                <div className="flex gap-2">
                  <span className={`badge ${session.isNewUser ? 'badge-warning' : 'badge-success'} badge-outline`}>
                    {session.isNewUser ? t('firstAccessBadge') : t('returningAccessBadge')}
                  </span>
                  <span className="badge badge-outline">{session.email}</span>
                </div>
              </div>
            </div>
          ) : null}
        </div>
      </section>
    </main>
  );
}
