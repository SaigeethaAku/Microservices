using System;

namespace Play.Catalog.Contracts
{
    public record CatalogItemCreated(Guid ItemId, string name, string description);

    public record CatalogItemUpdated(Guid ItemId, string name, string description);

    public record CatalogItemDeleted(Guid ItemId);

}