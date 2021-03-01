using ProjectTracker.Model;
using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.ClassLibrary.ServiceInterfaces
{
    public interface IBoardDataService : IDataService<Board>
    {
        Task<IEnumerable<Board>> GetAllInProject(int projectId);

        Task<Board> GetBoardWithInnerEntities(int boardId);
        Task<ObservableCollection<Group>> GetGroupsInBoard(int boardId, ProjectTrackerDBContext context);
        Task<ObservableCollection<Issue>> GetIssuesInGroup(int groupId, ProjectTrackerDBContext context);
        Task<ObservableCollection<Issue>> GetIssuesInBoard(int boardId);

    }
}
