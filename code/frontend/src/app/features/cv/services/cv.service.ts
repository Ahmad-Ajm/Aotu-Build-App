import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CVDto, CreateCVDto, UpdateCVDto, ExportOptionsDto, CVStatisticsDto } from '../models/cv.model';

@Injectable({
  providedIn: 'root'
})
export class CVService {
  private apiUrl = '/api/app/cv';

  constructor(private http: HttpClient) { }

  // إنشاء سيرة ذاتية جديدة
  createCV(cvData: CreateCVDto): Observable<CVDto> {
    return this.http.post<CVDto>(this.apiUrl, cvData);
  }

  // تحديث سيرة ذاتية موجودة
  updateCV(id: string, cvData: UpdateCVDto): Observable<CVDto> {
    return this.http.put<CVDto>(`${this.apiUrl}/${id}`, cvData);
  }

  // الحصول على سيرة ذاتية
  getCV(id: string): Observable<CVDto> {
    return this.http.get<CVDto>(`${this.apiUrl}/${id}`);
  }

  // الحصول على جميع سير المستخدم الذاتية
  getUserCVs(userId: string): Observable<CVDto[]> {
    return this.http.get<CVDto[]>(`${this.apiUrl}/user/${userId}`);
  }

  // الحصول على سيرة ذاتية عبر رابط المشاركة
  getCVByShareLink(shareLink: string): Observable<CVDto> {
    return this.http.get<CVDto>(`${this.apiUrl}/share/${shareLink}`);
  }

  // حذف سيرة ذاتية
  deleteCV(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  // تبديل حالة رؤية السيرة الذاتية
  toggleCVVisibility(id: string): Observable<CVDto> {
    return this.http.post<CVDto>(`${this.apiUrl}/${id}/toggle-visibility`, {});
  }

  // زيادة عدد مشاهدات السيرة الذاتية
  incrementCVViewCount(id: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${id}/increment-view`, {});
  }

  // تصدير إلى PDF
  exportToPdf(id: string, options: ExportOptionsDto): Observable<Blob> {
    return this.http.post(`${this.apiUrl}/${id}/export/pdf`, options, {
      responseType: 'blob'
    });
  }

  // تصدير إلى Word
  exportToWord(id: string, options: ExportOptionsDto): Observable<Blob> {
    return this.http.post(`${this.apiUrl}/${id}/export/word`, options, {
      responseType: 'blob'
    });
  }

  // نسخ سيرة ذاتية
  duplicateCV(id: string, newTitle?: string): Observable<CVDto> {
    return this.http.post<CVDto>(`${this.apiUrl}/${id}/duplicate`, { newTitle });
  }

  // البحث في السير الذاتية
  searchCVs(searchTerm: string, userId?: string): Observable<CVDto[]> {
    let url = `${this.apiUrl}/search?searchTerm=${encodeURIComponent(searchTerm)}`;
    if (userId) {
      url += `&userId=${userId}`;
    }
    return this.http.get<CVDto[]>(url);
  }

  // الحصول على إحصائيات السيرة الذاتية
  getCVStatistics(id: string): Observable<CVStatisticsDto> {
    return this.http.get<CVStatisticsDto>(`${this.apiUrl}/${id}/statistics`);
  }

  // الحصول على السير الذاتية العامة
  getPublicCVs(skip: number = 0, take: number = 10): Observable<CVDto[]> {
    return this.http.get<CVDto[]>(`${this.apiUrl}/public?skip=${skip}&take=${take}`);
  }

  // الحصول على عدد سير المستخدم الذاتية
  getUserCVCount(userId: string): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/user/${userId}/count`);
  }

  // التحقق من صلاحية الوصول إلى سيرة ذاتية
  checkAccess(userId: string, cvId: string): Observable<boolean> {
    return this.http.get<boolean>(`${this.apiUrl}/check-access?userId=${userId}&cvId=${cvId}`);
  }

  // تحديث آخر تحديث للسيرة الذاتية
  updateLastUpdated(id: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${id}/update-last-updated`, {});
  }
}