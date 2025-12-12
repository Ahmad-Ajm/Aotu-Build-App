import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '../../../core/services/auth.service';
import { UserService } from '../../../core/services/user.service';
import { User } from '../../../core/models/user.model';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  profileForm: FormGroup;
  userProfileForm: FormGroup;
  isLoading = false;
  user: User | null = null;
  userProfile: any = null;
  activeTab = 'basic';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private userService: UserService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.profileForm = this.fb.group({
      fullName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
      email: ['', [Validators.required, Validators.email, Validators.maxLength(255)]],
      userName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
      dateOfBirth: [''],
      nationality: ['', [Validators.maxLength(100)]]
    });

    this.userProfileForm = this.fb.group({
      phoneNumber: ['', [Validators.maxLength(20)]],
      address: ['', [Validators.maxLength(500)]],
      profileImageUrl: ['', [Validators.maxLength(500)]],
      bio: ['', [Validators.maxLength(1000)]]
    });
  }

  ngOnInit(): void {
    this.loadUserData();
  }

  // تحميل بيانات المستخدم
  loadUserData(): void {
    this.isLoading = true;

    // تحميل بيانات المستخدم الأساسية
    this.authService.currentUser$.subscribe({
      next: (user) => {
        this.user = user;
        if (user) {
          this.profileForm.patchValue({
            fullName: user.fullName,
            email: user.email,
            userName: user.userName,
            dateOfBirth: user.dateOfBirth ? new Date(user.dateOfBirth) : null,
            nationality: user.nationality || ''
          });
        }
      },
      error: (error) => {
        console.error('Error loading user data:', error);
        this.showError('حدث خطأ أثناء تحميل بيانات المستخدم');
      }
    });

    // تحميل الملف الشخصي
    this.userService.getProfile().subscribe({
      next: (profile) => {
        this.userProfile = profile;
        if (profile) {
          this.userProfileForm.patchValue({
            phoneNumber: profile.phoneNumber || '',
            address: profile.address || '',
            profileImageUrl: profile.profileImageUrl || '',
            bio: profile.bio || ''
          });
        }
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading user profile:', error);
        this.isLoading = false;
        // لا نعرض خطأ هنا لأن الملف الشخصي قد لا يكون موجوداً بعد
      }
    });
  }

  // تحديث البيانات الأساسية
  onBasicInfoSubmit(): void {
    if (this.profileForm.invalid) {
      this.markFormGroupTouched(this.profileForm);
      return;
    }

    this.isLoading = true;
    const updateData = {
      fullName: this.profileForm.value.fullName,
      email: this.profileForm.value.email,
      userName: this.profileForm.value.userName,
      dateOfBirth: this.profileForm.value.dateOfBirth,
      nationality: this.profileForm.value.nationality
    };

    this.userService.updateProfile(updateData).subscribe({
      next: (updatedUser) => {
        this.isLoading = false;
        this.showSuccess('تم تحديث البيانات الأساسية بنجاح');
        this.authService.currentUserSubject.next(updatedUser);
      },
      error: (error) => {
        this.isLoading = false;
        console.error('Update error:', error);
        this.handleErrors(error);
      }
    });
  }

  // تحديث الملف الشخصي
  onProfileSubmit(): void {
    if (this.userProfileForm.invalid) {
      this.markFormGroupTouched(this.userProfileForm);
      return;
    }

    this.isLoading = true;
    const profileData = {
      phoneNumber: this.userProfileForm.value.phoneNumber,
      address: this.userProfileForm.value.address,
      profileImageUrl: this.userProfileForm.value.profileImageUrl,
      bio: this.userProfileForm.value.bio
    };

    this.userService.updateUserProfile(profileData).subscribe({
      next: (updatedProfile) => {
        this.isLoading = false;
        this.userProfile = updatedProfile;
        this.showSuccess('تم تحديث الملف الشخصي بنجاح');
      },
      error: (error) => {
        this.isLoading = false;
        console.error('Profile update error:', error);
        this.handleErrors(error);
      }
    });
  }

  // تغيير كلمة المرور
  onChangePassword(): void {
    this.router.navigate(['/auth/change-password']);
  }

  // تغيير التبويب النشط
  setActiveTab(tab: string): void {
    this.activeTab = tab;
  }

  // معالجة أخطاء التحقق من الصحة
  private handleErrors(error: any): void {
    if (error.errors) {
      Object.keys(error.errors).forEach(key => {
        const control = this.profileForm.get(key) || this.userProfileForm.get(key);
        if (control) {
          control.setErrors({ serverError: error.errors[key][0] });
          control.markAsTouched();
        }
      });
    } else {
      this.showError(error.message || 'حدث خطأ أثناء حفظ البيانات');
    }
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
  getErrorMessage(controlName: string, form: FormGroup = this.profileForm): string {
    const control = form.get(controlName);
    
    if (!control || !control.errors || !control.touched) {
      return '';
    }

    if (control.errors['required']) {
      return 'هذا الحقل مطلوب';
    }

    if (control.errors['email']) {
      return 'تنسيق البريد الإلكتروني غير صحيح';
    }

    if (control.errors['minlength']) {
      return `الحد الأدنى ${control.errors['minlength'].requiredLength} أحرف`;
    }

    if (control.errors['maxlength']) {
      return `الحد الأقصى ${control.errors['maxlength'].requiredLength} أحرف`;
    }

    if (control.errors['serverError']) {
      return control.errors['serverError'];
    }

    return 'قيمة غير صحيحة';
  }

  // عرض رسالة نجاح
  private showSuccess(message: string): void {
    this.snackBar.open(message, 'حسناً', {
      duration: 3000,
      horizontalPosition: 'center',
      verticalPosition: 'top'
    });
  }

  // عرض رسالة خطأ
  private showError(message: string): void {
    this.snackBar.open(message, 'حسناً', {
      duration: 5000,
      horizontalPosition: 'center',
      verticalPosition: 'top'
    });
  }

  // التحقق مما إذا كان الحقل به خطأ
  hasError(controlName: string, form: FormGroup = this.profileForm): boolean {
    const control = form.get(controlName);
    return control ? control.invalid && control.touched : false;
  }

  // الحصول على تاريخ الميلاد بصيغة مقروءة
  getFormattedDateOfBirth(): string {
    const date = this.profileForm.get('dateOfBirth')?.value;
    if (!date) {
      return 'غير محدد';
    }
    
    const dateObj = new Date(date);
    return dateObj.toLocaleDateString('ar-SA', {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  }

  // الحصول على صورة الملف الشخصي
  getProfileImage(): string {
    return this.userProfileForm.get('profileImageUrl')?.value || 'assets/images/default-avatar.png';
  }

  // التحقق مما إذا كان المستخدم مسجلاً حديثاً
  isNewUser(): boolean {
    if (!this.user?.creationTime) {
      return false;
    }
    
    const creationDate = new Date(this.user.creationTime);
    const thirtyDaysAgo = new Date();
    thirtyDaysAgo.setDate(thirtyDaysAgo.getDate() - 30);
    
    return creationDate > thirtyDaysAgo;
  }

  // الحصول على وقت التسجيل بصيغة مقروءة
  getFormattedCreationTime(): string {
    if (!this.user?.creationTime) {
      return 'غير معروف';
    }
    
    const creationDate = new Date(this.user.creationTime);
    return creationDate.toLocaleDateString('ar-SA', {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  }

  // الحصول على آخر تاريخ دخول بصيغة مقروءة
  getFormattedLastLogin(): string {
    if (!this.user?.lastLoginDate) {
      return 'لم يسجل دخول من قبل';
    }
    
    const lastLoginDate = new Date(this.user.lastLoginDate);
    const now = new Date();
    const diffInHours = Math.floor((now.getTime() - lastLoginDate.getTime()) / (1000 * 60 * 60));
    
    if (diffInHours < 1) {
      return 'منذ أقل من ساعة';
    } else if (diffInHours < 24) {
      return `منذ ${diffInHours} ساعة`;
    } else {
      const diffInDays = Math.floor(diffInHours / 24);
      return `منذ ${diffInDays} يوم`;
    }
  }
}