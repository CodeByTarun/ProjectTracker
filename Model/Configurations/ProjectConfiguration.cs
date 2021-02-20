using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTracker.Model.Models;

namespace ProjectTracker.Model.Configurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            // One-to-Many Relations
            builder.HasMany<Document>(p => p.Documents)
                   .WithOne(d => d.Project)
                   .HasForeignKey(d => d.ProjectID)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany<Board>(p => p.Boards)
                .WithOne(b => b.Project)
                .HasForeignKey(b => b.ProjectID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
