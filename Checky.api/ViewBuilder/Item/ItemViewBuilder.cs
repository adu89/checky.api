using System;
using Checky.api.Model;
using Checky.api.View;

namespace Checky.api.ViewBuilder.Device
{
    public class ItemViewBuilder : IItemViewBuilder
    {
        private readonly IVendorViewBuilder vendorViewBuilder;

        public ItemViewBuilder(IVendorViewBuilder vendorViewBuilder)
        {
            this.vendorViewBuilder = vendorViewBuilder;
        }

        public View.Item Build(Model.Item item)
        {
            return new View.Item
            {
                ItemGuid = item.ItemGuid,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price.ToString(),
                Calories = item.Calories.ToString(),
                Weight = item.Weight.ToString(),
                Vendor = item.Vendor == null ? null : vendorViewBuilder.Build(item.Vendor),
                CreatedOn = item.CreatedOn.ToString(),
                UpdatedOn = item.UpdatedOn.ToString()
            };
        }
    }
}
