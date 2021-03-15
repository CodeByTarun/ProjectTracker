using Microsoft.EntityFrameworkCore.Design;
using ProjectTracker.ClassLibrary.ServiceInterfaces;
using ProjectTracker.Model;
using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectTracker.ClassLibrary.Services
{
    public class TagDataService : GenericDataService<Tag>, ITagDataService
    {
        public TagDataService(IDesignTimeDbContextFactory<ProjectTrackerDBContext> contextFactory) : base(contextFactory)
        {
        }
    }
}
