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
            builder.Property(prop => prop.Nom).IsRequired();
            builder.Property(prop => prop.Prenom).IsRequired();
            builder.Property(prop => prop.UserId).IsRequired();
            builder.Property(prop => prop.UserGuid).IsRequired();
            builder.Property(prop => prop.State).IsRequired();
        }
    }

}
