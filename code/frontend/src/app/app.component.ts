import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  template: `
    <mat-toolbar color="primary">
      <button mat-icon-button (click)="sidenav.toggle()">
        <mat-icon>menu</mat-icon>
      </button>
      <span>نظام السير الذاتية</span>
      <span class="spacer"></span>
      <button mat-button routerLink="/cv/create">إنشاء سيرة ذاتية</button>
      <button mat-button routerLink="/cv">سيري الذاتية</button>
    </mat-toolbar>

    <mat-sidenav-container>
      <mat-sidenav #sidenav mode="side">
        <mat-nav-list>
          <a mat-list-item routerLink="/cv" routerLinkActive="active">
            <mat-icon>list</mat-icon>
            <span>سيري الذاتية</span>
          </a>
          <a mat-list-item routerLink="/cv/create" routerLinkActive="active">
            <mat-icon>add</mat-icon>
            <span>إنشاء سيرة ذاتية</span>
          </a>
          <a mat-list-item routerLink="/cv/public" routerLinkActive="active">
            <mat-icon>public</mat-icon>
            <span>السير الذاتية العامة</span>
          </a>
          <mat-divider></mat-divider>
          <a mat-list-item routerLink="/settings" routerLinkActive="active">
            <mat-icon>settings</mat-icon>
            <span>الإعدادات</span>
          </a>
        </mat-nav-list>
      </mat-sidenav>

      <mat-sidenav-content>
        <div class="container">
          <router-outlet></router-outlet>
        </div>
      </mat-sidenav-content>
    </mat-sidenav-container>
  `,
  styles: [`
    .spacer {
      flex: 1 1 auto;
    }
    
    .container {
      padding: 20px;
    }
    
    mat-sidenav {
      width: 250px;
    }
    
    mat-nav-list a.active {
      background-color: rgba(0, 0, 0, 0.04);
    }
    
    mat-nav-list mat-icon {
      margin-right: 8px;
    }
  `]
})
export class AppComponent {
  title = 'CV System';
}