using ProjectTracker.Model.Interfaces;
using System;

namespace ProjectTracker.Model.Models
{
    public class Issue : DomainObject, IItem, ILinkedList
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }
        public DateTime DateCreated { get; set; }
 
        public int GroupID { get; set; }
        public Group Group { get; set; }
        public int NextID { get; set; }
    }
}
