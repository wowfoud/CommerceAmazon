using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Commerce.Amazon.Web.Repositories
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(prop => prop.Id);
            builder.Property(prop => prop.IdUser).IsRequired();
            builder.Property(prop => prop.Url).IsRequired();
            builder.Property(prop => prop.Description).IsRequired(false);
            builder.Property(prop => prop.Prix).IsRequired(false);
            builder.Property(prop => prop.DateCreate).IsRequired();
            builder.Property(prop => prop.State).IsRequired();

            builder.HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.IdUser);
        }
    }

}
