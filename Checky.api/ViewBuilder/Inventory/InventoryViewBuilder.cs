using System;
using System.Collections.Generic;
using System.Linq;
using Checky.api.Model;
using Checky.api.View;

namespace Checky.api.ViewBuilder.Device
{
    public class InventoryViewBuilder : IInventoryViewBuilder
    {
        private readonly IOrganizationViewBuilder organizationViewBuilder;
        private readonly IInventoryItemViewBuilder inventoryItemViewBuilder;

        public InventoryViewBuilder(IOrganizationViewBuilder organizationViewBuilder, IInventoryItemViewBuilder inventoryItemViewBuilder) 
        {
            this.organizationViewBuilder = organizationViewBuilder;
            this.inventoryItemViewBuilder = inventoryItemViewBuilder;
        }

        public View.Inventory Build(Model.Inventory inventory)
        {
            List<View.InventoryItem> inventoryItems = new List<View.InventoryItem>();

            if (inventory.InventoryItems != null)
                inventoryItems = inventory.InventoryItems.Select(x => inventoryItemViewBuilder.Build(x)).ToList();

            return new View.Inventory
            {
                InventoryGuid = inventory.InventoryGuid,
                Organization = inventory.Organization == null ? null : organizationViewBuilder.Build(inventory.Organization),
                InventoryItems = inventoryItems,
                CreatedOn = inventory.CreatedOn.ToString(),
                UpdatedOn = inventory.UpdatedOn.ToString()
            };
        }
    }
}
