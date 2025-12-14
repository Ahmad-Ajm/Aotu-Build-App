using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.AutoMapper;

namespace CVSystem.Application
{
    [DependsOn(
        typeof(CVSystemDomainModule),
        typeof(CVSystemApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
        )]
    public class CVSystemApplicationModule : AbpModule
    {
        public override void Configure(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<CVSystemApplicationAutoMapperProfile>(validate: true);
            });
        }
    }
}
