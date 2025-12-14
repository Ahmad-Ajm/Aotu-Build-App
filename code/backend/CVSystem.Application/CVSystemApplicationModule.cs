using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace CVSystem.Application;

[DependsOn(
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule),
    typeof(CVSystemApplicationContractsModule)
)]
public class CVSystemApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<CVSystemApplicationModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<CVSystemApplicationModule>();
        });
    }
}
