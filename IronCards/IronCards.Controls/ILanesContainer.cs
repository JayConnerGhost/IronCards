using IronCards.Objects;

namespace IronCards.Controls
{
    public interface ILanesContainer
    {
        void AddLane(int projectId, string projectName, string todo);
        void LoadLane(LaneDocument lane);
        void RemoveLanes();
    }
}