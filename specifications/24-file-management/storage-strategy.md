# File Management

إدارة الملفات (File Management) في النظام.

---

## Storage Strategy

### Storage Providers

#### Local File System
- **Use Case**: Development, Small Projects
- **Pros**: Simple, Fast, No Cost
- **Cons**: Not Scalable, Single Point of Failure
- **Path**: `/wwwroot/files/` or `/uploads/`

#### Azure Blob Storage
- **Use Case**: Production, Scalable
- **Pros**: Highly Available, CDN Integration, Scalable
- **Cons**: Cost, External Dependency
- **Container**: [container-name]

#### AWS S3
- **Use Case**: Production, Scalable
- **Pros**: Reliable, Global, Scalable
- **Cons**: Cost, External Dependency
- **Bucket**: [bucket-name]

#### Database (BLOB)
- **Use Case**: Small Files, Simple Setup
- **Pros**: Transactional, Backup with DB
- **Cons**: Performance, DB Size
- **Max Size**: 1 MB per file

---

## File Organization

### Folder Structure
```
/files/
├── /users/
│   ├── /{userId}/
│   │   ├── /profile-pictures/
│   │   └── /documents/
├── /[module]/
│   ├── /{entityId}/
│   │   ├── /attachments/
│   │   └── /images/
└── /temp/
```

### File Naming Convention
```
{entityType}_{entityId}_{timestamp}_{originalName}
Example: invoice_12345_20250113_receipt.pdf
```

---

## File Types & Limits

### Allowed File Types

| Category | Extensions | Max Size | Purpose |
|----------|-----------|----------|---------|
| Images | .jpg, .jpeg, .png, .gif, .webp | 5 MB | صور المستخدمين، المنتجات |
| Documents | .pdf, .doc, .docx, .xls, .xlsx | 10 MB | مستندات، تقارير |
| Videos | .mp4, .avi, .mov | 100 MB | فيديوهات تعليمية |
| Archives | .zip, .rar, .7z | 50 MB | ملفات مضغوطة |
| Other | .txt, .csv | 2 MB | ملفات نصية |

### Blocked File Types
- .exe, .bat, .cmd, .sh (Executable files)
- .js, .vbs, .ps1 (Script files)
- [قائمة إضافية حسب السياسة الأمنية]

---

## File Upload Process

### Upload Flow
```
1. Client selects file
2. Frontend validates (size, type, name)
3. POST to /api/files/upload
4. Backend validates (virus scan, type check)
5. Generate unique filename
6. Save to storage
7. Create FileMetadata record in DB
8. Return file URL
```

### Upload Endpoint
```
POST /api/app/files/upload
Content-Type: multipart/form-data

Response:
{
  "id": "guid",
  "fileName": "...",
  "fileSize": 123456,
  "contentType": "image/jpeg",
  "url": "https://..."
}
```

---

## File Metadata

```csharp
public class FileMetadata : FullAuditedAggregateRoot<Guid>
{
    public string FileName { get; set; }
    public string OriginalFileName { get; set; }
    public string ContentType { get; set; }
    public long FileSize { get; set; }
    public string StorageProvider { get; set; }
    public string StoragePath { get; set; }
    public string PublicUrl { get; set; }
    public string EntityType { get; set; }
    public string EntityId { get; set; }
    public FileCategory Category { get; set; }
    public bool IsPublic { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string UploadedBy { get; set; }
}
```

---

## File Access Control

### Public Files
- Accessible without authentication
- Example: Product images, Public documents

### Private Files
- Requires authentication
- Permission check before download
- Example: User documents, Invoices

### Temporary Files
- Auto-delete after X days
- Example: Export files, Generated reports

### Access URL Pattern
```
Public: /files/public/{fileId}
Private: /files/private/{fileId}?token={accessToken}
```

---

## File Processing

### Image Processing
- **Resize**: Generate thumbnails (150x150, 300x300)
- **Compress**: Reduce file size
- **Format Conversion**: Convert to WebP
- **Watermark**: Add logo/text

### Document Processing
- **Preview Generation**: PDF thumbnail
- **Text Extraction**: For search indexing
- **Virus Scan**: Before storage

### Video Processing
- **Thumbnail Generation**: First frame
- **Transcoding**: Different resolutions
- **Compression**: Reduce size

---

## CDN Integration

### Configuration
- **Provider**: Cloudflare / Azure CDN / AWS CloudFront
- **Cache Duration**: 1 day for images, 1 hour for documents
- **Purge Strategy**: On file update/delete

### URL Pattern
```
CDN: https://cdn.example.com/files/{fileId}
Origin: https://api.example.com/files/{fileId}
```

---

## File Cleanup

### Orphaned Files
- Files not linked to any entity
- **Cleanup Schedule**: Weekly
- **Grace Period**: 30 days

### Temporary Files
- Files in /temp/
- **Cleanup Schedule**: Daily
- **Max Age**: 24 hours

### Expired Files
- Files past ExpiryDate
- **Cleanup Schedule**: Daily

---

## Virus Scanning

### Integration
- **Provider**: ClamAV / Windows Defender / Cloud Service
- **Scan Timing**: Before storage
- **Action on Detection**: Reject upload, Log, Notify admin

---

## Backup Strategy

### Strategy
- Full Backup: Weekly
- Incremental: Daily
- Retention: 30 days

### Verification
- Monthly restore test
- File integrity check

---

## Best Practices

1. **Unique Names**: استخدام GUID أو Timestamp
2. **Validation**: Client + Server side
3. **Virus Scan**: قبل التخزين
4. **CDN**: للملفات العامة
5. **Cleanup**: حذف الملفات القديمة بانتظام
6. **Monitoring**: حجم التخزين، عدد الملفات

---

## ملاحظات التنفيذ

- استخدام ABP BlobStoring abstraction
- Background Job للمعالجة (resize, scan)
- Stream للملفات الكبيرة
- Chunk Upload للملفات الضخمة (> 100 MB)
- Rate Limiting للـ Upload

