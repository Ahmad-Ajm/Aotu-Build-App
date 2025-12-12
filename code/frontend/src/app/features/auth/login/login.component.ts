import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  isLoading = false;
  hidePassword = true;
  showForgotPassword = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.loginForm = this.fb.group({
      emailOrUserName: ['', [Validators.required, Validators.maxLength(255)]],
      password: ['', [Validators.required]],
      rememberMe: [false]
    });
  }

  ngOnInit(): void {
    // إذا كان المستخدم مسجل دخول بالفعل، توجيهه إلى الصفحة الرئيسية
    if (this.authService.isLoggedIn()) {
      this.router.navigate(['/']);
    }
    
    // محاولة تحميل بيانات تسجيل الدخول المحفوظة
    this.loadSavedCredentials();
  }

  // تحميل بيانات تسجيل الدخول المحفوظة
  private loadSavedCredentials(): void {
    const savedEmail = localStorage.getItem('rememberedEmail');
    if (savedEmail) {
      this.loginForm.patchValue({
        emailOrUserName: savedEmail,
        rememberMe: true
      });
    }
  }

  // إرسال نموذج تسجيل الدخول
  onSubmit(): void {
    if (this.loginForm.invalid) {
      this.markFormGroupTouched(this.loginForm);
      return;
    }

    this.isLoading = true;
    
    const loginData = {
      emailOrUserName: this.loginForm.value.emailOrUserName,
      password: this.loginForm.value.password,
      rememberMe: this.loginForm.value.rememberMe
    };

    this.authService.login(loginData).subscribe({
      next: (response) => {
        this.isLoading = false;
        
        if (response.success) {
          // حفظ البريد الإلكتروني إذا طلب المستخدم التذكر
          if (loginData.rememberMe) {
            localStorage.setItem('rememberedEmail', loginData.emailOrUserName);
          } else {
            localStorage.removeItem('rememberedEmail');
          }

          this.snackBar.open('تم تسجيل الدخول بنجاح!', 'حسناً', {
            duration: 3000,
            horizontalPosition: 'center',
            verticalPosition: 'top'
          });
          
          // توجيه المستخدم إلى الصفحة الرئيسية
          this.router.navigate(['/']);
        } else {
          this.handleErrors(response.errors);
        }
      },
      error: (error) => {
        this.isLoading = false;
        console.error('Login error:', error);
        
        let errorMessage = 'حدث خطأ أثناء تسجيل الدخول. يرجى المحاولة مرة أخرى.';
        
        if (error.status === 401) {
          errorMessage = 'البريد الإلكتروني أو كلمة المرور غير صحيحة';
        } else if (error.status === 423) {
          errorMessage = 'الحساب مقفل مؤقتاً. يرجى المحاولة لاحقاً';
        }
        
        this.snackBar.open(errorMessage, 'حسناً', {
          duration: 5000,
          horizontalPosition: 'center',
          verticalPosition: 'top'
        });
      }
    });
  }

  // معالجة أخطاء التحقق من الصحة
  private handleErrors(errors: any): void {
    Object.keys(errors).forEach(key => {
      const control = this.loginForm.get(key);
      if (control) {
        control.setErrors({ serverError: errors[key][0] });
        control.markAsTouched();
      }
    });
  }

  // وضع علامة على جميع الحقول كملموسة
  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.values(formGroup.controls).forEach(control => {
      control.markAsTouched();
      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }

  // الحصول على رسائل الخطأ
  getErrorMessage(controlName: string): string {
    const control = this.loginForm.get(controlName);
    
    if (!control || !control.errors || !control.touched) {
      return '';
    }

    if (control.hasError('required')) {
      return 'هذا الحقل مطلوب';
    }

    if (control.hasError('maxlength')) {
      const requiredLength = control.errors['maxlength'].requiredLength;
      return `يجب أن لا يتجاوز ${requiredLength} حرف`;
    }

    if (control.hasError('serverError')) {
      return control.errors['serverError'];
    }

    return '';
  }

  // الانتقال إلى صفحة التسجيل
  goToRegister(): void {
    this.router.navigate(['/auth/register']);
  }

  // الانتقال إلى صفحة استعادة كلمة المرور
  goToForgotPassword(): void {
    this.router.navigate(['/auth/forgot-password']);
  }

  // تبديل عرض/إخفاء كلمة المرور
  togglePasswordVisibility(): void {
    this.hidePassword = !this.hidePassword;
  }

  // تبديل عرض قسم استعادة كلمة المرور
  toggleForgotPassword(): void {
    this.showForgotPassword = !this.showForgotPassword;
  }

  // استعادة كلمة المرور
  onForgotPasswordSubmit(email: string): void {
    if (!email || !this.isValidEmail(email)) {
      this.snackBar.open('يرجى إدخال بريد إلكتروني صحيح', 'حسناً', {
        duration: 3000,
        horizontalPosition: 'center',
        verticalPosition: 'top'
      });
      return;
    }

    this.isLoading = true;
    
    this.authService.forgotPassword(email).subscribe({
      next: (response) => {
        this.isLoading = false;
        
        if (response.success) {
          this.snackBar.open('تم إرسال رابط استعادة كلمة المرور إلى بريدك الإلكتروني', 'حسناً', {
            duration: 5000,
            horizontalPosition: 'center',
            verticalPosition: 'top'
          });
          
          this.showForgotPassword = false;
        } else {
          this.snackBar.open(response.message || 'حدث خطأ أثناء إرسال رابط الاستعادة', 'حسناً', {
            duration: 5000,
            horizontalPosition: 'center',
            verticalPosition: 'top'
          });
        }
      },
      error: (error) => {
        this.isLoading = false;
        console.error('Forgot password error:', error);
        
        this.snackBar.open('حدث خطأ أثناء إرسال رابط الاستعادة. يرجى المحاولة مرة أخرى.', 'حسناً', {
          duration: 5000,
          horizontalPosition: 'center',
          verticalPosition: 'top'
        });
      }
    });
  }

  // التحقق من صحة البريد الإلكتروني
  private isValidEmail(email: string): boolean {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
  }
}