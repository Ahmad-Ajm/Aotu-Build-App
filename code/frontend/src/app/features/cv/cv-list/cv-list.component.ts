import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';
import { CVService } from '../services/cv.service';
import { CVDto } from '../models/cv.model';

@Component({
  selector: 'app-cv-list',
  template: `
    <div class="cv-list-container">
      <div class="header">
        <h1>{{ mode === 'public' ? 'السير الذاتية العامة' : 'سيري الذاتية' }}</h1>
        <button mat-raised-button color="primary" (click)="createNewCV()" *ngIf="mode !== 'public'">
          <mat-icon>add</mat-icon>
          إنشاء سيرة ذاتية جديدة
        </button>
      </div>

      <mat-form-field appearance="outline" class="search-field">
        <mat-label>بحث في السير الذاتية</mat-label>
        <input matInput [(ngModel)]="searchTerm" (keyup.enter)="searchCVs()" placeholder="اكتب للبحث...">
        <button matSuffix mat-icon-button (click)="searchCVs()">
          <mat-icon>search</mat-icon>
        </button>
      </mat-form-field>

      <div *ngIf="loading" class="loading">
        <mat-spinner diameter="40"></mat-spinner>
        <p>جاري تحميل السير الذاتية...</p>
      </div>

      <div *ngIf="!loading && cvs.length === 0" class="empty-state">
        <mat-icon>description</mat-icon>
        <h3>لا توجد سير ذاتية</h3>
        <p *ngIf="mode !== 'public'">ابدأ بإنشاء سيرتك الذاتية الأولى</p>
        <p *ngIf="mode === 'public'">لا توجد سير ذاتية عامة متاحة حالياً</p>
        <button mat-raised-button color="primary" (click)="createNewCV()" *ngIf="mode !== 'public'">
          إنشاء سيرة ذاتية
        </button>
      </div>

      <div class="cv-grid" *ngIf="!loading && cvs.length > 0">
        <mat-card *ngFor="let cv of cvs" class="cv-card">
          <mat-card-header>
            <mat-card-title>{{ cv.title }}</mat-card-title>
            <mat-card-subtitle>
              <span class="status" [class.public]="cv.isPublic" [class.private]="!cv.isPublic">
                {{ cv.isPublic ? 'عام' : 'خاص' }}
              </span>
              <span class="views" *ngIf="cv.viewCount">
                <mat-icon>visibility</mat-icon>
                {{ cv.viewCount }}
              </span>
            </mat-card-subtitle>
          </mat-card-header>
          
          <mat-card-content>
            <p class="last-updated">
              آخر تحديث: {{ cv.lastUpdated | date:'medium' }}
            </p>
            <div class="actions">
              <button mat-button color="primary" (click)="viewCV(cv.id)">
                <mat-icon>visibility</mat-icon>
                عرض
              </button>
              <button mat-button color="accent" (click)="editCV(cv.id)" *ngIf="mode !== 'public'">
                <mat-icon>edit</mat-icon>
                تحرير
              </button>
              <button mat-button color="warn" (click)="deleteCV(cv)" *ngIf="mode !== 'public'">
                <mat-icon>delete</mat-icon>
                حذف
              </button>
              <button mat-button (click)="toggleVisibility(cv)" *ngIf="mode !== 'public'">
                <mat-icon>{{ cv.isPublic ? 'lock' : 'public' }}</mat-icon>
                {{ cv.isPublic ? 'جعل خاص' : 'جعل عام' }}
              </button>
              <button mat-button (click)="duplicateCV(cv)" *ngIf="mode !== 'public'">
                <mat-icon>content_copy</mat-icon>
                نسخ
              </button>
            </div>
          </mat-card-content>
        </mat-card>
      </div>

      <div class="pagination" *ngIf="totalCount > pageSize">
        <button mat-button [disabled]="currentPage === 0" (click)="previousPage()">
          <mat-icon>chevron_right</mat-icon>
          السابق
        </button>
        <span>الصفحة {{ currentPage + 1 }} من {{ totalPages }}</span>
        <button mat-button [disabled]="currentPage >= totalPages - 1" (click)="nextPage()">
          التالي
          <mat-icon>chevron_left</mat-icon>
        </button>
      </div>
    </div>
  `,
  styles: [`
    .cv-list-container {
      max-width: 1200px;
      margin: 0 auto;
      padding: 20px;
    }
    
    .header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 20px;
    }
    
    .search-field {
      width: 100%;
      margin-bottom: 20px;
    }
    
    .loading {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding: 40px;
    }
    
    .empty-state {
      text-align: center;
      padding: 60px 20px;
      color: #666;
    }
    
    .empty-state mat-icon {
      font-size: 64px;
      width: 64px;
      height: 64px;
      margin-bottom: 20px;
      color: #ccc;
    }
    
    .cv-grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
      gap: 20px;
      margin-bottom: 20px;
    }
    
    .cv-card {
      transition: transform 0.2s, box-shadow 0.2s;
    }
    
    .cv-card:hover {
      transform: translateY(-4px);
      box-shadow: 0 8px 16px rgba(0,0,0,0.1);
    }
    
    .status {
      padding: 2px 8px;
      border-radius: 12px;
      font-size: 12px;
      font-weight: 500;
    }
    
    .status.public {
      background-color: #e8f5e9;
      color: #2e7d32;
    }
    
    .status.private {
      background-color: #f5f5f5;
      color: #616161;
    }
    
    .views {
      display: inline-flex;
      align-items: center;
      margin-left: 10px;
      color: #666;
      font-size: 12px;
    }
    
    .views mat-icon {
      font-size: 16px;
      width: 16px;
      height: 16px;
      margin-right: 4px;
    }
    
    .last-updated {
      color: #666;
      font-size: 12px;
      margin-bottom: 15px;
    }
    
    .actions {
      display: flex;
      flex-wrap: wrap;
      gap: 8px;
    }
    
    .actions button {
      font-size: 12px;
    }
    
    .pagination {
      display: flex;
      justify-content: center;
      align-items: center;
      gap: 20px;
      margin-top: 20px;
    }
    
    @media (max-width: 768px) {
      .header {
        flex-direction: column;
        gap: 10px;
        align-items: flex-start;
      }
      
      .cv-grid {
        grid-template-columns: 1fr;
      }
    }
  `]
})
export class CVListComponent implements OnInit {
  cvs: CVDto[] = [];
  loading = false;
  searchTerm = '';
  mode: 'user' | 'public' = 'user';
  currentPage = 0;
  pageSize = 10;
  totalCount = 0;
  totalPages = 0;

