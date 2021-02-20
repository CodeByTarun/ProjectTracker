using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTracker.Model.Models;

namespace ProjectTracker.Model.Configurations
{
    public class BoardConfiguration : IEntityTypeConfiguration<Board>
    {
        public void Configure(EntityTypeBuilder<Board> builder)
        {
            // One-to-Many Relations
            builder.HasMany<Group>(b => b.Groups)
                   .WithOne(g => g.Board)
                   .HasForeignKey(g => g.BoardID)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
