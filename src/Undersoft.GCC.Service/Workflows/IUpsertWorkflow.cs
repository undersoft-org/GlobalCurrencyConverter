using Quartz;
using Undersoft.GCC.Infrastructure.Currencies;
using Undersoft.SDK.Workflows;

namespace Undersoft.GCC.Service.API.Workflows
{
    public interface IUpsertWorkflow<T> : IJob where T : CurrenciesContext
    {
        void ConfigureFlow();
        void ConfigureWork();
    }
}