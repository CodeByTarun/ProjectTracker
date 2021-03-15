using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectTracker.Model.Configurations
{
    class IssueConfiguration : IEntityTypeConfiguration<Issue>
    {
        public void Configure(EntityTypeBuilder<Issue> builder)
        {
            // One-to-Many Relations
            builder.HasMany<Tag>(p => p.Tags)
                .WithMany(t => t.Issues);
        }
    }
}
