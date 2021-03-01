using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.ClassLibrary.ServiceInterfaces
{
    public interface IGroupDataService : IDataService<Group>
    {
        Task<bool> Move(Group entityMoving, Group entityBefore, Group entityAfter);
    }
}
