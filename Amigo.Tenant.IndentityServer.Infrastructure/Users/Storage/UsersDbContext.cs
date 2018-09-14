using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Amigo.Tenant.IdentityServer.Infrastructure.Storage;
using Amigo.Tenant.IdentityServer.Infrastructure.Users.Storage.Model;
using IdentityUserClaim = Amigo.Tenant.IdentityServer.Infrastructure.Users.Storage.Model.IdentityUserClaim;
using IdentityUserLogin = Amigo.Tenant.IdentityServer.Infrastructure.Users.Storage.Model.IdentityUserLogin;
using IdentityUserRole = Amigo.Tenant.IdentityServer.Infrastructure.Users.Storage.Model.IdentityUserRole;

namespace Amigo.Tenant.IdentityServer.Infrastructure.Users.Storage
{
    public class UsersDbContext : IdentityDbContext<User,Role,int, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public UsersDbContext(string connString)
            : base(connString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.HasDefaultSchema(Constants.Schema);

            modelBuilder.Entity<IdentityUserLogin>().Map(c =>
            {
                c.ToTable("UserLogin");
                c.Properties(p => new
                {
                    p.UserId,
                    p.LoginProvider,
                    p.ProviderKey
                });
            }).HasKey(p => new { p.LoginProvider, p.ProviderKey, p.UserId });

            // Mapping for ApiRole
            modelBuilder.Entity<Role>().Map(c =>
            {
                c.ToTable("Role");
                c.Property(p => p.Id).HasColumnName("RoleId");
                c.Properties(p => new
                {
                    p.Name
                });
            }).HasKey(p => p.Id);
            modelBuilder.Entity<Role>().HasMany(c => c.Users).WithRequired().HasForeignKey(c => c.RoleId);

            modelBuilder.Entity<User>().Map(c =>
            {
                c.ToTable("User");
                c.Property(p => p.Id).HasColumnName("UserId");
                c.Properties(p => new
                {
                    p.AccessFailedCount,
                    p.Email,
                    p.EmailConfirmed,
                    p.PasswordHash,
                    p.PhoneNumber,
                    p.PhoneNumberConfirmed,
                    p.TwoFactorEnabled,
                    p.SecurityStamp,
                    p.LockoutEnabled,
                    p.LockoutEndDateUtc,
                    p.FirstName,
                    p.LastName,
                    p.ProfilePictureUrl,
                    p.UserName,
                    p.RowStatus
                });
            }).HasKey(c => c.Id);
            modelBuilder.Entity<User>().HasMany(c => c.Logins).WithOptional().HasForeignKey(c => c.UserId);
            modelBuilder.Entity<User>().HasMany(c => c.Claims).WithOptional().HasForeignKey(c => c.UserId);
            modelBuilder.Entity<User>().HasMany(c => c.Roles).WithRequired().HasForeignKey(c => c.UserId);

            modelBuilder.Entity<IdentityUserRole>().Map(c =>
            {
                c.ToTable("UserRole");
                c.Properties(p => new
                {
                    p.UserId,
                    p.RoleId
                });
            })
                .HasKey(c => new { c.UserId, c.RoleId });

            modelBuilder.Entity<IdentityUserClaim>().Map(c =>
            {
                c.ToTable("UserClaim");
                c.Property(p => p.Id).HasColumnName("UserClaimId");
                c.Properties(p => new
                {
                    p.UserId,
                    p.ClaimValue,
                    p.ClaimType
                });
            }).HasKey(c => c.Id);
        }    
    }
}