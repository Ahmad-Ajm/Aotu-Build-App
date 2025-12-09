قالب
# API Endpoint Template

قالب لتوثيق نقطة نهاية API جديدة.

---

## [HTTP METHOD] /api/[module]/[resource]

**الوصف**: [وصف مختصر لما يفعله هذا Endpoint]

**Module**: [اسم الـ Module في ABP]

---

### Request

**Headers**:
| Header | Required | Description |
|--------|----------|-------------|
| Authorization | Yes | Bearer token |
| Content-Type | Yes | application/json |
| Accept-Language | No | ar / en (default: ar) |

**Path Parameters**:
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| [param] | Guid/int | Yes | [الوصف] |

**Query Parameters**:
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| [param1] | string | No | null | [الوصف] |
| skipCount | int | No | 0 | Pagination |
| maxResultCount | int | No | 10 | Pagination (max: 1000) |
| sorting | string | No | id DESC | Sorting |

**Request Body** (للـ POST/PUT):
```json
{
  "[property1]": "[value]",
  "[property2]": "[value]"
}
```

**Request Body Schema**:
| Property | Type | Required | Max Length | Validation |
|----------|------|----------|------------|------------|
| [property1] | string | Yes | 200 | [القاعدة] |
| [property2] | int | No | - | Min: 0, Max: 100 |

---

### Response

**Success (200/201/204)**:
```json
{
  "id": "guid",
  "[property1]": "value",
  "[property2]": value
}
```

**Response Schema**:
| Property | Type | Description |
|----------|------|-------------|
| id | Guid | المعرف الفريد |
| [property1] | string | [الوصف] |

---

### Error Responses

| Status Code | Description | Example Response |
|-------------|-------------|------------------|
| 400 | Bad Request - Validation Error | `{"error": {"code": "ValidationError", "message": "...", "validationErrors": [...]}}` |
| 401 | Unauthorized - Invalid/Missing Token | `{"error": {"code": "Unauthorized", "message": "..."}}` |
| 403 | Forbidden - Insufficient Permissions | `{"error": {"code": "Forbidden", "message": "..."}}` |
| 404 | Not Found | `{"error": {"code": "NotFound", "message": "..."}}` |
| 409 | Conflict - Business Rule Violation | `{"error": {"code": "BusinessException", "message": "..."}}` |
| 500 | Internal Server Error | `{"error": {"code": "InternalError", "message": "..."}}` |

---

### Permissions Required

- `[ModuleName].[Resource].[Action]`

---

### Business Rules

1. [قاعدة عمل 1]
2. [قاعدة عمل 2]
3. [قاعدة عمل 3]

---

### Examples

**Example Request**:
```bash
curl -X [METHOD] "https://api.example.com/api/[module]/[resource]" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{
    "[property]": "[value]"
  }'
```

**Example Success Response**:
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "[property]": "value"
}
```

**Example Error Response (400)**:
```json
{
  "error": {
    "code": "ValidationError",
    "message": "One or more validation errors occurred.",
    "validationErrors": [
      {
        "message": "[Property] is required",
        "members": ["[Property]"]
      }
    ]
  }
}
```

---

### Notes

- [ملاحظة 1]
- [ملاحظة 2]

---

### Implementation Checklist

- [ ] Application Service method created
- [ ] Input DTO validation implemented
- [ ] Output DTO mapping configured
- [ ] Permissions applied
- [ ] Business rules enforced
- [ ] Domain events raised (if applicable)
- [ ] Unit tests written
- [ ] Integration tests written
- [ ] API documentation (Swagger) verified

