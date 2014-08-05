namespace Uma.Eservices.DbAccess.Mappings
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Diagnostics.CodeAnalysis;
    using Uma.Eservices.DbObjects;

    /// <summary>
    /// Mapping for EF Identity Application User (Web User)
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ApplicationUserMap : EntityTypeConfiguration<ApplicationUser>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationUserMap"/> class.
        /// </summary>
        public ApplicationUserMap()
        {
            this.ToTable("User");
            this.HasKey(m => m.Id);
            this.Property(m => m.Id).HasColumnName("UserId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(m => m.CustomerId);
            this.Property(m => m.FirstName).HasMaxLength(70);
            this.Property(m => m.LastName).HasMaxLength(70);
            this.Property(m => m.PersonCode).HasMaxLength(20);
            this.Property(m => m.BirthDate);
            this.Property(m => m.IsStronglyAuthenticated);
            this.Property(m => m.Email).HasMaxLength(128);
            this.Property(m => m.EmailConfirmed);
            this.Property(m => m.PhoneNumber).HasMaxLength(20);
            this.Property(m => m.PhoneNumberConfirmed);
            this.Property(m => m.PasswordHash).HasMaxLength(70);
            this.Property(m => m.SecurityStamp).HasMaxLength(64);
            this.Property(m => m.TwoFactorEnabled);
            this.Property(m => m.LockoutEndDateUtc);
            this.Property(m => m.LockoutEnabled);
            this.Property(m => m.AccessFailedCount);
            this.Property(m => m.UserName).HasMaxLength(128);
            
            this.HasMany(u => u.Logins).WithRequired().HasForeignKey(ul => ul.UserId);
            this.HasMany(u => u.Claims).WithRequired().HasForeignKey(uc => uc.UserId);
            this.HasMany(u => u.Roles).WithRequired().HasForeignKey(ur => ur.UserId);
        }
    }
}
