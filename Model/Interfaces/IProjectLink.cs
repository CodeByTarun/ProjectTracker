using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectTracker.Model.Interfaces
{
    public interface IProjectLink
    {
        int ProjectID { get; set; }
        Project Project { get; set; }
    }
}
