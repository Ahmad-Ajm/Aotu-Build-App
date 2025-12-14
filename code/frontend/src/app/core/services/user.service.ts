import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { User, UserProfile, UpdateProfileData } from '../models/user.model';

interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = '/api/account';

  constructor(private http: HttpClient) {}

  // الحصول على بيانات المستخدم الأساسية
  getProfile(): Observable<User> {
    return this.http
      .get<ApiResponse<User>>(`${this.apiUrl}/profile`)
      .pipe(map(response => response.data));
  }

  // تحديث بيانات المستخدم الأساسية
  updateProfile(updateData: UpdateProfileData): Observable<User> {
    return this.http
      .put<ApiResponse<User>>(`${this.apiUrl}/profile`, updateData)
      .pipe(map(response => response.data));
  }

  // الحصول على الملف الشخصي التفصيلي
  getUserProfile(): Observable<UserProfile> {
    return this.http
      .get<ApiResponse<UserProfile>>(`${this.apiUrl}/user-profile`)
      .pipe(map(response => response.data));
  }

  // تحديث الملف الشخصي التفصيلي
  updateUserProfile(profileData: Partial<UserProfile>): Observable<UserProfile> {
    return this.http
      .put<ApiResponse<UserProfile>>(`${this.apiUrl}/user-profile`, profileData)
      .pipe(map(response => response.data));
  }
}


