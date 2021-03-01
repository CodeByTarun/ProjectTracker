using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.ClassLibrary.ServiceInterfaces
{
    public interface IIssueDataService : IDataService<Issue>
    {
        Task Move(Issue issue, Issue issueBefore, Issue issueAfter, int groupMovedToId);
        Task<IEnumerable<string>> GetAllTags(int projectId);
    }
}
