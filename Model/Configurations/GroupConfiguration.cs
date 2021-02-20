using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTracker.Model.Models;

namespace ProjectTracker.Model.Configurations
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            // One-to-Many Relations
            builder.HasMany<Issue>(g => g.Issues)
                   .WithOne(i => i.Group)
                   .HasForeignKey(i => i.GroupID)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
