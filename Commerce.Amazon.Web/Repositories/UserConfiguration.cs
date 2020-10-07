using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Commerce.Amazon.Web.Repositories
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(prop => prop.Id);
            builder.Property(prop => prop.Email).IsRequired();
            builder.Property(prop => prop.Nom).IsRequired(false);
            builder.Property(prop => prop.Prenom).IsRequired(false);
            builder.Property(prop => prop.UserId).IsRequired();
            builder.Property(prop => prop.State).IsRequired();
            builder.Property(prop => prop.IdGroup).IsRequired(false);

            builder.HasOne(u => u.Group)
                .WithMany(g => g.Users)
                .HasForeignKey(u => u.IdGroup);
        }
    }

}
