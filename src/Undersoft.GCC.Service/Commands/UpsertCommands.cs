using Undersoft.GCC.Domain.Entities;
using Undersoft.SDK.Service;
using Undersoft.SDK.Service.Data.Remote.Repository;
using Undersoft.SDK.Service.Data.Store;

namespace Undersoft.GCC.Service.Commands
{
    public class UpsertCommands
    {
        IServicer _servicer = default!;

        public UpsertCommands() { }

        public UpsertCommands(IServicer servicer)
        {
            _servicer = servicer;
        }

        public async Task UpsertCurrencies(IEnumerable<Currency> currencies)
        {
            await _servicer.Serve<IRemoteRepository<IDataStore, Contracts.Currency>>(r =>
            {
                r.PutBy(
                    currencies,
                    d => e => d.Id == e.Id && d.CurrencyCode == e.CurrencyCode
                )
               .Commit();
                
                return Task.CompletedTask;
            }).ConfigureAwait(false);
        }

        public async Task UpsertLatestRates(IEnumerable<CurrencyRate> currencies)
        {
            await _servicer.Serve<IRemoteRepository<IDataStore, Contracts.CurrencyRate>>(r =>
            {
                r.PutBy(
                    currencies,
                    d =>
                        e =>
                            d.PublishDate == e.PublishDate
                            && d.ProviderId == e.ProviderId
                            && d.TargetCurrencyId == e.TargetCurrencyId
                )
                .Commit();
                return Task.CompletedTask;
            });
        }

        public async Task UpsertAllRates(IEnumerable<CurrencyRate> currencies)
        {
            await _servicer.Serve<IRemoteRepository<IDataStore, Contracts.CurrencyRate>>(r =>
            {
                r.PutBy(
                        currencies,
                        d =>
                            e =>
                                d.PublishDate == e.PublishDate
                                && d.ProviderId == e.ProviderId
                                && d.TargetCurrencyId == e.TargetCurrencyId
                    )
                    .Commit();
                return Task.CompletedTask;
            });
        }
    }
}
