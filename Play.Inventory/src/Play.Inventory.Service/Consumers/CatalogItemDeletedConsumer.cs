using David.Common;
using MassTransit;
using Play.Catalog.Contracts;
using System.Threading.Tasks;

namespace Play.Inventory.Service.Consumers
{
    public class CatalogItemDeletedConsumer : IConsumer<CatalogItemDeleted>
    {
        private readonly IRepository<CatalogItem> _rep;

        public CatalogItemDeletedConsumer(IRepository<CatalogItem> repository)
        {
            this._rep = repository;
        }
        public async Task Consume(ConsumeContext<CatalogItemDeleted> context)
        {
            var message = context.Message;

            if (await _rep.GetAsync(message.Id) == null)
            {
                return;
            }

            await _rep.RemoveAsync(message.Id);
        }
    }
}
