using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Commerce.Amazon.Web.Repositories
{
    public class GroupUserConfiguration : IEntityTypeConfiguration<GroupUser>
    {
        public void Configure(EntityTypeBuilder<GroupUser> builder)
        {
            builder.HasKey(prop => new { prop.UserId, prop.GroupId });

            builder.HasOne(ug => ug.User)
                .WithMany(u => u.Groups)
                .HasForeignKey(u => u.UserId);

            builder.HasOne(ug => ug.Group)
                .WithMany(u => u.Users)
                .HasForeignKey(u => u.GroupId);
        }
    }

}
