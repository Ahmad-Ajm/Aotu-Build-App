import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, throwError } from 'rxjs';
import { map, catchError, tap } from 'rxjs/operators';
import { Router } from '@angular/router';

// Models
import { AuthResponse } from '../models/auth.model';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = '/api/auth';
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  private tokenKey = 'auth_token';
  private refreshTokenKey = 'refresh_token';
  private userKey = 'current_user';

  constructor(
    private http: HttpClient,
    private router: Router
  ) {
    // محاولة تحميل بيانات المستخدم من التخزين المحلي
    this.loadUserFromStorage();
  }

  // الحصول على المستخدم الحالي كمشاهد
  get currentUser$(): Observable<User | null> {
    return this.currentUserSubject.asObservable();
  }

  // الحصول على المستخدم الحالي كقيمة
  get currentUserValue(): User | null {
    return this.currentUserSubject.value;
  }

  // تحديث بيانات المستخدم الحالي من خارج الخدمة (مثل صفحة الملف الشخصي)
  updateCurrentUser(user: User | null): void {
    this.currentUserSubject.next(user);
    if (user) {
      this.saveUserToStorage(user);
    } else {
      localStorage.removeItem(this.userKey);
    }
  }

  // التحقق مما إذا كان المستخدم مسجل دخول
  isLoggedIn(): boolean {
    const token = this.getToken();
    return !!token && !this.isTokenExpired(token);
  }

  // تسجيل مستخدم جديد
  register(registerData: any): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/register`, registerData).pipe(
      tap(response => {
        if (response.success) {
          this.handleAuthSuccess(response);
        }
      }),
      catchError(this.handleError)
    );
  }

  // تسجيل الدخول
  login(loginData: any): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, loginData).pipe(
      tap(response => {
        if (response.success) {
          this.handleAuthSuccess(response);
        }
      }),
      catchError(this.handleError)
    );
  }

  // تسجيل الخروج
  logout(): Observable<any> {
    const userId = this.currentUserValue?.id;
    
    return this.http.post(`${this.apiUrl}/logout`, { userId }).pipe(
      tap(() => {
        this.clearAuthData();
        this.currentUserSubject.next(null);
        this.router.navigate(['/auth/login']);
      }),
      catchError(this.handleError)
    );
  }

  // تحديث التوكن
  refreshToken(): Observable<AuthResponse> {
    const refreshToken = this.getRefreshToken();
    
    if (!refreshToken) {
      return throwError(() => new Error('لا يوجد توكن تحديث'));
    }

    return this.http.post<AuthResponse>(`${this.apiUrl}/refresh-token`, { refreshToken }).pipe(
      tap(response => {
        if (response.success) {
          this.handleAuthSuccess(response);
        }
      }),
      catchError(error => {
        // إذا فشل تحديث التوكن، تسجيل الخروج
        this.clearAuthData();
        this.currentUserSubject.next(null);
        this.router.navigate(['/auth/login']);
        return throwError(() => error);
      })
    );
  }

  // استعادة كلمة المرور
  forgotPassword(email: string): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/forgot-password`, { email }).pipe(
      catchError(this.handleError)
    );
  }

  // إعادة تعيين كلمة المرور
  resetPassword(token: string, newPassword: string): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/reset-password`, {
      token,
      newPassword
    }).pipe(
      catchError(this.handleError)
    );
  }

  // تغيير كلمة المرور
  changePassword(currentPassword: string, newPassword: string): Observable<AuthResponse> {
    const userId = this.currentUserValue?.id;
    
    return this.http.post<AuthResponse>(`${this.apiUrl}/change-password`, {
      userId,
      currentPassword,
      newPassword
    }).pipe(
      catchError(this.handleError)
    );
  }

  // الحصول على ملف المستخدم الشخصي
  getProfile(): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/profile`).pipe(
      tap(user => {
        this.currentUserSubject.next(user);
        this.saveUserToStorage(user);
      }),
      catchError(this.handleError)
    );
  }

  // تحديث ملف المستخدم الشخصي
  updateProfile(userData: any): Observable<User> {
    return this.http.put<User>(`${this.apiUrl}/profile`, userData).pipe(
      tap(user => {
        this.currentUserSubject.next(user);
        this.saveUserToStorage(user);
      }),
      catchError(this.handleError)
    );
  }

  // الحصول على التوكن
  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  // الحصول على توكن التحديث
  getRefreshToken(): string | null {
    return localStorage.getItem(this.refreshTokenKey);
  }

  // التحقق من صلاحية التوكن
  isTokenExpired(token: string): boolean {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      const expiry = payload.exp;
      return (Math.floor((new Date).getTime() / 1000)) >= expiry;
    } catch {
      return true;
    }
  }

  // إضافة التوكن إلى طلبات HTTP
  getAuthHeaders(): { [header: string]: string } {
    const token = this.getToken();
    return token ? { Authorization: `Bearer ${token}` } : {};
  }

  // Private Methods

  private handleAuthSuccess(response: AuthResponse): void {
    // حفظ التوكن
    if (response.data.accessToken) {
      localStorage.setItem(this.tokenKey, response.data.accessToken);
    }
    
    if (response.data.refreshToken) {
      localStorage.setItem(this.refreshTokenKey, response.data.refreshToken);
    }

    // إنشاء كائن المستخدم
    const now = new Date();
    const user: User = {
      id: response.data.userId,
      fullName: response.data.fullName,
      email: response.data.email,
      userName: response.data.userName,
      phoneNumber: undefined,
      emailConfirmed: false,
      phoneNumberConfirmed: false,
      isActive: response.data.isActive,
      failedLoginAttempts: 0,
      lastLoginDate: response.data.lastLoginDate,
      lockoutEndDate: undefined,
      creationTime: now,
      createdAt: now,
      updatedAt: now
    };

    // تحديث المستخدم الحالي
    this.updateCurrentUser(user);
  }

  private loadUserFromStorage(): void {
    const userJson = localStorage.getItem(this.userKey);
    if (userJson) {
      try {
        const user = JSON.parse(userJson);
        this.currentUserSubject.next(user);
      } catch {
        localStorage.removeItem(this.userKey);
      }
    }
  }

  private saveUserToStorage(user: User): void {
    localStorage.setItem(this.userKey, JSON.stringify(user));
  }

  private clearAuthData(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.refreshTokenKey);
    localStorage.removeItem(this.userKey);
  }

  private handleError(error: any): Observable<never> {
    console.error('Auth Service Error:', error);
    
    let errorMessage = 'حدث خطأ غير متوقع';
    
    if (error.error?.message) {
      errorMessage = error.error.message;
    } else if (error.status === 0) {
      errorMessage = 'تعذر الاتصال بالخادم. يرجى التحقق من اتصال الإنترنت';
    } else if (error.status === 401) {
      errorMessage = 'انتهت صلاحية الجلسة. يرجى تسجيل الدخول مرة أخرى';
    } else if (error.status === 403) {
      errorMessage = 'ليس لديك صلاحية للوصول إلى هذا المورد';
    } else if (error.status === 404) {
      errorMessage = 'المورد غير موجود';
    } else if (error.status === 500) {
      errorMessage = 'حدث خطأ في الخادم. يرجى المحاولة لاحقاً';
    }
    
    return throwError(() => ({
      message: errorMessage,
      status: error.status,
      errors: error.error?.errors
    }));
  }
}