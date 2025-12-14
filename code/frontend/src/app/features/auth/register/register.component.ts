import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  isLoading = false;
  hidePassword = true;
  hideConfirmPassword = true;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.registerForm = this.fb.group({
      fullName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
      email: ['', [Validators.required, Validators.email, Validators.maxLength(255)]],
      userName: ['', [Validators.maxLength(100)]],
      password: ['', [
        Validators.required,
        Validators.minLength(8),
        Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$/)
      ]],
      confirmPassword: ['', [Validators.required]],
      dateOfBirth: [''],
      nationality: ['', [Validators.maxLength(100)]],
      acceptTerms: [false, [Validators.requiredTrue]]
    }, { validator: this.passwordMatchValidator });
  }

  ngOnInit(): void {
    // إذا كان المستخدم مسجل دخول بالفعل، توجيهه إلى الصفحة الرئيسية
    if (this.authService.isLoggedIn()) {
      this.router.navigate(['/']);
    }
  }

  // التحقق من مطابقة كلمة المرور
  passwordMatchValidator(form: FormGroup) {
    const password = form.get('password')?.value;
    const confirmPassword = form.get('confirmPassword')?.value;
    
    if (password !== confirmPassword) {
      form.get('confirmPassword')?.setErrors({ mismatch: true });
      return { mismatch: true };
    }
    return null;
  }

  // إرسال نموذج التسجيل
  onSubmit(): void {
    if (this.registerForm.invalid) {
      this.markFormGroupTouched(this.registerForm);
      return;
    }

    this.isLoading = true;
    
    const registerData = {
      fullName: this.registerForm.value.fullName,
      email: this.registerForm.value.email,
      userName: this.registerForm.value.userName || this.registerForm.value.email,
      password: this.registerForm.value.password,
      confirmPassword: this.registerForm.value.confirmPassword,
      dateOfBirth: this.registerForm.value.dateOfBirth,
      nationality: this.registerForm.value.nationality
    };

    this.authService.register(registerData).subscribe({
      next: (response) => {
        this.isLoading = false;
        
        if (response.success) {
          this.snackBar.open('تم إنشاء الحساب بنجاح!', 'حسناً', {
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
        console.error('Registration error:', error);
        
        this.snackBar.open('حدث خطأ أثناء إنشاء الحساب. يرجى المحاولة مرة أخرى.', 'حسناً', {
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
      const control = this.registerForm.get(key);
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
    const control = this.registerForm.get(controlName);
    
    if (!control || !control.errors || !control.touched) {
      return '';
    }

    if (control.hasError('required')) {
      return 'هذا الحقل مطلوب';
    }

    if (control.hasError('email')) {
      return 'تنسيق البريد الإلكتروني غير صحيح';
    }

    if (control.hasError('minlength')) {
      const requiredLength = control.errors['minlength'].requiredLength;
      return `يجب أن يكون ${requiredLength} أحرف على الأقل`;
    }

    if (control.hasError('maxlength')) {
      const requiredLength = control.errors['maxlength'].requiredLength;
      return `يجب أن لا يتجاوز ${requiredLength} حرف`;
    }

    if (control.hasError('pattern')) {
      if (controlName === 'password') {
        return 'كلمة المرور يجب أن تحتوي على حرف كبير وصغير ورقم';
      }
      return 'التنسيق غير صحيح';
    }

    if (control.hasError('mismatch')) {
      return 'كلمة المرور وتأكيدها غير متطابقتين';
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
}