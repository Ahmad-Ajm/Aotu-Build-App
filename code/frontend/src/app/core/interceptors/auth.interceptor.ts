import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, BehaviorSubject } from 'rxjs';
import { catchError, filter, take, switchMap } from 'rxjs/operators';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  private isRefreshing = false;
  private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // إضافة التوكن إلى الطلبات التي تتطلب مصادقة
    const token = this.authService.getToken();
    if (token && this.shouldAddToken(request)) {
      request = this.addToken(request, token);
    }

    return next.handle(request).pipe(
      catchError(error => {
        if (error instanceof HttpErrorResponse && error.status === 401) {
          return this.handle401Error(request, next);
        } else if (error instanceof HttpErrorResponse && error.status === 403) {
          return this.handle403Error(error);
        } else {
          return throwError(() => error);
        }
      })
    );
  }

  // التحقق مما إذا كان يجب إضافة التوكن
  private shouldAddToken(request: HttpRequest<any>): boolean {
    // استثناء طلبات المصادقة من إضافة التوكن
    const authUrls = ['/api/auth/login', '/api/auth/register', '/api/auth/refresh-token'];
    return !authUrls.some(url => request.url.includes(url));
  }

  // إضافة التوكن إلى الطلب
  private addToken(request: HttpRequest<any>, token: string): HttpRequest<any> {
    return request.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  // معالجة خطأ 401 (غير مصرح)
  private handle401Error(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.refreshTokenSubject.next(null);

      return this.authService.refreshToken().pipe(
        switchMap((response: any) => {
          this.isRefreshing = false;
          this.refreshTokenSubject.next(response.data.accessToken);
          
          // إعادة الطلب مع التوكن الجديد
          return next.handle(this.addToken(request, response.data.accessToken));
        }),
        catchError((error) => {
          this.isRefreshing = false;
          
          // إذا فشل تحديث التوكن، تسجيل الخروج
          this.authService.logout().subscribe();
          this.router.navigate(['/auth/login']);
          
          return throwError(() => error);
        })
      );
    } else {
      // انتظار انتهاء عملية التحديث ثم إعادة الطلب
      return this.refreshTokenSubject.pipe(
        filter(token => token != null),
        take(1),
        switchMap(token => {
          return next.handle(this.addToken(request, token));
        })
      );
    }
  }

  // معالجة خطأ 403 (ممنوع)
  private handle403Error(error: HttpErrorResponse): Observable<never> {
    // عرض رسالة للمستخدم
    console.error('Access forbidden:', error);
    
    // توجيه المستخدم إلى الصفحة الرئيسية
    this.router.navigate(['/']);
    
    return throwError(() => error);
  }
}