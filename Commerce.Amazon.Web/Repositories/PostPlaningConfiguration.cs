﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Commerce.Amazon.Web.Repositories
{
    public class PostPlaningConfiguration : IEntityTypeConfiguration<PostPlaning>
    {
        public void Configure(EntityTypeBuilder<PostPlaning> builder)
        {
            builder.HasKey(prop => new { prop.IdPost, prop.IdUser });
            builder.Property(prop => prop.IdPost).IsRequired();
            builder.Property(prop => prop.IdUser).IsRequired();
            builder.Property(prop => prop.State).IsRequired();
            builder.Property(prop => prop.PathScreenComment).IsRequired(false);
            builder.Property(prop => prop.DatePlanifie).IsRequired(false);
            builder.Property(prop => prop.DateLimite).IsRequired(false);
            builder.Property(prop => prop.DateNotified).IsRequired(false);
            builder.Property(prop => prop.DateComment).IsRequired(false);

            builder.HasOne(pp => pp.User)
                .WithMany(pp => pp.PostsAchat)
                .HasForeignKey(pp => pp.IdUser);
            
            builder.HasOne(pp => pp.Post)
                .WithMany(pp => pp.Planings)
                .HasForeignKey(pp => pp.IdPost);

        }
    }

}
