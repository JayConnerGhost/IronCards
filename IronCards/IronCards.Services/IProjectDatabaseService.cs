using System.Collections.Generic;
using IronCards.Objects;

namespace IronCards.Services
{
    public interface IProjectDatabaseService
    {
        int New(string projectName);
        List<ProjectDocument> GetAll();
        ProjectDocument Get(int projectId);
    }
}