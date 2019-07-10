using System.Collections;
using System.Collections.Generic;

namespace IronCards.Services
{
    public interface INotesDatabaseService
    {
        int Insert(string title, string description, int projectId);
        IList<NoteDocument> GetAllForProject(int projectId);
    }
}