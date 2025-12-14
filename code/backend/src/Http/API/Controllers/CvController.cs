using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using CVSystem.CVs.Dtos;

namespace CVSystem.HttpApi.Controllers;

[RemoteService]
[Area("cv")]
[Route("api/cv")] 
[Authorize]
public class CvController : AbpController
{
    private readonly ICvAppService _cvAppService;

    public CvController(ICvAppService cvAppService)
    {
        _cvAppService = cvAppService;
    }

    [HttpGet]
    [Route("{id}")]
    public Task<CvDto> GetAsync(Guid id)
    {
        return _cvAppService.GetAsync(id);
    }

    [HttpGet]
    public Task<PagedResultDto<CvDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        return _cvAppService.GetListAsync(input);
    }

    [HttpPost]
    public Task<CvDto> CreateAsync([FromBody] CreateCvDto input)
    {
        return _cvAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public Task<CvDto> UpdateAsync(Guid id, [FromBody] UpdateCvDto input)
    {
        return _cvAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}")]
    public Task DeleteAsync(Guid id)
    {
        return _cvAppService.DeleteAsync(id);
    }
}
