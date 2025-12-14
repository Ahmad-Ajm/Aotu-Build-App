using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using CVSystem.CVs.Dtos;

namespace CVSystem.CVs
{
    public interface ICvAppService : IApplicationService
    {
        Task<CvDto> GetAsync(Guid id);
        Task<PagedResultDto<CvDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        Task<CvDto> CreateAsync(CreateCvDto input);
        Task<CvDto> UpdateAsync(Guid id, UpdateCvDto input);
        Task DeleteAsync(Guid id);
    }
}
