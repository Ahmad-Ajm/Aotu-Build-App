import { Routes } from '@angular/router';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { ProfileComponent } from './profile/profile.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { AuthGuard } from '../../core/guards/auth.guard';

export const authRoutes: Routes = [
  {
    path: 'register',
    component: RegisterComponent,
    data: { title: 'إنشاء حساب جديد' }
  },
  {
    path: 'login',
    component: LoginComponent,
    data: { title: 'تسجيل الدخول' }
  },
  {
    path: 'forgot-password',
    component: ForgotPasswordComponent,
    data: { title: 'استعادة كلمة المرور' }
  },
  {
    path: 'profile',
    component: ProfileComponent,
    canActivate: [AuthGuard],
    data: { title: 'الملف الشخصي' }
  },
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  }
];