using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Commerce.Amazon.Web.Repositories
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasKey(prop => prop.Id);
            builder.Property(prop => prop.Name).IsRequired();
            builder.Property(prop => prop.State).IsRequired();
            builder.Property(prop => prop.MaxDays).IsRequired();
            builder.Property(prop => prop.CountNotifyPerDay).IsRequired();
            builder.Property(prop => prop.CountUsersCanNotify).IsRequired();

        }
    }

}
