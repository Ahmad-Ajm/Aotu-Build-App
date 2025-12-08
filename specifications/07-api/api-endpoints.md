# مواصفات API المفصلة

هذا الملف يوثق جميع نقاط النهاية (Endpoints) في التطبيق وفق معايير RESTful API.

---

## معايير عامة

### Base URL
```
Production: https://api.example.com
Staging: https://staging-api.example.com
Development: https://localhost:44300
```

### Authentication
- **نوع المصادقة**: Bearer Token (JWT)
- **Header**: `Authorization: Bearer {token}`
- **Token Lifetime**: 1 hour (Refresh Token: 30 days)

### Request/Response Format
- **Content-Type**: `application/json`
- **Character Encoding**: UTF-8
- **Date Format**: ISO 8601 (`2025-01-13T10:30:00Z`)

### Pagination
جميع قوائم الكيانات تدعم Pagination:
```json
{
  "skipCount": 0,
  "maxResultCount": 10
}
```

### Sorting
```
?sorting=fieldName ASC
?sorting=fieldName DESC
?sorting=field1 ASC, field2 DESC
```

### Filtering
```
?filter=searchText
?[propertyName]=value
```

---

## قالب توثيق Endpoint

### [HTTP METHOD] /api/[module]/[resource]

**الوصف**: [وصف مختصر لما يفعله هذا Endpoint]

**Module**: [اسم الـ Module في ABP]

**Permissions Required**:
- `[ModuleName].[Resource].[Action]`

**Request**:

**Headers**:
| Header | Required | Description |
|--------|----------|-------------|
| Authorization | Yes | Bearer token |
| Content-Type | Yes | application/json |
| Accept-Language | No | ar / en (default: ar) |

**Path Parameters**:
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| [param] | [type] | Yes/No | [الوصف] |

**Query Parameters**:
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| [param] | [type] | Yes/No | [value] | [الوصف] |

**Request Body**:
```json
{
  "[property]": "[type/value]",
  "[property2]": "[type/value]"
}
```

**Request Body Schema**:
| Property | Type | Required | Max Length | Validation |
|----------|------|----------|------------|------------|
| [property] | [type] | Yes/No | [length] | [القاعدة] |

**Response**:

**Success (200/201/204)**:
```json
{
  "[property]": "[value]",
  "[property2]": "[value]"
}
```

**Response Schema**:
| Property | Type | Description |
|----------|------|-------------|
| [property] | [type] | [الوصف] |

**Error Responses**:
| Status Code | Description | Example |
|-------------|-------------|---------|
| 400 | Bad Request - Validation Error | `{"error": {"code": "ValidationError", "message": "...", "validationErrors": [...]}}` |
| 401 | Unauthorized - Invalid/Missing Token | `{"error": {"code": "Unauthorized", "message": "..."}}` |
| 403 | Forbidden - Insufficient Permissions | `{"error": {"code": "Forbidden", "message": "..."}}` |
| 404 | Not Found | `{"error": {"code": "NotFound", "message": "..."}}` |
| 409 | Conflict - Business Rule Violation | `{"error": {"code": "BusinessException", "message": "..."}}` |
| 500 | Internal Server Error | `{"error": {"code": "InternalError", "message": "..."}}` |

**Business Rules**:
1. [قاعدة عمل 1]
2. [قاعدة عمل 2]

**Notes**:
- [ملاحظة 1]
- [ملاحظة 2]

**Examples**:

**Example Request**:
```bash
curl -X [METHOD] "https://api.example.com/api/[module]/[resource]" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{
    "[property]": "[value]"
  }'
```

**Example Response**:
```json
{
  "[property]": "[value]"
}
```

---

## CRUD Endpoints Template

معظم الموديولات تتبع نمط CRUD القياسي:

### GET /api/app/[resource]
**الوصف**: جلب قائمة [الموارد] مع فلترة وترتيب

**Query Parameters**:
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| filter | string | No | null | بحث نصي عام |
| skipCount | int | No | 0 | عدد العناصر المتجاوزة (للصفحات) |
| maxResultCount | int | No | 10 | الحد الأقصى للنتائج (max: 1000) |
| sorting | string | No | id DESC | ترتيب النتائج |

**Response (200)**:
```json
{
  "totalCount": 0,
  "items": []
}
```

### GET /api/app/[resource]/{id}
**الوصف**: جلب تفاصيل [مورد] واحد بالمعرف

**Path Parameters**:
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| id | Guid/int | Yes | معرف [المورد] |

**Response (200)**:
```json
{
  "id": "...",
  "[properties]": "..."
}
```

### POST /api/app/[resource]
**الوصف**: إنشاء [مورد] جديد

**Request Body**: CreateDto
**Response (201)**: [المورد] المُنشأ

### PUT /api/app/[resource]/{id}
**الوصف**: تحديث [مورد] موجود

**Path Parameters**: id
**Request Body**: UpdateDto
**Response (200)**: [المورد] المُحدّث

### DELETE /api/app/[resource]/{id}
**الوصف**: حذف [مورد]

