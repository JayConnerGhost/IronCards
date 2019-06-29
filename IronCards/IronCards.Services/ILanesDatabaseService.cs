using System.Collections.Generic;
using IronCards.Objects;

namespace IronCards.Services
{
    public interface ILanesDatabaseService
    {
        int Insert(string laneLabel, int projectId);
        void Update(int targetId, string laneLabel, int projectId);
        List<LaneDocument> GetAll(int projectId);
        bool Delete(int laneId);
    }
}