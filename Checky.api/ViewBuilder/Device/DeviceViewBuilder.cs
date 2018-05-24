using System;
using Checky.api.Model;
using Checky.api.View;

namespace Checky.api.ViewBuilder.Device
{
    public class DeviceViewBuilder : IDeviceViewBuilder
    {
        private readonly IOrganizationViewBuilder organizationViewBuilder;
        private readonly IInventoryViewBuilder inventoryViewBuilder;

        public DeviceViewBuilder(IInventoryViewBuilder inventoryViewBuilder, IOrganizationViewBuilder organizationViewBuilder) 
        {
            this.organizationViewBuilder = organizationViewBuilder;
            this.inventoryViewBuilder = inventoryViewBuilder;
        }

        public View.Device Build(Model.Device device)
        {
            return new View.Device
            {
                DeviceGuid = device.DeviceGuid,
                DeviceName = device.DeviceName,
                Inventory = device.Inventory == null ? null : inventoryViewBuilder.Build(device.Inventory),
                Organization = device.Organization == null ? null : organizationViewBuilder.Build(device.Organization),
                CreatedOn = device.CreatedOn.ToString(),
                UpdatedOn = device.UpdatedOn.ToString()
            };
        }
    }
}
