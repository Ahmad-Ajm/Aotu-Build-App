import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit {
  forgotPasswordForm: FormGroup;
  isLoading = false;
  isSubmitted = false;
  emailSent = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.forgotPasswordForm = this.fb.group({
      email: ['', [Validators.required, Validators.email, Validators.maxLength(255)]]
    });
  }

  ngOnInit(): void {
    // إذا كان المستخدم مسجل دخول بالفعل، توجيهه إلى الصفحة الرئيسية
    if (this.authService.isLoggedIn()) {
      this.router.navigate(['/']);
    }
  }

  // إرسال نموذج استعادة كلمة المرور
  onSubmit(): void {
    if (this.forgotPasswordForm.invalid) {
      this.markFormGroupTouched(this.forgotPasswordForm);
      return;
    }

    this.isLoading = true;
    this.isSubmitted = true;

    const email = this.forgotPasswordForm.value.email;

    this.authService.forgotPassword(email).subscribe({
      next: (response) => {
        this.isLoading = false;
        
        if (response.success) {
          this.emailSent = true;
          
          this.snackBar.open('تم إرسال رابط إعادة تعيين كلمة المرور إلى بريدك الإلكتروني', 'حسناً', {
            duration: 5000,
            horizontalPosition: 'center',
            verticalPosition: 'top'
          });
        } else {
          this.snackBar.open(response.message || 'حدث خطأ أثناء إرسال رابط إعادة التعيين', 'حسناً', {
            duration: 5000,
            horizontalPosition: 'center',
            verticalPosition: 'top'
          });
        }
      },
      error: (error) => {
        this.isLoading = false;
        console.error('Forgot password error:', error);
        
        this.snackBar.open('حدث خطأ أثناء إرسال رابط إعادة التعيين. يرجى المحاولة مرة أخرى.', 'حسناً', {
          duration: 5000,
          horizontalPosition: 'center',
          verticalPosition: 'top'
        });
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

  // الحصول على رسالة الخطأ
  getErrorMessage(controlName: string): string {
    const control = this.forgotPasswordForm.get(controlName);
    
    if (!control || !control.errors || !control.touched) {
      return '';
    }

    if (control.hasError('required')) {
      return 'البريد الإلكتروني مطلوب';
    }

    if (control.hasError('email')) {
      return 'تنسيق البريد الإلكتروني غير صحيح';
    }

    if (control.hasError('maxlength')) {
      const requiredLength = control.errors['maxlength'].requiredLength;
      return `البريد الإلكتروني يجب أن لا يتجاوز ${requiredLength} حرف`;
    }

    if (control.hasError('serverError')) {
      return control.errors['serverError'];
    }

    return '';
  }

  // الانتقال إلى صفحة تسجيل الدخول
  goToLogin(): void {
    this.router.navigate(['/auth/login']);
  }

  // الانتقال إلى صفحة التسجيل
  goToRegister(): void {
    this.router.navigate(['/auth/register']);
  }

  // إعادة تعيين النموذج
  resetForm(): void {
    this.forgotPasswordForm.reset();
    this.isSubmitted = false;
    this.emailSent = false;
  }
}