**Path Parameters**: id
**Response (204)**: No Content

---

## Batch Operations

### POST /api/app/[resource]/batch
**الوصف**: عمليات جماعية (إنشاء/تحديث/حذف دفعة)

**Request Body**:
```json
{
  "operations": [
    {"operation": "create", "data": {...}},
    {"operation": "update", "id": "...", "data": {...}},
    {"operation": "delete", "id": "..."}
  ]
}
```

**Response (200)**:
```json
{
  "results": [
    {"operation": "create", "success": true, "id": "..."},
    {"operation": "update", "success": false, "error": "..."}
  ]
}
```

---

## File Upload Endpoints

### POST /api/app/files/upload
**الوصف**: رفع ملف واحد

**Request**:
- **Content-Type**: `multipart/form-data`
- **Body**:
  - `file`: [الملف]
  - `metadata`: JSON (optional)

**Response (201)**:
```json
{
  "id": "...",
  "fileName": "...",
  "fileSize": 0,
  "contentType": "...",
  "url": "..."
}
```

### POST /api/app/files/upload-multiple
**الوصف**: رفع ملفات متعددة

**Request**:
- **Content-Type**: `multipart/form-data`
- **Body**: `files[]`: [قائمة الملفات]

**Response (201)**:
```json
{
  "files": [
    {"id": "...", "fileName": "...", "url": "..."}
  ]
}
```

---

## Export Endpoints

### GET /api/app/[resource]/export
**الوصف**: تصدير البيانات

**Query Parameters**:
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| format | string | No | excel | excel / csv / pdf |
| filter | string | No | null | فلتر البيانات |

**Response (200)**:
- **Content-Type**: `application/vnd.openxmlformats-officedocument.spreadsheetml.sheet` (Excel)
- **Content-Disposition**: `attachment; filename="export.xlsx"`

---

## Webhooks

### POST /api/webhooks/subscribe
**الوصف**: الاشتراك في webhook

**Request Body**:
```json
{
  "url": "https://example.com/webhook",
  "events": ["[Resource].Created", "[Resource].Updated"],
  "secret": "..."
}
```

### Webhook Payload Format
```json
{
  "eventType": "[Resource].Created",
  "timestamp": "2025-01-13T10:30:00Z",
  "data": {...},
  "signature": "..."
}
```

---

## Real-time Endpoints (SignalR)

### Connection
```
ws://api.example.com/signalr/[hubName]
```

### Hub Methods
| Method | Parameters | Description |
|--------|------------|-------------|
| [MethodName] | [params] | [الوصف] |

### Client Events
| Event | Payload | Description |
|-------|---------|-------------|
| [EventName] | {...} | [الوصف] |

---

## Rate Limiting

| Endpoint Category | Rate Limit | Window |
|-------------------|------------|--------|
| Anonymous | 10 requests | 1 minute |
| Authenticated | 100 requests | 1 minute |
| Premium | 1000 requests | 1 minute |

**Rate Limit Headers**:
```
X-Rate-Limit-Limit: 100
X-Rate-Limit-Remaining: 95
X-Rate-Limit-Reset: 1673611200
```

---

## Versioning

### URL Versioning
```
/api/v1/[resource]
/api/v2/[resource]
```

### Header Versioning
```
X-API-Version: 1
X-API-Version: 2
```

---

## Health Check Endpoints

### GET /health
**الوصف**: فحص صحة التطبيق

**Response (200)**:
```json
{
  "status": "Healthy",
  "totalDuration": "00:00:00.123",
  "entries": {
    "database": {"status": "Healthy"},
    "redis": {"status": "Healthy"}
  }
}
```

---

## OpenAPI / Swagger

### Swagger UI
```
https://api.example.com/swagger
```

### OpenAPI Specification
```
https://api.example.com/swagger/v1/swagger.json
```

---

## Best Practices

1. **استخدام HTTP Methods الصحيحة**:
   - GET: قراءة
   - POST: إنشاء
   - PUT: تحديث كامل
   - PATCH: تحديث جزئي
   - DELETE: حذف

2. **Status Codes**:
   - 2xx: نجاح
   - 4xx: خطأ من العميل
   - 5xx: خطأ من الخادم

3. **Naming Conventions**:
   - استخدام اسم الجمع للموارد: `/users` وليس `/user`
   - استخدام kebab-case في URLs
   - استخدام camelCase في JSON properties

4. **Security**:
   - HTTPS فقط
   - التحقق من الصلاحيات في كل request
   - تنظيف المدخلات (Input Sanitization)
   - Rate Limiting

5. **Performance**:
   - استخدام Caching عند الحاجة
   - Pagination إجبارية للقوائم
   - Compression (gzip)
   - Async/Await

---

## ملاحظات التنفيذ

- جميع Endpoints مبنية باستخدام ABP Framework
- استخدام ABP Application Services
- التحقق التلقائي (Auto Validation) باستخدام Data Annotations
- Authorization باستخدام ABP Authorization
- Swagger documentation مُولّد تلقائياً
- استخدام ABP BlobStoring لرفع الملفات
