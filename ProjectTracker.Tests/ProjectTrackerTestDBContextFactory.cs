using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProjectTracker.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectTracker.Tests
{
    public class ProjectTrackerTestDBContextFactory : IDesignTimeDbContextFactory<ProjectTrackerDBContext>
    {
        public ProjectTrackerDBContext CreateDbContext(string[] args = null)
        {
            var options = new DbContextOptionsBuilder<ProjectTrackerDBContext>();
            options.UseSqlite("Filename=Test.db");

            return new ProjectTrackerDBContext(options.Options);
        }
    }
}
