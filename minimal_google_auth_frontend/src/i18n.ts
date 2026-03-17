import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';

void i18n.use(initReactI18next).init({
  lng: 'pt-BR',
  fallbackLng: 'pt-BR',
  interpolation: {
    escapeValue: false,
  },
  resources: {
    'pt-BR': {
      translation: {
        title: 'Entrar',
        loading: 'Autenticando com a API...',
        genericError:
          'Nao foi possivel concluir o login. Se for seu primeiro acesso, revise o invitation code.',
        successTitle: 'Login realizado',
        successDescription: 'Sessao autenticada com sucesso.',
        firstAccessBadge: 'Primeiro acesso',
        returningAccessBadge: 'Acesso recorrente',
        clear: 'Sair',
      },
    },
  },
});
