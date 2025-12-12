import { Routes } from '@angular/router';
import { CVListComponent } from './cv-list/cv-list.component';
import { CVEditorComponent } from './cv-editor/cv-editor.component';

export const cvRoutes: Routes = [
  {
    path: '',
    component: CVListComponent,
    data: { title: 'سيري الذاتية' }
  },
  {
    path: 'create',
    component: CVEditorComponent,
    data: { title: 'إنشاء سيرة ذاتية', mode: 'create' }
  },
  {
    path: 'edit/:id',
    component: CVEditorComponent,
    data: { title: 'تحرير السيرة الذاتية', mode: 'edit' }
  },
  {
    path: 'view/:id',
    component: CVEditorComponent,
    data: { title: 'عرض السيرة الذاتية', mode: 'view' }
  },
  {
    path: 'public',
    component: CVListComponent,
    data: { title: 'السير الذاتية العامة', mode: 'public' }
  }
];