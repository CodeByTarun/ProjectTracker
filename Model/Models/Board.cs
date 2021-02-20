using System;
using System.Collections.ObjectModel;
using ProjectTracker.Model.Interfaces;

namespace ProjectTracker.Model.Models
{
    public class Board : DomainObject, IProjectLink
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DeadlineDate { get; set; }

        public ObservableCollection<Group> Groups { get; set; }

        public int ProjectID { get; set; }
        public Project Project { get; set; }
    }
}
