using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectTracker.Model.Interfaces
{
    public interface ITag
    {
        public ICollection<Tag> Tags { get; set; }
    }
}
