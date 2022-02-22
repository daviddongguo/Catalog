using David.Common;
using MassTransit;
using Play.Catalog.Contracts;
using System.Threading.Tasks;

namespace Play.Inventory.Service.Consumers
{
    public class CatalogItemUpdatedConsumer : IConsumer<CatalogItemUpdated>
    {
        private readonly IRepository<CatalogItem> _rep;

        public CatalogItemUpdatedConsumer(IRepository<CatalogItem> repository)
        {
            this._rep = repository;
        }

        public async Task Consume(ConsumeContext<CatalogItemUpdated> context)
        {
            var message = context.Message;

            var item = await _rep.GetAsync(message.Id);
            if (item == null)
            {
                item = new CatalogItem
                {
                    Id = message.Id,
                    Name = message.Name,
                    ImageUrl = message.ImageUrl,
                    Price = message.Price
                };
                await _rep.CreateAsync(item);
                return;
            }
            item.Name = message.Name;
            item.ImageUrl = message.ImageUrl;
            item.Price = message.Price;

            await _rep.UpdateAsync(item);
        }
    }
}
