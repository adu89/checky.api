using System;
using Checky.api.Model;
using Checky.api.View;

namespace Checky.api.ViewBuilder.Device
{
    public class VendorViewBuilder : IVendorViewBuilder
    {
        public View.Vendor Build(Model.Vendor vendor)
        {
            return new View.Vendor
            {
                VendorGuid = vendor.VendorGuid,
                Name = vendor.Name,
                CreatedOn = vendor.CreatedOn.ToString(),
                UpdatedOn = vendor.UpdatedOn.ToString()
            };
        }
    }
}
