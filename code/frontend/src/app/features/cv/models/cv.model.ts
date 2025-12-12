export interface CVDto {
  id: string;
  userId: string;
  title: string;
  personalInfo: string;
  workExperience: string;
  education: string;
  skills: string;
  template: string;
  isPublic: boolean;
  shareLink?: string;
  lastUpdated?: Date;
  viewCount?: number;
  creationTime?: Date;
  creatorId?: string;
  lastModificationTime?: Date;
  lastModifierId?: string;
}

export interface CreateCVDto {
  title: string;
  template: string;
  isPublic: boolean;
  personalInfo: PersonalInfoDto;
  contactInfo: ContactInfoDto;
}

export interface UpdateCVDto {
  title?: string;
  template?: string;
  personalInfo?: PersonalInfoDto;
  workExperience?: any;
  education?: any;
  skills?: any;
  contactInfo?: ContactInfoDto;
}

export interface PersonalInfoDto {
  fullName: string;
  email: string;
  phoneNumber?: string;
  address?: string;
  city?: string;
  country?: string;
  postalCode?: string;
  website?: string;
  linkedIn?: string;
  github?: string;
  twitter?: string;
  dateOfBirth?: Date;
  nationality?: string;
}

export interface ContactInfoDto {
  fullName: string;
  email: string;
  phoneNumber?: string;
  address?: string;
  city?: string;
  country?: string;
  postalCode?: string;
  website?: string;
  linkedIn?: string;
  github?: string;
  twitter?: string;
  dateOfBirth?: Date;
  nationality?: string;
}

export interface EducationDto {
  id?: string;
  cvId?: string;
  degree: string;
  institution: string;
  fieldOfStudy?: string;
  startDate: Date;
  endDate?: Date;
  isCurrentlyStudying?: boolean;
  gpa?: number;
  gpaScale?: number;
  description?: string;
  location?: string;
  order?: number;
}

export interface ExperienceDto {
  id?: string;
  cvId?: string;
  jobTitle: string;
  company: string;
  location?: string;
  startDate: Date;
  endDate?: Date;
  isCurrentlyWorking?: boolean;
  employmentType?: string;
  industry?: string;
  description?: string;
  achievements?: string;
  skillsUsed?: string;
  order?: number;
}

export interface SkillDto {
  id?: string;
  cvId?: string;
  name: string;
  level: SkillLevel;
  yearsOfExperience?: number;
  category?: string;
  description?: string;
  isFeatured?: boolean;
  order?: number;
  lastUsed?: Date;
}

export enum SkillLevel {
  Beginner = 'Beginner',
  Intermediate = 'Intermediate',
  Advanced = 'Advanced',
  Expert = 'Expert'
}

export interface ExportOptionsDto {
  template: string;
  language: string;
  includePhoto: boolean;
  includeSensitiveInfo: boolean;
}

export interface CVStatisticsDto {
  cvId: string;
  viewCount: number;
  lastUpdated: Date;
  createdAt: Date;
  isPublic: boolean;
}