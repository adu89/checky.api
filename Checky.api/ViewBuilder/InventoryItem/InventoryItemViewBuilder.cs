using System;
using Checky.api.Model;
using Checky.api.View;

namespace Checky.api.ViewBuilder.Device
{
    public class InventoryItemViewBuilder : IInventoryItemViewBuilder
    {
        private readonly IItemViewBuilder itemViewBuilder;

        public InventoryItemViewBuilder(IItemViewBuilder itemViewBuilder)
        {
            this.itemViewBuilder = itemViewBuilder;
        }

        public View.InventoryItem Build(Model.InventoryItem inventoryItem)
        {
            return new View.InventoryItem
            {
                InventoryItemGuid = inventoryItem.InventoryItemGuid,
                Item = inventoryItem.Item == null ? null : itemViewBuilder.Build(inventoryItem.Item),
                Quantity = inventoryItem.Quantity.ToString(),
                CreatedOn = inventoryItem.CreatedOn.ToString(),
                UpdatedOn = inventoryItem.UpdatedOn.ToString(),
            };
        }
    }
}
