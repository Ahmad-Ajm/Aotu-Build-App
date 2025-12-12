import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: '', redirectTo: '/cv', pathMatch: 'full' },
  {
    path: 'cv',
    loadChildren: () => import('./features/cv/cv.module').then(m => m.CVModule)
  },
  { path: '**', redirectTo: '/cv' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }