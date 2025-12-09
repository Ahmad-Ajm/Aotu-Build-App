قالب
# CQRS Handlers

معالجات الأوامر والاستعلامات (Command & Query Handlers).

---

## Command Handlers vs Query Handlers

| Aspect | Command Handler | Query Handler |
|--------|----------------|---------------|
| Purpose | تغيير الحالة | قراءة البيانات |
| Side Effects | Yes | No |
| Caching | No | Yes |
| Idempotency | مطلوب | تلقائي |
| Transactions | Yes | Optional |

---

## قالب Command Handler

```csharp
public class [CommandName]CommandHandler : 
    ITransientDependency
{
    private readonly IRepository<[Entity]> _repository;
    private readonly IDistributedEventBus _eventBus;
    
    public [CommandName]CommandHandler(
        IRepository<[Entity]> repository,
        IDistributedEventBus eventBus)
    {
        _repository = repository;
        _eventBus = eventBus;
    }
    
    public async Task<[ResultType]> ExecuteAsync(
        [CommandName]Command command)
    {
        // 1. Validate
        await ValidateAsync(command);
        
        // 2. Execute business logic
        var entity = await _repository.GetAsync(command.Id);
        entity.[DoSomething](command.Data);
        
        // 3. Save
        await _repository.UpdateAsync(entity);
        
        // 4. Raise events
        await _eventBus.PublishAsync(new [Event]Event { ... });
        
        // 5. Return result
        return new [ResultType] { ... };
    }
    
    private async Task ValidateAsync([CommandName]Command command)
    {
        // Validation logic
    }
}
```

---

## قالب Query Handler

```csharp
public class [QueryName]QueryHandler : 
    ITransientDependency
{
    private readonly IRepository<[Entity]> _repository;
    private readonly IDistributedCache _cache;
    
    public [QueryName]QueryHandler(
        IRepository<[Entity]> repository,
        IDistributedCache cache)
    {
        _repository = repository;
        _cache = cache;
    }
    
    public async Task<[ResultType]> ExecuteAsync(
        [QueryName]Query query)
    {
        // 1. Check cache
        var cacheKey = GetCacheKey(query);
        var cached = await _cache.GetAsync<[ResultType]>(cacheKey);
        if (cached != null) return cached;
        
        // 2. Query data
        var queryable = await _repository.GetQueryableAsync();
        var result = await queryable
            .Where([filter])
            .OrderBy([sorting])
            .Skip(query.SkipCount)
            .Take(query.MaxResultCount)
            .ToListAsync();
        
        // 3. Project to DTO
        var dto = ObjectMapper.Map<List<[Entity]>, [ResultType]>(result);
        
        // 4. Cache result
        await _cache.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(5));
        
        // 5. Return
        return dto;
    }
    
    private string GetCacheKey([QueryName]Query query)
    {
        return $"[Module]:[Entity]:{query.GetHashCode()}";
    }
}
```

---

## Handler Responsibilities

### Command Handler
1. Validate input
2. Check permissions
3. Execute business logic
4. Update database
5. Raise domain events
6. Return result/confirmation

### Query Handler
1. Check permissions
2. Check cache
3. Build query
4. Execute query
5. Project to DTO
6. Cache result
7. Return data

---

## Error Handling

```csharp
public async Task<Result> ExecuteAsync(Command command)
{
    try
    {
        // Logic
    }
    catch (BusinessException ex)
    {
        // Log and return business error
        _logger.LogWarning(ex, "Business rule violated");
        return Result.Failure(ex.Code, ex.Message);
    }
    catch (Exception ex)
    {
        // Log and return system error
        _logger.LogError(ex, "Unexpected error");
        throw;
    }
}
```

---

## Transaction Management

Commands يجب أن تُنفذ ضمن Transaction:

```csharp
[UnitOfWork]
public async Task ExecuteAsync(Command command)
{
    // All changes within this method
    // will be in one transaction
}
```

---

## Testing Handlers

### Unit Test
```csharp
[Fact]
public async Task Should_Execute_Command_Successfully()
{
    // Arrange
    var handler = new [CommandName]CommandHandler(...);
    var command = new [CommandName]Command { ... };
    
    // Act
    var result = await handler.ExecuteAsync(command);
    
    // Assert
    result.Success.ShouldBeTrue();
}
```

---

## Best Practices

1. **Single Responsibility**: Handler واحد لكل Command/Query
2. **Thin Handlers**: المنطق في Domain Layer، Handler فقط ينسق
3. **Async**: استخدام Async/Await دائماً
4. **Logging**: تسجيل البداية والنهاية والأخطاء
5. **Validation**: التحقق قبل التنفيذ

