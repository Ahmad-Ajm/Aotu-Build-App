export interface User {
  id: string;
  fullName: string;
  email: string;
  userName: string;
  phoneNumber?: string;
  emailConfirmed: boolean;
  phoneNumberConfirmed: boolean;
  dateOfBirth?: Date;
  nationality?: string;
  isActive: boolean;
  failedLoginAttempts: number;
  lastLoginDate?: Date;
  lockoutEndDate?: Date;
  // تاريخ إنشاء الحساب (من جانب الخادم)
  creationTime?: Date;
  createdAt: Date;
  updatedAt: Date;
}

export interface UserProfile {
  id: string;
  userId: string;
  phoneNumber?: string;
  address?: string;
  city?: string;
  country?: string;
  postalCode?: string;
  profileImageUrl?: string;
  bio?: string;
  website?: string;
  linkedInUrl?: string;
  gitHubUrl?: string;
  twitterUrl?: string;
  languagePreference: string;
  themePreference: string;
  receiveNotifications: boolean;
  receiveEmails: boolean;
  lastProfileUpdate: Date;
}

export interface UpdateProfileData {
  fullName?: string;
  phoneNumber?: string;
  dateOfBirth?: Date;
  nationality?: string;
  address?: string;
  city?: string;
  country?: string;
  postalCode?: string;
  bio?: string;
  website?: string;
  linkedInUrl?: string;
  gitHubUrl?: string;
  twitterUrl?: string;
  languagePreference?: string;
  themePreference?: string;
  receiveNotifications?: boolean;
  receiveEmails?: boolean;
}

export interface UserPreferences {
  languagePreference: string;
  themePreference: string;
  receiveNotifications: boolean;
  receiveEmails: boolean;
}

export interface UserStatistics {
  totalCVs: number;
  publicCVs: number;
  totalViews: number;
  lastCVCreated?: Date;
  averageCVScore?: number;
}

export interface UserWithProfile extends User {
  profile?: UserProfile;
  statistics?: UserStatistics;
}