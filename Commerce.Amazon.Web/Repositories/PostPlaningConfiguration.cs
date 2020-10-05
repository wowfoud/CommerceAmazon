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

}
