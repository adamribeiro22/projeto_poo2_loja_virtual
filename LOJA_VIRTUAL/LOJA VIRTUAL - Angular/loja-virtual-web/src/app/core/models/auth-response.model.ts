import { Usuario } from './usuario.model';

export interface AuthResponse {
  sucesso: boolean;
  mensagem: string;
  token?: string;
  usuario?: Usuario;
}