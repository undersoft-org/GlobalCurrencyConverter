using Quartz;
using Undersoft.GCC.Infrastructure.Currencies;
using Undersoft.GCC.Service.Commands;
using Undersoft.SDK.Service;
using Undersoft.SDK.Workflows;

namespace Undersoft.GCC.Service.API.Workflows
{
    public class UpsertWorkflow<T> : Workflow<T>, IUpsertWorkflow<T> where T : CurrenciesContext
    {
        protected IServicer _servicer { get; }
        protected T _context { get; }

        public UpsertWorkflow(IServicer servicer)
        {
            _servicer = servicer;
            _context = _servicer.GetService<T>();
            Configure();
        }

        public override void ConfigureWork()
        {
            Aspect<T>()
                    .AddWork(_context, c => c.GetAllRates)
                    .AddWork(_context, c => c.GetCurrencies)
                    .AddWork(_context, c => c.GetLatestRates)
                .Allocate(3);

            Aspect<UpsertCommands>()
                    .AddWork<UpsertCommands>(a => a.UpsertCurrencies, _servicer)
                    .AddWork<UpsertCommands>(a => a.UpsertAllRates, _servicer)
                    .AddWork<UpsertCommands>(a => a.UpsertLatestRates, _servicer)
                .Allocate(3);
        }

        public override void ConfigureFlow()
        {
            Aspect<T>()
                    .Work(_context, w => w.GetCurrencies)
                        .FlowTo<UpsertCommands>(a => a.UpsertCurrencies)
                    .Work(_context, w => w.GetAllRates)
                        .FlowTo<UpsertCommands>(a => a.UpsertAllRates)
                    .Work(_context, w => w.GetLatestRates)
                        .FlowTo<UpsertCommands>(a => a.UpsertLatestRates);
        }

        public override void Execute(params object[] args)
        {
            Aspect<T>()
                .Work(_context, w => w.GetCurrencies).Post()
                .Work(_context, w => w.GetLatestRates).Post()
                .Work(_context, w => w.GetAllRates).Post();
        }

        public async Task Execute(IJobExecutionContext context)
        {            
            Execute();

            await Task.CompletedTask;
        }
    }
}
