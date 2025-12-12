export interface AuthResponse {
  success: boolean;
  message: string;
  data: AuthData;
  errors?: { [key: string]: string[] };
}

export interface AuthData {
  userId: string;
  fullName: string;
  email: string;
  userName: string;
  accessToken: string;
  tokenType: string;
  expiresAt: Date;
  refreshToken?: string;
  isActive: boolean;
  lastLoginDate?: Date;
}

export interface RegisterData {
  fullName: string;
  email: string;
  userName?: string;
  password: string;
  confirmPassword: string;
  dateOfBirth?: Date;
  nationality?: string;
}

export interface LoginData {
  emailOrUserName: string;
  password: string;
  rememberMe: boolean;
}

export interface ForgotPasswordData {
  email: string;
}

export interface ResetPasswordData {
  token: string;
  newPassword: string;
}

export interface ChangePasswordData {
  currentPassword: string;
  newPassword: string;
}

export interface AuthError {
  message: string;
  status: number;
  errors?: { [key: string]: string[] };
}