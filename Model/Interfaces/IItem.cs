using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectTracker.Model.Interfaces
{
    public interface IItem
    {
        string Name { get; set; }
        string Description { get; set; }
        DateTime DateCreated { get; set; }
    }
}
