using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Commerce.Amazon.Web.Repositories
{
    public class PostPlaningConfiguration : IEntityTypeConfiguration<PostPlaning>
    {
        public void Configure(EntityTypeBuilder<PostPlaning> builder)
        {
            builder.HasKey(prop => new { prop.IdPost, prop.IdUser });
        }
    }
    public class GroupPlaningConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasKey(prop => prop.Id);
        }
    }

}
