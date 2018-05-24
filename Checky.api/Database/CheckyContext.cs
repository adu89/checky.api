using System;
using Checky.api.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Checky.api.Database
{
    public class CheckyContext : IdentityDbContext<Identity>
    {
        public DbSet<Device> Devices { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<OrganizationItem> OrganizationItems { get; set; }
        public DbSet<Vendor> Vendors { get; set; }

        public CheckyContext(DbContextOptions<CheckyContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var device = modelBuilder.Entity<Device>();

            device
                .Property(x => x.DeviceGuid)
                .HasDefaultValueSql("NEWID()");

            device.HasOne(x => x.Inventory)
                  .WithMany(x => x.Devices)
                  .OnDelete(DeleteBehavior.Restrict);

            device.HasOne(x => x.Organization)
                  .WithMany(x => x.Devices)
                  .OnDelete(DeleteBehavior.Restrict);

            device
                .Property(x => x.Deleted)
                .HasDefaultValue(false);

            device
                .Property(x => x.DeviceName)
                .IsRequired();

            device
                .Property(x => x.CreatedOn)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");
            
            device
                .Property(x => x.UpdatedOn)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            device.HasAlternateKey(x => x.DeviceGuid);

            var inventory = modelBuilder.Entity<Inventory>();

            inventory
                .Property(x => x.InventoryGuid)
                .HasDefaultValueSql("NEWID()");

            inventory.HasOne(x => x.Organization)
                     .WithMany(x => x.Inventories)
                     .OnDelete(DeleteBehavior.Restrict);

            inventory.HasMany(x => x.Devices)
                     .WithOne(x => x.Inventory)
                     .OnDelete(DeleteBehavior.Restrict);

            inventory.HasMany(x => x.InventoryItems)
                     .WithOne(x => x.Inventory)
                     .OnDelete(DeleteBehavior.Restrict);

            inventory.HasMany(x => x.Orders)
                     .WithOne(x => x.Inventory)
                     .OnDelete(DeleteBehavior.Restrict);

            inventory
                .Property(x => x.CreatedOn)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            inventory
                .Property(x => x.UpdatedOn)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            inventory.HasAlternateKey(x => x.InventoryGuid);

            var inventoryItem = modelBuilder.Entity<InventoryItem>();

            inventoryItem.Property(x => x.InventoryItemGuid)
                         .HasDefaultValueSql("NEWID()");

            inventoryItem.HasOne(x => x.Inventory)
                         .WithMany(x => x.InventoryItems)
                         .OnDelete(DeleteBehavior.Restrict);

            inventoryItem.HasOne(x => x.Item)
                         .WithMany(x => x.InventoryItems)
                         .OnDelete(DeleteBehavior.Restrict);

            inventoryItem
                .Property(x => x.CreatedOn)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            inventoryItem
                .Property(x => x.UpdatedOn)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            inventoryItem.HasAlternateKey(x => x.InventoryItemGuid);

            inventoryItem.HasAlternateKey(x => new {x.InventoryId, x.ItemId});

            var item = modelBuilder.Entity<Item>();

            item.Property(x => x.ItemGuid)
                .HasDefaultValueSql("NEWID()");

            item.Property(x => x.Name)
                .IsRequired();

            item.Property(x => x.Description)
                .IsRequired();

            item.HasOne(x => x.Vendor)
                .WithMany(x => x.Items)
                .OnDelete(DeleteBehavior.Restrict);

            item.HasMany(x => x.InventoryItems)
                .WithOne(x => x.Item)
                .OnDelete(DeleteBehavior.Restrict);

            item.HasMany(x => x.OrderItems)
                .WithOne(x => x.Item)
                .OnDelete(DeleteBehavior.Restrict);

            item.HasMany(x => x.OrderItems)
                .WithOne(x => x.Item)
                .OnDelete(DeleteBehavior.Restrict);

            item
                .Property(x => x.CreatedOn)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            item
                .Property(x => x.UpdatedOn)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            item.HasAlternateKey(x => x.ItemGuid);

            var order = modelBuilder.Entity<Order>();

            order.Property(x => x.OrderGuid)
                .HasDefaultValueSql("NEWID()");

            order.HasOne(x => x.User)
                 .WithMany(x => x.Orders)
                 .OnDelete(DeleteBehavior.Restrict);

            order.HasOne(x => x.Inventory)
                 .WithMany(x => x.Orders)
                 .OnDelete(DeleteBehavior.Restrict);

            order.HasMany(x => x.OrderItems)
                 .WithOne(x => x.Order)
                 .OnDelete(DeleteBehavior.Restrict);

            order.HasOne(x => x.Transaction)
                 .WithOne(x => x.Order)
                 .OnDelete(DeleteBehavior.Restrict);

            order
                .Property(x => x.CreatedOn)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            order
                .Property(x => x.UpdatedOn)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            order.HasAlternateKey(x => x.OrderGuid);

            var orderItem = modelBuilder.Entity<OrderItem>();

            orderItem.Property(x => x.orderItemGuid)
                     .HasDefaultValueSql("NEWID()");

            orderItem.HasOne(x => x.Order)
                     .WithMany(x => x.OrderItems)
                     .OnDelete(DeleteBehavior.Restrict);

            orderItem.HasOne(x => x.Item)
                     .WithMany(x => x.OrderItems)
                     .OnDelete(DeleteBehavior.Restrict);

            orderItem.HasAlternateKey(x => new {x.OrderId, x.ItemId});

            orderItem.HasAlternateKey(x => x.orderItemGuid);

            var organization = modelBuilder.Entity<Organization>();

            organization.Property(x => x.OrganizationGuid)
                        .HasDefaultValueSql("NEWID()");

            organization.Property(x => x.OrganizationName)
                        .IsRequired();

            organization.Property(x => x.MaxDevices)
                        .IsRequired();

            organization.HasMany(x => x.Users)
                        .WithOne(x => x.Organization)
                        .OnDelete(DeleteBehavior.Restrict);

            organization.HasMany(x => x.Inventories)
                        .WithOne(x => x.Organization)
                        .OnDelete(DeleteBehavior.Restrict);

            organization.HasMany(x => x.Devices)
                        .WithOne(x => x.Organization)
                        .OnDelete(DeleteBehavior.Restrict);

            organization.HasMany(x => x.OrganizationItems)
                        .WithOne(x => x.Organization)
                        .OnDelete(DeleteBehavior.Restrict);

            organization
                .Property(x => x.CreatedOn)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            organization
                .Property(x => x.UpdatedOn)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            organization.HasAlternateKey(x => x.OrganizationGuid);

            var organizationItem = modelBuilder.Entity<OrganizationItem>();

            organizationItem.Property(x => x.OrganizationItemGuid)
                         .HasDefaultValueSql("NEWID()");

            organizationItem.HasOne(x => x.Organization)
                            .WithMany(x => x.OrganizationItems)
                            .OnDelete(DeleteBehavior.Restrict);

            organizationItem.HasOne(x => x.Item)
                            .WithMany(x => x.OrganizationsItems)
                            .OnDelete(DeleteBehavior.Restrict);

            organizationItem
                .Property(x => x.CreatedOn)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            organizationItem
                .Property(x => x.UpdatedOn)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            organizationItem.HasAlternateKey(x => new { x.OrganizationId, x.ItemId });

            organizationItem.HasAlternateKey(x => x.OrganizationItemGuid);

            var paymentMethod = modelBuilder.Entity<PaymentMethod>();

            paymentMethod.Property(x => x.PaymentMethodGuid)
                         .HasDefaultValueSql("NEWID()");

            paymentMethod.Property(x => x.Type)
                         .IsRequired();

            paymentMethod.Property(x => x.Details)
                         .IsRequired();

            paymentMethod.Property(x => x.Default)
                         .HasDefaultValue(false);

            paymentMethod.HasOne(x => x.User)
                         .WithMany(x => x.PaymentMethods)
                         .OnDelete(DeleteBehavior.Restrict);

            paymentMethod.HasMany(x => x.Trancsactions)
                         .WithOne(x => x.PaymentMethod)
                         .OnDelete(DeleteBehavior.Restrict);

            paymentMethod
                .Property(x => x.CreatedOn)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            paymentMethod
                .Property(x => x.UpdatedOn)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            paymentMethod.HasAlternateKey(x => x.PaymentMethodGuid);

            var role = modelBuilder.Entity<Role>();

            role.Property(x => x.RoleGuid)
                .HasDefaultValueSql("NEWID()");

            role.Property(x => x.RoleName)
                .IsRequired();

            role.HasMany(x => x.UserRoles)
                .WithOne(x => x.Role)
                .OnDelete(DeleteBehavior.Restrict);

            role
                .Property(x => x.CreatedOn)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            role
                .Property(x => x.UpdatedOn)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            role.HasAlternateKey(x => x.RoleGuid);

            var transaction = modelBuilder.Entity<Transaction>();

            transaction.Property(x => x.TransactionGuid)
                       .HasDefaultValueSql("NEWID()");

            transaction.Property(x => x.Total)
                       .IsRequired();

            transaction.Property(x => x.TransactionToken)
                       .IsRequired();

            transaction.HasOne(x => x.Order)
                       .WithOne(x => x.Transaction)
                       .OnDelete(DeleteBehavior.Restrict);

            transaction.HasOne(x => x.PaymentMethod)
                       .WithMany(x => x.Trancsactions)
                       .OnDelete(DeleteBehavior.Restrict);

            transaction
                .Property(x => x.CreatedOn)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            transaction
                .Property(x => x.UpdatedOn)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            transaction.HasAlternateKey(x => x.TransactionGuid);

            var user = modelBuilder.Entity<User>();

            user.HasOne(x => x.Organization)
                .WithMany(x => x.Users)
                .OnDelete(DeleteBehavior.Restrict);

            user.HasMany(x => x.Orders)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.Restrict);

            user.Property(x => x.Email)
                .IsRequired();

            user.Property(x => x.Birthdate)
                .IsRequired();

            user.Property(x => x.Gender)
                .IsRequired();

            user.Property(x => x.Pin)
                .IsRequired();

            user
                .Property(x => x.CreatedOn)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            user
                .Property(x => x.UpdatedOn)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            user.HasMany(x => x.PaymentMethods)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.Restrict);

            user.HasMany(x => x.UserRoles)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.Restrict);

            user.HasAlternateKey(x => x.UserGuid);

            var userRole = modelBuilder.Entity<UserRole>();

            userRole.Property(x => x.UserRoleGuid)
                    .HasDefaultValueSql("NEWID()");
            
            userRole.HasOne(x => x.User)
                    .WithMany(x => x.UserRoles)
                    .OnDelete(DeleteBehavior.Restrict);

            userRole.HasOne(x => x.Role)
                    .WithMany(x => x.UserRoles)
                    .OnDelete(DeleteBehavior.Restrict);

            userRole.HasAlternateKey(x => new { x.UserId, x.RoleId });

            userRole.HasAlternateKey(x => x.UserRoleGuid);

            userRole
                .Property(x => x.CreatedOn)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            userRole
                .Property(x => x.UpdatedOn)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");
        
            var vendor = modelBuilder.Entity<Vendor>();

            vendor.Property(x => x.VendorGuid)
                  .HasDefaultValueSql("NEWID()");

            vendor.Property(x => x.Name)
                  .IsRequired();

            vendor.HasMany(x => x.Items)
                  .WithOne(x => x.Vendor)
                  .OnDelete(DeleteBehavior.Restrict);

            vendor
                .Property(x => x.CreatedOn)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            vendor
                .Property(x => x.UpdatedOn)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            vendor.HasAlternateKey(x => x.VendorGuid); 
        }
    }
}
