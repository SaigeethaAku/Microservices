using System;
using Play.Common;

namespace Play.Catalog.Service.Entities
{
    public class Item : IEntity
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public DateTimeOffset createdDate { get; set; }
    }
}