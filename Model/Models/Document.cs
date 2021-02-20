using ProjectTracker.Model.Interfaces;
using System;

namespace ProjectTracker.Model.Models
{
    public class Document : DomainObject, IItem, IProjectLink
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }
        public DateTime DateCreated { get; set; }
        public string Path { get; set; }
        public bool IsURL { get; set; }


        public int ProjectID { get; set; }
        public Project Project { get; set; }
        
    }
}
