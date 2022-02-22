using System;
using David.Common;

namespace Play.Catalog.Service.Entities
{

    public class Item : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
    }

}