  constructor(
    private cvService: CVService,
    private router: Router,
    private snackBar: MatSnackBar,
    private dialog: MatDialog
  ) {}

  ngOnInit() {
    // TODO: Get mode from route data
    this.loadCVs();
  }

  loadCVs() {
    this.loading = true;
    
    if (this.mode === 'public') {
      this.cvService.getPublicCVs(this.currentPage * this.pageSize, this.pageSize)
        .subscribe({
          next: (cvs) => {
            this.cvs = cvs;
            this.loading = false;
          },
          error: (error) => {
            this.showError('حدث خطأ أثناء تحميل السير الذاتية');
            this.loading = false;
          }
        });
    } else {
      // TODO: Get current user ID
      const userId = 'current-user-id'; // Replace with actual user ID
      this.cvService.getUserCVs(userId)
        .subscribe({
          next: (cvs) => {
            this.cvs = cvs;
            this.loading = false;
          },
          error: (error) => {
            this.showError('حدث خطأ أثناء تحميل سيرك الذاتية');
            this.loading = false;
          }
        });
    }
  }

  searchCVs() {
    if (!this.searchTerm.trim()) {
      this.loadCVs();
      return;
    }

    this.loading = true;
    // TODO: Get current user ID
    const userId = this.mode === 'user' ? 'current-user-id' : undefined;
    
    this.cvService.searchCVs(this.searchTerm, userId)
      .subscribe({
        next: (cvs) => {
          this.cvs = cvs;
          this.loading = false;
        },
        error: (error) => {
          this.showError('حدث خطأ أثناء البحث');
          this.loading = false;
        }
      });
  }

  createNewCV() {
    this.router.navigate(['/cv/create']);
  }

  viewCV(id: string) {
    this.router.navigate(['/cv/view', id]);
  }

  editCV(id: string) {
    this.router.navigate(['/cv/edit', id]);
  }

  deleteCV(cv: CVDto) {
    if (confirm(`هل أنت متأكد من حذف السيرة الذاتية "${cv.title}"؟`)) {
      this.cvService.deleteCV(cv.id)
        .subscribe({
          next: () => {
            this.cvs = this.cvs.filter(c => c.id !== cv.id);
            this.showSuccess('تم حذف السيرة الذاتية بنجاح');
          },
          error: (error) => {
            this.showError('حدث خطأ أثناء حذف السيرة الذاتية');
          }
        });
    }
  }

  toggleVisibility(cv: CVDto) {
    this.cvService.toggleCVVisibility(cv.id)
      .subscribe({
        next: (updatedCV) => {
          const index = this.cvs.findIndex(c => c.id === cv.id);
          if (index !== -1) {
            this.cvs[index] = updatedCV;
          }
          this.showSuccess(`تم ${updatedCV.isPublic ? 'جعل السيرة الذاتية عامة' : 'جعل السيرة الذاتية خاصة'}`);
        },
        error: (error) => {
          this.showError('حدث خطأ أثناء تغيير حالة الرؤية');
        }
      });
  }

  duplicateCV(cv: CVDto) {
    const newTitle = prompt('أدخل عنواناً جديداً للسيرة الذاتية المنسوخة:', `${cv.title} (نسخة)`);
    if (newTitle) {
      this.cvService.duplicateCV(cv.id, newTitle)
        .subscribe({
          next: (newCV) => {
            this.cvs.unshift(newCV);
            this.showSuccess('تم نسخ السيرة الذاتية بنجاح');
          },
          error: (error) => {
            this.showError('حدث خطأ أثناء نسخ السيرة الذاتية');
          }
        });
    }
  }

  previousPage() {
    if (this.currentPage > 0) {
      this.currentPage--;
      this.loadCVs();
    }
  }

  nextPage() {
    if (this.currentPage < this.totalPages - 1) {
      this.currentPage++;
      this.loadCVs();
    }
  }

  private showSuccess(message: string) {
    this.snackBar.open(message, 'حسناً', {
      duration: 3000,
      horizontalPosition: 'left',
      verticalPosition: 'bottom'
    });
  }

  private showError(message: string) {
    this.snackBar.open(message, 'خطأ', {
      duration: 5000,
      horizontalPosition: 'left',
      verticalPosition: 'bottom',
      panelClass: ['error-snackbar']
    });
  }
}