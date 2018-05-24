using System;
using Checky.api.Model;
using Checky.api.View;

namespace Checky.api.ViewBuilder.Device
{
    public class OrganizationViewBuilder : IOrganizationViewBuilder
    {
        public View.Organization Build(Model.Organization organization)
        {
            return new View.Organization
            {
                OrganizationGuid = organization.OrganizationGuid,
                OrganizationName = organization.OrganizationName,
                MaxDevices = organization.MaxDevices.ToString(),
                CreatedOn = organization.CreatedOn.ToString(),
                UpdatedOn = organization.UpdatedOn.ToString()
            };
        }
    }
}
