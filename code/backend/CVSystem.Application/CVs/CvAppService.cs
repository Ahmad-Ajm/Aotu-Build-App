using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using CVSystem.CVs.Dtos;

namespace CVSystem.CVs
{
    public class CvAppService : ApplicationService, ICvAppService
    {
        private readonly IRepository<CV, Guid> _cvRepository;

        public CvAppService(IRepository<CV, Guid> cvRepository)
        {
            _cvRepository = cvRepository;
        }

        public async Task<CvDto> GetAsync(Guid id)
        {
            var cv = await _cvRepository.GetAsync(id);
            return ObjectMapper.Map<CV, CvDto>(cv);
        }

        public async Task<PagedResultDto<CvDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var queryable = await _cvRepository.GetQueryableAsync();

            var cvs = await AsyncExecuter.ToListAsync(
                queryable
                    .OrderBy(c => c.CreationTime) // Or other sorting based on input.Sorting
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)
            );

            var count = await _cvRepository.GetCountAsync();

            return new PagedResultDto<CvDto>(count, ObjectMapper.Map<List<CV>, List<CvDto>>(cvs));
        }

        public async Task<CvDto> CreateAsync(CreateCvDto input)
        {
            var cv = ObjectMapper.Map<CreateCvDto, CV>(input);
            cv.PersonalInfo = JsonSerializer.Serialize(input.PersonalInfo);
            cv.WorkExperience = JsonSerializer.Serialize(input.WorkExperience);
            cv.Education = JsonSerializer.Serialize(input.Education);
            cv.Skills = JsonSerializer.Serialize(input.Skills);
            cv.UserId = CurrentUser.Id.Value; // Assign current user's ID

            var createdCv = await _cvRepository.InsertAsync(cv);
            return ObjectMapper.Map<CV, CvDto>(createdCv);
        }

        public async Task<CvDto> UpdateAsync(Guid id, UpdateCvDto input)
        {
            var cv = await _cvRepository.GetAsync(id);

            // Only update if the current user owns the CV
            if (cv.UserId != CurrentUser.Id)
            {
                // Handle unauthorized access, e.g., throw an exception
                throw new Volo.Abp.Authorization.AbpAuthorizationException("You are not authorized to update this CV.");
            }

            ObjectMapper.Map(input, cv);
            cv.PersonalInfo = JsonSerializer.Serialize(input.PersonalInfo);
            cv.WorkExperience = JsonSerializer.Serialize(input.WorkExperience);
            cv.Education = JsonSerializer.Serialize(input.Education);
            cv.Skills = JsonSerializer.Serialize(input.Skills);

            var updatedCv = await _cvRepository.UpdateAsync(cv);
            return ObjectMapper.Map<CV, CvDto>(updatedCv);
        }

        public async Task DeleteAsync(Guid id)
        {
            var cv = await _cvRepository.GetAsync(id);

            if (cv.UserId != CurrentUser.Id)
            {
                throw new Volo.Abp.Authorization.AbpAuthorizationException("You are not authorized to delete this CV.");
            }
            await _cvRepository.DeleteAsync(id);
        }
    }
}
