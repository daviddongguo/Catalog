using System.Threading.Tasks;
using MassTransit;
using Play.Catalog.Contracts;
using David.Common;

namespace Play.Inventory.Service.Consumers
{
    public class CatalogItemCreatedConsumer : IConsumer<CatalogItemCreated>
    {
        private readonly IRepository<CatalogItem> _rep;

        public CatalogItemCreatedConsumer(IRepository<CatalogItem> repository)
        {
            this._rep = repository;
        }

        public async Task Consume(ConsumeContext<CatalogItemCreated> context)
        {
            var message = context.Message;


            if (await _rep.GetAsync(message.Id) != null)
            {
                return;
            }

            var item = new CatalogItem
            {
                Id = message.Id,
                Name = message.Name,
                ImageUrl = message.ImageUrl,
                Price = message.Price
            };

            await _rep.CreateAsync(item);
        }
    }
}
