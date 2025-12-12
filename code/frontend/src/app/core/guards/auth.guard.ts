import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { AuthService } from '../services/auth.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    
    // التحقق مما إذا كان المستخدم مسجل دخول
    if (!this.authService.isLoggedIn()) {
      this.showLoginRequiredMessage();
      return this.router.createUrlTree(['/auth/login'], {
        queryParams: { returnUrl: state.url }
      });
    }

    // التحقق من صلاحية التوكن
    const token = this.authService.getToken();
    if (token && this.authService.isTokenExpired(token)) {
      // محاولة تحديث التوكن
      return this.authService.refreshToken().pipe(
        map(response => {
          if (response.success) {
            return true;
          } else {
            this.showSessionExpiredMessage();
            return this.router.createUrlTree(['/auth/login'], {
              queryParams: { returnUrl: state.url }
            });
          }
        }),
        catchError(() => {
          this.showSessionExpiredMessage();
          return of(this.router.createUrlTree(['/auth/login'], {
            queryParams: { returnUrl: state.url }
          }));
        })
      );
    }

    // التحقق من الصلاحيات الإضافية إذا كانت موجودة
    const requiredRoles = route.data['roles'] as Array<string>;
    if (requiredRoles && requiredRoles.length > 0) {
      const user = this.authService.currentUserValue;
      if (!user || !this.hasRequiredRole(user, requiredRoles)) {
        this.showAccessDeniedMessage();
        return this.router.createUrlTree(['/']);
      }
    }

    return true;
  }

  // التحقق من وجود الصلاحية المطلوبة
  private hasRequiredRole(user: any, requiredRoles: string[]): boolean {
    // TODO: تنفيذ منطق التحقق من الصلاحيات عند إضافة نظام الصلاحيات
    // حالياً، جميع المستخدمين لديهم نفس الصلاحيات
    return true;
  }

  // عرض رسالة مطلوب تسجيل الدخول
  private showLoginRequiredMessage(): void {
    this.snackBar.open('يجب تسجيل الدخول للوصول إلى هذه الصفحة', 'حسناً', {
      duration: 3000,
      horizontalPosition: 'center',
      verticalPosition: 'top'
    });
  }

  // عرض رسالة انتهاء صلاحية الجلسة
  private showSessionExpiredMessage(): void {
    this.snackBar.open('انتهت صلاحية جلسة العمل. يرجى تسجيل الدخول مرة أخرى', 'حسناً', {
      duration: 3000,
      horizontalPosition: 'center',
      verticalPosition: 'top'
    });
  }

  // عرض رسالة رفض الوصول
  private showAccessDeniedMessage(): void {
    this.snackBar.open('ليس لديك صلاحية للوصول إلى هذه الصفحة', 'حسناً', {
      duration: 3000,
      horizontalPosition: 'center',
      verticalPosition: 'top'
    });
  }
}