using Microsoft.Extensions.DependencyInjection;
using PsecKit.Core.Application.Contracts.Services;
using PsecKit.Core.Application.Services;
using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Identity;

namespace PsecKit.Core
{
    [DependsOn(
        typeof(PsecKitCoreDomainModule),
        typeof(AbpAutofacModule),
        typeof(AbpIdentityDomainModule)
        )]
    public class PsecKitCoreApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddTransient<IPasswordHasher, PasswordHasher>();
        }
    }
}
