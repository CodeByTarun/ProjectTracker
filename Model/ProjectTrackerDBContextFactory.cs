using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProjectTracker.Model
{
    public class ProjectTrackerDBContextFactory : IDesignTimeDbContextFactory<ProjectTrackerDBContext>
    {
        public ProjectTrackerDBContext CreateDbContext(string[] args = null)
    {
        var options = new DbContextOptionsBuilder<ProjectTrackerDBContext>();
            options.EnableSensitiveDataLogging(true);
        options.UseSqlite("Filename=ProjectTracker.db");

        return new ProjectTrackerDBContext(options.Options);
    }
}
}
