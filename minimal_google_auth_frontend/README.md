# minimal_google_auth_frontend

Frontend em React + Vite para o fluxo de login do `MarketAPI`.

## Stack usada

- React + TypeScript
- Vite
- Zustand
- TanStack Query
- Tailwind CSS + DaisyUI
- i18next
- Vitest

## Fluxo implementado

- Login apenas com Google.
- Campo opcional de `invitation code`.
- Se o usuário já existir, o campo pode ficar vazio.
- Se for o primeiro acesso, o frontend envia o valor do campo no header `Invitation-Code`.
- A resposta do `POST /auth/google` fica persistida localmente com Zustand.

## Como rodar

1. Crie `minimal_google_auth_frontend/.env.local`.
2. Adicione `VITE_GOOGLE_CLIENT_ID=seu_client_id`.
3. Rode `npm install`.
4. Rode `npm run dev`.
5. Acesse a URL exibida pelo Vite.

O frontend está apontando fixamente para `https://localhost:7114`.
O Google Client ID é lido de `.env.local`, que nao deve ser commitado.

## Scripts

- `npm run dev`
- `npm run build`
- `npm run test`
